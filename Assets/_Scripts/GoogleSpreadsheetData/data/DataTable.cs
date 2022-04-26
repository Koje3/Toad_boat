using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Specifies the reference is optional and is allowed to not find any entries
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class OptionalAttribute : Attribute
{
}

/// <summary>
/// Specifies this field is used as an index for rows of this type
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class IndexAttribute : Attribute
{
}

/// <summary>
/// Specifies which column to use as data source for this field
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class ColumnAttribute : Attribute
{
	public string Key;

	public ColumnAttribute(string key)
	{
		Key = key;
	}
}

/// <summary>
/// Specifies which data files are used to fill this table
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class SourceAttribute : Attribute
{
	public string[] Files { get; private set; }

	public SourceAttribute(params string[] files)
	{
		Files = files;
	}
}

public class BaseRow
{
}

public class DataTable
{
	class Ref
	{
		public Type Type;
		public object Obj;
		public string Id;
		public string[] Ids;
		public FieldInfo FieldInfo;
		public string Filename;
		public int RowNumber;
		public bool Optional;
	}

	delegate object Parser(string value);

	static Lazy<Dictionary<Type, Parser>> sm_parsers = new Lazy<Dictionary<Type, Parser>>(CreateParsers);

	static Dictionary<Type, Dictionary<string, List<object>>> sm_idMap = new Dictionary<Type, Dictionary<string, List<object>>>();
	static List<Ref> sm_references = new List<Ref>();

	public static T[] Parse<T>(List<string[]> values, string filename) where T : new()
	{
		return (T[])Parse(typeof(T), values, filename);
	}

	public static object Parse(Type type, List<string[]> values, string filename)
	{
		// Get fields for the type
		var fieldInfos = type.GetFields();

		// Build lookup table for fields
		var fields = new (int index, Parser parser, FieldInfo fi, bool isNullable, bool isArray, bool isID, bool notColumn, bool optional)[fieldInfos.Length];
		for (int i = 0; i < fields.Length; ++i)
		{
			string columnName = fieldInfos[i].GetCustomAttribute<ColumnAttribute>()?.Key ?? fieldInfos[i].Name;
			fields[i].index = Array.IndexOf(values[0], columnName);
			fields[i].fi = fieldInfos[i];
			fields[i].isNullable = Nullable.GetUnderlyingType(fieldInfos[i].FieldType) != null || !fieldInfos[i].FieldType.IsValueType;
			fields[i].isArray = fieldInfos[i].FieldType.IsArray;
			fields[i].isID = fieldInfos[i].Name == "IDText" || fieldInfos[i].GetCustomAttribute<IndexAttribute>() != null;
			fields[i].notColumn = fieldInfos[i].GetCustomAttribute<ColumnAttribute>() != null;
			fields[i].optional = fieldInfos[i].GetCustomAttribute<OptionalAttribute>() != null;

			// If field is index, add entry for it
			if (fields[i].isID && !sm_idMap.ContainsKey(type))
				sm_idMap.Add(type, new Dictionary<string, List<object>>());

			Type t = Nullable.GetUnderlyingType(fieldInfos[i].FieldType) ?? fieldInfos[i].FieldType.GetElementType() ?? fields[i].fi.FieldType;

			if (!typeof(BaseRow).IsAssignableFrom(t))
			{
				if (t.IsEnum)
				{
					fields[i].parser = (value) => Enum.Parse(t, value);
				}
				else if (fields[i].isArray)
				{
					var elementType = fieldInfos[i].FieldType.GetElementType();
					if (!sm_parsers.Value.TryGetValue(elementType, out fields[i].parser))
					{
						var parserMethod = elementType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public);
						if (parserMethod != null)
						{
							object[] args = new object[1];
							fields[i].parser = (string value) => { args[0] = value; return parserMethod.Invoke(null, args); };
						}
						else
						{
							throw new Exception($"Failed for find parser for type {fieldInfos[i].FieldType.GetElementType()}");
						}
					}
				}
				else if (!fields[i].isArray)
				{
					if(!sm_parsers.Value.TryGetValue(t, out fields[i].parser))
					{
						var parserMethod = t.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public);
						if (parserMethod != null)
						{
							object[] args = new object[1];
							fields[i].parser = (string value) => { args[0] = value; return parserMethod.Invoke(null, args); };
						}
						else
						{
							throw new Exception($"Failed for find parser for type {t}");
						}
					}
				}
			}
		}

		// Allocate array for result rows
		var result = Array.CreateInstance(type, values.Count - 1);

		// Iterate all rows of data (except header row)
		for (int row = 1; row < values.Count; ++row)
		{
			var obj = Activator.CreateInstance(type);

			// Iterate all fields
			for (int i = 0; i < fieldInfos.Length; ++i)
			{
				var field = fields[i];

				// Get mapping from field to column, and skip if column is not present in data
				int column = field.index;
				if (column == -1)
				{
					if (!field.isNullable)
						throw new Exception($"Missing a required column '{field.fi.Name}' of type '{CSharpName(field.fi.FieldType)}' in {type.Name} ({filename})");
					continue;
				}

				string cell = column < values[row].Length ? values[row][column] : null;

				// Add row to IDText -> Row mapping table
				if (field.isID)
				{
					List<object> list;
					if (!sm_idMap[type].TryGetValue(cell, out list))
					{
						list = new List<object>();
						sm_idMap[type].Add(cell, list);
					}
					list.Add(obj);
				}

				if (string.IsNullOrWhiteSpace(cell))
				{
					// If type is nullable it is ok for the data to be missing
					if (field.isNullable)
						continue;
					else
						Debug.LogError($"Missing {CSharpName(field.fi.FieldType)} (Type: '{type.Name}' Column: '{field.fi.Name}' Row: {row + 1} File:'{filename}')");
				}
				else if (field.isArray)
				{
					string[] tokens = cell.Split(';', '\n'); // array-within-cell split -char
					var elementType = field.fi.FieldType.GetElementType();

					if (field.parser != null)
					{
						var array = Array.CreateInstance(elementType, tokens.Length);

						for (int j = 0; j < tokens.Length; ++j)
						{
							try
							{
								var value = field.parser(tokens[j]);
								array.SetValue(value, j);
							}
							catch (Exception)
							{
								Debug.LogError($"Failed to parse '{tokens[j]}' as {CSharpName(elementType)} (Type: '{type.Name}' Column: '{field.fi.Name}' Row: {row + 1} Element: {j}) File:'{filename}'");
							}
						}

						field.fi.SetValue(obj, array);
					}
					else
					{
						sm_references.Add(new Ref() { Type = elementType, Obj = obj, Ids = tokens, FieldInfo = field.fi, Filename = filename, RowNumber = row + 1, Optional = field.optional });
					}
				}
				else
				{
					try
					{
						// Run parser
						if (field.parser != null)
						{
							var value = field.parser(cell);
							field.fi.SetValue(obj, value);
						}
						else
						{
							sm_references.Add(new Ref() { Type = field.fi.FieldType, Obj = obj, Id = cell, FieldInfo = field.fi, Filename = filename, RowNumber = row + 1, Optional = field.optional });
						}
					}
					catch (Exception)
					{
						Debug.LogError($"Failed to parse '{cell}' as {CSharpName(field.fi.FieldType)} (Type: '{type.Name}' Column: '{field.fi.Name}' Row: {row + 1} File:'{filename}')");
					}
				}
			}

			result.SetValue(obj, row - 1);
		}

		return result;
	}

	public static void ClearReferences()
	{
		sm_idMap.Clear();
		sm_references.Clear();
	}

	public static void ConnectReferences()
	{
		// Go through all references which require connecting
		foreach (var r in sm_references)
		{
			if (sm_idMap.TryGetValue(r.Type, out var rowsForType))
			{
				if (r.Id != null)
				{
					// Connect references
					if (rowsForType.TryGetValue(r.Id, out var rows))
					{
						if(rows.Count > 1)
							Debug.LogError($"{rows.Count} rows of type {r.Type} found with ID '{r.Id}' referenced by column {r.FieldInfo.Name} on row {r.RowNumber} in {r.Filename}");
						else
							r.FieldInfo.SetValue(r.Obj, rows[0]);
					}
					else if(!r.Optional)
					{
						Debug.LogError($"Can not find {r.Type} with ID '{r.Id}' referenced by column {r.FieldInfo.Name} on row {r.RowNumber} in {r.Filename}");
					}
				}
				else
				{
					// Validate references
					if (!r.Optional)
					{
						foreach (var id in r.Ids)
						{
							if (!rowsForType.ContainsKey(id))
							{
								Debug.LogError($"Can not find {r.Type} with ID {id} referenced by column {r.FieldInfo.Name} on row {r.RowNumber} in {r.Filename}");
							}
						}
					}

					// Connect array references
					var rows = rowsForType.Where(e => r.Ids.Contains(e.Key)).SelectMany(e => e.Value).ToArray();
					var array = Array.CreateInstance(r.Type, rows.Length);

					for (int i = 0; i < rows.Length; ++i)
					{
						array.SetValue(rows[i], i);
					}

					r.FieldInfo.SetValue(r.Obj, array);
				}
			}
			else
			{
				Debug.LogError($"Can not find rows of type '{r.Type}' referenced by column {r.FieldInfo.Name} in {r.Filename}");
			}
		}

		ClearReferences();
	}

	/// <summary>
	/// Creates string-to-value parsers for all types supported.
	/// </summary>
	static Dictionary<Type, Parser> CreateParsers()
	{
		var parsers = new Dictionary<Type, Parser>();

		DateTime epoch = new DateTime(1899, 12, 30, 0, 0, 0, DateTimeKind.Utc);

		parsers[typeof(string)] = (string value) => value;
		parsers[typeof(int)] = (string value) => int.Parse(value, CultureInfo.InvariantCulture);
		parsers[typeof(long)] = (string value) => long.Parse(value, CultureInfo.InvariantCulture);
		parsers[typeof(float)] = (string value) => float.Parse(value, CultureInfo.InvariantCulture);
		parsers[typeof(double)] = (string value) => double.Parse(value, CultureInfo.InvariantCulture);
		parsers[typeof(bool)] = (string value) => bool.Parse(value);
		parsers[typeof(DateTime)] = (string value) => epoch.AddDays(double.Parse(value, CultureInfo.InvariantCulture));

		return parsers;
	}

	/// <summary>
	/// Formats Type name into C# name instead of returning the CLR name.
	/// </summary>
	/// <param name="type"></param>
	public static string CSharpName(Type type)
	{
#if UNITY_EDITOR
		if (!type.FullName.StartsWith("System"))
			return type.Name;
		var compiler = new Microsoft.CSharp.CSharpCodeProvider();
		var t = new System.CodeDom.CodeTypeReference(type);
		var output = compiler.GetTypeOutput(t);
		output = output.Replace("System.", "");
		if (output.Contains("Nullable<"))
			output = output.Replace("Nullable", "").Replace(">", "").Replace("<", "") + "?";
		return output;
#else
		return type.Name;
#endif
	}
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

public class CSV
{
	[Flags]
	public enum Options
	{
		Default = 0,
		Trim = 1,
		PreserveEmptyRows = 2
	}

	public static List<string[]> Parse(string text, Options options = Options.Default)
	{
		bool trim = (options & Options.Trim) != 0;
		bool preserveTrailingEmptyRows = (options & Options.PreserveEmptyRows) != 0;

		int columns = CountColumns(text);

		int length = text.Length;
		List<string[]> rows = new List<string[]>();
		string[] row = new string[columns];
		int column = 0;
		bool inQuote = false;
		bool needsEscaping = false;
		int begin = 0;
		int quoteEnd = 0;
		bool emptyRow = true;
		bool wasQuoted = false;

		for (int i = 0; i <= length; ++i)
		{
			char c = (i < length) ? text[i] : '\n';

			switch (c)
			{
				case '"':
					{
						if (inQuote)
						{
							if (i < length - 1 && text[i + 1] == '\"')
							{
								needsEscaping = true;
								i++;
							}
							else
							{
								inQuote = false;
								quoteEnd = i;
							}
						}
						else
						{
							inQuote = true;
							wasQuoted = true;
							begin = i + 1;
						}
						break;
					}

				case ',':
				case '\r':
				case '\n':
					{
						if (!inQuote)
						{
							int end = (i > 0 && text[i - 1] == '\"') ? (i - 1) : ((quoteEnd > 0) ? quoteEnd : i);
							quoteEnd = 0;

							// Trim end
							if (trim && !wasQuoted)
							{
								while (end > begin && char.IsWhiteSpace(text[end - 1]))
									--end;
							}

							string entry = text.Substring(begin, end - begin);

							emptyRow &= (end == begin);

							if (needsEscaping)
							{
								entry = entry.Replace("\"\"", "\"");
								needsEscaping = false;
							}

							if (column < columns)
							{
								row[column++] = entry;
							}

							if (c != ',')
							{
								if (!emptyRow || preserveTrailingEmptyRows)
								{
									while (column < columns)
										row[column++] = string.Empty;
									rows.Add(row);
									emptyRow = true;
									wasQuoted = false;
								}

								row = new string[columns];
								column = 0;

								if (c == '\r')
									i++;
							}

							begin = i + 1;
						}
						break;
					}

				default:
					{
						if (trim && !wasQuoted)
						{
							while (i == begin && char.IsWhiteSpace(c))
								++begin;
						}
						break;
					}
			}
		}

		return rows;
	}

	public static string Write(IList<IList<object>> values)
	{
		StringBuilder sb = new StringBuilder();

		int columnCount = values[0].Count;
		for (int i = 0; i < values.Count; ++i)
		{
			for (int j = 0; j < columnCount; ++j)
			{
				string data = (j < values[i].Count && values[i][j] != null) ? Convert.ToString(values[i][j], CultureInfo.InvariantCulture) : "";

				bool needsEscape = data.IndexOfAny(new char[] { ',', '\r', '\n' }) != -1;
				if (needsEscape)
					sb.Append('"');

				sb.Append(data);

				if (needsEscape)
					sb.Append('"');

				if (j != columnCount - 1)
					sb.Append(",");
				else if (i != values.Count - 1)
					sb.AppendLine();
			}
		}

		return sb.ToString();
	}

	static int CountColumns(string text)
	{
		int r = 0;
		bool inQuote = false;
		int length = text.Length;

		for (int i = 0; i <= length; ++i)
		{
			char c = (i < length) ? text[i] : '\n';

			switch (c)
			{
				case '"':
					{
						if (inQuote)
						{
							if (i < length - 1 && text[i + 1] == '\"')
							{
								i++;
							}
							else
							{
								inQuote = false;
							}
						}
						else
						{
							inQuote = true;
						}
						break;
					}

				case ',':
				case '\r':
				case '\n':
					{
						if (!inQuote)
						{
							r++;

							if (c != ',')
							{
								return r;
							}
						}
						break;
					}
			}
		}

		return r;
	}
}
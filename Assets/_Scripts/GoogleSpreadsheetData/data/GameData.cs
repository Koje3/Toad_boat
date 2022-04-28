using System;
using System.IO;
using System.Reflection;
using UnityEngine;

public static class GameData
{
	public static bool IsLoaded { get; private set; }

	// Sheets

	public static Sam1BehaviorRow[] Sam1Behavior;

	// Examples
	//public static StatRow[] Stats;
	//public static SequenceRow[] Sequences;
	//public static EnemyCollectionRow[] EnemyCollections;
	//public static EnemyRow[] Enemies;
	//[Source(nameof(LootBundles), "PurchaseBundles", "SideQuestRewards")]
	//public static LootBundleRow[] LootBundles;
	

	public static void Load()
	{
		IsLoaded = false;

		DataTable.ClearReferences();

		// Load all members based on their name (Assumes 1:1 name-to-csv filename correlation for now)
		foreach (var fi in typeof(GameData).GetFields(BindingFlags.Static | BindingFlags.Public))
		{
			// Release old data
			fi.SetValue(null, null);

			var files = fi.GetCustomAttribute<SourceAttribute>()?.Files ?? new[] { fi.Name };

			foreach (var file in files)
			{
				string path =  Path.Combine(file).Replace("\\", "/"); //Debug.Log(path);
				var asset = Resources.Load<TextAsset>(path);
				if (asset == null)
				{
					Debug.LogError($"Failed to load {path}.csv");
					continue;
				}

				var csv = CSV.Parse(asset.text);
				Array data = DataTable.Parse(fi.FieldType.GetElementType(), csv, $"{file}.csv") as Array;

				var arr = fi.GetValue(null) as Array;
				if (arr != null)
				{
					var combined = Array.CreateInstance(fi.FieldType.GetElementType(), arr.Length + data.Length);
					Array.Copy(arr, combined, arr.Length);
					Array.Copy(data, 0, combined, arr.Length, data.Length);
					data = combined;
				}

				fi.SetValue(null, data);
			}

			// Invoke "public static OnAfterLoad(T[])" after load. Can be used to post-process data, build index etc
			var onAfterLoad = fi.FieldType.GetElementType().GetMethod("OnAfterLoad");
			if (onAfterLoad != null)
				onAfterLoad.Invoke("OnAfterLoad", new[] { fi.GetValue(null) });
		}

		DataTable.ConnectReferences();

		IsLoaded = true;
	}
}
using System;
using System.Collections.Generic;
using GameEnums;
//using Data;

// Add all data objects here. Use exact same names for variables as headers in the spreadsheet.
// Data types that are nullable are optional in the spreasheet: they can be empty by design.

[System.Serializable]
public class Sam1BehaviorRow : BaseRow
{
    public string IDText;
    public string Dialogue;
    public string Location;
    public Mood Mood;
    public SamAction SamAction;
    public string NextIDText;
}



//public class CelestialBodiesRow : BaseRow
//{
//    public string IDText;
//    public StellarBody BodyType;
//    public Element? Element1;
//    public int? Element1Value;
//    public Element? Element2;
//    public int? Element2Value;
//    public Element? Element3;
//    public int? Element3Value;
//    public Element? Element4;
//    public int? Element4Value;
//    public string Icon;
//    public string Audio;
//}


// Examples

//public class StatRow : BaseRow
//{
//	//[Index]
//	//public Data.StatType Stat;
//	public string Name;
//	public string ShortName;
//	public string Format;
//	public string FormatDelta;
//	public string Icon;
//	public int? LoadoutOrder;
//	public float? Scale;
//}

//public class StatBundleRow : BaseRow
//{
//	public string IDText;
//	public StatRow Stat;
//	public float Value;
//	//public Stat.Operation Op;
//	public int Prio;
//}


//// This class needs to be serializable if it's also used in an Editor window that needs to save its state
//[Serializable]
//public class SequenceRow : BaseRow
//{
//	public string IDText;
//	public int ThreatLevel;
//	public EnemyCollectionRow[] EnemyCollections;
//	public float HPMultiplier;
//	public float DamageMultiplier;
//}

//public class EnemyCollectionRow : BaseRow
//{
//	public string IDText;
//	public EnemyRow Enemy;
//	public float Weight;
//}

//public class EnemyRow : BaseRow
//{
//	public string IDText;
//	public string Name;
//	public float Health;
//	public float Damage;
//	public float TouchDamage;
//	public float IdleSpeed;
//	public float WalkSpeed;
//	public float RunSpeed;
//	public float AttackCooldown;
//	public float AttackAimTime;
//	public float AttackRange;
//	public float DamageRange;
//	public float ChaseRange;
//	public float IdleRange;
//	public float AimRotationSpeed;
//	public float LookRotationSpeed;
//	public int ThreatLevel;
//	public string Archetype;
//	public int XPValue;
//	public float ThreatModifier;
//	public string Element;
//	public string IngamePrefab;
//	public string SequencePrefab;
//	public ProjectileRow Projectile;
//	[Optional] [Column(nameof(IDText))] public StatBundleRow[] Stats;
//}

//public class BaseItemRow : BaseRow
//{
//	public enum ItemType
//	{
//		Hero,
//		Weapon,
//		Gloves,
//		Boots,
//		Element
//	}

//	public int ID;
//	public string IDText;
//	public string Name;
//	public string Icon;
//	public ItemType Type;
//	public string Description;
//	//public Data.CurrencyType Currency;

//	[Optional] [Column(nameof(IDText))] public StatBundleRow[] Stats;
//}

//public class ItemRow : BaseItemRow
//{
//	public StatRow[] ShowStats;
//	public StatRow MainStat;
//	//public Stat.Operation MainStatOp;
//	//[Column(nameof(IDText))] public StatCurveRow[] MainStatCurve;
//}


//public class SkillStatModifierRow : BaseRow
//{
//	public string IDText;
//	public StatRow Stat;
//	public float Value;
//	//public Stat.Operation Op;
//	public int Prio;
//	public bool VisibleInUI;
//}

//public class ProjectileRow : BaseRow
//{
//	public string IDText;
//	public string BasePrefab;
//	public string HitMask;
//	public float MaxLifetime;
//	public float Speed;
//	//public Projectile.FlightStyle FlightStyle;
//	public float? Height;
//	public string FlightCurve;
//	public string IndicatorPrefab;
//	public bool RotateTowardsVelocity;
//}

//public class EffectRow : BaseRow
//{
//	public string[] RequiredTags;
//	public string MaxProcCountValue;
//	//public Effect.ProcEffect Effect;
//	public string Value;
//	//public StatType? ValueStat;
//	public float? Range;
//	public DamageDataRow DamageData;
//	public string VfxToSpawn;
//	public bool? ScaleVfxWithRange;
//	public string Sfx;

//	public HashSet<string> RequiredTagSet { get; private set; }

//	public static void OnAfterLoad(EffectRow[] effectRows)
//	{
//		foreach (EffectRow effectRow in effectRows)
//		{
//			effectRow.RequiredTagSet = new HashSet<string>(effectRow.RequiredTags);
//		}
//	}
//}


//public class IAPRow : BaseRow
//{
//	public enum IAPPlacement
//	{
//		Gems,
//		Offers,
//		Deals
//	}

//	public string IDText;
//	public IAPPlacement Placement;
//	public string Name;
//	public string Description;
//	public string Icon;
//	public string Price;
//	//[Column(nameof(IDText))] public LootBundleRow[] LootBundles;
//}


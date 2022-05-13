using System.Reflection;
using UnityEngine;

namespace GameEnums
{
    public enum Element // TODO: move to somewhere more sensible later
    {
        Earth,
        Water,
        Fire,
        Air,
        Dark,
        Light,
        Arcane
    }

    public enum StellarBody
    {
        BodyMoon,
        BodyCalendar,
        BodyStellar
    }

    [System.Serializable]
    public enum Mood
    {
        Default,
        Sarcastic,
        Angry,
        Panicking,
        Bored,
        Annoyed,
        Embarrassed,
        Relieved
    }

    [System.Serializable]
    public enum SamAction
    {
        Idle,
        WaitForPlayer,
        Panic,
        RandomMove,
        TurnTowardsPlayer,
        ChangeLocation
    }

    [System.Serializable]
    public class ElementForces
    {
        public string id;

        public int Air;
        public int Fire;
        public int Earth;
        public int Water;
        public int Dark;
        public int Light;
        public int Arcane;

        public ElementForces()
        { }
        public ElementForces(ElementForces[] addForces)
        {
            foreach (var addedForce in addForces)
            {
                Air += addedForce.Air;
                Fire += addedForce.Fire;
                Earth += addedForce.Earth;
                Water += addedForce.Water;
                Dark += addedForce.Dark;
                Light += addedForce.Light;
                Arcane += addedForce.Arcane;
            }
        }
        //public ElementForces(CelestialBodiesRow row)
        //{
        //    id = row.IDText;
        //    if (row.Element1 != null && row.Element1Value != null) AddElementValue((Element)row.Element1, (int)row.Element1Value);
        //    if (row.Element2 != null && row.Element2Value != null) AddElementValue((Element)row.Element2, (int)row.Element2Value);
        //    if (row.Element3 != null && row.Element3Value != null) AddElementValue((Element)row.Element3, (int)row.Element3Value);
        //    if (row.Element4 != null && row.Element4Value != null) AddElementValue((Element)row.Element4, (int)row.Element4Value);
        //}
        void AddElementValue(Element element, int value)
        {
            switch (element)
            {
                case Element.Earth:
                    Earth += value;
                    break;
                case Element.Water:
                    Water += value;
                    break;
                case Element.Fire:
                    Fire += value;
                    break;
                case Element.Air:
                    Air += value;
                    break;
                case Element.Dark:
                    Dark += value;
                    break;
                case Element.Light:
                    Light += value;
                    break;
                case Element.Arcane:
                    Arcane += value;
                    break;

                default:
                    Debug.Log("No element case for: " + element);
                    break;
            }
        }


        public ElementForces(int air, int fire, int earth, int water, int dark, int light, int arcane)
        {
            Air = air;
            Fire = fire;
            Earth = earth;
            Water = water;
            Dark = dark;
            Light = light;
            Arcane = arcane;
        }
        public int GetElementValue(Element element)
        {
            switch (element)
            {
                case Element.Earth:
                    return Earth;
                case Element.Water:
                    return Water;
                case Element.Fire:
                    return Fire;
                case Element.Air:
                    return Air;
                case Element.Dark:
                    return Dark;
                case Element.Light:
                    return Light;
                case Element.Arcane:
                    return Arcane;
                default:
                    {
                        Debug.LogError("GetElement didn't have a case for: " + element);
                        return 0;
                    }
            }
        }

        public string ToUiString()
        {
            string rv = id;

            foreach (var item in this.GetType().GetFields())
            {
                if (item.GetType() == typeof(int) && item.GetValue(BindingFlags.Public | BindingFlags.Instance).ToString() != "0")
                {
                    rv += "\n" + item.Name + " " + item.GetValue(BindingFlags.Public | BindingFlags.Instance).ToString();
                }
            }
            return rv;
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum IdCardCategory
{
    Date,
    Validity,
    Other,
}

[System.Serializable]
public struct IdCardInfoConfig
{
    public IdCardCategory category;
    public string text;
    public bool isInvalid;
}

[CreateAssetMenu]
public class PNJConfig : ScriptableObject
{
    public string[] menFirstNames;
    public string[] womenFirstNames;
    public string[] secondNames;
    public IdCardInfoConfig[] info;

}

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
    [TextArea(3,10)]
    public string text;
    public bool isInvalid;
}

[System.Serializable]
public struct PNJDisplayProfile
{
    public Sprite dialogueSprite;
    public SpriteAnimList animations;
}

[CreateAssetMenu]
public class PNJConfig : ScriptableObject
{
    public string[] menFirstNames;
    public string[] womenFirstNames;
    public string[] secondNames;
    public IdCardInfoConfig[] info;

    public PNJDisplayProfile[] pnjDisplayProfileMen;
    public PNJDisplayProfile[] pnjDisplayProfileWomen;

}

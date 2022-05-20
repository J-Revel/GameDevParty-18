using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PNJConfig : ScriptableObject
{
    [Range(0, 1)]
    public float minEcology;
    
    [Range(0, 1)]
    public float maxEcology;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CharacterMovementConfig : ScriptableObject
{
    public float acceleration;
    public float inertia;
    public float maxSpeed;
    public float mass;
    public float gravity;
    
}

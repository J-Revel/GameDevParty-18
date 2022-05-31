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
    public float throwSpeed;
    public float throwVerticalSpeed;
    public float onGroundDuration = 2;
    public float collisionThrowSpeed;
    public float collisionVerticalSpeed;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    private new Rigidbody rigidbody;
    public System.Action grabStartedDelegate;
    public System.Action thrownDelegate;
    private CharacterMovement movement;

    
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        movement = GetComponent<CharacterMovement>();
    }

    public void StartGrabbed()
    {
        grabStartedDelegate?.Invoke();
        movement.SetState(CharacterState.Carried);
    }

    public void OnThrow(Vector3 velocity)
    {
        rigidbody.velocity = velocity;
        movement.SetState(CharacterState.Flying);
    }
}

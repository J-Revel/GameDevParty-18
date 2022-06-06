using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    private new Rigidbody rigidbody;
    public System.Action grabStartedDelegate;
    public System.Action thrownDelegate;
    private CharacterMovement movement;
    public float invincibilityDuration = 5;
    private float invincibilityTime = 0;

    public bool isInvincible {get { return invincibilityTime > 0; }}

    
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
        thrownDelegate?.Invoke();
        movement.SetState(CharacterState.Flying);
        invincibilityTime = invincibilityDuration;
    }

    private void Update()
    {
        invincibilityTime -= Time.deltaTime;
    }
}

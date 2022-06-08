using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHitbox : MonoBehaviour
{
    private CharacterMovement characterMovement;
    private new Rigidbody rigidbody;
    public Transform collisionFxHitbox;
    public System.Action collisionDelegate;

    void Start()
    {
        rigidbody = GetComponentInParent<Rigidbody>();
        characterMovement = GetComponentInParent<CharacterMovement>();
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        CharacterMovement movement = other.GetComponentInParent<CharacterMovement>();
        if(movement == null || characterMovement == movement)
            return;
        Grabbable grabbable = movement.GetComponent<Grabbable>();
        if(grabbable == null)
            return;
            
        switch(movement.currentState)
        {
            case CharacterState.Idle:
            case CharacterState.Carrying:
                Vector3 collisionDirection = (other.transform.position - transform.position);
                collisionDirection.y = 0;
                Instantiate(collisionFxHitbox, other.ClosestPoint(movement.transform.position), movement.transform.rotation);
                
                grabbable.OnThrow(collisionDirection.normalized * movement.collisionThrowSpeed + Vector3.up * movement.collisionVerticalSpeed);
                collisionDelegate?.Invoke();
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHitbox : MonoBehaviour
{
    public CharacterMovement thrower;
    private new Rigidbody rigidbody;
    void Start()
    {
        rigidbody = GetComponentInParent<Rigidbody>();
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        CharacterMovement movement = other.GetComponentInParent<CharacterMovement>();
        if(movement == null)
            return;
        Grabbable grabbable = movement.GetComponent<Grabbable>();
        if(movement != thrower && grabbable != null)
        {
            switch(movement.currentState)
            {
                case CharacterState.Idle:
                case CharacterState.Carrying:
                    Vector3 collisionDirection = (other.transform.position - transform.position);
                    collisionDirection.y = 0;
                
                    grabbable.OnThrow(collisionDirection.normalized * movement.collisionThrowSpeed + Vector3.up * movement.throwVerticalSpeed);
                    break;

            }
        }
    }
}

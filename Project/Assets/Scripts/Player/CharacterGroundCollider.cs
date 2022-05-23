using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGroundCollider : MonoBehaviour
{
    private CharacterMovement movement;
    public Collider groundCollider;
    void Start()
    {
        movement = GetComponent<CharacterMovement>();
        if(movement == null)
            movement = GetComponentInParent<CharacterMovement>();
    }

    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(movement == null)
            return;
        if(collision.GetContact(0).thisCollider == groundCollider)
            movement.OnTouchGround();
    }
}

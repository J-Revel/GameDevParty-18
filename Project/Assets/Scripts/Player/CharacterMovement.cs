using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public CharacterMovementConfig config;
    private new Rigidbody rigidbody;
    public Vector3 movementInput;
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        movementInput.x = Input.GetAxis("Horizontal");
        movementInput.z = Input.GetAxis("Vertical");
        spriteRenderer.flipX = rigidbody.velocity.x < 0;
    }
    
    public void FixedUpdate()
    {
        rigidbody.AddForce(movementInput * config.acceleration);
        rigidbody.velocity = rigidbody.velocity * Mathf.Pow(config.inertia, Time.fixedDeltaTime);
        if(rigidbody.velocity.sqrMagnitude > config.maxSpeed * config.maxSpeed)
        {
            rigidbody.velocity = rigidbody.velocity * config.maxSpeed / rigidbody.velocity.magnitude;
        }
    }
}

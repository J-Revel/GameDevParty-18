using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public CharacterMovementConfig config;
    private new Rigidbody rigidbody;
    public Vector3 movementInput;
    public SpriteRenderer spriteRenderer;
    public AnimatedSprite animatedSprite;
    public float speedWalkThreshold = 0.1f;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.mass = config.mass;
    }

    public void Update()
    {
        spriteRenderer.flipX = rigidbody.velocity.x < 0;
    }
    
    public void FixedUpdate()
    {
        rigidbody.AddForce(movementInput * config.acceleration);
        rigidbody.velocity = rigidbody.velocity * Mathf.Pow(config.inertia, Time.fixedDeltaTime);
        animatedSprite.SelectAnim(movementInput.sqrMagnitude > speedWalkThreshold * speedWalkThreshold ? "Walk" : "Idle");
        if(rigidbody.velocity.sqrMagnitude > config.maxSpeed * config.maxSpeed)
        {
            rigidbody.velocity = rigidbody.velocity * config.maxSpeed / rigidbody.velocity.magnitude;
        }
    }
}

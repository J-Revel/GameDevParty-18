using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState
{
    Idle,
    Grabbing, // Play the grab animation, go to carrying right after
    Carrying,
    Throwing, // Play the throw animation, go to Idle right after
    Flying,
    OnGround,
    Carried,
    
}

public class CharacterMovement : MonoBehaviour
{
    public CharacterMovementConfig config;
    private new Rigidbody rigidbody;
    public Vector3 movementInput;
    public SpriteRenderer spriteRenderer;
    public AnimatedSprite animatedSprite;
    public float speedWalkThreshold = 0.1f;
    public CharacterState currentState;
    public System.Action throwDelegate;
    public System.Action<CharacterState> stateChangedDelegate;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.mass = config.mass;
    }

    public void Update()
    {
        spriteRenderer.flipX = rigidbody.velocity.x < 0;
        bool animFinished = animatedSprite.isAnimationFinished;
        if(animFinished)
        {
            switch(currentState)
            {
                case CharacterState.Throwing:
                    throwDelegate?.Invoke();
                    SetState(CharacterState.Idle);
                    break;
                case CharacterState.Grabbing:
                    SetState(CharacterState.Carrying);
                    break;
            }

        }
    }

    public void SetState(CharacterState newState)
    {
        if(newState != currentState)
        {
            stateChangedDelegate?.Invoke(newState);
        }
        currentState = newState;
        switch(newState)
        {
            case CharacterState.Throwing:
                animatedSprite.SelectAnim("Throw");
                break;
            case CharacterState.Grabbing:
                animatedSprite.SelectAnim("Grab");
                break;
        }
    }
    
    public void FixedUpdate()
    {
        rigidbody.AddForce(movementInput * config.acceleration);
        rigidbody.AddForce(Vector3.down * config.gravity);
        rigidbody.velocity = rigidbody.velocity * Mathf.Pow(config.inertia, Time.fixedDeltaTime);
        switch(currentState)
        {
            case CharacterState.Idle:
                animatedSprite.SelectAnim(movementInput.sqrMagnitude > speedWalkThreshold * speedWalkThreshold ? "Walk" : "Idle");
                break;
            case CharacterState.Carrying:
                animatedSprite.SelectAnim(movementInput.sqrMagnitude > speedWalkThreshold * speedWalkThreshold ? "Carry Walk" : "Carry Idle");
                break;
        }
        
        if(rigidbody.velocity.sqrMagnitude > config.maxSpeed * config.maxSpeed)
        {
            rigidbody.velocity = rigidbody.velocity * config.maxSpeed / rigidbody.velocity.magnitude;
        }
    }
}

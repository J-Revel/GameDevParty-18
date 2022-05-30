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
    Talking,
}

[System.Serializable]
public struct CharacterMovementSetting
{
    public CharacterState state;
    public CharacterMovementConfig config;
}

public class CharacterMovement : MonoBehaviour
{
    private CharacterMovementConfig currentConfig;
    public CharacterMovementConfig defaultConfig;
    public CharacterMovementSetting[] configOverrides;
    private new Rigidbody rigidbody;
    public Vector3 movementInput;
    public Vector3 lookDirection = Vector3.right;
    public SpriteRenderer spriteRenderer;
    public AnimatedSprite animatedSprite;
    public float speedWalkThreshold = 0.1f;
    public CharacterState currentState;
    public SimpleEvent throwDelegate;
    public System.Action touchGroundDelegate;
    public System.Action grabStartDelegate;
    public System.Action<CharacterState> stateChangedDelegate;
    public System.Action footstepDelegate;
    public float footstepInterval = 0.3f;
    private float footstepDistance = 0;

    private Grabbable grabbable;
    private float onGroundTime = 0;
    public int animFps = 2;

    public float collisionThrowSpeed {
        get {
            return currentConfig.collisionThrowSpeed;
        }
    }

    public float throwVerticalSpeed {
        get => currentConfig.throwVerticalSpeed;
    }

    public Vector3 throwVelocity {
        get {
            return lookDirection * currentConfig.throwSpeed + Vector3.up * currentConfig.throwVerticalSpeed + rigidbody.velocity;
        }
    }

    public float getUpAnimDuration = 0.75f;
    public float[] getUpAnimValues = new float[]{0.9f, 1.05f, 0.95f};

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        grabbable = GetComponent<Grabbable>();
        currentConfig = defaultConfig;
        UpdatePhysicsConfig();
        
    }

    private void UpdatePhysicsConfig()
    {
        rigidbody.mass = currentConfig.mass;
    }

    public void Update()
    {
        spriteRenderer.flipX = lookDirection.x < 0;
        bool animFinished = animatedSprite.isAnimationFinished;
        if(animFinished)
        {
            switch(currentState)
            {
                case CharacterState.Throwing:
                    SetState(CharacterState.Idle);
                    break;
                case CharacterState.Grabbing:
                    SetState(CharacterState.Carrying);
                    break;
                case CharacterState.OnGround:
                    onGroundTime += Time.deltaTime;
                    if(onGroundTime >= currentConfig.onGroundDuration)
                    {
                        onGroundTime = 0;
                        SetState(CharacterState.Idle);
                    }
                    break;
            }

        }
    }

    public void SetState(CharacterState newState)
    {
        CharacterState oldState = currentState;
        currentState = newState;
        if(newState != oldState)
        {
            currentConfig = defaultConfig;
            for(int i=0; i<configOverrides.Length; i++)
            {
                if(configOverrides[i].state == newState)
                {
                    currentConfig = configOverrides[i].config;
                    break;
                }
            }
            UpdatePhysicsConfig();
            stateChangedDelegate?.Invoke(newState);
        }
        switch(newState)
        {
            case CharacterState.Idle:
                if(oldState == CharacterState.OnGround)
                {
                    StartCoroutine(GetUpAnimCoroutine());
                }
                break;
            case CharacterState.Throwing:
                throwDelegate?.Invoke();
                animatedSprite.SelectAnim("Throw");
                break;
            case CharacterState.Grabbing:
                animatedSprite.SelectAnim("Grab");
                grabStartDelegate?.Invoke();
                
                break;
            case CharacterState.OnGround:
                animatedSprite.SelectAnim("Down");
                break;

            case CharacterState.Flying:
                animatedSprite.SelectAnim("Flying");
                break;
            case CharacterState.Carried:
                animatedSprite.SelectAnim("Carried");
                break;
        }
    }

    private IEnumerator GetUpAnimCoroutine()
    {
        for(int i=0; i<getUpAnimValues.Length; i++)
        {
            transform.localScale = new Vector3(1, getUpAnimValues[i], 1);
            yield return new WaitForSeconds(1.0f/animFps);
        }
        transform.localScale = Vector3.one;
    }
    
    public void FixedUpdate()
    {
        if(movementInput.sqrMagnitude > 0)
        {
            lookDirection = movementInput.normalized;
        }
        rigidbody.AddForce(movementInput * currentConfig.acceleration);
        rigidbody.AddForce(Vector3.down * currentConfig.gravity);
        rigidbody.velocity = rigidbody.velocity * Mathf.Pow(currentConfig.inertia, Time.fixedDeltaTime);
        switch(currentState)
        {
            case CharacterState.Idle:
            case CharacterState.Talking:
                animatedSprite.SelectAnim(movementInput.sqrMagnitude > speedWalkThreshold * speedWalkThreshold ? "Walk" : "Idle");
                break;
            case CharacterState.Carrying:
                animatedSprite.SelectAnim(movementInput.sqrMagnitude > speedWalkThreshold * speedWalkThreshold ? "Carry Walk" : "Carry Idle");
                break;
        }
        
        if(rigidbody.velocity.sqrMagnitude > currentConfig.maxSpeed * currentConfig.maxSpeed)
        {
            rigidbody.velocity = rigidbody.velocity * currentConfig.maxSpeed / rigidbody.velocity.magnitude;
        }
    }

    public void OnTouchGround()
    {
        if(currentState == CharacterState.Flying)
        {
            touchGroundDelegate?.Invoke();
            SetState(CharacterState.OnGround);
        }
    }
}

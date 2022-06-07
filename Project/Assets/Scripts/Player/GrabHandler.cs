using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabHandler : MonoBehaviour
{
    private CharacterMovement characterMovement;
    public Grabbable grabbedElement;
    private Grabbable grabbable;
    public float grabOffset = 0.2f;
    public AnimatedSprite animatedSprite;
    
    public System.Action grabDelegate;

    

    private void Start()
    {
        characterMovement = GetComponent<CharacterMovement>();
        grabbable = GetComponent<Grabbable>();
        grabbable.grabStartedDelegate += OnGrabbed;
    }

    

    public void OnGrabbed()
    {
        if(grabbedElement != null)
        {
            grabbedElement.OnThrow(characterMovement.throwVelocity * 0.01f);
            grabbedElement = null;
        }
    }

    public void Update()
    {
        
    }

    

    public void GrabTarget(Grabbable grabbable)
    {
        characterMovement.SetState(CharacterState.Grabbing);
        grabDelegate?.Invoke();
        grabbedElement = grabbable;
        grabbable.grabStartedDelegate?.Invoke();
        grabbedElement.StartGrabbed();
    }

    public void Throw()
    {
        if(grabbedElement != null)
        {
            characterMovement.SetState(CharacterState.Throwing);
            grabbedElement.OnThrow(characterMovement.throwVelocity);
            grabbedElement = null;
        }
    }

    private void FixedUpdate()
    {
        if(grabbedElement != null)
            grabbedElement.transform.position = animatedSprite.GetPointPosition("Grab Point") - transform.forward * grabOffset;
    }

    
}

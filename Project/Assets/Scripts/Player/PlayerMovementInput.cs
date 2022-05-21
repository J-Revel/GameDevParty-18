using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementInput : MonoBehaviour
{
    private CharacterMovement characterMovement;
    private List<Grabbable> inRangeGrabbables = new List<Grabbable>();
    private Grabbable closestGrabbable;
    public AnimatedSprite animatedSprite;
    public Grabbable grabbedElement;
    public float grabOffset = 0.2f;

    private void Start()
    {
        characterMovement = GetComponent<CharacterMovement>();
        characterMovement.throwDelegate += OnThrow;
    }

    public void Update()
    {
        characterMovement.movementInput.x = Input.GetAxis("Horizontal");
        characterMovement.movementInput.z = Input.GetAxis("Vertical");
        
        int closestGrabbableIndex = -1;
        float closestGrabbableDistance = Mathf.Infinity;
        for(int i=0; i < inRangeGrabbables.Count; i++)
        {
            float distance = Vector3.SqrMagnitude(inRangeGrabbables[i].transform.position - transform.position);
            if(closestGrabbableDistance > distance)
            {
                closestGrabbableDistance = distance;
                closestGrabbableIndex = i;
            }
        }
        if(closestGrabbableIndex < 0)
        {
            if(closestGrabbable != null)
                closestGrabbable.GetComponent<Highlightable>().SetHighlighted(false);
            closestGrabbable = null;
        }
        else if(inRangeGrabbables[closestGrabbableIndex] != closestGrabbable)
        {
            if(closestGrabbable != null)
                closestGrabbable.GetComponent<Highlightable>().SetHighlighted(false);
            closestGrabbable = inRangeGrabbables[closestGrabbableIndex];
            closestGrabbable.GetComponent<Highlightable>().SetHighlighted(true);
            
        }

        if(Input.GetButtonDown("Interact") && closestGrabbable != null)
        {
            switch(characterMovement.currentState)
            {
                case CharacterState.Idle:
                    characterMovement.SetState(CharacterState.Grabbing);
                    grabbedElement = closestGrabbable;
                    closestGrabbable.grabStartedDelegate?.Invoke();

                    break;
                case CharacterState.Carrying:
                    characterMovement.SetState(CharacterState.Throwing);
                    closestGrabbable.grabFinishedDelegate?.Invoke();
                    grabbedElement = null;
                    break;
            }
        }
    }

    private void FixedUpdate()
    {
        if(grabbedElement != null)
            grabbedElement.transform.position = animatedSprite.GetPointPosition("Grab Point") - transform.forward * grabOffset;
    }

    private void OnTriggerEnter(Collider collider)
    {
        Grabbable grabbable = collider.GetComponent<Grabbable>();
        if(grabbable != null)
            inRangeGrabbables.Add(grabbable);
    }

    private void OnTriggerExit(Collider collider)
    {
        Grabbable grabbable = collider.GetComponent<Grabbable>();
        inRangeGrabbables.Remove(grabbable);
    }

    private void OnThrow()
    {
        
    }
}

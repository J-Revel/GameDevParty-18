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
    public ThemeSelector themeSelector;
    private bool talking = false;

    private void Start()
    {
        characterMovement = GetComponent<CharacterMovement>();
        characterMovement.throwDelegate += OnThrow;
    }

    public void Update()
    {
        if(characterMovement.currentState == CharacterState.Talking)
        {
            characterMovement.movementInput = Vector3.zero;
            if(Input.GetButtonDown("Interact"))
            {
                themeSelector.CloseMenu();
                if(closestGrabbable != null)
                {
                    characterMovement.SetState(CharacterState.Grabbing);
                    grabbedElement = closestGrabbable;
                    closestGrabbable.grabStartedDelegate?.Invoke();
                    grabbedElement.StartGrabbed();
                    inRangeGrabbables.Remove(grabbedElement);
                    closestGrabbable.GetComponent<Highlightable>().SetHighlighted(false);
                    closestGrabbable = null;
                }
                talking = false;
            }
        }
        else
        {
            characterMovement.movementInput.x = Input.GetAxis("Horizontal");
            characterMovement.movementInput.z = Input.GetAxis("Vertical");
        
            int closestGrabbableIndex = -1;
            float closestGrabbableDistance = Mathf.Infinity;
            for(int i=inRangeGrabbables.Count - 1; i >= 0; i--)
            {
                if(inRangeGrabbables[i] == null || !inRangeGrabbables[i].isActiveAndEnabled)
                    inRangeGrabbables.RemoveAt(i);
            }
            for(int i=0; i < inRangeGrabbables.Count; i++)
            {
                float distance = Vector3.SqrMagnitude(inRangeGrabbables[i].transform.position - transform.position);
                if(closestGrabbableDistance > distance)
                {
                    closestGrabbableDistance = distance;
                    closestGrabbableIndex = i;
                }
            }
            if(closestGrabbableIndex < 0 || characterMovement.currentState != CharacterState.Idle)
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

            if(Input.GetButtonDown("Interact"))
            {
                switch(characterMovement.currentState)
                {
                    case CharacterState.Idle:
                        if(closestGrabbable != null)
                        {
                            characterMovement.SetState(CharacterState.Grabbing);
                            grabbedElement = closestGrabbable;
                            closestGrabbable.grabStartedDelegate?.Invoke();
                            grabbedElement.StartGrabbed();
                            inRangeGrabbables.Remove(grabbedElement);
                            closestGrabbable.GetComponent<Highlightable>().SetHighlighted(false);
                            closestGrabbable = null;
                        }
                        break;
                    case CharacterState.Carrying:
                        if(grabbedElement != null)
                        {
                            characterMovement.SetState(CharacterState.Throwing);
                            grabbedElement.thrownDelegate?.Invoke();
                            grabbedElement.OnThrow(characterMovement.throwVelocity);
                            grabbedElement = null;
                        }
                        break;
                }
            }
        }

        if(Input.GetButtonDown("Talk"))
        {
            
            if(characterMovement.currentState == CharacterState.Talking)
            {
                themeSelector.OnTalkButtonPressed();
            }
            else
            {
                switch(characterMovement.currentState)
                {
                    case CharacterState.Idle:
                        if(closestGrabbable != null)
                        {
                            PNJProfile config = closestGrabbable.GetComponent<PNJProfile>();
                            themeSelector.StartDialogue(config);
                            characterMovement.SetState(CharacterState.Talking);
                            closestGrabbable.GetComponent<CharacterMovement>().SetState(CharacterState.Talking);
                        }
                        break;
                }
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
        Grabbable grabbable = collider.GetComponentInParent<Grabbable>();
        if(grabbable != null && !inRangeGrabbables.Contains(grabbable))
            inRangeGrabbables.Add(grabbable);
    }

    private void OnTriggerExit(Collider collider)
    {
        Grabbable grabbable = collider.GetComponentInParent<Grabbable>();
        inRangeGrabbables.Remove(grabbable);
    }

    private void OnThrow()
    {

    }
}

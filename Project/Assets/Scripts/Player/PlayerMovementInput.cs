using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementInput : MonoBehaviour
{
    private CharacterMovement characterMovement;
    private GrabHandler grabHandler;
    public float grabOffset = 0.2f;
    public DialogueUI dialogueUI;
    
    public System.Action grabDelegate;

    private void Start()
    {
        characterMovement = GetComponent<CharacterMovement>();
        grabHandler = GetComponent<GrabHandler>();
    }

    public void Update()
    {
        if(characterMovement.currentState == CharacterState.Talking)
        {
            characterMovement.movementInput = Vector3.zero;
            if(Input.GetButtonDown("Interact"))
            {
                dialogueUI.CloseDialogue();
                grabHandler.GrabClosest();
            }
        }
        else
        {
            characterMovement.movementInput.x = Input.GetAxis("Horizontal");
            characterMovement.movementInput.z = Input.GetAxis("Vertical");
        
            
            if(Input.GetButtonDown("Interact"))
            {
                switch(characterMovement.currentState)
                {
                    case CharacterState.Idle:
                        grabHandler.GrabClosest();
                        break;
                    case CharacterState.Carrying:
                        grabHandler.Throw();
                        break;
                }
            }
        }

        if(Input.GetButtonDown("Talk"))
        {
            
            if(characterMovement.currentState == CharacterState.Talking)
            {
                dialogueUI.OnTalkButtonPressed();
            }
            else
            {
                switch(characterMovement.currentState)
                {
                    case CharacterState.Idle:
                        Grabbable closestGrabbable = grabHandler.closestGrabbable;
                        if(closestGrabbable != null)
                        {
                            PNJProfile config = closestGrabbable.GetComponent<PNJProfile>();
                            dialogueUI.StartDialogue(config);
                            characterMovement.SetState(CharacterState.Talking);
                            closestGrabbable.GetComponent<CharacterMovement>().SetState(CharacterState.Talking);
                        }
                        break;
                }
            }
        }
    }
}

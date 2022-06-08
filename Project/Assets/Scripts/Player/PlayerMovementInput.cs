using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementInput : MonoBehaviour
{
    public static PlayerMovementInput instance;
    private CharacterMovement characterMovement;
    private GrabHandler grabHandler;
    public float grabOffset = 0.2f;
    public DialogueUI dialogueUI;
    
    public System.Action grabDelegate;
    private Grabbable grabbable;
    private Grabbable _closestGrabbable;

    public Grabbable closestGrabbable { get { return _closestGrabbable; } }
    private List<Grabbable> inRangeGrabbables = new List<Grabbable>();

    private Highlightable highlighted;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        characterMovement = GetComponent<CharacterMovement>();
        grabHandler = GetComponent<GrabHandler>();
        grabbable = GetComponent<Grabbable>();
        grabbable.grabStartedDelegate += OnGrabbed;
    }

    public void Update()
    {
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

        if(characterMovement.currentState == CharacterState.Talking)
        {
            
        }
        else
        {
            if(characterMovement.currentState == CharacterState.Idle)
            {
                if(closestGrabbableIndex < 0)
                {
                    if(highlighted != null)
                        highlighted.SetHighlighted(false, false);
                    _closestGrabbable = null;
                    highlighted = null;
                }
                else if(inRangeGrabbables[closestGrabbableIndex] != _closestGrabbable)
                {
                    if(highlighted != null)
                        highlighted.SetHighlighted(false, false);
                    _closestGrabbable = inRangeGrabbables[closestGrabbableIndex];
                    highlighted = _closestGrabbable.GetComponent<Highlightable>();
                    highlighted.SetHighlighted(true, true);
                }
            }

            characterMovement.movementInput.x = Input.GetAxis("Horizontal");
            characterMovement.movementInput.z = Input.GetAxis("Vertical");
        
        }

        switch(characterMovement.currentState)
        {
            case CharacterState.Idle:
                if(closestGrabbable != null && Input.GetButtonDown("Talk"))
                {
                    PNJProfile config = closestGrabbable.GetComponent<PNJProfile>();
                    dialogueUI.StartDialogue(config);
                    characterMovement.SetState(CharacterState.Talking);
                    closestGrabbable.GetComponent<CharacterMovement>().SetState(CharacterState.Talking);
                }
                if(Input.GetButtonDown("Interact"))
                    GrabClosest();
                break;
            case CharacterState.Talking:
                characterMovement.movementInput = Vector3.zero;
                if(highlighted != null)
                    highlighted.SetHighlighted(true, false);
                if(Input.GetButtonDown("Close"))
                {
                    dialogueUI.CloseDialogue();
                    GrabClosest();
                }
                if(Input.GetButtonDown("Talk") || Input.GetButtonDown("Interact"))
                    dialogueUI.OnTalkButtonPressed();
                break;
            case CharacterState.Carrying:
                if(Input.GetButtonDown("Interact"))
                    grabHandler.Throw();
                break;
        }
    }

    private void OnGrabbed()
    {
        if(characterMovement.currentState == CharacterState.Talking)
        {
            dialogueUI.CloseDialogue();
        }
    }

    public void GrabClosest()
    {
        if(_closestGrabbable != null && _closestGrabbable.isActiveAndEnabled)
        {
            characterMovement.SetState(CharacterState.Grabbing);
            grabDelegate?.Invoke();
            grabHandler.GrabTarget(_closestGrabbable);
            inRangeGrabbables.Remove(_closestGrabbable);
            if(highlighted != null)
                highlighted.SetHighlighted(false, false);
            _closestGrabbable = null;
        }
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
}

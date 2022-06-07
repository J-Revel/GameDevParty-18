using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabHandler : MonoBehaviour
{
    private CharacterMovement characterMovement;
    private List<Grabbable> inRangeGrabbables = new List<Grabbable>();
    private Grabbable _closestGrabbable;
    public Grabbable grabbedElement;
    private Grabbable grabbable;
    public float grabOffset = 0.2f;
    public AnimatedSprite animatedSprite;
    
    public System.Action grabDelegate;
    public bool inRangeHilight = false;
    public bool closestGrabbableLocked = false;

    public Grabbable closestGrabbable { get { return _closestGrabbable; } }

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
        if(characterMovement.currentState != CharacterState.Talking)
        {
            if(closestGrabbableIndex < 0)
            {
                if(inRangeHilight && _closestGrabbable != null)
                    _closestGrabbable.GetComponent<Highlightable>().SetHighlighted(false);
                _closestGrabbable = null;
            }
            else if(inRangeGrabbables[closestGrabbableIndex] != _closestGrabbable)
            {
                if(inRangeHilight && _closestGrabbable != null)
                    _closestGrabbable.GetComponent<Highlightable>().SetHighlighted(false);
                _closestGrabbable = inRangeGrabbables[closestGrabbableIndex];
                if(inRangeHilight)
                    _closestGrabbable.GetComponent<Highlightable>().SetHighlighted(true);
            }
        }
    }

    public void GrabClosest()
    {
        if(_closestGrabbable != null && _closestGrabbable.isActiveAndEnabled)
        {
            characterMovement.SetState(CharacterState.Grabbing);
            grabDelegate?.Invoke();
            grabbedElement = _closestGrabbable;
            _closestGrabbable.grabStartedDelegate?.Invoke();
            grabbedElement.StartGrabbed();
            inRangeGrabbables.Remove(grabbedElement);
            _closestGrabbable.GetComponent<Highlightable>().SetHighlighted(false);
            _closestGrabbable = null;
        }
    }

    public void GrabTarget(Grabbable grabbable)
    {
        characterMovement.SetState(CharacterState.Grabbing);
        grabDelegate?.Invoke();
        grabbedElement = grabbable;
        grabbable.grabStartedDelegate?.Invoke();
        grabbedElement.StartGrabbed();
        inRangeGrabbables.Remove(grabbedElement);
        if(_closestGrabbable != null && inRangeHilight)
            _closestGrabbable.GetComponent<Highlightable>().SetHighlighted(false);
        _closestGrabbable = null;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BirdFlyMovement : MonoBehaviour
{
    public UnityEvent takeoffEvent;
    public float takeoffSpeed = 3;
    public float finalSpeed = 5;
    public Transform takeoffTarget;
    public Transform finalTarget;
    private Vector3 takeoffDirection;
    private Vector3 finalDirection;
    public float lerpDuration = 3;
    private float lerpTime = 0;
    private bool playing = false;

    private void Start()
    {
        takeoffDirection = (takeoffTarget.position - transform.position).normalized;
        finalDirection = (finalTarget.position - transform.position).normalized;
    }
    
    public void TakeOff()
    {
        if(!playing)
            takeoffEvent.Invoke();
        playing = true;
    }

    private void Update()
    {
        if(playing)
        {
            lerpTime += Time.deltaTime;
            float lerpFactor = lerpTime / lerpDuration;
            transform.position += Time.deltaTime * Vector3.Lerp(takeoffDirection * takeoffSpeed, finalDirection * finalSpeed, lerpFactor);
        }
    }
}

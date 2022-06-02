using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSound : MonoBehaviour
{
    public float stepDistance = 0.5f;
    private float currentDistance;
    private Vector3 previousPosition;
    public System.Action stepDelegate;

    void Start()
    {
        previousPosition = transform.position;
    }

    void Update()
    {
        currentDistance += Vector3.Distance(previousPosition, transform.position);
        previousPosition = transform.position;
        if(currentDistance > stepDistance)
        {
            currentDistance -= stepDistance;
            stepDelegate?.Invoke();
        }
    }
}

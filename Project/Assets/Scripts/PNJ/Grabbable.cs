using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    private new Rigidbody rigidbody;
    public System.Action grabStartedDelegate;
    public System.Action thrownDelegate;
    
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void OnThrow(Vector3 velocity)
    {
        rigidbody.velocity = velocity;
    }
}

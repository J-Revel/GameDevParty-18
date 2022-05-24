using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionFX : MonoBehaviour
{
    public Transform fxPrefab;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        Instantiate(fxPrefab, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
    }
}

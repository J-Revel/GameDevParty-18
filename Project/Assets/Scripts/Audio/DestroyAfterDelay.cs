using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    public float duration = 5;
    void Start()
    {
    }

    void Update()
    {
        duration -= Time.deltaTime;
        if(duration <= 0)
            Destroy(gameObject);
    }
}

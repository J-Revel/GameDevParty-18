using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNJDestroyer : MonoBehaviour
{
    public Transform destructionFX;

    void OnTriggerEnter(Collider other)
    {
        PNJProfile pnj = other.GetComponentInParent<PNJProfile>();
        if(pnj != null)
        {
            Transform destructionFXInstance = Instantiate(destructionFX, pnj.transform.position, Quaternion.LookRotation(transform.forward));
            StartCoroutine(FXCoroutine(pnj.gameObject, destructionFXInstance.gameObject));
        }
    }

    private IEnumerator FXCoroutine(GameObject pnj, GameObject destructionFX)
    {
        for(float time=0; time<2; time += Time.deltaTime)
        {
            pnj.transform.localScale = Vector3.one * (1 - time / 2);
            yield return null;
        }
        Destroy(pnj);
        Destroy(destructionFX);
    }
    
    void Update()
    {
        
    }
}

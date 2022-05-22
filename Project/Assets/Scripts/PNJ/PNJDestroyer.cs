using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PNJDestroyer : MonoBehaviour
{
    public Transform destructionFX;
    public CharacterState[] states;
    public UnityEvent destroyPNJEvent;
    public System.Action<PNJProfile> pnjDestroyDelegate;
    private List<GameObject> pendingDestroys = new List<GameObject>();

    void OnTriggerEnter(Collider other)
    {
        PNJProfile pnj = other.GetComponentInParent<PNJProfile>();
        if(pnj != null && !pendingDestroys.Contains(pnj.gameObject))
        {
            pendingDestroys.Add(pnj.gameObject);
            CharacterState state = pnj.GetComponent<CharacterMovement>().currentState;
            for(int i=0; i<states.Length; i++)
            {
                if(state == states[i])
                {
                    if(destructionFX != null)
                    {
                        Transform destructionFXInstance = Instantiate(destructionFX, pnj.transform.position, Quaternion.LookRotation(transform.forward));
                        StartCoroutine(FXCoroutine(pnj.gameObject, destructionFXInstance.gameObject));
                    }
                    else
                    {

                        StartCoroutine(FXCoroutine(pnj.gameObject, null));
                    }
                    destroyPNJEvent.Invoke();
                    pnjDestroyDelegate?.Invoke(pnj);
                    return;
                }
            }
        }
    }

    private IEnumerator FXCoroutine(GameObject pnj, GameObject destructionFX)
    {
        for(float time=0; time<2; time += Time.deltaTime)
        {
            pnj.transform.localScale = Vector3.one * (1 - time / 2);
            yield return null;
        }
        pendingDestroys.Remove(pnj);
        Destroy(pnj);
        if(destructionFX != null)
            Destroy(destructionFX);
    }
}

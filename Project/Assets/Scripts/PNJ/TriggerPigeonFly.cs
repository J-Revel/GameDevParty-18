using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPigeonFly : MonoBehaviour
{
    [SerializeField] private Animator PigeonFlying;

    [SerializeField] private string PigeonFly = "PigeonFly";
    public System.Action pigeonFlyDelegate;

    AudioSource audioData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PigeonFlying.SetInteger("PigeonFly_index", Random.Range(0, 2));
            PigeonFlying.SetTrigger("CharacterEnterCollider");
            pigeonFlyDelegate?.Invoke();

        }
    }
}

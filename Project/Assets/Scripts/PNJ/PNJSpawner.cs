using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNJSpawner : MonoBehaviour
{
    public GameObject pnjPrefab;
    public PNJConfig config;

    public void Start()
    {
        GameObject spawnedPnj = Instantiate(pnjPrefab, transform.position, pnjPrefab.transform.rotation);
        PNJProfile profile = spawnedPnj.AddComponent<PNJProfile>();
        profile.GenerateProfile(config);
    }
}
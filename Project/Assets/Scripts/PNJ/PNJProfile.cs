using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNJProfile : MonoBehaviour
{
    public float ecology;

    public void GenerateProfile(PNJConfig config)
    {
        this.ecology = Random.Range(config.minEcology, config.maxEcology);
    }
}

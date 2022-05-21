using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PNJState
{
    GoingToVote,
    Walking,
    Fleeing,
    Aggressive,
}

public class PNJProfile : MonoBehaviour
{

    public PNJState state;
    public bool isAlly;
    public void GenerateProfile(PNJConfig config)
    {
        this.ecology = Random.Range(config.minEcology, config.maxEcology);
    }
}

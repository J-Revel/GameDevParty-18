using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNJSpawner : MonoBehaviour
{
    public float leftWingProbability = 0.4f;
    public PNJProfile pnjPrefab;
    public SpriteAnimList[] animationSets;
    public int minInterestCount = 1;
    public int maxInterestCount = 5;
    public BoxCollider spawnBox;
    public BoxCollider targetBox;
    public float minSpawnInterval = -1;
    public float maxSpawnInterval = -1;

    public int initialSpawnCount = 0;

    IEnumerator Start()
    {
        for(int i=0; i<initialSpawnCount; i++)
        {
            Spawn();
        }
        if(minSpawnInterval > 0 && maxSpawnInterval > 0)
        {
            while(true)
            {
                yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));
                Spawn();
            }
        }
    }
    

    public void Spawn()
    {
        Vector3 spawnPoint = transform.position;
        spawnPoint.x = Random.Range(spawnBox.bounds.min.x, spawnBox.bounds.max.x);
        spawnPoint.z = Random.Range(spawnBox.bounds.min.z, spawnBox.bounds.max.z);
        PNJProfile pnj = Instantiate(pnjPrefab, spawnPoint, pnjPrefab.transform.rotation);
        List<QuestionTheme> availableThemes = new List<QuestionTheme>();
        pnj.favouriteThemes = new QuestionTheme[Random.Range(minInterestCount, maxInterestCount + 1)];
        for(int i=0; i<pnj.favouriteThemes.Length; i++)
        {
            foreach(QuestionTheme theme in System.Enum.GetValues(typeof(QuestionTheme)))
            {
                availableThemes.Add(theme);
            }
            int index = Random.Range(0, availableThemes.Count);
            pnj.favouriteThemes[i] = availableThemes[index];
            availableThemes.RemoveAt(index);
        }
        pnj.leftWing = Random.Range(0, 1.0f) < leftWingProbability;
        pnj.GetComponentInChildren<AnimatedSprite>().animList = animationSets[Random.Range(0, animationSets.Length)];
        pnj.GetComponent<CharacterMovement>().movementInput = transform.forward;
        
        if(targetBox != null)
            pnj.GetComponent<RandomPNJMovement>().initialTarget = new Vector3(Random.Range(targetBox.bounds.min.x, targetBox.bounds.max.x), 0, Random.Range(targetBox.bounds.min.z, targetBox.bounds.max.z));
        else pnj.GetComponent<RandomPNJMovement>().initialTarget = pnj.transform.position;
    }
    
    void Update()
    {
        
    }
}

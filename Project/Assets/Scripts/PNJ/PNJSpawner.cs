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
    public BoxCollider targetBox;

    void Start()
    {
        PNJProfile pnj = Instantiate(pnjPrefab, transform.position, pnjPrefab.transform.rotation);
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
        
        pnj.GetComponent<RandomPNJMovement>().initialTarget = new Vector3(Random.Range(targetBox.bounds.min.x, targetBox.bounds.max.x), 0, Random.Range(targetBox.bounds.min.z, targetBox.bounds.max.z));
    }
    
    void Update()
    {
        
    }
}

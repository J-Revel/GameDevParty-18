using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class PNJSpawner : MonoBehaviour
{
    public float leftWingProbability = 0.4f;
    public PNJConfig config;
    public PNJProfile pnjPrefab;
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
        if(pnj.canVote)
            pnj.procurationCount = Mathf.FloorToInt(4 - Mathf.Sqrt(Random.Range(0, 16)));
        else pnj.procurationCount = 0;
        StringBuilder idCardContent = new StringBuilder();
        pnj.genre = Random.Range(0, 1.0f) > 0.5f ? Genre.Male : Genre.Female;
        if(config.pnjDisplayProfileMen.Length == 0)
            pnj.genre = Genre.Female;
        if(config.pnjDisplayProfileWomen.Length == 0)
            pnj.genre = Genre.Male;
            
        PNJDisplayProfile[] displayProfileList = pnj.genre == Genre.Male ? config.pnjDisplayProfileMen : config.pnjDisplayProfileWomen;
        
        PNJDisplayProfile displayProfile = displayProfileList[Random.Range(0, displayProfileList.Length)];
        pnj.dialogueSprite = displayProfile.dialogueSprite;
        pnj.GetComponentInChildren<AnimatedSprite>().animList = displayProfile.animations;
        string[] firstNameList = pnj.genre == Genre.Male ? config.menFirstNames : config.womenFirstNames;
        idCardContent.Append(firstNameList[Random.Range(0, config.menFirstNames.Length)]);

        idCardContent.Append(" ").Append(config.secondNames[Random.Range(0, config.secondNames.Length)]).Append("\n");
        List<IdCardCategory> presentCategories = new List<IdCardCategory>();
        for(int i=0; i<3; i++)
        {
            IdCardInfoConfig info = config.info[Random.Range(0, config.info.Length)];
            if(presentCategories.Contains(info.category))
                continue;
            else presentCategories.Add(info.category);
            if(info.isInvalid)
                pnj.canVote = false;
            idCardContent.Append(info.text).Append("\n");
        }
        pnj.idCard = idCardContent.ToString();
        
        pnj.GetComponent<CharacterMovement>().movementInput = transform.forward;

        RandomPNJMovement pnjMovement = pnj.GetComponent<RandomPNJMovement>();
        if(pnjMovement != null)
        {
            if(targetBox != null)
                pnjMovement.initialTarget = new Vector3(Random.Range(targetBox.bounds.min.x, targetBox.bounds.max.x), 0, Random.Range(targetBox.bounds.min.z, targetBox.bounds.max.z));
            else pnjMovement.initialTarget = pnj.transform.position;
        }
        
    }
    
    void Update()
    {
        
    }
}

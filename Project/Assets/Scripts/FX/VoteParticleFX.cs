using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteParticleFX : MonoBehaviour
{
    public Rigidbody particlePrefab;
    private List<Rigidbody> particlePool = new List<Rigidbody>();
    public float spawnInterval = 0.05f;
    public float spawnDuration = 1;
    public int burstCount = 30;
    public BoxCollider spawnZone;
    public float minSpeed = 3;
    public float maxSpeed = 10;
    public float shrinkDuration = 2;
    

    IEnumerator Start()
    {
        
        for(int i=0; i<burstCount; i++)
        {
            particlePool.Add(Instantiate(particlePrefab, transform.position, particlePrefab.rotation));
            particlePool[particlePool.Count - 1].gameObject.SetActive(false);
        }
        while(true)
        {
            
            yield return new WaitForSeconds(spawnInterval);
            for(int i=0; i<burstCount; i++)
            {
                Vector3 spawnPosition = new Vector3(
                    Random.Range(spawnZone.bounds.min.x, spawnZone.bounds.max.x),
                    Random.Range(spawnZone.bounds.min.y, spawnZone.bounds.max.y),
                    Random.Range(spawnZone.bounds.min.z, spawnZone.bounds.max.z)
                );
                particlePool[i].transform.localScale = Vector3.one;
                particlePool[i].transform.position = spawnPosition;
                particlePool[i].velocity = transform.forward * Random.Range(minSpeed, maxSpeed);
                particlePool[i].gameObject.SetActive(true);
            }
            for(float time = 0; time < shrinkDuration; time += Time.deltaTime)
            {
                float ratio = time / shrinkDuration;
                for(int i=0; i<burstCount; i++)
                    particlePool[i].transform.localScale = Vector3.one * (1 - ratio * ratio);
                yield return null;
            }
            for(int i=0; i<burstCount; i++)
                particlePool[i].gameObject.SetActive(false);
        }
    }

    void Update()
    {
        
    }
}

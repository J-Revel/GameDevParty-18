using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPNJMovement : MonoBehaviour
{
    public CharacterMovement movement;
    public float walkMinDuration = 0.5f;
    public float walkMaxDuration = 2f;
    public float stopMinDuration = 1;
    public float stopMaxDuration = 4;

    IEnumerator Start()
    {
        movement = GetComponent<CharacterMovement>();
        while(true)
            yield return WalkCoroutine();
    }
    
    private IEnumerator WalkCoroutine()
    {
        Vector2 direction = Random.insideUnitCircle;
        float duration = Random.Range(walkMinDuration, walkMaxDuration);
        movement.movementInput = new Vector3(direction.x, 0, direction.y);
        yield return new WaitForSeconds(duration);
        movement.movementInput = Vector3.zero;
        yield return new WaitForSeconds(Random.Range(stopMinDuration, stopMaxDuration));
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private CharacterMovement characterMovement;
    private GrabHandler grabHandler;
    public float grabOffset = 0.2f;
    
    public System.Action grabDelegate;
    public Grabbable target;
    public float grabRange = 0.5f;
    public float throwDelay = 2;
    private float throwTime = 0;
    private Vector3 throwDirection;

    private void Start()
    {
        characterMovement = GetComponent<CharacterMovement>();
        grabHandler = GetComponent<GrabHandler>();
        target = PlayerMovementInput.instance.GetComponent<Grabbable>();
    }

    public void Update()
    {
        if(grabHandler.grabbedElement == null)
        {
            if(!target.isInvincible)
            {
                Vector3 input = target.transform.position - transform.position;
                input.y = 0;
                characterMovement.movementInput = input.normalized;
                Debug.Log(Vector3.Magnitude(target.transform.position - transform.position));
                if(Vector3.SqrMagnitude(target.transform.position - transform.position) < grabRange * grabRange)
                {
                    grabHandler.GrabTarget(target.GetComponent<Grabbable>());
                    throwTime = 0;
                    float throwAngle = Random.Range(0, Mathf.PI * 2);
                    throwDirection = new Vector3(Mathf.Cos(throwAngle), 0, Mathf.Sin(throwAngle));
                }
            }
            else
            {
                characterMovement.movementInput = Vector3.zero;
            }
        }
        else
        {
            characterMovement.movementInput = throwDirection;
            throwTime += Time.deltaTime;
            if(throwTime >= throwDelay)
            {
                grabHandler.Throw();
            }
        }
    }
}

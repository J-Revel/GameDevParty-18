using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementInput : MonoBehaviour
{
    private CharacterMovement characterMovement;

    private void Start()
    {
        characterMovement = GetComponent<CharacterMovement>();
    }

    public void Update()
    {
        characterMovement.movementInput.x = Input.GetAxis("Horizontal");
        characterMovement.movementInput.z = Input.GetAxis("Vertical");
    }
}

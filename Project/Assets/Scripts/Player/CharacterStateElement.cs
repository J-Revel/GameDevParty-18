using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateElement : MonoBehaviour
{
    private CharacterMovement movement;
    public CharacterState[] states;
    
    void Start()
    {
        movement = GetComponentInParent<CharacterMovement>();
        UpdateDisplay();
        movement.stateChangedDelegate += (state) => UpdateDisplay();
    }

    // Update is called once per frame
    void UpdateDisplay()
    {
        for(int i=0; i<states.Length; i++)
        {
            if(states[i] == movement.currentState)
            {
                gameObject.SetActive(true);
                return;
            }
        }
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeSelector : MonoBehaviour
{
    public RectTransform cursor;
    public float oscillationDuration = 1;
    private float animTime = 0;
    public bool playing = true;

    void Start()
    {
        
    }

    void Update()
    {
        if(playing)
        {
            animTime += Time.deltaTime;
            float animRatio = 1 - Mathf.Abs(((animTime % oscillationDuration) / oscillationDuration) * 2 - 1);

            cursor.anchorMax = new Vector2(animRatio, 1);
            cursor.anchorMin = new Vector2(animRatio, 0);
            if(Input.GetButtonDown("Talk"))
            {
                playing = false;
            }
        }
    }

    // public IEnumerator AskQuestionCoroutine()
    // {
        
    // }
}

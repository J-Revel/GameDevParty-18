using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Foldable))]
public class ScaleFoldAnim : MonoBehaviour
{
    private RectTransform rectTransform;
    private Foldable foldable;

    public float[] bubbleAnimAppearScales = new float[]{0.3f, 0.8f, 1.1f, 0.9f, 1};
    public float[] bubbleAnimDisappearScales = new float[]{0.9f, 1.1f, 0.8f, 0.3f, 0};

    private float[] currentAnimAppearScales;
    private bool reversePlay = false;
    public float animFPS = 20;
    

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        foldable = GetComponent<Foldable>();
        foldable.foldStateChangedDelegate += OnFoldStateChanged;
        currentAnimAppearScales = bubbleAnimAppearScales;
    }

    void Update()
    {
        float ratio = foldable.animRatio;
        int appearIndex = Mathf.FloorToInt(ratio * (currentAnimAppearScales.Length + 1));
        if(reversePlay)
            appearIndex = currentAnimAppearScales.Length - appearIndex;
        appearIndex = Mathf.Min(appearIndex, currentAnimAppearScales.Length - 1);
        Vector3 scale = Vector3.one * currentAnimAppearScales[appearIndex];
        transform.localScale = scale;
    }

    private void OnFoldStateChanged(bool foldState)
    {
        if(foldState)
        {
            foldable.SetAnimDuration((float)bubbleAnimAppearScales.Length / animFPS);
            currentAnimAppearScales = bubbleAnimAppearScales;
            reversePlay = false;
        }
        else
        {
            foldable.SetAnimDuration((float)bubbleAnimDisappearScales.Length / animFPS);
            currentAnimAppearScales = bubbleAnimDisappearScales;
            reversePlay = true;
        }
    }
}

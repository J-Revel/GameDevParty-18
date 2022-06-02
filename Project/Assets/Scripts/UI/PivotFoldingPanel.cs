using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Foldable))]
public class PivotFoldingPanel : MonoBehaviour
{
    private Foldable foldable;
    private RectTransform rectTransform;
    public Vector2 offPivot;
    public Vector2 onPivot;


    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        foldable = GetComponent<Foldable>();
    }

    void Update()
    {
        rectTransform.pivot = Vector2.Lerp(offPivot, onPivot, foldable.animRatio);
    }
}

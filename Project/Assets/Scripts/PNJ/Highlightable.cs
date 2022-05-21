using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlightable : MonoBehaviour
{
    public new Renderer renderer;
    private MaterialPropertyBlock propertyBlock;
    [ColorUsageAttribute(true,true)]
    public Color highlightColor;
    public string hilightColorShaderParam = "_HighlightColor";
    
    public void SetHighlighted(bool highlighted)
    {
        renderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(hilightColorShaderParam, highlighted ? highlightColor : Color.white);
        renderer.SetPropertyBlock(propertyBlock);
    }

    void Start()
    {
        propertyBlock = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(hilightColorShaderParam, Color.white);
        renderer.SetPropertyBlock(propertyBlock);
    }
}

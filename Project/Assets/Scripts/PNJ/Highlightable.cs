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
    public GameObject[] hilightElements;
    
    public void SetHighlighted(bool highlighted, bool showInput)
    {
        renderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(hilightColorShaderParam, highlighted ? highlightColor : Color.white);
        renderer.SetPropertyBlock(propertyBlock);
        foreach(GameObject hilightElement in hilightElements)
        {
            hilightElement.SetActive(showInput);
        }
    }

    void Start()
    {
        propertyBlock = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(hilightColorShaderParam, Color.white);
        renderer.SetPropertyBlock(propertyBlock);
        foreach(GameObject hilightElement in hilightElements)
        {
            hilightElement.SetActive(false);
        }
    }
}

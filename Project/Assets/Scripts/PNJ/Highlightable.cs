using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlightable : MonoBehaviour
{
    public new Renderer renderer;
    private MaterialPropertyBlock propertyBlock;
    private Vector3 startPosition;

    [ColorUsageAttribute(true,true)]
    public Color highlightColor;
    public string hilightColorShaderParam = "_HighlightColor";
    public GameObject[] hilightElements;
    public Color tint = Color.white;
    private bool highlighted = false;
    public float shakeIntensity = 0;
    
    public void SetHighlighted(bool highlighted, bool showInput)
    {
        this.highlighted = highlighted;
        renderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(hilightColorShaderParam, (highlighted ? highlightColor : Color.white) * tint);
        renderer.SetPropertyBlock(propertyBlock);
        foreach(GameObject hilightElement in hilightElements)
        {
            hilightElement.SetActive(showInput);
        }
    }

    private void Update()
    {
        renderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(hilightColorShaderParam, (highlighted ? highlightColor : Color.white) * tint);
        renderer.SetPropertyBlock(propertyBlock);
        renderer.transform.localPosition = startPosition + new Vector3(Random.Range(-shakeIntensity, shakeIntensity), Random.Range(-shakeIntensity, shakeIntensity), Random.Range(-shakeIntensity, shakeIntensity));
    }

    void Start()
    {
        startPosition = renderer.transform.localPosition;
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

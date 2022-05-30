using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatedSpriteBoundSetter : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public float angle = 45;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        CalculateBounds();
    }

    void Update()
    {
        CalculateBounds();
    }

    void CalculateBounds()
    {
        Bounds spriteBounds = spriteRenderer.sprite.bounds;
        Bounds newBounds = GeometryUtility.CalculateBounds(new Vector3[] {spriteBounds.min, spriteBounds.max}, Matrix4x4.Rotate(Quaternion.AngleAxis(angle, Vector3.right)));
        spriteRenderer.localBounds = newBounds;
    }
}

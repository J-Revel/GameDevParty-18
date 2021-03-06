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
        // spriteRenderer.allowOcclusionWhenDynamic = false;
        CalculateBounds();
    }

    void Update()
    {
        CalculateBounds();
        DrawBounds(spriteRenderer.bounds);
    }

    void CalculateBounds()
    {
        Bounds spriteBounds = spriteRenderer.sprite.bounds;
        Vector3 offsetMin = spriteBounds.min;
        Vector3 offsetMax = spriteBounds.max;
        Bounds newBounds = GeometryUtility.CalculateBounds(new Vector3[] {offsetMin, offsetMax}, Matrix4x4.Rotate(Quaternion.AngleAxis(angle, Vector3.right)));
        newBounds.center += Vector3.forward * transform.position.y / 2;
        spriteRenderer.localBounds = newBounds;
    }

    void DrawBounds(Bounds b, float delay=0)
    {
        // bottom
        var p1 = new Vector3(b.min.x, b.min.y, b.min.z);
        var p2 = new Vector3(b.max.x, b.min.y, b.min.z);
        var p3 = new Vector3(b.max.x, b.min.y, b.max.z);
        var p4 = new Vector3(b.min.x, b.min.y, b.max.z);

        Debug.DrawLine(p1, p2, Color.blue, delay);
        Debug.DrawLine(p2, p3, Color.red, delay);
        Debug.DrawLine(p3, p4, Color.yellow, delay);
        Debug.DrawLine(p4, p1, Color.magenta, delay);

        // top
        var p5 = new Vector3(b.min.x, b.max.y, b.min.z);
        var p6 = new Vector3(b.max.x, b.max.y, b.min.z);
        var p7 = new Vector3(b.max.x, b.max.y, b.max.z);
        var p8 = new Vector3(b.min.x, b.max.y, b.max.z);

        Debug.DrawLine(p5, p6, Color.blue, delay);
        Debug.DrawLine(p6, p7, Color.red, delay);
        Debug.DrawLine(p7, p8, Color.yellow, delay);
        Debug.DrawLine(p8, p5, Color.magenta, delay);

        // sides
        Debug.DrawLine(p1, p5, Color.white, delay);
        Debug.DrawLine(p2, p6, Color.gray, delay);
        Debug.DrawLine(p3, p7, Color.green, delay);
        Debug.DrawLine(p4, p8, Color.cyan, delay);
    }
}

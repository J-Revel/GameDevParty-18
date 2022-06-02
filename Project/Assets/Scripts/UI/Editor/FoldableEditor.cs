using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Foldable))]
[CanEditMultipleObjects]
public class LookAtPointEditor : Editor 
{
    SerializedProperty lookAtPoint;
    
    void OnEnable()
    {
        lookAtPoint = serializedObject.FindProperty("lookAtPoint");
    }

    public override void OnInspectorGUI()
    {
        Foldable foldable = target as Foldable;
        if(foldable.isOn != EditorGUILayout.Toggle("isOn", foldable.isOn))
        {
            foldable.isOn = !foldable.isOn;
        }
    }
}
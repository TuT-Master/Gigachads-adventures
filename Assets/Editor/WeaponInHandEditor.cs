using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WeaponInHand))]
public class WeaponInHandEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        WeaponInHand weapon = (WeaponInHand)target;
        if (GUILayout.Button("Apply Model Animations"))
            weapon.ApplyModelAnimations();
    }
}

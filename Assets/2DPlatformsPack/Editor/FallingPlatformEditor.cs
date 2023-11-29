using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FallingPlatformGenerator))]
public class FallingPlatformEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FallingPlatformGenerator myScript = (FallingPlatformGenerator)target;

        if (GUILayout.Button("Generate Platform"))
        {
            ClearAndGenerateSquares(myScript);
        }
    }

    private void ClearAndGenerateSquares(FallingPlatformGenerator generator)
    {
        ClearChildren(generator.transform);
        generator.GenerateSquares(generator.blockCount);
    }

    private void ClearChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(parent.GetChild(i).gameObject);
        }
    }
}

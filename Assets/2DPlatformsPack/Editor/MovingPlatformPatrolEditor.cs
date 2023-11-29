using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MovingPlatform_Patrol))]
public class MovingPlatformPatrolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MovingPlatform_Patrol myScript = (MovingPlatform_Patrol)target;

        if (GUILayout.Button("Add Points"))
        {
            myScript.AddPoints();
        }

        if (GUILayout.Button("Delete Points"))
        {
            myScript.DeletePoint();
        }
    }
}

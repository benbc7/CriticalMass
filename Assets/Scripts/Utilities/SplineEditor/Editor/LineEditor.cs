using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (Line))]
public class LineEditor : Editor {

    private void OnSceneGUI () {
        Line line = target as Line;
        Transform handleTransform = line.transform;
        Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;
        Vector3 point1 = handleTransform.TransformPoint (line.point1);
        Vector3 point2 = handleTransform.TransformPoint (line.point2);

        Handles.color = Color.white;
        Handles.DrawLine (point1, point2);

        EditorGUI.BeginChangeCheck ();
        point1 = Handles.DoPositionHandle (point1, handleRotation);
        if (EditorGUI.EndChangeCheck ()) {
            Undo.RecordObject (line, "Move Point");
            EditorUtility.SetDirty (line);
            line.point1 = handleTransform.InverseTransformPoint (point1);
        }
        EditorGUI.BeginChangeCheck ();
        point2 = Handles.DoPositionHandle (point2, handleRotation);
        if (EditorGUI.EndChangeCheck ()) {
            Undo.RecordObject (line, "Move Point");
            EditorUtility.SetDirty (line);
            line.point2 = handleTransform.InverseTransformPoint (point2);
        }
    }
}

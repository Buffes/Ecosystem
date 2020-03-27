using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (Ecosystem.Attributes.Animal))]
public class FieldOfView : Editor
{
    private void OnSceneGUI() {
        Ecosystem.Attributes.Animal animal = (Ecosystem.Attributes.Animal)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(animal.transform.position, Vector3.up, Vector3.forward, 360, animal.viewRadius);
        Vector3 viewAngleA = animal.DirFromAngle(-animal.viewAngle / 2, false);
        Vector3 viewAngleB = animal.DirFromAngle(animal.viewAngle / 2, false);

        Handles.DrawLine(animal.transform.position, animal.transform.position + viewAngleA * animal.viewRadius);
        Handles.DrawLine(animal.transform.position, animal.transform.position + viewAngleB * animal.viewRadius);

        Handles.color = Color.red;
        foreach (Transform visibleTarget in animal.visibleTargets) {
            Handles.DrawLine(animal.transform.position, visibleTarget.position);
        }

    }
}

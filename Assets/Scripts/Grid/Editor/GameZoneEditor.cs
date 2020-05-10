using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Ecosystem.Grid
{
    /// <summary>
    /// This only exists because the noise seed field should get disabled if random noise seed is selected.
    /// </summary>
    [CustomEditor(typeof(GameZone))]
    public class GameZoneEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GameZone gameZone = (GameZone)target;
            DrawDefaultInspector();
            
            gameZone.WaterThresholdIndex = EditorGUILayout.Popup("Water threshold", gameZone.WaterThresholdIndex, gameZone.GetRegionNames());
            EditorGUILayout.LabelField("Noise Map Seed", EditorStyles.boldLabel);
            gameZone.RandomNoiseSeed = EditorGUILayout.Toggle("Random Seed", gameZone.RandomNoiseSeed);
            
            using (new EditorGUI.DisabledScope(gameZone.RandomNoiseSeed))
            {
                gameZone.NoiseSeed = EditorGUILayout.IntField("Seed", gameZone.NoiseSeed);
            }

            if (GUI.changed)
            {
                // To update the variables after changing in the GUI.
                EditorUtility.SetDirty(gameZone);
                EditorSceneManager.MarkSceneDirty(gameZone.gameObject.scene);
            }
        }
    }
}
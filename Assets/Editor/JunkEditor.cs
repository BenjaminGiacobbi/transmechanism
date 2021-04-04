using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(JunkGenerator))]
public class JunkEditor : Editor
{
    public override void OnInspectorGUI()
    {
        JunkGenerator generator = (JunkGenerator)target;

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Spawn Object"))
        {
            Debug.Log("Change Color");
            generator.Spawn();
        }
        if (GUILayout.Button("Save Spawns"))
        {
            Debug.Log("Saving Objects to json file");
            string path = EditorUtility.SaveFilePanel("Select JSON File", "", "spawns.json", "json");
            if (path != "")
                generator.SaveToFile(path);
        }
        GUILayout.EndHorizontal();
        

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("AutoSpawn"))
        {
            Debug.Log("You Fucken moron");
            generator.AutoSpawn();
        }
        if (GUILayout.Button("RecreateSpawns"))
        {
            string path = EditorUtility.OpenFilePanel("Select JSON File", "", "json");
            if (path != "")
                generator.RecreateSpawns(path);
        }
        GUILayout.EndHorizontal();
        DrawDefaultInspector();
    }
}
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MonsterSpawnManager))]
public class SpawnEditer : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MonsterSpawnManager generator = (MonsterSpawnManager)target;

        if (GUILayout.Button("Generate"))
        {
            generator.InitializeBiomeMonsters();
        }

        if (GUILayout.Button("Clear"))
        {
            generator.ClearSpawnedMonsters();
        }

    }
}

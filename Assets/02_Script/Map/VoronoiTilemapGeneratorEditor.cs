using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VoronoiTilemapGenerator))]
public class VoronoiTilemapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        VoronoiTilemapGenerator generator = (VoronoiTilemapGenerator)target;

        if (GUILayout.Button("Generate"))
        {
            generator.GenerateVoronoiMap();
        }

        if (GUILayout.Button("Clear"))
        {
            generator.Clear();
        }
    }
}

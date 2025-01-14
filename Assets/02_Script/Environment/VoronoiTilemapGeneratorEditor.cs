using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VoronoiMapGenerator))]
public class VoronoiTilemapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        VoronoiMapGenerator generator = (VoronoiMapGenerator)target;

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

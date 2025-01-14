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
            generator.Generate();
        }

        if (GUILayout.Button("Clear"))
        {
            generator.Clear();
        }
    }
}

using UnityEngine;

[CreateAssetMenu(menuName = "Environment/Object/NatureResourceData")]
public class NatureResourceData : ScriptableObject
{
    [SerializeField] GameObject prefab;
    [SerializeField] BiomeType biomeType;
    [SerializeField] ObjectType objectType;
    [SerializeField] int width;
    [SerializeField] int height;

    public GameObject Prefab { get { return prefab; } }
    public BiomeType BiomeType { get { return biomeType; } }
    public ObjectType ObjectType { get { return objectType; } }
    public int Width { get { return width; } }
    public int Height { get { return height; } }
}

using UnityEngine;

public enum ObjectType
{
    Empty,
    Unknown,
    Tree
}

[CreateAssetMenu(menuName = "Environment/Object/RenewableResourceData")]
public class RenewableResourceData : ScriptableObject
{
    [SerializeField] GameObject prefab;
    [SerializeField] BiomeType biomeType;
    [SerializeField] ObjectType objectType;
    [SerializeField] int width;
    [SerializeField] int height;

    public GameObject Prefab { get { return prefab; } }
    public BiomeType BiomeType {  get {  return biomeType; } }
    public int Width { get { return width; } }
    public int Height { get { return height; } }
}

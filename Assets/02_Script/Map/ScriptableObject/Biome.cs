using UnityEngine;
using UnityEngine.Tilemaps;

public enum BiomeType
{
    None,
    Grass,
    Desert,
    Pollution,
    Water
}

[CreateAssetMenu(menuName = "Map/Biome")]
public class Biome : ScriptableObject
{
    [SerializeField] TileBase tile;
    [SerializeField] BiomeType biomeType;

    public TileBase Tile {  get { return tile; } }
    public BiomeType BiomeType { get { return biomeType; } }
}

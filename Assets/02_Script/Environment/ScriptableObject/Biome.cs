using System.Collections.Generic;
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

[CreateAssetMenu(menuName = "Environment/Map/Biome")]
public class Biome : ScriptableObject
{
    [SerializeField] TileBase tile;
    [SerializeField] BiomeType biomeType;
    [SerializeField] List<RenewableResourceData> trees;
    [SerializeField]
    [Range(0f, 1f)] float treesIntensity;

    public TileBase Tile {  get { return tile; } }
    public BiomeType BiomeType { get { return biomeType; } }
    public List<RenewableResourceData> Trees { get { return trees; } }
    public float TreesIntensity { get {  return treesIntensity; } }
}

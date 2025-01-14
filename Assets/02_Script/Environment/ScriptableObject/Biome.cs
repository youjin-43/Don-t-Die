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
    [SerializeField] List<RenewableResourceData> plants;
    [SerializeField]
    [Range(0f, 1f)] float plantsIntensity;
    [SerializeField] List<RenewableResourceData> minerals;
    [SerializeField]
    [Range(0f, 1f)] float mineralsIntensity;

    public TileBase Tile {  get { return tile; } }
    public BiomeType BiomeType { get { return biomeType; } }
    public List<RenewableResourceData> Trees { get { return trees; } }
    public float TreesIntensity { get {  return treesIntensity; } }
    public List<RenewableResourceData> Plants { get {  return plants; } }
    public float PlantsIntensity { get { return plantsIntensity; } }
    public List<RenewableResourceData> Minerals { get { return minerals; } }
    public float MineralsIntensity { get {  return mineralsIntensity;} }
}

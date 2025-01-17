using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum BiomeType
{
    None,
    GrassBiome,
    DesertBiome,
    PollutionBiome,
    WaterBiome
}

[Serializable]
[CreateAssetMenu(menuName = "Environment/Map/Biome")]
public class Biome : ScriptableObject
{
    [SerializeField] TileBase tile;
    [SerializeField] BiomeType biomeType;
    [SerializeField] List<NatureResourceData> trees;
    [SerializeField]
    [Range(0f, 1f)] float treesIntensity;
    [SerializeField] List<NatureResourceData> plants;
    [SerializeField]
    [Range(0f, 1f)] float plantsIntensity;
    [SerializeField] List<NatureResourceData> minerals;
    [SerializeField]
    [Range(0f, 1f)] float mineralsIntensity;
    Dictionary<ObjectType, List<NatureResourceData>> natureResources;
    Dictionary<ObjectType, float> intensities;

    public TileBase Tile { get { return tile; } }
    public BiomeType BiomeType { get { return biomeType; } }
    public List<NatureResourceData> Trees { get { return trees; } }
    public float TreesIntensity { get { return treesIntensity; } }
    public List<NatureResourceData> Plants { get { return plants; } }
    public float PlantsIntensity { get { return plantsIntensity; } }
    public List<NatureResourceData> Minerals { get { return minerals; } }
    public float MineralsIntensity { get { return mineralsIntensity; } }
    public Dictionary<ObjectType, List<NatureResourceData>> NatureResources 
    {
        get
        {
            if (natureResources == null)
            {
                natureResources = new Dictionary<ObjectType, List<NatureResourceData>> {
                { ObjectType.Tree, trees },
                {ObjectType.Plant, plants },
                {ObjectType.Mineral, minerals }
            };

                natureResources[ObjectType.Tree].AddRange(trees);
                natureResources[ObjectType.Plant].AddRange(plants);
                natureResources[ObjectType.Mineral].AddRange(minerals);
            }

            return natureResources;
        }
    }
    public Dictionary<ObjectType, float> Intensities
    {
        get
        {
            if (intensities == null)
            {
                intensities = new Dictionary<ObjectType, float> {
                    {ObjectType.Tree, treesIntensity },
                    {ObjectType.Plant, plantsIntensity },
                    {ObjectType.Mineral, mineralsIntensity }
                };
            }
            return intensities;
        }
    }
}

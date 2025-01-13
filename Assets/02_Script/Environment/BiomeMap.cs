using System;
using UnityEngine;

public class BiomeMap
{
    public int width;
    public int height;

    Biome[,] map;

    public BiomeMap(int width, int height)
    {
        this.width = width;
        this.height = height;

        map = new Biome[height, width];
    }

    public void MarkTile(int x, int y, Biome biome)
    {
        map[y, x] = biome;
    }

    public Biome[,] GetMap() { return map; }

    public Biome GetTileBiome(int x, int y)
    {
        return map[y, x];
    }

    public bool IsValidPosition(Vector2Int pos, int width, int height, BiomeType type)
    {
        for (int i = pos.y - height + 1; i <= pos.y; i++)
        {
            for (int j = pos.x - width + 1; j <= pos.x; j++)
            {
                if (map[j, i].BiomeType != type) return false;
            }
        }
        return true;
    }
}

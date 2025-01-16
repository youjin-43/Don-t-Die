using System;
using UnityEngine;

public class BiomeMap
{
    public int width;
    public int height;

    Biome[,] map;

    public Biome[,] Map {  get { return map; } }

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

    /// <summary>
    /// 해당 position에 위치한 타일이 어떤 바이옴인지 반환한다
    /// </summary>
    /// <param name="pos">transform의 position 좌표.</param>
    /// <returns></returns>
    public Biome GetTileBiome(Vector2Int pos)
    {
        return map[pos.y, pos.x];
    }

    public bool IsValidPosition(Vector2Int pos, int width, int height, BiomeType type)
    {
        for (int i = pos.y - height + 1; i <= pos.y; i++)
        {
            for (int j = pos.x - width + 1; j <= pos.x; j++)
            {
                if (map[i, j].BiomeType != type) return false;
            }
        }
        return true;
    }
}

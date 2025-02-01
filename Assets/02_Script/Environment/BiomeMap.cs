using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BiomeMap
{
    public int width;
    public int height;

    public List<List<BiomeType>> map;

    public BiomeMap(int width, int height)
    {
        this.width = width;
        this.height = height;

        map = new List<List<BiomeType>>();

        for (int y = 0; y < height; y++)
        {
            List<BiomeType> row = new List<BiomeType>();
            for (int x = 0; x < width; x++)
            {
                row.Add(BiomeType.None);
            }
            map.Add(row);
        }
    }

    public void MarkTile(int x, int y, BiomeType biome)
    {
        map[y][x] = biome;
    }

    /// <summary>
    /// 해당 position에 위치한 타일이 어떤 바이옴인지 반환한다
    /// </summary>
    /// <param name="pos">transform의 position 좌표.</param>
    /// <returns></returns>
    public BiomeType GetTileBiome(Vector2Int pos)
    {
        if (pos.y < 0 || pos.y >= map.Count) return BiomeType.None; // y 좌표가 유효한지 확인
        if (pos.x < 0 || pos.x >= map[pos.y].Count) return BiomeType.None; // x 좌표가 유효한지 확인

        return map[pos.y][pos.x];
    }

    public bool IsValidPosition(Vector2Int pos, int width, int height, BiomeType type)
    {
        for (int i = pos.y - height + 1; i <= pos.y; i++)
        {
            for (int j = pos.x - width + 1; j <= pos.x; j++)
            {
                if (map[i][j] != type) return false;
            }
        }
        return true;
    }

    public bool IsOnMap(Vector2Int pos, int width, int height)
    {
        for (int i = pos.y - height + 1; i <= pos.y; i++)
        {
            for (int j = pos.x - width + 1; j <= pos.x; j++)
            {
                if (map[i][j] == BiomeType.WaterBiome) return false;
            }
        }
        return true;
    }
}

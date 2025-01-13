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

        map = new Biome[width, height];
    }

    public void MarkTile(int x, int y, Biome biome)
    {
        map[x, y] = biome;
    }
}

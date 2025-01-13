using UnityEngine;

public class ObjectMap
{
    public int width;
    public int height;

    ObjectType[,] map;

    public ObjectMap(int width, int height)
    {
        this.width = width;
        this.height = height;

        map = new ObjectType[height, width];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                map[i, j] = ObjectType.Empty;
            }
        }
    }

    public bool IsTileEmpty(Vector2Int pos)
    {
        return map[pos.y, pos.x] == ObjectType.Empty;
    }

    public bool AreTilesEmpty(Vector2Int pos, int width, int height)
    {
        for (int i = pos.y - height + 1; i <= pos.y; i++)
        {
            for (int j = pos.x - width + 1; j <= pos.x; j++)
            {
                if (!IsTileEmpty(new Vector2Int(j, i))) return false;
            }
        }
        return true;
    }

    public void MarkTiles(Vector2Int pos, int width, int height, ObjectType type)
    {
        for (int i = pos.y - height + 1; i <= pos.y; i++)
        {
            for (int j = pos.x - width + 1; j <= pos.x; j++)
            {
                map[i, j] = type;
            }
        }
    }
}

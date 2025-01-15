using UnityEngine;

public class Item : MonoBehaviour
{
    SpriteRenderer   _spriteRenderer;
    CircleCollider2D _circleCollider;

    void Start()
    {
        AdjustCollider();
    }

    void Update()
    {
        
    }

    private void AdjustCollider()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();

        if (spriteRenderer != null && circleCollider != null)
        {
            Sprite sprite = spriteRenderer.sprite;
            Texture2D texture = sprite.texture;

            Rect spriteRect = sprite.textureRect;
            int xMin   = Mathf.FloorToInt(spriteRect.x);
            int yMin   = Mathf.FloorToInt(spriteRect.y);
            int width  = Mathf.FloorToInt(spriteRect.width);
            int height = Mathf.FloorToInt(spriteRect.height);

            float maxRadius = CalculateEffectiveRadius(texture, xMin, yMin, width, height);

            circleCollider.radius = maxRadius;
        }
    }

    private float CalculateEffectiveRadius(Texture2D texture, int xMin, int yMin, int width, int height)
    {
        float maxRadius = 0f;
        Vector2 center = new Vector2(width / 2f, height / 2f);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color pixel = texture.GetPixel(xMin + x, yMin + y);

                if (pixel.a > 0)
                {
                    float distance = Vector2.Distance(center, new Vector2(x, y));
                    maxRadius = Mathf.Max(maxRadius, distance);
                }
            }
        }

        maxRadius *= texture.width / width;
        return maxRadius / texture.width;
    }
}

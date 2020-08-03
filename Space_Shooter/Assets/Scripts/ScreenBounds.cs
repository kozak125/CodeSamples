using UnityEngine;

/// <summary>
/// Contains static methods and fields for determining screen bounds
/// </summary>
public class ScreenBounds : MonoBehaviour
{
    private static Vector3 bounds;
    private static float spriteBorder = .5f;
    private static float spawningOffset = 3f;

    public static float left { get { return -bounds.x + spriteBorder; } }
    public static float right { get { return bounds.x - spriteBorder; } }
    public static float top { get { return bounds.y - spriteBorder; } }
    public static float bottom { get { return -bounds.y + spriteBorder; } }

    void Start()
    {
        bounds = GetScreenBounds();
    }

    private static Vector3 GetScreenBounds()
    {
        Vector3 screenVector = new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z);

        return Camera.main.ScreenToWorldPoint(screenVector);
    }

    public static Vector2 RandomTopPosition()
    {
        float horizontalPosition = Random.Range(left, right);
        return new Vector2(horizontalPosition, top + spawningOffset);
    }

    public static Vector2 RandomLeftPosition()
    {
        float verticalPosition = Random.Range(bottom + spawningOffset, top);
        return new Vector2(left - 2, verticalPosition);
    }

    public static Vector2 RandomRightPosition()
    {
        float verticalPosition = Random.Range(bottom + spawningOffset, top);
        return new Vector2(right + 2, verticalPosition);
    }

    public static Vector2 GetRandomPosition()
    {
        float targetVerticalPos = Random.Range(-bounds.y + spawningOffset, bounds.y - spriteBorder);
        float targetHorizontalPos = Random.Range(-bounds.x + spriteBorder, bounds.x - spriteBorder);

        return new Vector2(targetHorizontalPos, targetVerticalPos);
    }

    public static bool OutOfBounds(Vector2 position, float border)
    {
        float x = Mathf.Abs(position.x);
        float y = Mathf.Abs(position.y);

        return (x > bounds.x + border || y > bounds.y + border);
    }
}

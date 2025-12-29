using UnityEngine;

public static class GizmosFs 
{
    /// <summary>
    /// Draws a rectangle at the specified position.
    /// </summary>
    /// <param name="center">The center of the rectangle.</param>
    /// <param name="rotation">The orientation of the rectangle.</param>
    /// <param name="size">The size of the rectangle.</param>
    public static void DrawRect(Vector3 center, Quaternion rotation, Vector2 size)
    {
        var halfSize = size / 2;

        var topLeft     = center - halfSize.x * (rotation * Vector3.right) + halfSize.y * (rotation * Vector3.up);
        var topRight    = center + halfSize.x * (rotation * Vector3.right) + halfSize.y * (rotation * Vector3.up);

        var bottomLeft  = center - halfSize.x * (rotation * Vector3.right) - halfSize.y * (rotation * Vector3.up);
        var bottomRight = center + halfSize.x * (rotation * Vector3.right) - halfSize.y * (rotation * Vector3.up);

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }

    public static Vector3[] GetBoxCorners(Vector3 min, Vector3 max) 
    {
        return new Vector3[] {
            new Vector3(min.x, min.y, min.z),
            new Vector3(max.x, min.y, min.z),
            new Vector3(min.x, max.y, min.z),
            new Vector3(max.x, max.y, min.z),
            new Vector3(min.x, min.y, max.z),
            new Vector3(max.x, min.y, max.z),
            new Vector3(min.x, max.y, max.z),
            new Vector3(max.x, max.y, max.z)
        };
    }

    public static void DrawBox(Vector3[] corners, Color color, float duration = 0.2f) 
    {
        Debug.DrawLine(corners[0], corners[1], color, duration);
        Debug.DrawLine(corners[1], corners[3], color, duration);
        Debug.DrawLine(corners[3], corners[2], color, duration);
        Debug.DrawLine(corners[2], corners[0], color, duration);

        Debug.DrawLine(corners[4], corners[5], color, duration);
        Debug.DrawLine(corners[5], corners[7], color, duration);
        Debug.DrawLine(corners[7], corners[6], color, duration);
        Debug.DrawLine(corners[6], corners[4], color, duration);

        Debug.DrawLine(corners[0], corners[4], color, duration);
        Debug.DrawLine(corners[1], corners[5], color, duration);
        Debug.DrawLine(corners[2], corners[6], color, duration);
        Debug.DrawLine(corners[3], corners[7], color, duration);
    }
}
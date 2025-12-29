using System;
using System.Collections.Generic;
using UnityEngine;



public class MirrorDisplay : MonoBehaviour
{
    [field:SerializeField] public Vector2 Size { get; set; }
    [field:SerializeField] public Vector2Int Resolution { get; set; }

    public float AspectRation => (float)Resolution.x / Resolution.y;

    private float UpperBound    => transform.position.y + Size.y / 2;
    private float LowerBound => transform.position.y - Size.y / 2;
    private float RightBound  => transform.position.x + Size.x / 2;
    private float LeftBound   => transform.position.x - Size.x / 2;


    public bool IsPointOnScreen(Vector2 point)
    {
        return point.x <= RightBound && 
               point.x >= LeftBound  && 
               point.y <= UpperBound && 
               point.y >= LowerBound;
    }
    
    /// <summary>
    /// Converts the Unity World coordinates to Screen Coordinates. The origin is the top left corner.
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public bool TryGetScreenCoordinates(Vector2 point, out Vector2Int ScreenCoordinates)
    {
        if (TryGetScreenCoordinates(point, out Vector2 coordinates))
        {
            var x = (int) (coordinates.x * Resolution.x);
            var y = (int) (coordinates.y * Resolution.y);
            ScreenCoordinates = new Vector2Int(x, y);
            return true;
        }

        ScreenCoordinates = default;
        return false;
    }
    
    /// <summary>
    /// Converts the Unity World coordinates to Screen Coordinates. The origin is the top left corner.
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public bool TryGetScreenCoordinates(Vector2 point, out Vector2 ScreenCoordinates)
    {
        if (!IsPointOnScreen(point))
        {
            ScreenCoordinates = default;
            return false;
        }
        
        var x = 1 - (point.x - LeftBound) / (RightBound - LeftBound);
        var y = 1 - (point.y - LowerBound) / (UpperBound - LowerBound);

        ScreenCoordinates = new Vector2(x, y);
        return true;
    }


    private void OnDrawGizmos()
    {
        GizmosFs.DrawRect(transform.position, Quaternion.identity, Size);
    }
}

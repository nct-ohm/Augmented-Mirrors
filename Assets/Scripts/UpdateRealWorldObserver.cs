using UnityEngine;

public class UpdateRealWorldObserver : MonoBehaviour
{
    public Transform RealWorldObserver;
    public Transform CameraPosition;
    
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();

         // Configure the LineRenderer
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        lineRenderer.material = new Material(Shader.Find("Standard"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.positionCount = 24; //
    }

    public void OnUpdate(Face face)
    {
        var direction = new Vector3(face.Center.X, -face.Center.Y, face.Center.Z);
        var worldDirection = CameraPosition.TransformDirection(direction);

        RealWorldObserver.position = CameraPosition.position + worldDirection;


        // var bBoxMin = ToWorldSpace(ConvertPoint3DToVector3(face.BboxMin), CameraPosition);
        // var bBoxMax = ToWorldSpace(ConvertPoint3DToVector3(face.BboxMax), CameraPosition);


        // SetBoundingBox(bBoxMin, bBoxMax);
    }

    private void SetBoundingBox(Vector3 min, Vector3 max)
    {
        Vector3[] corners = GizmosFs.GetBoxCorners(min, max);

        // Define the points to draw the lines
        lineRenderer.SetPositions(new Vector3[] {
            corners[0], corners[1],
            corners[1], corners[3],
            corners[3], corners[2],
            corners[2], corners[0],
            corners[4], corners[5],
            corners[5], corners[7],
            corners[7], corners[6],
            corners[6], corners[4],
            corners[0], corners[4],
            corners[1], corners[5],
            corners[2], corners[6],
            corners[3], corners[7]
        });
    }

    private static Vector3 ToWorldSpace(Vector3 point, Transform CameraPosition)
    {
        var direction = new Vector3(point.x, -point.y, point.z);
        var worldDirection = CameraPosition.TransformDirection(direction);
        return  CameraPosition.position + worldDirection;
    }

    private static Vector3 ConvertPoint3DToVector3(Point3D point)
    {
        return new Vector3(point.X, point.Y, point.Z);
    }
}

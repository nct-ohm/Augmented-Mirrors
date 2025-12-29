using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArMirror : Singleton<ArMirror>
{
    [field:SerializeField] public Camera ArCamera { get; set; }
    [field:SerializeField] public MirrorDisplay Mirror { get; set;}
    [field: SerializeField] public Transform RealWorldObserver { get; set; }
    [field:SerializeField] public List<TrackedObject> TrackedObjects { get; private set; }
    
    [field:SerializeField] public Transform CameraPosition { get; set; }

    // Update is called once per frame
    void Update()
    {
        var arCamera = ArUtils.ComputeVirtualCameraPosition(RealWorldObserver.position, Mirror.transform.position);
        ArUtils.SetTransformFromMatrix(ArCamera.transform, arCamera);
        
        //ArCamera.transform.rotation = Quaternion.identity;
        //ArCamera.transform.localScale = Vector3.one;
        ArCamera.projectionMatrix = ArUtils.CalculateViewMatrix(ArCamera.transform, ArCamera, Mirror);
        
        // Debug.Log(ArCamera.projectionMatrix);
        
        UpdateVisibleObjects();
    }

    private void UpdateVisibleObjects()
    {

        var planeNormal = Vector3.forward;
        var pointOnPlane = Mirror.transform.position;
        const float epsilon = 1e-6f;
        
        
        foreach (var obj in TrackedObjects)
        {
            var arCameraPosition = ArCamera.transform.position;
            var direction = obj.transform.position - arCameraPosition;
            var ray = new Ray(arCameraPosition, direction);

            
            var dotProduct = Vector3.Dot(planeNormal, ray.direction);
            
            // Check for intersection. If Epsilon is zero the ray and plane are in parallel
            if (Mathf.Abs(dotProduct) > epsilon)
            {
                var w = ray.origin - pointOnPlane;
                var si = -Vector3.Dot(planeNormal, w) / dotProduct;
                var psi = w + si * ray.direction + pointOnPlane;
                
                
                Debug.DrawLine(arCameraPosition, obj.transform.position, Color.white);
                // Check if inside mirror
                if (Mirror.IsPointOnScreen(psi))
                {
                    Debug.DrawLine(arCameraPosition, psi, Color.red);

                    Mirror.TryGetScreenCoordinates(psi, out Vector2 ScreenRelative);
                    Mirror.TryGetScreenCoordinates(psi, out Vector2Int ScreenAbsolute);
                    
                    // Debug.Log($"OnScreenCoordinate \t {ScreenRelative.ToString()} -- {ScreenAbsolute.ToString()} ");
                }
            }
        }
        
    }
}

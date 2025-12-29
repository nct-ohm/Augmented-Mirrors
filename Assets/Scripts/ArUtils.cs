using System.Runtime.CompilerServices;
using UnityEngine;

public static class ArUtils 
{
    public static Matrix4x4 CalculateViewMatrix(Transform cameraTransform, Camera arCamera, MirrorDisplay display)
    {
        var viewProjection = Matrix4x4.identity;
        
        var displaySize = display.Size;
        
        //var center = _calibrationSettings.CameraOrigin + _calibrationSettings.DisplayOffset - Vector3.up * (displaySize.y / 2);
        var center = display.transform.position;
        var halfSize = displaySize / 2;
        
        var topRightAbs =    center + halfSize.x * Vector3.right + halfSize.y * Vector3.up;
        var bottomLeftAbs =  center - halfSize.x * Vector3.right - halfSize.y * Vector3.up;
        var topRight   = cameraTransform.InverseTransformPoint(topRightAbs);
        var bottomLeft = cameraTransform.InverseTransformPoint(bottomLeftAbs);
        
        var n = arCamera.nearClipPlane;
        var f = arCamera.farClipPlane;
        
        var trNear = cameraTransform.InverseTransformDirection(topRight.normalized * n);
        var blNear = cameraTransform.InverseTransformDirection(bottomLeft.normalized * n);
        
        var cameraPosition = cameraTransform.position;
        var pointOnPlane = new Vector3(cameraPosition.x, cameraPosition.y, cameraPosition.z - n);
        var plane = new Plane(Vector3.forward, pointOnPlane);
        
        var trRay = new Ray(cameraPosition, trNear * 100f);
        var blRay = new Ray(cameraPosition, blNear * 100f);
        
        var trHit = plane.Raycast(trRay, out var distanceTR);
        var blHit = plane.Raycast(blRay, out var distanceBL);
                
        trNear = trNear.normalized * distanceTR;
        blNear = blNear.normalized * distanceBL;
        
        
        var r = trNear.x;
        var l = blNear.x;
        var t = trNear.y;
        var b = blNear.y;
                
        
        // https://i.stack.imgur.com/G4vP8.png
        viewProjection[0, 0] = (2 * n) / (r - l);
        viewProjection[0, 2] = (r + l) / (r - l);
        
        viewProjection[1, 1] = 2 * n / (t - b);
        viewProjection[1, 2] = (t + b) / (t - b);
        
        viewProjection[2, 2] = -((f + n) / (f - n));
        viewProjection[2, 3] = -(2f * f * n / (f - n));
        
        viewProjection[3, 2] = -1;
        viewProjection[3, 3] = 0;
        

        return viewProjection;
    }
    
    public static Matrix4x4 ComputeVirtualCameraPosition(Vector3 realWorldPosition, Vector3 mirrorPosition)
    {
        var realWorldObserver = Matrix4x4.Translate(realWorldPosition);
        var mirrorTransform = Matrix4x4.Translate(mirrorPosition);
        
        var mirrorTransformInv = mirrorTransform.inverse;
        var reflectionMatrix = Matrix4x4.identity;
        reflectionMatrix[2, 2] = -1;

        var cameraPos =  mirrorTransform * reflectionMatrix * mirrorTransformInv * realWorldObserver;
        return cameraPos;
    }
    
    /// <summary>
    /// Set transform component from TRS matrix.
    /// </summary>
    /// <param name="transform">Transform component.</param>
    /// <param name="matrix">Transform matrix.</param>
    public static void SetTransformFromMatrix(Transform transform, in Matrix4x4 matrix)
    {
        transform.localPosition = ExtractTranslationFromMatrix(in matrix);
        transform.localRotation = ExtractRotationFromMatrix(in matrix);
        transform.localScale = ExtractScaleFromMatrix(in matrix);
    }
    
    public static Vector3 ExtractTranslationFromMatrix(in Matrix4x4 matrix)
    {
        Vector3 translate;
        translate.x = matrix.m03;
        translate.y = matrix.m13;
        translate.z = matrix.m23;
        return translate;
    }

    /// <summary>
    /// Extract rotation quaternion from transform matrix.
    /// </summary>
    /// <param name="matrix">Transform matrix.</param>
    /// <returns>
    /// Quaternion representation of rotation transform.
    /// </returns>
    public static Quaternion ExtractRotationFromMatrix(in Matrix4x4 matrix)
    {
        Vector3 forward;
        forward.x = matrix.m02;
        forward.y = matrix.m12;
        forward.z = matrix.m22;

        Vector3 upwards;
        upwards.x = matrix.m01;
        upwards.y = matrix.m11;
        upwards.z = matrix.m21;

        return Quaternion.LookRotation(forward, upwards);
    }

    /// <summary>
    /// Extract scale from transform matrix.
    /// </summary>
    /// <param name="matrix">Transform matrix.</param>
    /// <returns>
    /// Scale vector.
    /// </returns>
    public static Vector3 ExtractScaleFromMatrix(in Matrix4x4 matrix)
    {
        Vector3 scale;
        scale.x = new Vector4(matrix.m00, matrix.m10, matrix.m20, matrix.m30).magnitude;
        scale.y = new Vector4(matrix.m01, matrix.m11, matrix.m21, matrix.m31).magnitude;
        scale.z = new Vector4(matrix.m02, matrix.m12, matrix.m22, matrix.m32).magnitude;

        if (Vector3.Cross(matrix.GetColumn(0), matrix.GetColumn(1)).normalized != (Vector3)matrix.GetColumn(2).normalized)
        {
            scale.x *= -1;
        }

        return scale;
    }
}

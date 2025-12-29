using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackedObject : MonoBehaviour
{
    private void OnEnable()
    {
        ArMirror.Instance.TrackedObjects.Add(this);
    }
    private void OnDisable()
    {
        if(ArMirror.Instance != null)
            ArMirror.Instance.TrackedObjects.Remove(this);
    }
}

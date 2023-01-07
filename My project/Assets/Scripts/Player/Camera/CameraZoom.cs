using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{


    [Header("Preferences")] 
    public float minZoomMultiplier = 0.5f;

    public float maxZoomMultiplier = 2f;

    public float zoomSpeed = 20f;
    
    public float zoomSmoothing = 10f;

    public CinemachineFreeLook _cinemachineFreeLook;

    private float[] _originalOrbitRadius;

    private float _zoomMultiplier;

    private void Start()
    {
        _cinemachineFreeLook.GetRig(0);
    }
}

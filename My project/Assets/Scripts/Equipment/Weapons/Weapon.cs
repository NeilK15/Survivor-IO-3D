using System;
using UnityEngine;
using UnityEngine.Rendering;

public class Weapon : MonoBehaviour
{
    [Header("General Weapon Settings")] public AudioSource attackAudio;

    private void Awake()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer)
        {
            meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
        }
    }
}
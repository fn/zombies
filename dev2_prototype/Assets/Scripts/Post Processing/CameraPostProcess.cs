using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Only allowed to be attached to a Camera.
[RequireComponent(typeof(Camera))]
public class CameraPostProcess : MonoBehaviour
{
    [SerializeField] Material PostProcessMaterial;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (PostProcessMaterial != null)
        {
            Graphics.Blit(source, destination, PostProcessMaterial);
            return;
        }

        Graphics.Blit(source, destination);
    }
}

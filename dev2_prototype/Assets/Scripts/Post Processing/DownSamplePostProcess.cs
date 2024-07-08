using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Only allowed to be attached to a Camera.
[RequireComponent(typeof(Camera))]
public class DownSamplePostProcess : MonoBehaviour
{
    // The amount of times we want to Downsample.
    [SerializeField] int DownSampleCount;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        var w = source.width;
        var h = source.height;

        if (DownSampleCount <= 0)
        {
            Graphics.Blit(source, destination);
            return;
        }

        var samples = new RenderTexture[DownSampleCount];
        var curSample = source;

        // By downsampling we half the width and height of our image each time.
        for (int i = 0; i < DownSampleCount; i++)
        {
            // Halve the width and height.
            w /= 2;
            h /= 2;

            // Create a temporary render texture.
            samples[i] = RenderTexture.GetTemporary(w, h, 0, source.format);

            Graphics.Blit(curSample, samples[i]);

            curSample = samples[i];
        }

        // Blit our last downsampled texture.
        Graphics.Blit(curSample, destination);

        // Release all of our sampled textures.
        foreach (RenderTexture sample in samples)
            RenderTexture.ReleaseTemporary(sample);
    }
}

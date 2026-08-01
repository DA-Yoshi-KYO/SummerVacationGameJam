using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

[Serializable, VolumeComponentMenu("Post-processing/Custom/Mosaic")]
public sealed class CS_Mosaic : CustomPostProcessVolumeComponent, IPostProcessComponent
{
    [Tooltip("Controls the intensity of the effect.")]
    public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f);

    [Tooltip("Controls the number of mosaic blocks across the screen width. Higher values produce smaller blocks.")]
    public ClampedFloatParameter blockCount = new ClampedFloatParameter(64f, 4f, 512f);

    Material mosaicMaterial;

    public bool IsActive() => mosaicMaterial != null && intensity.value > 0f;

    public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

    const string kShaderName = "Hidden/Shader/SH_Mosaic";

    public override void Setup()
    {
        if (Shader.Find(kShaderName) != null)
            mosaicMaterial = new Material(Shader.Find(kShaderName));
        else
            Debug.LogError($"Unable to find shader '{kShaderName}'. Post Process Mosaic is unable to load.");
    }

    public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
    {
        if (mosaicMaterial == null)
            return;

        mosaicMaterial.SetFloat("_Intensity", intensity.value);
        mosaicMaterial.SetFloat("_BlockCount", blockCount.value);
        mosaicMaterial.SetTexture("_MainTex", source);
        HDUtils.DrawFullScreen(cmd, mosaicMaterial, destination);
    }

    public override void Cleanup() => CoreUtils.Destroy(mosaicMaterial);
}

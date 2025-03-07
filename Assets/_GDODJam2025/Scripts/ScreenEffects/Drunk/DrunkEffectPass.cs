using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DrunkEffectPass : ScriptableRenderPass
{
    private Material material;
    private RTHandle source;
    private RTHandle tempTexture;

    private float timeValue;
    public float waveStrength = 0.05f;
    public float speed = 1.0f;
    public float distortion = 0.02f;

    public DrunkEffectPass(Material material)
    {
        this.material = material;
        // Create RTHandle for temporary texture
        tempTexture = RTHandles.Alloc("_TemporaryDrunkEffect", name: "_TemporaryDrunkEffect");
    }

    public void SetSource(RTHandle source) => this.source = source;

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (material == null) return;

        timeValue += Time.deltaTime;

        material.SetFloat("_TimeValue", timeValue);
        material.SetFloat("_WaveStrength", waveStrength);
        material.SetFloat("_Speed", speed);
        material.SetFloat("_Distortion", distortion);

        CommandBuffer cmd = CommandBufferPool.Get("Drunk Effect");
        RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
        
        // Allocate temporary RT
        RenderingUtils.ReAllocateIfNeeded(ref tempTexture, opaqueDesc);

        Blitter.BlitCameraTexture(cmd, source, tempTexture, material, 0);
        Blitter.BlitCameraTexture(cmd, tempTexture, source);

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        if (cmd != null)
        {
            tempTexture?.Release();
        }
    }
}

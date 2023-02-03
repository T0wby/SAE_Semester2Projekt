using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static Unity.VisualScripting.Member;

public class NightVisionURP : ScriptableRendererFeature
{
    public ComputeShader shader = null;

    [Range(0.0f, 100.0f)]
    public float radius = 70;
    [Range(0.0f, 1.0f)]
    public float tintStrength = 0.7f;
    [Range(0.0f, 100.0f)]
    public float softenEdge = 3;
    public Color tint = Color.green;
    [Range(50, 500)]
    public int lines = 100;
    class CustomRenderPass : ScriptableRenderPass
    {
        private ComputeShader shader = null;
        private float radius = 70;
        private float tintStrength = 0.7f;
        private float softenEdge = 3;
        private Color tint = Color.green;
        private int lines = 100;
        private string kernelName = "CSMain";
        private Vector2Int texSize = new Vector2Int(0, 0);
        private Vector2Int groupSize = new Vector2Int();
        private Camera thisCamera;
        private RenderTexture output = null;
        private RenderTexture renderedSource = null;
        private int kernelHandle = -1;

        public CustomRenderPass(ComputeShader shader, float radius, float tintStrength, float softenEdge, Color tint, int lines)
        {
            this.shader = shader;
            this.radius = radius;
            this.tintStrength = tintStrength;
            this.softenEdge = softenEdge;
            this.tint = tint;
            this.lines = lines;
        }

        // This method is called before executing the render pass.
        // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
        // When empty this render pass will render to the active camera render target.
        // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
        // The render pipeline will ensure target setup and clearing happens in a performant manner.
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            kernelHandle = shader.FindKernel(kernelName);
            thisCamera = Camera.main;
            CreateTextures();
        }

        // Here you can implement the rendering logic.
        // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
        // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
        // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            SetProperties();
            shader.SetFloat("time", Time.time);

            CheckResolution(out _);
            DispatchWithSource(ref renderingData.cameraData.targetTexture, ref renderingData.cameraData.targetTexture);

        }

        // Cleanup any allocated resources that were created during the execution of this render pass.
        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            ClearTextures();
        }

        #region Methods

        private void SetProperties()
        {
            float rad = (radius / 100.0f) * texSize.y;
            shader.SetFloat("radius", rad);
            shader.SetFloat("edgeWidth", rad * softenEdge / 100.0f);
            shader.SetVector("tintColor", tint);
            shader.SetFloat("tintStrength", tintStrength);
            shader.SetInt("lines", lines);
        }

        private void CreateTexture(ref RenderTexture textureToMake, int divide = 1)
        {
            textureToMake = new RenderTexture(texSize.x / divide, texSize.y / divide, 0);
            textureToMake.enableRandomWrite = true;
            textureToMake.Create();
        }

        private void CreateTextures()
        {
            texSize.x = thisCamera.pixelWidth;
            texSize.y = thisCamera.pixelHeight;

            if (shader)
            {
                uint x, y;
                shader.GetKernelThreadGroupSizes(kernelHandle, out x, out y, out _);

                groupSize.x = Mathf.CeilToInt((float)texSize.x / (float)x);
                groupSize.y = Mathf.CeilToInt((float)texSize.y / (float)y);
            }

            CreateTexture(ref output);
            CreateTexture(ref renderedSource);
            shader.SetTexture(kernelHandle, "output", output);
            shader.SetTexture(kernelHandle, "source", renderedSource);
        }

        private void ClearTexture(ref RenderTexture textureToClear)
        {
            if (null != textureToClear)
            {
                textureToClear.Release();
                textureToClear = null;
            }
        }

        private void ClearTextures()
        {
            ClearTexture(ref output);
            ClearTexture(ref renderedSource);
        }

        private void DispatchWithSource(ref RenderTexture source, ref RenderTexture destination)
        {
            Graphics.Blit(source, renderedSource);
            shader.Dispatch(kernelHandle, groupSize.x, groupSize.y, 1);

            Graphics.Blit(output, destination);
        }

        private void CheckResolution(out bool resChange)
        {
            resChange = false;

            if (texSize.x != thisCamera.pixelWidth || texSize.y != thisCamera.pixelHeight)
            {
                resChange = true;
                CreateTextures();
            }
        }
        #endregion

    }

    CustomRenderPass m_ScriptablePass;

    /// <inheritdoc/>
    public override void Create()
    {
        m_ScriptablePass = new CustomRenderPass(shader, radius, tintStrength, softenEdge, tint, lines);

        // Configures where the render pass should be injected.
        m_ScriptablePass.renderPassEvent = RenderPassEvent.AfterRendering;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_ScriptablePass);
    }

    
}



using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using System.Runtime.CompilerServices;
using System.Net.WebSockets;


public class BlurRenderPass : ScriptableRenderPass
{
    private BlurSettings defaultSettings; // we are not using additional settings here. The volume defines some values for this
    private Material material;
    private RenderTextureDescriptor blurTextureDescriptor;

    // declare the variables for interacting with the shader properties
    private static readonly int spread = Shader.PropertyToID("_Spread");
    private const string k_BlurTextureName = "_BlurTexture";
    private const string k_VerticalPassName = "VerticalBlurRenderPass";
    private const string k_HorizontalPassName = "HorizontalBlurRenderPass";
    public BlurRenderPass(Material material)
    {
        this.material = material;
        

        blurTextureDescriptor = new RenderTextureDescriptor(Screen.width, Screen.height, RenderTextureFormat.Default, 0); // gotta change this
    }
    public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
    {
        UniversalResourceData resourceData = frameData.Get<UniversalResourceData>(); // everything one might need...

        // lets gets references for the input and output textures
        TextureHandle srcCamColor = resourceData.activeColorTexture;
        TextureHandle dst = UniversalRenderer.CreateRenderGraphTexture(renderGraph, blurTextureDescriptor, k_BlurTextureName, false);

        UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();

        // The following line ensures that the render pass doesn't blit
        // from the back buffer.
        if (resourceData.isActiveTargetBackBuffer)
            return;

        // Set the blur texture size to be the same as the camera target size.
        blurTextureDescriptor.width = cameraData.cameraTargetDescriptor.width;
        blurTextureDescriptor.height = cameraData.cameraTargetDescriptor.height;
        blurTextureDescriptor.depthBufferBits = 0;


        // Update the blur settings in the material
        UpdateBlurSettings();

        // This check is to avoid an error from the material preview in the scene
        if (!srcCamColor.IsValid() || !dst.IsValid())
            return;

        // this following part is the heart of the code
        // The AddBlitPass method adds a vertical blur render graph pass that blits from the source texture (camera color in this case) to the destination texture using the first shader pass (the shader pass is defined in the last parameter)
        RenderGraphUtils.BlitMaterialParameters paraVertical = new(srcCamColor, dst, material, 0); // get from here, write there, with this function
        renderGraph.AddBlitPass(paraVertical, k_VerticalPassName);

        RenderGraphUtils.BlitMaterialParameters paraHorizontal = new(dst, srcCamColor, material, 1);
        renderGraph.AddBlitPass(paraHorizontal, k_HorizontalPassName);
    }
    // here we will update shader values
    private void UpdateBlurSettings()
    {
        if (material == null) return;
        var volumeComponent = VolumeManager.instance.stack.GetComponent<BlurSettings>();
        float spreadFromSettings = volumeComponent.strength.value;
        material.SetFloat(spread, spreadFromSettings);

        int gridSize = Mathf.CeilToInt(volumeComponent.strength.value * 6.0f);

        if (gridSize % 2 == 0)
        {
            gridSize++;
        }

        material.SetInteger("_GridSize", gridSize);

    }
       
        
    
       
}




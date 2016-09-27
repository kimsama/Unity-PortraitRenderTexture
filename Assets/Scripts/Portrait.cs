using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Create portrait rendertexture image on the runtime then set it on the RawImage's texture.
/// 
/// Note that use RawImage not Image component to show render texture as UI image.
/// </summary>
public class Portrait : MonoBehaviour 
{
    public Camera targetCamera;

    RawImage rawImage;
    Texture2D portrait;

	void Start () 
    {
        rawImage = GetComponent<RawImage>();

        // recall that the height is now the "actual" size from now on
        // the .aspect property is very tricky in Unity, and bizarrely is NOT shown in the editor
        // the editor will still incorrectly show the frustrum being screen-shaped
        targetCamera.aspect = 1.0f;

        // Create RenderTexture image.
        int sqr = 256;
        RenderTexture renderTexture = new RenderTexture(sqr, sqr, 24);

        // no need to generate mipmaps.
        renderTexture.generateMips = false;

        // set the created RenderTexture to the target texture of the target camera.
        targetCamera.targetTexture = renderTexture;

        // render image on the RenderTexture
        targetCamera.Render();
        //RenderTexture.active = renderTexture;

        // set the rendered image to the RawImage.
        rawImage.texture = getTexture2DFromRenderTexture(renderTexture);

        // set null to make everyting is rendered in the main window.
        RenderTexture.active = null;

        // Note that set higher depth value on the MainCamer than portrait camera's one.
        // e.g. MainCamera: 0, PortrainCamera: -1
        // Invalidating targetTexture without correct depth value, it causes to get wrong renderer scene.
        targetCamera.targetTexture = null;

        // no more needed.
        Destroy(renderTexture);
	}

    /// <summary>
    /// Create Texture2D from the given RenderTexture.
    /// </summary>
    /// <returns></returns>
    public Texture2D getTexture2DFromRenderTexture(RenderTexture rTex)
    {
        Texture2D texture2D = new Texture2D(rTex.width, rTex.height);
        RenderTexture.active = rTex;
        texture2D.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        texture2D.Apply();
        return texture2D;
    }
}

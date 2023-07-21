using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class Preprocess : MonoBehaviour
{
    RenderTexture renderTexture;
    Vector2 scale = new Vector2(1, 1);
    Vector2 offset = Vector2.zero;

    UnityAction<byte[]> callback;

    public void ScaleAndCropImage(WebCamTexture webCamTexture, int imageSize, UnityAction<byte[]> callback) {
        this.callback = callback;

        if (renderTexture == null) {
            renderTexture = new RenderTexture(imageSize, imageSize, 0, RenderTextureFormat.ARGB32);
        }

        scale.x = (float) webCamTexture.height / (float) webCamTexture.width;
        offset.x = (1 - scale.x) / 2f;
        Graphics.Blit(webCamTexture, renderTexture, scale, offset);
        AsyncGPUReadback.Request(renderTexture, 0, TextureFormat.RGB24, OnCompleteReadback);
        // DEBUG
        Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        // copy a rendertexture to a texture2D
        RenderTexture.active = renderTexture;
        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        byte[] bytes = tex.EncodeToPNG();
        Debug.Log(Application.dataPath + "/../010108.png");
        System.IO.File.WriteAllBytes(Application.dataPath + "/../010108.png", bytes);
    }

    void OnCompleteReadback(AsyncGPUReadbackRequest request) {
        if (request.hasError) {
            Debug.Log("GPU readback error detected.");
            return;
        }

        callback.Invoke(request.GetData<byte>().ToArray());
    }
}

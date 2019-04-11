using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Display : MonoBehaviour {

    public RenderTexture RenderTextureRef;
    private Texture2D tex;
    private Texture2D texture;
    RawImage image;

    void Start () {
        tex = new Texture2D(RenderTextureRef.width, RenderTextureRef.height);
        texture = new Texture2D(RenderTextureRef.width, RenderTextureRef.height);
        image = GetComponent<RawImage>();
    }

    void Update()
    {
        RenderTexture currentActiveRT = RenderTexture.active;
        RenderTexture.active = RenderTextureRef;

        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        tex.Apply();
        RenderTexture.active = currentActiveRT;

        byte[] receiveBytes = tex.EncodeToJPG();
        

        texture.LoadImage(receiveBytes);
        
        image.texture = texture;
    }
}

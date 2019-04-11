using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Display : MonoBehaviour {

    public RenderTexture RenderTextureRef;

    void Start () {
        
    }

    private void Update()
    {
        RenderTexture currentActiveRT = RenderTexture.active;
        RenderTexture.active = RenderTextureRef;
        Texture2D tex = new Texture2D(RenderTextureRef.width, RenderTextureRef.height);
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        tex.Apply();
        RenderTexture.active = currentActiveRT;
        /*
        byte[] receiveBytes = tex.EncodeToPNG();
        int pos = 16; // 16バイトから開始

        int width = 0;
        for (int i = 0; i < 4; i++)
        {
            width = width * 256 + receiveBytes[pos++];
        }

        int height = 0;
        for (int i = 0; i < 4; i++)
        {
            height = height * 256 + receiveBytes[pos++];
        }

        Texture2D texture = new Texture2D(width, height);
        texture.LoadImage(receiveBytes);
        */
        RawImage image = GetComponent<RawImage>();
        image.texture = tex;

        Destroy(tex, 1);
        //Destroy(texture, 1);
    }
}

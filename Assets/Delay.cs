using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class TextureRecord
{
    public Texture2D texture2d;
    public Time time;
}

public class Delay : MonoBehaviour
{
    public RenderTexture renderTexture;

    private RawImage rawImage;
    private Queue<TextureRecord> textureQueue;
    
    void Start()
    {
        rawImage = GetComponent<RawImage>();
        textureQueue = new Queue<TextureRecord>();
    }

    // Update is called once per frame
    void Update()
    {
        TextureRecord textureRecord = new TextureRecord();
        textureRecord.texture2d = new Texture2D(renderTexture.width, renderTexture.height);
        RenderTexture currentActiveRT = RenderTexture.active;
        RenderTexture.active = renderTexture;
        textureRecord.texture2d.ReadPixels(new Rect(0, 0, textureRecord.texture2d.width, textureRecord.texture2d.height), 0, 0);
        textureRecord.texture2d.Apply();
        RenderTexture.active = currentActiveRT;

        rawImage.texture = textureRecord.texture2d;
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UdpSender : MonoBehaviour
{
    public int listenPort = 2002; // ポートはサーバ・クライアントで合わせる必要がある
    public RenderTexture RenderTextureRef;
    private static bool sent = false;
    private Coroutine coroutine_ = null;

    public static void SendCallback(System.IAsyncResult ar)
    {
        // System.Net.Sockets.UdpClient u = (System.Net.Sockets.UdpClient)ar.AsyncState;
        sent = true;
    }

    IEnumerator send(string server, byte[] message)
    {
        Debug.Log("sending..");
        var u = new System.Net.Sockets.UdpClient();
        u.EnableBroadcast = true;
        u.Connect(server, listenPort);
        var sendBytes = message;
        sent = false;
        u.BeginSend(sendBytes, sendBytes.Length,
                    new System.AsyncCallback(SendCallback), u);
        while (!sent)
        {
            yield return null;
        }
        u.Close();
        coroutine_ = null;
        Debug.Log("done.");
    }

    void Update()
    {
        RenderTexture currentActiveRT = RenderTexture.active;
        RenderTexture.active = RenderTextureRef;
        Texture2D tex = new Texture2D(RenderTextureRef.width, RenderTextureRef.height);
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        tex.Apply();
        RenderTexture.active = currentActiveRT;

        byte[] bytes = tex.EncodeToPNG();
        Debug.Log(BitConverter.ToString(bytes));
        string server = "127.0.0.1"; // ブロードキャストアドレスが使えるならそれがベスト。できなかったら相手のＩＰアドレスを直打ち
        coroutine_ = StartCoroutine(send(server, bytes));
    }
}

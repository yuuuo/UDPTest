using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UdpSender : MonoBehaviour {
	public int listenPort = 2002; // ポートはサーバ・クライアントで合わせる必要がある
	public Texture2D tex;
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
		while (!sent) {
			yield return null;
		}
		u.Close();
		coroutine_ = null;
		Debug.Log("done.");
	}

	void Update() {
		if (coroutine_ == null && Input.GetKeyDown(KeyCode.Space)) {
            byte[] bytes = tex.EncodeToPNG();
			string server = "127.0.0.1"; // ブロードキャストアドレスが使えるならそれがベスト。できなかったら相手のＩＰアドレスを直打ち
			coroutine_ = StartCoroutine(send(server, bytes));
			
        }
    }
}

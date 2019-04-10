using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UdpReceiver : MonoBehaviour {
	public int listenPort = 2002; // ポートはサーバ・クライアントで合わせる必要がある
	private bool received = false;

    private struct UdpState {
		public System.Net.IPEndPoint e;
		public System.Net.Sockets.UdpClient u;
	}

	public void ReceiveCallback(System.IAsyncResult ar) {
		System.Net.Sockets.UdpClient u = (System.Net.Sockets.UdpClient)((UdpState)(ar.AsyncState)).u;
		System.Net.IPEndPoint e = (System.Net.IPEndPoint)((UdpState)(ar.AsyncState)).e;
		var receiveBytes = u.EndReceive(ar, ref e);

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

        RawImage image = GetComponent<RawImage>();
        image.texture = texture;

        Debug.Log("Received"); // ここに任意の処理を書く

        received = true;
	}

	IEnumerator receive_loop() {
		var e = new System.Net.IPEndPoint(System.Net.IPAddress.Any, listenPort);
		var u = new System.Net.Sockets.UdpClient(e);
		u.EnableBroadcast = true;
		var s = new UdpState();
		s.e = e;
		s.u = u;
		for (;;) {
			received = false;
			u.BeginReceive(new System.AsyncCallback(ReceiveCallback), s);
			while (!received) {
				yield return null;
			}
		}
	}

	void Start () {
        StartCoroutine(receive_loop());
	}
}

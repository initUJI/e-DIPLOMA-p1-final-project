using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;

public class SocketTest : MonoBehaviour
{
    private SocketIOUnity socket;

    // Start is called before the first frame update
    void Start()
    {
        socket = new SocketIOUnity("http://localhost:3000");

        socket.OnConnected += (sender, e) =>
        {
            Debug.Log("Connected");
        };
        socket.Connect();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            socket.Emit("user-one-play", 123);
        }if(Input.GetKeyDown(KeyCode.S))
        {
            socket.Emit("user-two-play", 456);
        }
    }
}

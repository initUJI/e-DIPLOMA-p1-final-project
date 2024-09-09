using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;

public class GameSocket_Manager : MonoBehaviour
{
    private SocketIOUnity socket;

    [Header("Local Player Information")]
    public int playerID;
    public string roomID;


    // Start is called before the first frame update
    void Start()
    {
        socket = new SocketIOUnity("http://localhost:3000", new SocketIOOptions
        {
            Query = new Dictionary<string, string>
        {
            {"user", "UNITY" }
        }
            ,
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });

        socket.OnConnected += (sender, e) =>
        {
            Debug.Log("Connected");
        };

        socket.On("joined-room", response =>
        {
            /* Do Something with data! */
            string res = response.ToString();
            res = res.Replace("]", "");
            res = res.Replace("[", "");

            Debug.Log(res);
            roomInfo rInfo = JsonUtility.FromJson<roomInfo>(res);

            playerID = rInfo.user;
            roomID = rInfo.room;
        });


        socket.Connect();
    }
}

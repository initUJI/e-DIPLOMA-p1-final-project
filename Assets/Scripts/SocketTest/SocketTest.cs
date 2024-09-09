using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;
using TMPro;

public class SocketTest : MonoBehaviour
{
    private SocketIOUnity socket;
    private roomInfo roomInfo;

    public TextMeshProUGUI ID_Text;

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
            roomInfo = JsonUtility.FromJson<roomInfo>(res);
        });


        socket.Connect();

        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Current player count: " + roomInfo.user + " with room ID: " + roomInfo.room);

            if(ID_Text != null)
            {
                ID_Text.text = "P: " + roomInfo.user + " R: " + roomInfo.room;
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            
        }
    }
}

[System.Serializable]
public class roomInfo
{
    public string room;
    public int user;
}
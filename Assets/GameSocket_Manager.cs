using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;
using UnityEngine.Events;
using System.Text.RegularExpressions;
using System;

public class GameSocket_Manager : MonoBehaviour
{
    private SocketIOUnity socket;

    [Header("Local Player Information")]
    public int playerID;
    public string roomID;
    public UnityEvent<List<string>> onPlayerMove = new UnityEvent<List<string>>();
    public UnityEvent onPlayerReady = new UnityEvent();
    public UnityEvent onRoomReady = new UnityEvent();


    // Start is called before the first frame update 
    void Start()
    {
        string uniqueIdentifier = "UNITY_" + Guid.NewGuid().ToString();

        //socket = new SocketIOUnity("http://150.128.97.41:3000", new SocketIOOptions
        socket = new SocketIOUnity("http://localhost:3000", new SocketIOOptions
        {
            Query = new Dictionary<string, string>
        {
            {"user", uniqueIdentifier }
        }
            ,
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });

        Debug.Log(socket.Connected);

        socket.OnConnected += (sender, e) =>
        {
            Debug.Log("Connected for real");
        };

        socket.OnError += (sender, e) =>
        {
            Debug.Log("Error: " + e);
        };

        socket.On("connect_error", (err) => {
            Debug.LogError("Connection error: " + err);
        });

        socket.On("joined-room", response =>
        {
            /* Do Something with data */  
            string res = response.ToString();
            res = res.Replace("]", "");
            res = res.Replace("[", "");

            Debug.Log(res);
            roomInfo rInfo = JsonUtility.FromJson<roomInfo>(res);

            playerID = rInfo.user;
            roomID = rInfo.room;

        });

        socket.On("move", response =>
        {
            /* Do Something with data! */
            string res = Regex.Unescape(response.ToString());
            res = res.Substring(2, res.Length - 4);

            Sequence seq = JsonUtility.FromJson<Sequence>(res);
            
            onPlayerMove.Invoke(seq.actions);
        });

        socket.On("execute", response =>
        {
            onPlayerReady.Invoke();
        });

        socket.On("room-ready", response =>
        {
            Debug.Log("Que empiece el juego.");
            onRoomReady.Invoke();
        });

        socket.On("connect_error", (err) => {
        // the reason of the error, for example "xhr poll error"
        Debug.Log(err.ToString());
        });

         socket.OnDisconnected += (sender, e) =>
        {
            Debug.LogWarning("Disconnected from server: " + e);
        };

        // Attempt to connect
        try
        {
            socket.Connect();
            Debug.Log("Attempting to connect...");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Exception while trying to connect: " + ex.Message);
        }
    }

    public void sendLocalSecuence(List<string> localStringSecuence)
    {
        // Create json with JsonUtility
        Sequence localSequence = new Sequence {
            actions = localStringSecuence
        };
        string json = JsonUtility.ToJson(localSequence);
        socket.Emit("update", json);
    }

    public void sendPlayerReady()
    {
        socket.Emit("ready", "");
    }
}

[System.Serializable]
class Sequence
{
    public List<string> actions;
}
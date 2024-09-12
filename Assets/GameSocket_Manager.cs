using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;
using UnityEngine.Events;
using System.Text.RegularExpressions;

public class GameSocket_Manager : MonoBehaviour
{
    private SocketIOUnity socket;

    [Header("Local Player Information")]
    public int playerID;
    public string roomID;
    public UnityEvent<List<string>> onPlayerMove = new UnityEvent<List<string>>();


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

        socket.On("move", response =>
        {
            /* Do Something with data! */
            string res = Regex.Unescape(response.ToString());
            res = res.Substring(2, res.Length - 4);

            Sequence seq = JsonUtility.FromJson<Sequence>(res);
            
            onPlayerMove.Invoke(seq.actions);
        });

        socket.Connect();
    }

    public void sendLocalSecuence(List<string> localStringSecuence)
    {
        // Create json with JsonUtility
        Sequence localSequence = new Sequence {
            actions = localStringSecuence
        };
        string json = JsonUtility.ToJson(localSequence);
        socket.Emit("play", json);

    }
}

[System.Serializable]
class Sequence
{
    public List<string> actions;
}
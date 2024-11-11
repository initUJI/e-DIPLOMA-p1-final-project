using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class VoiceRecorderManual : MonoBehaviour
{
    private class VoiceRecord
    {
        public string Player { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    private List<VoiceRecord> records = new List<VoiceRecord>();
    private DateTime player1StartTime;
    private DateTime player2StartTime;
    private bool isPlayer1Recording = false;
    private bool isPlayer2Recording = false;

    void Update()
    {
        // Player 1 voice recording toggle
        if (Input.GetKeyDown(KeyCode.K))
        {
            player1StartTime = DateTime.Now;
            isPlayer1Recording = true;
            Debug.Log($"Player 1 started talking at {player1StartTime:HH:mm:ss}");
        }
        else if (Input.GetKeyUp(KeyCode.K) && isPlayer1Recording)
        {
            isPlayer1Recording = false;
            var endTime = DateTime.Now;
            records.Add(new VoiceRecord
            {
                Player = "Player 1",
                StartTime = player1StartTime,
                EndTime = endTime
            });
            Debug.Log($"Player 1 stopped talking at {endTime:HH:mm:ss}");
        }

        // Player 2 voice recording toggle
        if (Input.GetKeyDown(KeyCode.L))
        {
            player2StartTime = DateTime.Now;
            isPlayer2Recording = true;
            Debug.Log($"Player 2 started talking at {player2StartTime:HH:mm:ss}");
        }
        else if (Input.GetKeyUp(KeyCode.L) && isPlayer2Recording)
        {
            isPlayer2Recording = false;
            var endTime = DateTime.Now;
            records.Add(new VoiceRecord
            {
                Player = "Player 2",
                StartTime = player2StartTime,
                EndTime = endTime
            });
            Debug.Log($"Player 2 stopped talking at {endTime:HH:mm:ss}");
        }

        // Save to CSV when pressing "S"
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveRecordsToCSV();
        }
    }

    private void SaveRecordsToCSV()
    {
        // Generar una marca de tiempo para el nombre del archivo
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string fileName = $"VoiceRecords_{timestamp}.csv";
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        // Asegurarse de que la carpeta StreamingAssets exista
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
        }

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("PlayerID;StartTime;EndTime");
            foreach (var record in records)
            {
                writer.WriteLine($"{record.Player};{record.StartTime:HH:mm:ss};{record.EndTime:HH:mm:ss}");
            }
        }

        Debug.Log($"Voice records saved to {filePath}");
    }
}

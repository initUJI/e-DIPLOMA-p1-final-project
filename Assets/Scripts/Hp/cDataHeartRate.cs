using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System;
using System.Globalization;


public enum eDataHeartRate // Sacado de Google para Testing
{
    NONE,   //    0 bpm
    LOW,    // < 63 bpm
    MEDIUM, // < 76 bpm
    HIGH,   // < 95 bpm
    MAX     //>= 95 bpm
}
public class cDataHeartRate
{ 
    private float offsetTime;
    private string filepath;
    private string filename;
    StreamWriter csvWriter;

    public cDataHeartRate()
    {
        filepath = HPGameManager.Instance.UserDataFolder + "/";
        filename = "HeartRate" + DateTime.Now.ToString("yyyyMMdd-HHmmss");
        offsetTime = Time.realtimeSinceStartup;
        CSVCreate();
    }

    public void AddResponse(eEstados sit, float value)
    {
        float timeStamp = cDataManager.GetTime();
        CSVAddLine(cDataManager.TimeToStringGlobalized(cDataManager.GetUnixTime()) + ";" + cDataManager.TimeToStringGlobalized(timeStamp) + ";" + sit + ";" + f_GetHeartRateStatus(value) + ";" + value);
    }

    private void CSVCreate()
    {
        Debug.Log("<b>[SaveSit]</b> Creamos csv HeartRate");
        csvWriter = new StreamWriter(filepath + "_" + filename + ".csv");
        csvWriter.AutoFlush = true;
        csvWriter.WriteLine("userID;fecha");
        csvWriter.WriteLine(HPGameManager.Instance.UserID + ";" + DateTime.Now.ToString("dd/MM/yyyy_HH:mm"));
        csvWriter.WriteLine("unixTimestamp;Timestamp;Situacion;Estado;BPM");
    }
    private void CSVAddLine(string line)
    {
        csvWriter.WriteLine(line);
    }
    public void CSVCerrar()
    {
        csvWriter.Close();
        csvWriter.Dispose();
    }

    private eDataHeartRate f_GetHeartRateStatus(float value)
    {
        if(value != 0)
        {
            if(value < 63)
            {
                return eDataHeartRate.LOW;
            }
            else if(value < 76)
            {
                return eDataHeartRate.MEDIUM;
            }
            else if (value < 95)
            {
                return eDataHeartRate.HIGH;
            }
            else
            {
                return eDataHeartRate.MAX;
            }
        }
        else
        {
            return eDataHeartRate.NONE;
        }
    }
}


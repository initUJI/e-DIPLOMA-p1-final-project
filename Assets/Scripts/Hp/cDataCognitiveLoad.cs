using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System;
using System.Globalization;


public enum eDataCognitiveLoad // HP Thresholds for cognitiveLoad
{
    NONE,   //0
    LOW,    //<0.33
    MEDIUM, //<0.66
    HIGH    //<=1
}
public class cDataCognitiveLoad
{
    private float offsetTime;
    private string filepath;
    private string filename;
    StreamWriter csvWriter;

    public cDataCognitiveLoad()
    {
        filepath = HPGameManager.Instance.UserDataFolder + "/";
        filename = "CognitiveLoad" + DateTime.Now.ToString("yyyyMMdd-HHmmss");
        offsetTime = Time.realtimeSinceStartup;
        CSVCreate();
    }

    public void AddResponse(eEstados sit, float value, float deviation)
    {
        float timeStamp = cDataManager.GetTime();
        CSVAddLine(cDataManager.TimeToStringGlobalized(cDataManager.GetUnixTime()) + ";" + cDataManager.TimeToStringGlobalized(timeStamp) + ";" + sit + ";" + f_GetLoadFromValue(value) + ";" + value + ";" + deviation);
    }

    private void CSVCreate()
    {
        Debug.Log("<b>[SaveSit]</b> Creamos csv CognitiveLoad");
        csvWriter = new StreamWriter(filepath + "_" + filename + ".csv");
        csvWriter.AutoFlush = true;
        csvWriter.WriteLine("userID;fecha");
        csvWriter.WriteLine(HPGameManager.Instance.UserID + ";" + DateTime.Now.ToString("dd/MM/yyyy_HH:mm"));
        csvWriter.WriteLine("unixTimestamp;Timestamp;Situacion;Carga Cognitiva;Valor;Desviación");
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

    private eDataCognitiveLoad f_GetLoadFromValue(float value)
    {
        if(value != 0)
        {
            if(value < 0.33)
            {
                return eDataCognitiveLoad.LOW;
            }
            else if(value < 0.66)
            {
                return eDataCognitiveLoad.MEDIUM;
            }
            else
            {
                return eDataCognitiveLoad.HIGH;
            }
        }
        else
        {
            return eDataCognitiveLoad.NONE;
        }
    }
}


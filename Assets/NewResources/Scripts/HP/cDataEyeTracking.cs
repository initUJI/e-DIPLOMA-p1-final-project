using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System;
using System.Globalization;


public enum eDataEyeTracking {
    ENTRA,
    SALE,
    ENTRA_HEAD,
    SALE_HEAD,
    DIRECTION_CHANGE
}
public class cDataEyeTracking {
    private float offsetTime;
    private string filepath;
    private string filename;
    StreamWriter csvWriter;

    public cDataEyeTracking() {
        filepath = HPGameManager.Instance.UserDataFolder + "/";
        filename = "EyeTracking" + DateTime.Now.ToString("yyyyMMdd-HHmmss");
        offsetTime = Time.realtimeSinceStartup;
        CSVCreate();
    }

    public void AddResponse(eEstados sit, eDataEyeTracking accion, string obj) {
        float timeStamp = cDataManager.GetTime();
        CSVAddLine(cDataManager.TimeToStringGlobalized(cDataManager.GetUnixTime()) + ";" + cDataManager.TimeToStringGlobalized(timeStamp) + ";" + sit + ";" + accion + ";" + obj);
    }

    private void CSVCreate() {
        Debug.Log("<b>[SaveSit]</b> Creamos csv EyeTracking");
        csvWriter = new StreamWriter(filepath + "_" + filename + ".csv");
        csvWriter.AutoFlush = true;
        csvWriter.WriteLine("userID;fecha");
        csvWriter.WriteLine(HPGameManager.Instance.UserID+";"+ DateTime.Now.ToString("dd/MM/yyyy_HH:mm"));
        csvWriter.WriteLine("unixTimestamp;Timestamp;Situacion;Accion;Objeto");
    }
    private void CSVAddLine(string line) {
        csvWriter.WriteLine(line);
    }
    public void CSVCerrar() {
        csvWriter.Close();
        csvWriter.Dispose();
    }
}


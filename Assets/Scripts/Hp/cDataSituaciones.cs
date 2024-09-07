using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System;
using System.Globalization;


public enum eDataSitAction { // Modificarlo a nuestras tareas, esto es un ejemplo dado por la UPV
    START,
    PULSATECLA,
    VOZ,
    ESCANEAPRODUCTO,
    LLAMATELEFONO,
    SPAWNITEM,
    SPAWNITEMMANO,
    COGEITEM,
    DEJAITEM,
    MUESTRATICKET,
    CIERRATICKET,
    APARCACOMPRA,
    DESAPARCACOMPRA,
    BLOQUEAACCION,
    SHOWFBRESUMEN,
    CLOSEFBRESUMEN,
    SHOWFBFRONT,
    CLOSEFBFRONT,
    ACCEPTFBFRONT,
    SHOWFBBACK,
    CLOSEFBBACK,
    CLICKSHOWHELP,
    CLIENTEESTADO,
    EMBOLSAPRODUCTO,
    DESEMBOLSAPRODUCTO,
    RETIRAPRODUCTO,
    DESRETIRAPRODUCTO,
    IDENTIFICASOCIO,
    APLICAPROMOCION,
    MIRAETIQUETA
}
public class cDataSituaciones {
    private float offsetTime;
    private string filepath;
    private string filename;
    StreamWriter csvWriter;

    public cDataSituaciones() {
        filepath = HPGameManager.Instance.UserDataFolder + "/";
        filename = "Situaciones" + DateTime.Now.ToString("yyyyMMdd-HHmmss");
        offsetTime = Time.realtimeSinceStartup;
        CSVCreate();
    }

    public void AddResponse(eEstados sit, eDataSitAction accion, string dato1, float id1, string extra) {
        float timeStamp = cDataManager.GetTime();
        //Debug.Log("<b>[SaveSit]</b> TimeStamp: " + timeStamp + ", sit: " + sit + ", accion: " + accion + ", dato1: " + dato1 + ", id1: " + id1);
        CSVAddLine(cDataManager.TimeToStringGlobalized(cDataManager.GetUnixTime()) + ";" + cDataManager.TimeToStringGlobalized(timeStamp) + ";" + sit + ";" + accion + ";" + dato1 + ";" + id1 + ";" + extra);
    }

    private void CSVCreate() {
        Debug.Log("<b>[SaveSit]</b> Creamos csv Situaciones");
        csvWriter = new StreamWriter(filepath + "_" + filename + ".csv");
        csvWriter.AutoFlush = true;
        csvWriter.WriteLine("userID;fecha");
        csvWriter.WriteLine(HPGameManager.Instance.UserID+";"+ DateTime.Now.ToString("dd/MM/yyyy_HH:mm"));
        csvWriter.WriteLine("unixTimestamp;Timestamp;Situacion;Accion;Dato1;Id1;Extra");
    }
    private void CSVAddLine(string line) {
        csvWriter.WriteLine(line);
    }
    public void CSVCerrar() {
        csvWriter.Close();
        csvWriter.Dispose();
    }
}

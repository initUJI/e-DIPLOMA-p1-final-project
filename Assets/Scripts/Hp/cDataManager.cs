using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;


public class cDataManager : MonoBehaviour {

    private static cDataManager instance;
    private static bool iniciado = false;
    private static float offsetTime;
    //private static cDataSituaciones dataSituaciones;
    private static cDataEyeTracking dataEyeTracking;
    private static cDataCognitiveLoad dataCognitiveLoad;
    private static cDataHeartRate dataHeartRate;

    private static NumberFormatInfo nfi = new NumberFormatInfo();

    void Awake() {
        //instance = this;
    }
    private void Start()
    {
        Init();
    }

    public static void Init() {
        if (iniciado) return;
        Debug.Log("<b>[Save]</b>Iniciado DataManager");
        offsetTime = Time.realtimeSinceStartup;

        nfi.NumberDecimalSeparator = ".";

        //dataSituaciones = new cDataSituaciones();
        dataEyeTracking = new cDataEyeTracking();
        dataCognitiveLoad = new cDataCognitiveLoad();
        dataHeartRate = new cDataHeartRate();

        iniciado = true;
    }


    private void OnApplicationQuit() {
        //if (null != dataSituaciones) dataSituaciones.CSVCerrar();
        if (null != dataEyeTracking) dataEyeTracking.CSVCerrar();
        if (null != dataCognitiveLoad) dataCognitiveLoad.CSVCerrar();
        if (null != dataHeartRate) dataHeartRate.CSVCerrar();
    }

    public static float GetTime() {
        return Time.realtimeSinceStartup - offsetTime;
    }
    public static double GetUnixTime() {
        return DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
    }

    public static string TimeToStringGlobalized(double time) //This is the globalized use case for the time, the nfi is removing 0 when they are at the end. Example: time = 32.40 --> str with nfi(NumberFormatInfo) = "32.4"  // with this method = "32.40" so all strings has the same lenght
    {
        return time.ToString("#.00000"); // .Replace(",", ".") // Set the decimal separator as they wish. Default is "," 
    }

    public static string TimeStringGlobalizedNFI(double time) // nfi, I think that the main problem is when I try to visualize it in a excel file.
    {
        return time.ToString(nfi);
    }

    /*public void AddSituacionAction(eDataSitAction accion, string dato1 = "", float id1 = -1, string extra = "") {
        if (!iniciado) {
            Debug.LogError("<b>[Save]</b>Intentando guardar situacion sin iniciar sistema de datos");
            return;
        }
        dataSituaciones.AddResponse(HPGameManager.SitActualEnum, accion, dato1, id1, extra);
    }*/
    public void AddEyeAction(eDataEyeTracking accion, string objeto) {
        if (!iniciado) {
            Debug.LogError("<b>[Save]</b>Intentando guardar eyetracking sin iniciar sistema de datos");
            return;
        }
        dataEyeTracking.AddResponse(HPGameManager.SitActualEnum, accion, objeto);
    }

    public void AddCognitiveLoadStatus(float value, float deviation)
    {
        if (!iniciado)
        {
            Debug.LogError("<b>[Save]</b>Intentando guardar Cognitive Load sin iniciar sistema de datos");
            return;
        }
        dataCognitiveLoad.AddResponse(HPGameManager.SitActualEnum, value, deviation);
    }

    public void AddHeartRateStatus(float value)
    {
        if (!iniciado)
        {
            Debug.LogError("<b>[Save]</b>Intentando guardar Heart Rate sin iniciar sistema de datos");
            return;
        }
        dataHeartRate.AddResponse(HPGameManager.SitActualEnum, value);
    }
}

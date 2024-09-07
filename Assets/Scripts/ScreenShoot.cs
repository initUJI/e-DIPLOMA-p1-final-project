using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShoot : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ScreenCapture.CaptureScreenshot(Application.streamingAssetsPath + "/SensorsDataUser/screenshoot-" + DateTime.Now.ToString("dd-MM-yy-HH-mm-ss") + ".png", 4);
            print("Screenshoot!");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Data;
using static System.IO.StreamWriter;
using UnityEngine.Rendering;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using System.Net.Sockets;

public class EventsManager : MonoBehaviour
{
    [SerializeField] private GameObject xrOrigin;
    [SerializeField] private GameObject popAudio;
    [SerializeField] private GameObject buttonAudio;
    private XRGrabInteractable[] grabInteractables;

    private string userID = "anom";
    private string fileName = "anom";
    private LevelManager levelManager;
    private string directoryPath;
    private string filePath;
    private TextMeshProUGUI errorText;
    private GameObject canvas;
    private float timer = 0;
    private float timeShowingError = 10f;
    private XRSocketInteractor socket;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        canvas = transform.GetChild(0).gameObject;
        canvas.SetActive(false);
        errorText = canvas.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        errorText.text = string.Empty;
        grabInteractables = FindObjectsOfType<XRGrabInteractable>();

        string projectDirectory = Directory.GetParent(Application.dataPath).FullName;
        directoryPath = Path.Combine(projectDirectory, "Data");
        filePath = Path.Combine(directoryPath, "data_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") +".json");

        Data data = new Data("userID", "DateTime", "actualLevel", "action");
        writeInJson(data);
       

        if (xrOrigin == null && GameObject.Find("XR Origin") != null)
        {
            xrOrigin = GameObject.Find("XR Origin");
            TeleportationProvider teleportationProvider = xrOrigin.GetComponent<TeleportationProvider>();
            teleportationProvider.endLocomotion += OnTeleportationEnd;
        }
    }

    public string getUserID()
    {
        return userID;
    }

    public void playPressed()
    {
        Data data;

        tryFindingLevelManager();

        if (levelManager != null)
        {
            data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), levelManager.getActualLevel().ToString(), "BUTTON PLAY PRESSED");
        }
        else
        {
            data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), "MAIN MENU", "BUTTON PLAY PRESSED");
        }
        buttonAudio.GetComponent<AudioSource>().Play();
        writeInJson(data);
    }

    public void levelCompleted(int num)
    {
        Data data;

        tryFindingLevelManager();

        if (levelManager != null)
        {
            data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), levelManager.getActualLevel().ToString(), "LEVEL COMPLETED: " + num.ToString());
            writeInJson(data);
        }
    }

    public void sceneCleaned()
    {
        Data data;

        tryFindingLevelManager();

        if (levelManager != null)
        {
            data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), levelManager.getActualLevel().ToString(), "SCENE CLEANED, ALL BLOCKS RESETED");
            writeInJson(data);
        }

        buttonAudio.GetComponent<AudioSource>().Play();
    }

    public void clueUsed(string clue)
    {
        Data data;

        tryFindingLevelManager();

        if (levelManager != null)
        {
            data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), levelManager.getActualLevel().ToString(), "USED CLUE: " + clue);
            writeInJson(data);
        }
        buttonAudio.GetComponent<AudioSource>().Play();
    }

    public void testStandUsed(string direction)
    {
        Data data;

        tryFindingLevelManager();

        if (levelManager != null)
        {
            data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), levelManager.getActualLevel().ToString(), "PRESSED TEST STAND: " + direction);
            writeInJson(data);
        }
        buttonAudio.GetComponent<AudioSource>().Play();
    }

    public void characterMoving(string direction)
    {
        Data data;

        tryFindingLevelManager();

        if (levelManager != null)
        {
            data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), levelManager.getActualLevel().ToString(), "CHARACTER START MOVING: " + direction);
        }
        else
        {
            data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), "MAIN MENU", "CHARACTER START MOVING: " + direction);
        }

        writeInJson(data);
    }

    public void deleteWindowOpen()
    {
        Data data;

        tryFindingLevelManager();

        if (levelManager != null)
        {
            data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), levelManager.getActualLevel().ToString(), "DELETE BLOCK WINDOW OPEN");
        }
        else
        {
            data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), "MAIN MENU", "DELETE BLOCK WINDOW OPEN");
        }

        writeInJson(data);
    }

    public void deleteWindowClose()
    {
        Data data;

        tryFindingLevelManager();

        if (levelManager != null)
        {
            data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), levelManager.getActualLevel().ToString(), "DELETE BLOCK WINDOW CLOSE");
        }
        else
        {
            data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), "MAIN MENU", "DELETE BLOCK WINDOW CLOSE");
        }

        writeInJson(data);
    }

    public void deleteBlock(GameObject block)
    {
        Data data;

        tryFindingLevelManager();

        if (levelManager != null)
        {
            data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), levelManager.getActualLevel().ToString(), "DELETED BLOCK: " + block.name);
        }
        else
        {
            data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),"MAIN MENU", "DELETED BLOCK: " + block.name);
        }
        buttonAudio.GetComponent<AudioSource>().Play();
        writeInJson(data);
    }


    public void subscribeGrabEvents(XRGrabInteractable grab)
    {
        grab.selectEntered.AddListener(OnGrabbed);
        grab.selectExited.AddListener(OnReleased);
    }

    public void subscribeSocketsEvents(XRSocketInteractor xRSocket)
    {
        socket = xRSocket;
        xRSocket.selectEntered.AddListener(OnSocketEntered);
        xRSocket.selectExited.AddListener(OnSocketExit);
    }

    private void OnSocketEntered(SelectEnterEventArgs interactor)
    {
        //Debug.Log("Objeto cogido");

        Data data = new Data("", "", "", "");

        tryFindingLevelManager();

        if (levelManager != null)
        {
            if (!socket.gameObject.transform.root.gameObject.name.Contains("Player Area"))
            {
                data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), levelManager.getActualLevel().ToString(),
                "OBJECT " + interactor.interactableObject.transform.gameObject.name +
                " IN " + socket.gameObject.name + " FROM " + socket.gameObject.transform.root.gameObject.name);
            }
            
        }
        else
        {
            if (!socket.gameObject.transform.root.gameObject.name.Contains("Player Area"))
            {
                data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), "MAIN MENU",
                 "OBJECT " + interactor.interactableObject.transform.gameObject.name +
                " IN " + socket.gameObject.name + " FROM " + socket.gameObject.transform.root.gameObject.name);
            }
            
        }

        if (data.id != "")
        {
            writeInJson(data);
        }

    }

    // Método llamado cuando se suelta el objeto
    private void OnSocketExit(SelectExitEventArgs interactor)
    {
        //Debug.Log("Objeto soltado");

        Data data;

        tryFindingLevelManager();

        if (levelManager != null)
        {
            data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), levelManager.getActualLevel().ToString(),
                "OBJECT RELEASED: " + interactor.interactableObject.transform.gameObject.name.ToString());
        }
        else
        {
            data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                "MAIN MENU", "OBJECT RELEASED: " + interactor.interactableObject.transform.gameObject.name.ToString());
        }

        writeInJson(data);
    }

    private void OnGrabbed(SelectEnterEventArgs interactor)
    {
        //Debug.Log("Objeto cogido");

        Data data;

        tryFindingLevelManager();

        if (levelManager != null)
        {
            data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), levelManager.getActualLevel().ToString(), 
                "OBJECT GRABBED: " + interactor.interactableObject.transform.gameObject.name.ToString());
        }
        else
        {
            data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), "MAIN MENU", 
                "OBJECT GRABBED: " + interactor.interactableObject.transform.gameObject.name.ToString());
        }

        if (popAudio != null)
        {
            popAudio.GetComponent<AudioSource>().Play();
        }

        writeInJson(data);
    }

    // Método llamado cuando se suelta el objeto
    private void OnReleased(SelectExitEventArgs interactor)
    {
        //Debug.Log("Objeto soltado");

        Data data;

        tryFindingLevelManager();

        if (levelManager != null)
        {
            data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), levelManager.getActualLevel().ToString(), 
                "OBJECT RELEASED: " + interactor.interactableObject.transform.gameObject.name.ToString());
        }
        else
        {
            data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), 
                "MAIN MENU", "OBJECT RELEASED: " + interactor.interactableObject.transform.gameObject.name.ToString());
        }

        writeInJson(data);
    }

    // Update is called once per frame
    void Update()
    {
        if (xrOrigin != null)
        {
            xrOrigin.transform.position = new Vector3(xrOrigin.transform.position.x, 0, xrOrigin.transform.position.z);
        }
        

        if (canvas.activeInHierarchy)
        {
            timer += Time.deltaTime;
        }

        if (timer >= timeShowingError)
        {
            canvas.SetActive(false);
            errorText.text = string.Empty;
            timer = 0;
        }
    }

    private void writeInJson(Data data) 
    {
        try
        {
            if (filePath != null && filePath.Length > 0)
            {
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                CleanData(data);
                string jsonString = JsonUtility.ToJson(data);
                StreamWriter writer = new StreamWriter(filePath, true);
                writer.WriteLine(jsonString);

                writer.Close();
            }
        }
        catch (Exception ex)
        {
            //Debug.LogError("Error al escribir en el archivo JSON con Path " +filePath+ ": " + ex.Message);
            errorMessage("Error al escribir en el archivo JSON con Path " + filePath + ": " + ex.Message);
        }
    }

    private void errorMessage(string error)
    {
        canvas.SetActive(true);
        errorText.text = error;
    }

    private void tryFindingLevelManager()
    {
        if (levelManager == null && FindObjectOfType<LevelManager>() != null)
        {
            levelManager = FindObjectOfType<LevelManager>();
        }
    }

    private void OnTeleportationEnd(LocomotionSystem locomotionSystem)
    {
        try
        {
            Data data;

            tryFindingLevelManager();

            if (levelManager != null)
            {
                data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), levelManager.getActualLevel().ToString(), "TELEPORTATION");
            }
            else
            {
                data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), "MAIN MENU", "TELEPORTATION");
            }

            writeInJson(data);
        }
        catch 
        {
            errorMessage("Error trying to write in Json on teleportation");
        }
    }

    public void setUserID(string s)
    {
        try
        {
             userID = s;

            string dataFolderPath = Directory.GetParent(Application.dataPath).FullName;
            directoryPath = Path.Combine(dataFolderPath, "Data");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                Debug.Log("Carpeta 'data' creada en la ruta: " + directoryPath);
            }
            else
            {
                Debug.Log("La carpeta 'data' ya existe en la ruta: " + directoryPath);
            }

            fileName = "data_" + userID + "_" + DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() + ".json";
            filePath = Path.Combine(directoryPath, fileName);

            Data data;

            tryFindingLevelManager();

            if (levelManager != null)
            {
                data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), levelManager.getActualLevel().ToString(), "USER REGISTERED");
            }
            else
            {
                data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), "MAIN MENU", "USER REGISTERED");
            }
            PlayerPrefs.DeleteAll();
            buttonAudio.GetComponent<AudioSource>().Play();
            writeInJson(data);
        }
        catch
        {
            errorMessage("Error trying to write in Json on set user");
        }
    }
        

    public void buttonClicked(string button)
    {
        try
        {
            Data data;

            tryFindingLevelManager();

            if (levelManager != null)
            {
                data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), levelManager.getActualLevel().ToString(), "PRESSED BUTTON: " + button);
            }
            else
            {
                data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), "MAIN MENU", "PRESSED BUTTON: " + button);
            }
            buttonAudio.GetComponent<AudioSource>().Play();
            writeInJson(data);
        }
        catch 
        {
            errorMessage("Error trying to write in Json on button clicked");
        }
    }

    public Data CleanData(Data data)
    {
        data.id = RemoveClone(data.id);
        data.dateTime = RemoveClone(data.dateTime);
        data.actualLevel = RemoveClone(data.actualLevel);
        data.action = RemoveClone(data.action);

        return data;
    }

    private string RemoveClone(string input)
    {
        return input.Replace("(Clone)", "");
    }
}

[Serializable]
public class Data
{
    public string id;
    public string dateTime;
    public string actualLevel;
    public string action;

    // Optional constructor to initialize the data
    public Data(string id, string dateTime, string actualLevel, string action)
    {
        this.id = id;
        this.dateTime = dateTime;
        this.actualLevel = actualLevel;
        this.action = action;
    }
}

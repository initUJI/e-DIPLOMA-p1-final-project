using UnityEngine;
using System.IO;
using System;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class EventsManager : MonoBehaviour
{
    [SerializeField] private GameObject xrOrigin;
    [SerializeField] private GameObject popAudio;
    [SerializeField] private GameObject buttonAudio;

    private XRGrabInteractable[] grabInteractables;
    private string userID = "anom";
    private string fileName = "anom";
    //private LevelManager levelManager;
    private string directoryPath;
    private string filePath;
    private TextMeshProUGUI errorText;
    private GameObject canvas;
    private float timer = 0;
    private float timeShowingError = 10f;
    private XRSocketInteractor socket;
    private Vector3 lastPosition;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        SubscribeToAllSocketEvents();
        SubscribeToAllGrabEvents();
        SetupUI();
        SetupFilePath();
        SetupXROrigin();

        lastPosition = xrOrigin.transform.position;

        Data initialData = new Data(userID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),/* "actualLevel",*/ "action");
        writeInJson(initialData);
    }


    private void SetupUI()
    {
        canvas = transform.GetChild(0).gameObject;
        canvas.SetActive(false);
        errorText = canvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        errorText.text = string.Empty;
    }

    private void SetupFilePath()
    {
        string projectDirectory = Directory.GetParent(Application.dataPath).FullName;
        directoryPath = Path.Combine(projectDirectory, "Data");
        filePath = Path.Combine(directoryPath, "data_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".json");
    }

    private void SetupXROrigin()
    {
        if (xrOrigin == null && GameObject.Find("XR Origin") != null)
        {
            xrOrigin = GameObject.Find("XR Origin");
            TeleportationProvider teleportationProvider = xrOrigin.GetComponent<TeleportationProvider>();
            teleportationProvider.endLocomotion += OnTeleportationEnd;
        }
    }

    public string getUserID() // El nombre de este método se mantiene como "getUserID"
    {
        return userID;
    }

    public void playPressed()
    {
        buttonAudio.GetComponent<AudioSource>().Play();
        RecordEvent("BUTTON PLAY PRESSED");
    }

    public void levelCompleted(int num) // El nombre de este método se mantiene como "levelCompleted"
    {
        Data data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), /*levelManager.getActualLevel().ToString(),*/ $"LEVEL COMPLETED: {num}");
        writeInJson(data);
    }

    public void levelCompleted(string name) // El nombre de este método se mantiene como "levelCompleted"
    {
        Data data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), /*levelManager.getActualLevel().ToString(),*/ $"LEVEL COMPLETED: {name}");
        writeInJson(data);
    }

    public void messageOther(string m) // El nombre de este método se mantiene como "levelCompleted"
    {
        Data data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), /*levelManager.getActualLevel().ToString(),*/ m);
        writeInJson(data);
    }

    public void sceneCleaned()
    {
        buttonAudio.GetComponent<AudioSource>().Play();
        RecordEvent("SCENE CLEANED, ALL BLOCKS RESET");
    }

    public void testStandUsed(string direction)
    {
        buttonAudio.GetComponent<AudioSource>().Play();
        RecordEvent($"PRESSED TEST STAND: {direction}");
    }

    public void characterMoving(string direction)
    {
        RecordEvent($"CHARACTER START MOVING: {direction}");
    }

    public void deleteWindowOpen()
    {
        RecordEvent("DELETE BLOCK WINDOW OPEN");
    }

    public void deleteWindowClose()
    {
        RecordEvent("DELETE BLOCK WINDOW CLOSE");
    }

    public void deleteBlock(GameObject block)
    {
        buttonAudio.GetComponent<AudioSource>().Play();
        RecordEvent($"DELETED BLOCK: {block.name}");
    }

    public void SubscribeToAllGrabEvents()
    {
        // Find all XRGrabInteractable components in the scene
        XRGrabInteractable[] grabInteractables = FindObjectsOfType<XRGrabInteractable>();

        foreach (XRGrabInteractable grab in grabInteractables)
        {
            subscribeGrabEvents(grab);
        }
    }

    public void SubscribeToAllSocketEvents()
    {
        // Find all XRSocketInteractor components in the scene
        XRSocketInteractor[] sockets = FindObjectsOfType<XRSocketInteractor>();

        foreach (XRSocketInteractor xRSocket in sockets)
        {
            subscribeSocketsEvents(xRSocket);
        }
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
    }

    public void buttonClicked(string button)
    {
        try
        {
            Data data;

            /* tryFindingLevelManager();

             if (levelManager != null)
             {
                 data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), levelManager.getActualLevel().ToString(), "PRESSED BUTTON: " + button);
             }
             else
             {
                 data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), "MAIN MENU", "PRESSED BUTTON: " + button);
             }*/
            data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), "PRESSED BUTTON: " + button);
            buttonAudio.GetComponent<AudioSource>().Play();
            writeInJson(data);
        }
        catch
        {
            errorMessage("Error trying to write in Json on button clicked");
        }
    }
    /*private void tryFindingLevelManager()
    {
        if (levelManager == null)
        {
            levelManager = FindObjectOfType<LevelManager>();
        }
    }*/


    private void OnSocketEntered(SelectEnterEventArgs interactor)
    {
        string objectName = interactor.interactableObject.transform.gameObject.name;
        string socketName = socket.gameObject.name;
        string rootName = socket.gameObject.transform.root.gameObject.name;

        if (!rootName.Contains("Player Area"))
        {
            RecordEvent($"OBJECT {objectName} IN {socketName} FROM {rootName}");
        }
    }

    private void OnGrabbed(SelectEnterEventArgs interactor)
    {
        string objectName = interactor.interactableObject.transform.gameObject.name;
        popAudio?.GetComponent<AudioSource>().Play();
        RecordEvent($"OBJECT GRABBED: {objectName}");
    }

    private void OnReleased(SelectExitEventArgs interactor)
    {
        string objectName = interactor.interactableObject.transform.gameObject.name;
        if (objectName != "Platform")
        {
            RecordEvent($"OBJECT RELEASED: {objectName}");
        }
    }

    private void RecordEvent(string action)
    {
        //FindLevelManager();
        //string actualLevel = levelManager != null ? levelManager.getActualLevel().ToString() : "MAIN MENU";
        Data data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), /*actualLevel,*/ action);
        writeInJson(data);
    }

    /*private void FindLevelManager()
    {
        if (levelManager == null)
        {
            levelManager = FindObjectOfType<LevelManager>();
        }
    }*/

    public void setUserID(string id)
    {
        userID = id;

        string dataFolderPath = Directory.GetParent(Application.dataPath).FullName;
        directoryPath = Path.Combine(dataFolderPath, "Data");

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
            Debug.Log("Carpeta 'Data' creada en la ruta: " + directoryPath);
        }

        fileName = "data_" + userID + "_" + DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() + ".json";
        filePath = Path.Combine(directoryPath, fileName);

        Data data;

        /*tryFindingLevelManager();

        if (levelManager != null)
        {
            data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), levelManager.getActualLevel().ToString(), "USER REGISTERED");
        }
        else
        {
            data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), "MAIN MENU", "USER REGISTERED");
        }*/
        data = new Data(userID, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), "USER REGISTERED");
        PlayerPrefs.DeleteAll();
        buttonAudio.GetComponent<AudioSource>().Play();
        writeInJson(data);
    }


    private void writeInJson(Data data)
    {
        try
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                CleanData(data);
                string jsonString = JsonUtility.ToJson(data);
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine(jsonString);
                }
            }
        }
        catch (Exception ex)
        {
            errorMessage($"Error writing to JSON file at {filePath}: {ex.Message}");
        }
    }

    private void errorMessage(string error)
    {
        canvas.SetActive(true);
        errorText.text = error;
        timer = 0; // Reinicia el temporizador para que el mensaje de error se muestre por un tiempo específico
    }


    void Update()
    {
        if (xrOrigin != null)
        {
            xrOrigin.transform.position = new Vector3(xrOrigin.transform.position.x, 0, xrOrigin.transform.position.z);
            // Check if the position has changed significantly to infer completion
            if (Vector3.Distance(lastPosition, xrOrigin.transform.position) > 1f) // Set a threshold for movement
            {
                OnTeleportationEnd();
            }
        }

        if (canvas.activeInHierarchy)
        {
            timer += Time.deltaTime;
            if (timer >= timeShowingError)
            {
                canvas.SetActive(false);
                errorText.text = string.Empty;
                timer = 0;
            }
        }
    }

    private void OnTeleportationEnd(LocomotionSystem locomotionSystem)
    {
        RecordEvent("TELEPORTATION");
    }

    private void OnTeleportationEnd()
    {
        RecordEvent("TELEPORTATION");
    }

    public Data CleanData(Data data)
    {
        data.id = RemoveClone(data.id);
        data.dateTime = RemoveClone(data.dateTime);
        //data.actualLevel = RemoveClone(data.actualLevel);
        data.action = RemoveClone(data.action);
        return data;
    }

    private string RemoveClone(string input) => input.Replace("(Clone)", "");
}

[Serializable]
public class Data
{
    public string id;
    public string dateTime;
    //public string actualLevel;
    public string action;

    public Data(string id, string dateTime, /*string actualLevel,*/ string action)
    {
        this.id = id;
        this.dateTime = dateTime;
        //this.actualLevel = actualLevel;
        this.action = action;
    }
}

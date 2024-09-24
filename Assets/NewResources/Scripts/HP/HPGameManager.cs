using UnityEngine;
using TMPro;

public enum eEstados
{
    Test1, Test2, Test3
}
public class HPGameManager : MonoBehaviour
{
    public static HPGameManager Instance;

    public TMP_Text _csvPath;

    [HideInInspector] public string UserDataFolder = "";
    public string UserID = "FakeUser42"; //This is a value for Debug
    public static eEstados SitActualEnum = eEstados.Test1; //This is a value for Debug

    [HideInInspector] public cDataManager _dataManager;

    private void Awake()
    {
        UserID = GameObject.FindFirstObjectByType<EventsManager>().getUserID();

        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        UserDataFolder = Application.streamingAssetsPath + "/SensorsDataUser"; // Application.dataPath 
        _dataManager = GetComponent<cDataManager>();

        if (_csvPath != null)
        {
            _csvPath.SetText(UserDataFolder);
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class MainMenuController : MonoBehaviour
{
    public Button startButton;

    [SerializeField] private GameObject menuWindow;
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject xrOrigin;
    [SerializeField] private GameObject teleportArea;
    [SerializeField] private GameObject mainBlockPrefab;
    [SerializeField] private GameObject moveForwardPrefab;
    [SerializeField] private GameObject getHumidityPrefab;
    [SerializeField] private GameObject tutorialLevelPrefab;
    [SerializeField] private GameObject testStandPrefab;
    [SerializeField] private GameObject gameManagerPrefab;

    private GameObject MainOptions;
    private GameObject TutorialOptions;
    private GameObject ControlsTutorial;
    private GameObject userOptions;
    [HideInInspector] public XRGrabInteractable grabInteractable;
    private GameObject cube;
    [HideInInspector] public GameObject platform;
    [HideInInspector] public GameObject mainBlock;
    [HideInInspector] public GameObject moveForwardBlock;
    [HideInInspector] public GameObject getHumidityBlock;
    [HideInInspector] public TextMeshPro userInputField;
    [HideInInspector] public bool teleportCompleted;
    [HideInInspector] public bool dynamicTutorialCompleted;
    private Vector3 platformLocation;
    private Vector3 block1Location;
    private Vector3 block2Location;
    private Vector3 block3Location;
    private Vector3 levelLocation;
    private Vector3 testStandLocation;
    private const string TUTORIAL_COMPLETED_KEY = "TutorialCompleted_";

    private EventsManager eventsManager;

    // Start is called before the first frame update
    void Start()
    {
        eventsManager = FindObjectOfType<EventsManager>();

        MainOptions = menuWindow.transform.GetChild(0).gameObject;
        TutorialOptions = menuWindow.transform.GetChild(1).gameObject;
        ControlsTutorial = menuWindow.transform.GetChild(2).gameObject;
        userOptions = menuWindow.transform.GetChild(3).gameObject;

        MainOptions.SetActive(true);
        TutorialOptions.SetActive(false);
        ControlsTutorial.SetActive(false);

        teleportCompleted = false;
        dynamicTutorialCompleted= false;
        platformLocation = new Vector3(0.273f, 0.86f, 0.736f);
        block1Location = new Vector3(0.65f, 0.78f, -0.93f);
        block2Location = new Vector3(-0.3f, 0.8f, -0.82f);
        block3Location = new Vector3(0.13f, 1.0f, -0.92f);
        levelLocation = new Vector3(-0.233f, -0.46f, -0.746f);
        testStandLocation = new Vector3(0.5f, 0.35f, -0.68f);
    }

    private void Update()
    {
        if (IsTutorialCompleted("Controls") && IsTutorialCompleted("Dynamics"))
        {
            startButton.interactable = true;
        }
        else
        {
            startButton.interactable = false;
        }
    }

    public void saveUserId()
    {
        string inputField = userInputField.text;
        PlayerPrefs.SetString("UserID", inputField);
        eventsManager.setUserID(inputField);
    }

    public void loadUserOptions() 
    {
        eventsManager.buttonClicked("USER OPTIONS");
        MainOptions.SetActive(false);
        userOptions.SetActive(true);
    }
    public void startGameplay()
    {
        eventsManager.buttonClicked("START");
        SceneManager.LoadScene(1);
    }

    public void startTutorial()
    {
        eventsManager.buttonClicked("TUTORIAL");
        MainOptions.SetActive(false);
        TutorialOptions.SetActive(true);
    }

    public void startControlsTutorial()
    {
        eventsManager.buttonClicked("CONTROLS TUTORIAL");
        ControlsTutorial.SetActive(true);
        TutorialOptions.SetActive(false);
        cube = Instantiate(cubePrefab);
        grabInteractable = cube.GetComponent<XRGrabInteractable>();
        Instruccion instruccion = new Instruccion();
        StartCoroutine(InvokeTutorialInstruccions(instruccion, ControlsTutorial.transform.GetChild(0).GetComponent<TextMeshProUGUI>()));
    }

    public IEnumerator InvokeTutorialInstruccions(Instruccion ins, TextMeshProUGUI text)
    {
        DynamicTextUpdater dynamicTextUpdater = text.GetComponent<DynamicTextUpdater>();

        text.text = ins.getControlsString(1);
        dynamicTextUpdater.UpdateLocalizedString("controls1");
        yield return new WaitUntil(() => ins.checkControlsInstruccion(1, grabInteractable));

        text.text = ins.getControlsString(2);
        dynamicTextUpdater.UpdateLocalizedString("controls2");
        yield return new WaitUntil(() => ins.checkControlsInstruccion(2, grabInteractable));

        text.text = ins.getControlsString(3);
        platform = Instantiate(platformPrefab);
        dynamicTextUpdater.UpdateLocalizedString("controls3");
        yield return new WaitUntil(() => ins.checkControlsInstruccion(3, grabInteractable, platform));
        yield return new WaitForSeconds(1);

        text.text = ins.getControlsString(4);
        xrOrigin.GetComponent<ActionBasedSnapTurnProvider>().enabled = true;
        TeleportationProvider teleportationProvider = xrOrigin.GetComponent<TeleportationProvider>();
        xrOrigin.GetComponent<TeleportationProvider>().enabled = true;
        teleportationProvider.beginLocomotion += OnTeleportationStart;
        teleportationProvider.endLocomotion += OnTeleportationEnd;
        Destroy(cube);
        Destroy(platform);
        dynamicTextUpdater.UpdateLocalizedString("controls4");
        yield return new WaitUntil(() => ins.checkControlsInstruccion(4, null, null, teleportCompleted));

        text.text = ins.getControlsString(5);
        platform = Instantiate(platformPrefab);
        platform.transform.position = platformLocation;
        cube = Instantiate(cubePrefab);
        dynamicTextUpdater.UpdateLocalizedString("controls5");
        yield return new WaitUntil(() => ins.checkControlsInstruccion(5, grabInteractable, platform));

        text.text = "Tutorial completed!";
        dynamicTextUpdater.UpdateLocalizedString("completedTutorial");
        SaveTutorialState("Controls", true);
    }

    public void startDynamicsTutorial()
    {
        eventsManager.buttonClicked("DYNAMICS TUTORIAL");
        ControlsTutorial.SetActive(true);
        TutorialOptions.SetActive(false);
        Instruccion instruccion = new Instruccion();
        StartCoroutine(InvokeDynamicsInstruccions(instruccion, ControlsTutorial.transform.GetChild(0).GetComponent<TextMeshProUGUI>()));
    }

    public IEnumerator InvokeDynamicsInstruccions(Instruccion ins, TextMeshProUGUI text)
    {
        DynamicTextUpdater dynamicTextUpdater = text.GetComponent<DynamicTextUpdater>();

        text.text = ins.getDynamicsString(1);
        mainBlock = Instantiate(mainBlockPrefab);
        moveForwardBlock = Instantiate(moveForwardPrefab);
        mainBlock.transform.position = block1Location;
        moveForwardBlock.transform.position = block2Location;
        dynamicTextUpdater.UpdateLocalizedString("dynamics1");

        yield return new WaitUntil(() => ins.checkDynamicsInstruccion(1, mainBlock));
        yield return new WaitForSeconds(1);

        Destroy(moveForwardBlock);
        text.text = ins.getDynamicsString(2);
        dynamicTextUpdater.UpdateLocalizedString("dynamics2");
        yield return new WaitUntil(() => ins.checkDynamicsInstruccion(2));

        GameObject testStand = Instantiate(testStandPrefab);
        Destroy(testStand.transform.GetChild(0).gameObject);
        mainBlock = Instantiate(mainBlockPrefab);
        mainBlock.transform.position = block1Location;
        moveForwardBlock = Instantiate(moveForwardPrefab);
        moveForwardBlock.transform.position = block2Location;
        getHumidityBlock = Instantiate(getHumidityPrefab);
        getHumidityBlock.transform.position = block3Location;
        GameObject tutorialLevel = Instantiate(tutorialLevelPrefab);
        GameObject.FindObjectOfType<GameManager>().NewLevel();
        text.text = ins.getDynamicsString(3);
        dynamicTextUpdater.UpdateLocalizedString("dynamics3");
        yield return new WaitUntil(() => ins.checkDynamicsInstruccion(3, mainBlock, dynamicTutorialCompleted));

        Destroy(moveForwardBlock);
        Destroy(getHumidityBlock);
        Destroy(mainBlock);
        Destroy(tutorialLevel);
        Destroy(testStand);

        tutorialLevel = Instantiate(tutorialLevelPrefab);
        testStand = Instantiate(testStandPrefab);
        testStand.transform.position = testStandLocation;
        testStand.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        testStand.AddComponent<ButtonManager>();
        text.text = ins.getDynamicsString(4);
        dynamicTextUpdater.UpdateLocalizedString("dynamics4");
        GameObject.FindObjectOfType<GameManager>().setCharacter(GameObject.FindObjectOfType<Character>());
        GameObject.FindObjectOfType<GameManager>().resetCar();
        yield return new WaitUntil(() => ins.checkDynamicsInstruccion(4, null, false, testStand));

        text.text = "Tutorial completed!";
        dynamicTextUpdater.UpdateLocalizedString("completedTutorial");
        SaveTutorialState("Dynamics", true);
    }

    private void OnTeleportationStart(LocomotionSystem locomotionSystem)
    {
        // Se llama cuando se completa la teletransportación
        Debug.Log("Teleportación empezada");
    }

    private void OnTeleportationEnd(LocomotionSystem locomotionSystem)
    {
        // Se llama cuando se completa la teletransportación
        Debug.Log("Teleportación completada");
        teleportCompleted = true;
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void resetScene()
    {
        eventsManager.buttonClicked("RESET MAIN MENU (HOUSE ICON)");
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    public void SaveTutorialState(string tutorialName, bool isCompleted)
    {
        int value = isCompleted ? 1 : 0;
        PlayerPrefs.SetInt(TUTORIAL_COMPLETED_KEY + tutorialName, value);
        PlayerPrefs.Save();
        Debug.Log($"Tutorial '{tutorialName}' completado: {isCompleted}");
    }

    public bool LoadTutorialState(string tutorialName)
    {
        if (PlayerPrefs.HasKey(TUTORIAL_COMPLETED_KEY + tutorialName))
        {
            return PlayerPrefs.GetInt(TUTORIAL_COMPLETED_KEY + tutorialName) == 1;
        }
        else
        {
            return false; // Si no hay un valor guardado, asumimos que no ha sido completado
        }
    }

    public bool IsTutorialCompleted(string tutorialName)
    {
        return LoadTutorialState(tutorialName);
    }
}

public class Instruccion : MainMenuController
{
    private string controls1 = "Aim at the cube with the controller.\r\n\r\nHold down the controller grip to move the cube.\r\n";
    private string controls2 = "Release the grip to release the cube.";
    private string controls3 = "Move the controller while holding the cube to place it on the platform.";
    private string controls4 = "Aim at the platform floor and press the grip once to move to that location. You can also rotate the view with the left joystick.";
    private string controls5 = "Apply what you have learned to bring the cube to the new platform.";

    private string dynamics1 = "This is the ‘Main block’. It is in charge of executing all the instructions from top to bottom. " +
    "To place a block, simply grab it and pull it underneath the previous block.";
    private string dynamics2 = "To delete a block press the A button to display the delete window. To confirm " +
        "point the delete button with the controller and press the trigger.";
    private string dynamics3 = "The objective will be to drive the car to the plant to collect the humidity from it. "
        + "In this example, we will need a ‘Move forward’ block to move the car forward one square and a ‘Get humidity’ block to collect humidity.";
    private string dynamics4 = "This is the test stand, use it to help you visualise the path before you start programming. Press all the buttons to see what they do.";

    public string getDynamicsString(int num)
    {
        switch (num)
        {
            case 4: return dynamics4;
            case 3: return dynamics3;
            case 2: return dynamics2;
            case 1: return dynamics1;
            default: return "";
        }
    }

    public string getControlsString(int num)
    {
        switch (num)
        {
            case 5: return controls5;
            case 4: return controls4;
            case 3: return controls3;
            case 2: return controls2;
            case 1: return controls1;
            default: return "";
        }
    }

    public bool checkDynamicsInstruccion(int num, GameObject mainBlock = null, bool dynamicTutorialCompleted = false, GameObject testStand = null)
    {
        switch (num)
        {
            case 4: return checkDynamics4(testStand);
            case 3: return checkDynamics3(dynamicTutorialCompleted);
            case 2: return checkDynamics2();
            case 1: return checkDynamics1(mainBlock);
            default: return false;
        }
    }

    public bool checkControlsInstruccion(int num, XRGrabInteractable grab = null, GameObject platform = null, bool teleport = false)
    {
        switch (num)
        {
            case 5: return checkControls5(grab, platform);
            case 4: return checkControls4(teleport);
            case 3: return checkControls3(grab, platform);
            case 2: return checkControls2(grab);
            case 1: return checkControls1(grab);
            default: return false;
        }
    }

    public bool checkDynamics4(GameObject testStand)
    {
        return testStand.GetComponent<ButtonManager>().CheckAllButtonsPressed();
    }

    public bool checkDynamics3(bool dynamicTutorialCompleted)
    {
        return dynamicTutorialCompleted;
    }
    public bool checkDynamics2()
    {
        Block[] blocks = GameObject.FindObjectsOfType<Block>();
        if (blocks.Length <= 0)
        {
            return true;
        }
        return false;
    }

    public bool checkDynamics1(GameObject mainBlock)
    {
        if (mainBlock != null)
        {
            MainBlock mainBlockType = mainBlock.GetComponent<MainBlock>();

            if (mainBlockType.getBottonSocketControled() != null)
            {
                Block nextBlock = mainBlockType.getBottonSocketControled();
                if (nextBlock != null && nextBlock.GetType().ToString() == "ForwardBlock")
                {
                    return true;
                }
            }
            
        }
        return false;
    }

    public bool checkControls5(XRGrabInteractable grab, GameObject platform)
    {
        if (!grab.isSelected && platform != null && platform.GetComponent<platformController>().cubeColliding)
        {
            return true;
        }
        return false;
    }
    public bool checkControls4(bool teleport)
    {
        if (teleport)
        {
            return true;
        }
        return false;
    }

    public bool checkControls3(XRGrabInteractable grab, GameObject platform)
    {
        if (!grab.isSelected && platform != null && platform.GetComponent<platformController>().cubeColliding)
        {
            return true;
        }
        return false;
    }

    public bool checkControls1(XRGrabInteractable grab)
    {
        if (grab.isSelected)
        {
            return true;
        }
        return false;
    }
    public bool checkControls2(XRGrabInteractable grab)
    {
        if (grab.isSelected)
        {
            return false;
        }
        return true;
    }
}

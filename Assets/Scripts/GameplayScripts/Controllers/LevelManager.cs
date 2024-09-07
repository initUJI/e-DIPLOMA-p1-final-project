using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelManager : NumberOfBlocks
{
    private int actualLevel;
    private List<Clue> actualClues;
    private int cluesForLevel = 5;
    private bool clueAvalible = false;

    [SerializeField] private GameObject Level0;
    [SerializeField] private GameObject Level1;
    [SerializeField] private GameObject Level2;
    [SerializeField] private GameObject Level3;
    [SerializeField] private GameObject Level4;
    [SerializeField] private GameObject Level5;
    [SerializeField] private GameObject Level6;
    [SerializeField] private TextMeshProUGUI textPro;
    [SerializeField] private GameObject clueImage;
    [SerializeField] private GameObject noClueImage;
    [SerializeField] private GameObject optionsWindow;
    [SerializeField] private GameObject confettiSystem;
    [SerializeField] private GameObject phone;

    private GameObject optionsInGameplay;
    private GameObject optionsLevelCompleted;
    private EventsManager eventsManager;

    private GameManager gameManager;
    private float timerForClues = 0;
    private float timeBetweenClues = 180f;
    private float allLevelTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        ShelfController[] shelfControllers = FindObjectsOfType<ShelfController>();
        foreach (ShelfController controller in shelfControllers)
        {
            controller.levelManager = this;
        }

        optionsInGameplay = optionsWindow.transform.GetChild(0).gameObject;
        optionsLevelCompleted = optionsWindow.transform.GetChild(1).gameObject;
        eventsManager = FindObjectOfType<EventsManager>();

        optionsInGameplay.SetActive(true);
        optionsLevelCompleted.SetActive(false);

        clueImage.SetActive(false);
        noClueImage.SetActive(true);
        gameManager = FindObjectOfType<GameManager>();
        actualLevel = lastLevelCompleted() + 1;

        /*saveCompletedLevel(1);
        saveCompletedLevel(2);
        saveCompletedLevel(3);
        saveCompletedLevel(4);
        saveCompletedLevel(5);
        saveCompletedLevel(6);*/

        changeLevel(lastLevelCompleted() + 1);
        //changeLevel(3);
    }


    private void Update()
    {
        timerForClues += Time.deltaTime;
        if (timerForClues >= timeBetweenClues)
        {
            clueAvalible = true;
            clueImage.SetActive(true);
            noClueImage.SetActive(false);
            timerForClues = 0;
        }

        if (!checkCompletedLevel(actualLevel))
        {
            allLevelTimer += Time.deltaTime;
        }

        else if (!optionsLevelCompleted.activeInHierarchy)
        {
            optionsInGameplay.SetActive(false);
            optionsLevelCompleted.SetActive(true);

            optionsLevelCompleted.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Math.Round(allLevelTimer).ToString()  + " s";

            phone.GetComponent<AudioSource>().Play();

            CenterWallController centerWallController = GameObject.FindObjectOfType<CenterWallController>();

            if (!centerWallController.wallUP)
            {
                centerWallController.centerWallUp();
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            resetAllsaves();
        }
    }

    public void goToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void resetScene()
    {
        eventsManager.buttonClicked("NEXT LEVEL");
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public int getActualLevel()
    {
        return actualLevel;
    }

    public void SetLevelAndChange(int level)
    {
        GameObject.Find("OptionsFinish").SetActive(false);
        GameObject.Find("OptionsWindow").transform.GetChild(0).gameObject.SetActive(false);
        deleteCompletedLevel(level);
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + 2);
        changeLevel(level, "Button");
    }

    public void changeLevel(int num, string source = null)
    {

        if (source == "Button")
        {
            eventsManager.buttonClicked("LEVEL:" + num.ToString());
        }

        Block[] blocks = GameObject.FindObjectsOfType<Block>();

        foreach (Block block in blocks)
        {
            if (block.GetType().ToString() != "MainBlock")
            {
                Destroy(block);
            }
        }

        switch (num)
        {
            case 7: blocksNo(); changeToOptionsFinish(); actualLevel = 7; break;
            case 6: blocksForLevel6(); actualLevel = 6; break;
            case 5: blocksForLevel5(); actualLevel = 5; break;
            case 4: blocksForLevel4(); actualLevel = 4; break;
            case 3: blocksForLevel3(); actualLevel = 3; break;
            case 2: blocksForLevel2(); actualLevel = 2; break;
            case 1: blocksForLevel1(); actualLevel = 1; break;
            case 0: blocksForTest(); actualLevel = 0; break;
        }

        createLevel(actualLevel);
    }

    public void changeToOptionsFinish()
    {
        optionsWindow.transform.GetChild(0).gameObject.SetActive(false);
        optionsWindow.transform.GetChild(1).gameObject.SetActive(false);
        optionsWindow.transform.GetChild(2).gameObject.SetActive(true);
    }

    public void resetLevelBlocks()
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");

        foreach (GameObject block in blocks)
        {
            if (!block.name.Contains("Main"))
            {
                Destroy(block);
            }
            
        }
        switch (actualLevel)
        {
            case 6: blocksForLevel6(); break;
            case 5: blocksForLevel5(); break;
            case 4: blocksForLevel4(); break;
            case 3: blocksForLevel3(); break;
            case 2: blocksForLevel2(); break;
            case 1: blocksForLevel1(); break;
            case 0: blocksForTest(); break;
        }

        ShelfController[] shelves = FindObjectsOfType<ShelfController>();
        foreach (ShelfController s in shelves)
        {
            s.startCreatingBlocks();
        }

        eventsManager.sceneCleaned();
    }

    private void createLevel(int num)
    {
        GameObject level = gameObject;

        switch (num)
        {
            case 6: level = Instantiate(Level6); break;
            case 5: level = Instantiate(Level5); break;
            case 4: level = Instantiate(Level4); break;
            case 3: level = Instantiate(Level3); break;
            case 2: level = Instantiate(Level2); break;
            case 1: level = Instantiate(Level1); break;
            default: break;
        }

        level.transform.SetParent(this.transform);
        level.transform.localPosition = Vector3.zero;

        ShelfController[] shelves = FindObjectsOfType<ShelfController>();
        foreach (ShelfController s in shelves)
        {
            s.startCreatingBlocks();
        }
        selectClues();
        gameManager.NewLevel();
    }

    public void saveCompletedLevel(int num)
    {
        switch (num)
        {
            case 6: PlayerPrefs.SetInt("Level6Completed", 1); break;
            case 5: PlayerPrefs.SetInt("Level5Completed", 1); break;
            case 4: PlayerPrefs.SetInt("Level4Completed", 1); break;
            case 3: PlayerPrefs.SetInt("Level3Completed", 1); break;
            case 2: PlayerPrefs.SetInt("Level2Completed", 1); break;
            case 1: PlayerPrefs.SetInt("Level1Completed", 1); break;
        }
        if (confettiSystem != null)
        {
            confettiSystem.GetComponent<ParticleSystem>().Play();
            confettiSystem.GetComponent<AudioSource>().Play();
        }
        eventsManager.levelCompleted(num);
    }

    public void deleteCompletedLevel(int num)
    {
        switch (num)
        {
            case 6: PlayerPrefs.SetInt("Level6Completed", 0); break;
            case 5: PlayerPrefs.SetInt("Level5Completed", 0); break;
            case 4: PlayerPrefs.SetInt("Level4Completed", 0); break;
            case 3: PlayerPrefs.SetInt("Level3Completed", 0); break;
            case 2: PlayerPrefs.SetInt("Level2Completed", 0); break;
            case 1: PlayerPrefs.SetInt("Level1Completed", 0); break;
        }
    }

    public bool checkCompletedLevel(int num)
    {
        switch (num)
        {
            case 6: return PlayerPrefs.GetInt("Level6Completed", 0) == 1; 
            case 5: return PlayerPrefs.GetInt("Level5Completed", 0) == 1; 
            case 4: return PlayerPrefs.GetInt("Level4Completed", 0) == 1;
            case 3: return PlayerPrefs.GetInt("Level3Completed", 0) == 1;
            case 2: return PlayerPrefs.GetInt("Level2Completed", 0) == 1;
            case 1: return PlayerPrefs.GetInt("Level1Completed", 0) == 1; 
            default: return false;
        }
    }

    public void resetAllsaves()
    {
        PlayerPrefs.DeleteAll();
    }

    public int lastLevelCompleted()
    {
        if (checkCompletedLevel(6)) return 6;
        else if (checkCompletedLevel(5)) return 5;
        else if (checkCompletedLevel(4)) return 4;
        else if (checkCompletedLevel(3)) return 3;
        else if (checkCompletedLevel(2)) return 2;
        else if (checkCompletedLevel(1)) return 1;
        return 0;
    }

    private void selectClues()
    {
        switch (actualLevel)
        {
            case 6: actualClues = getClues(6); break;
            case 5: actualClues = getClues(5); break;
            case 4: actualClues = getClues(4); break;
            case 3: actualClues = getClues(3); break;
            case 2: actualClues = getClues(2); break;
            case 1: actualClues = getClues(1); break;
            default: break;
        }
    }

    private List<Clue> getClues(int level)
    {
        List<Clue> clues = new List<Clue>();

        List<Clue> cluesList = createClues();

        int i = 0;
        foreach (Clue c in cluesList)
        {
            if (c.level == level)
            {
                clues.Add(c);
                i++;
            }
        }

        return clues;
    }

    private List<Clue> createClues()
    {
        List<Clue> clues = new List<Clue>();
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < cluesForLevel; j++)
            {
                Clue c = new Clue();
                c.level = i + 1;
                c.clue = c.getString(c.level, j+1);
                clues.Add(c);
            }
        }
        return clues;
    }

    public void showClue()
    {
        DynamicTextUpdater dynamicTextUpdater = textPro.GetComponent<DynamicTextUpdater>();

        if (clueAvalible)
        {
            foreach (Clue c in actualClues)
            {
                if (!c.used)
                {
                    textPro.text = c.clue;
                    c.used = true;
                    dynamicTextUpdater.UpdateLocalizedString("level" + actualLevel.ToString() + "clue" + (actualClues.IndexOf(c) + 1).ToString());
                    break;
                }
            }
            clueAvalible = false;
            clueImage.SetActive(false);
            noClueImage.SetActive(true);

            
        }
        else
        {
            textPro.text = "No clue avalible";
            dynamicTextUpdater.UpdateLocalizedString("noClue");
        }

        eventsManager.clueUsed(textPro.text);
    }
}

public class Clue: MonoBehaviour
{
    public int level;
    public string clue;
    public bool used = false;

    private string level1clue1 = "The blocks are executed sequentially starting from main.";
    private string level1clue2 = "To move the car forward, the \"Move forward\" block needs to be used.";
    private string level1clue3 = "More than one block of the same type needs to be used.";
    private string level1clue4 = "\"Get humidity\" is the last block that needs to be used.";
    private string level1clue5 = "First, forward movement to the plant is needed. Then the humidity is collected.";

    private string level2clue1 = "It needs to be rotated at some point.";
    private string level2clue2 = "Obstacles need to be avoided.";
    private string level2clue3 = "A right turn is needed at some point.";
    private string level2clue4 = "\"Get humidity\" is the last block that needs to be used.";
    private string level2clue5 = "Move forward, turn and move forward again.";

    private string level3clue1 = "Note how many times the \"Move forward\" block needs to be executed.";
    private string level3clue2 = "The content between the repeat block and its end will be executed the specified number of times.";
    private string level3clue3 = "The end which corresponds to the repeat has the same colour.";
    private string level3clue4 = "\"Get humidity\" is the last block that needs to be used.";
    private string level3clue5 = "Repeat \"forward\" until you reach the plant, then pick up the humidity.";

    private string level4clue1 = "The content between \"if\" and its end is only executed if the condition is satisfied.";
    private string level4clue2 = "The end which corresponds to the if has the same colour.";
    private string level4clue3 = "Turn around if an obstacle is encountered.";
    private string level4clue4 = "\"Get humidity\" is the last block that needs to be used.";
    private string level4clue5 = "Repeat \"forward\" until you reach the plant, but turning if an obstacle is encountered, then pick up the humidity.";

    private string level5clue1 = "More than one rotation is needed.";
    private string level5clue2 = "\"Get humidity\" is not the last block that needs to be used.";
    private string level5clue3 = "The same sequence of movement is repeated.";
    private string level5clue4 = "The \"if\" block is not needed.";
    private string level5clue5 = "Repeat \"forward\" until you reach the plant, pick up the humidity and turn.";

    private string level6clue1 = "The shortest way is not always the right way.";
    private string level6clue2 = "\"Get humidity\" is the last block that needs to be used.";
    private string level6clue3 = "The same sequence of movement is repeated.";
    private string level6clue4 = "Need to check if there is an obstacle more than once.";
    private string level6clue5 = "Repeat turn if there is an obstacle and move forward.";
    
    public string getString(int level, int clue)
    {
        string stringToReturn = "";
        switch (level)
        {
            case 6:
                switch (clue)
                {
                    case 5:
                        stringToReturn = level6clue5;
                        break;
                    case 4:
                        stringToReturn = level6clue4;
                        break;
                    case 3:
                        stringToReturn = level6clue3;
                        break;
                    case 2:
                        stringToReturn = level6clue2;
                        break;
                    case 1:
                        stringToReturn = level6clue1;
                        break;
                    default:
                        break;
                }
                break;
            case 5:
                switch (clue)
                {
                    case 5:
                        stringToReturn = level5clue5;
                        break;
                    case 4:
                        stringToReturn = level5clue4;
                        break;
                    case 3:
                        stringToReturn = level5clue3;
                        break;
                    case 2:
                        stringToReturn = level5clue2;
                        break;
                    case 1:
                        stringToReturn = level5clue1;
                        break;
                    default:
                        break;
                }
                break;
            case 4:
                switch (clue)
                {
                    case 5:
                        stringToReturn = level4clue5;
                        break;
                    case 4:
                        stringToReturn = level4clue4;
                        break;
                    case 3:
                        stringToReturn = level4clue3;
                        break;
                    case 2:
                        stringToReturn = level4clue2;
                        break;
                    case 1:
                        stringToReturn = level4clue1;
                        break;
                    default:
                        break;
                }
                break;
            case 3:
                switch (clue)
                {
                    case 5:
                        stringToReturn = level3clue5;
                        break;
                    case 4:
                        stringToReturn = level3clue4;
                        break;
                    case 3:
                        stringToReturn = level3clue3;
                        break;
                    case 2:
                        stringToReturn = level3clue2;
                        break;
                    case 1:
                        stringToReturn = level3clue1;
                        break;
                    default:
                        break;
                }
                break;
            case 2:
                switch (clue)
                {
                    case 5:
                        stringToReturn = level2clue5;
                        break;
                    case 4:
                        stringToReturn = level2clue4;
                        break;
                    case 3:
                        stringToReturn = level2clue3;
                        break;
                    case 2:
                        stringToReturn = level2clue2;
                        break;
                    case 1:
                        stringToReturn = level2clue1;
                        break;
                    default:
                        break;
                }
                break;
            case 1:
                switch (clue)
                {
                    case 5:
                        stringToReturn = level1clue5;
                        break;
                    case 4:
                        stringToReturn = level1clue4;
                        break;
                    case 3:
                        stringToReturn = level1clue3;
                        break;
                    case 2:
                        stringToReturn = level1clue2;
                        break;
                    case 1:
                        stringToReturn = level1clue1;
                        break;
                    default:
                        break;
                }
                break;
            default: 
                break;
        }
        return stringToReturn;
    }
}

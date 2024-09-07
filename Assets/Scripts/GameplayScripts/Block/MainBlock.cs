using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;
//using Unity.Burst.Intrinsics;
//using System.Net.Sockets;
//using UnityEngine.UIElements.Experimental;
//using static UnityEditor.Sprites.Packer;

/*
 * MainBlock is associated with the game object of the same name. 
 * This is the starting point for the game. It is under this component that the user adds blocks. 
 * A button is provided to launch the execution of the algorithm thus created. 
 * It is also possible to pause the execution of the algorithm with an other button.
 */
public class MainBlock : Block, WithBottomSocket
{

    // Globals variables.
    public static Dictionary<string, int> variables;

    [SerializeField] XRSocketInteractor bottomSocket;
    [SerializeField] GameObject canvasFail;
    [HideInInspector] public ExecutableBlock currentBlock;
    [HideInInspector] public bool wasIfBlock = false;
    [HideInInspector] public bool ifConditionChecked = false;
    [HideInInspector] public bool allCorrect = false;

    public static bool paused;
    public static bool error;

    Coroutine currentCoroutine;

    [HideInInspector] public bool activeIf = false;
    [HideInInspector] public IfBlock ifBlock;

    bool activeFor = false;
    ForBlock forBlock;
    EventsManager eventsManager;
    int forBloks = 0;
    int endForBlocks = 0;
    int ifBlocks = 0;
    int endIfBlocks = 0;

    public XRSocketInteractor getBottomSocket()
    {
        return bottomSocket;
    }

    public override void Start() {
        base.Start();
        variables = new Dictionary<string, int>();
        paused = false;
        eventsManager = FindObjectOfType<EventsManager>();
        canvasFail.SetActive(false);
        //Execute();
    }

    void Update()
    {
        //test
        if (Input.GetKeyDown(KeyCode.P))
        {
            Execute();
        }
    }

    /* Calls the c_Execute coroutine */
    public void Execute() {

        if (!paused)
        {
            GameManager.DisplayOnPrompt("Execution !");
            error = false;
            variables = new Dictionary<string, int>();

            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            eventsManager.playPressed();
            GameManager.Reset();
            currentCoroutine = StartCoroutine(c_Execute());

        } else
        {
            GameManager.ReportError(this, "You cannot execute or re-execute when the pause is active");
        }

    }

    /* Go from bottom socket to bottom socket through the blocks.
     * Between each execution two pauses are made, 
     * one to wait for the end of the previous execution, 
     * and another as long as the pause attribute is not equal to false. 
     * In this way, the user has time to see the character perform each step and has the option of pausing.
     */
    public ExecutableBlock getBottonSocketControled()
    {
        return (ExecutableBlock)getSocketBlock(bottomSocket);
    }

    public IEnumerator c_Execute() {

        
        bool executingBlock = false;

        SetGlowing(true);

        //Debug.Log("MainBlock : We wait 1 second...");
        yield return new WaitForSeconds(1);


        Debug.Log("MainBlock : BEGIN");
        GameManager.character_P1.activeGlow();
        currentBlock = (ExecutableBlock) getSocketBlock(bottomSocket);
        forBloks = 0;
        endForBlocks = 0;
        ifBlocks = 0;
        endIfBlocks = 0;


        while (currentBlock != null && !error)
        {
            GameManager.currentBlock = currentBlock;

            currentBlock.SetGlowing(true);
            //Debug.Log(currentBlock.name);

            yield return new WaitUntil(() => !paused); // Wait until pause equals false
                                                       //Debug.Log("MainBlock : Next block !");

            if (currentBlock as ForBlock)
            {
                activeFor = true;
                forBloks++;
                forBlock = (ForBlock)currentBlock;
                currentBlock.Execute(variables);
                executingBlock = true;
            }

            else if (currentBlock as IfBlock)
            {
                ifBlocks++;
                wasIfBlock = true;
                ifConditionChecked = false;
                ifBlock = (IfBlock)currentBlock;
                if (ifBlock.conditionChecked(variables))
                {
                    ifConditionChecked = true;
                    //ifBlock.Execute(variables);
                    activeIf = true;
                } else
                {
                    ifBlock.isFinished = true;
                }
            }
            else if (currentBlock.name.Contains("EndIf"))
            {
                activeIf = false;
                ifBlock = null;
                endIfBlocks++;
                //currentBlock.Execute(variables);
            }
            else if (currentBlock as EndForBlock)
            {
                
                activeFor = false;
                endForBlocks++;
                forBlock = null;
                //currentBlock.Execute(variables);
            }
            else if (currentBlock as ElseBlock) {
                ElseBlock elseBlock = (ElseBlock) currentBlock;
                if (wasIfBlock)
                {
                    if(!ifConditionChecked)
                    {
                        elseBlock.Execute(variables);
                    } else
                    {
                        elseBlock.isFinished = true;
                    }
                } else
                {
                    GameManager.ReportError(elseBlock, "An else block must be preceded by an if block");
                }
            } else {
                if ((activeIf && ifBlock != null || !activeIf && ifBlock == null) && (forBlock == null && !activeFor))
                {
                    currentBlock.Execute(variables); // Execute the current block
                    executingBlock = true;
                }

            }

            //Debug.Log("W");
            yield return new WaitUntil(() => !error);
            if (executingBlock)
            {
                yield return new WaitUntil(() => currentBlock.IsFinished()); // Wait until the end of the previous block;
                executingBlock = false;
            }
            
            //Debug.Log("B");

            currentBlock.SetGlowing(false);

            yield return new WaitForSeconds(0.5f);
            yield return new WaitUntil(() => GameObject.FindObjectOfType<Character>().Motionless());
            currentBlock = (ExecutableBlock)currentBlock.getSocketBlock(((WithBottomSocket)currentBlock).getBottomSocket()); // Go to the next block  
            //Debug.Log(currentBlock);
        }

        SetGlowing(false);

        if (allCorrect && IsCollidingWithAny(GameManager.character_P1.gameObject, ConvertPlantListToGameObjectList(FindObjectsOfType<Plant>())) &&
            forBloks == endForBlocks && ifBlocks == endIfBlocks)
        {
            completeLevel();
        }
        else
        {
            canvasFail.SetActive(true);
        }

        GameManager.character_P1.desactiveGlow();
        Debug.Log("MainBlock : END");

    }

    public void TooglePause()
    {
        paused = !paused;
    }

    private void completeLevel()
    {
        //levelcompleted
        LevelManager lm = FindObjectOfType<LevelManager>();
        lm.saveCompletedLevel(lm.getActualLevel());
    }

    List<GameObject> ConvertPlantListToGameObjectList(Plant[] plants)
    {
        List<GameObject> gameObjects = new List<GameObject>();

        foreach (Plant plant in plants)
        {
            if (plant != null)
            {
                gameObjects.Add(plant.gameObject);
            }
        }

        return gameObjects;
    }

    bool IsCollidingWithAny(GameObject target, List<GameObject> objects)
    {
        Collider targetCollider = target.GetComponent<Collider>();

        if (targetCollider == null)
        {
            Debug.LogError("Target object does not have a collider.");
            return false;
        }

        foreach (GameObject obj in objects)
        {
            if (obj == null)
            {
                continue;
            }

            Collider objCollider = obj.GetComponent<Collider>();
            if (objCollider == null)
            {
                Debug.LogWarning("Object in the list does not have a collider: " + obj.name);
                continue;
            }

            if (targetCollider.bounds.Intersects(objCollider.bounds))
            {
                return true;
            }
        }

        return false;
    }
}

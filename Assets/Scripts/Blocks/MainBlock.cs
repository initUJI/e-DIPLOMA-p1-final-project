using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

/*
 * MainBlock is associated with the game object of the same name.
 * This class controls the main logic for executing algorithmic blocks, including loops and conditionals.
 * It handles execution, pausing, and failure handling during algorithm runs.
 */
public class MainBlock : Block, WithBottomSocket
{
    public static bool error;

    // Public variables to be set in the Unity inspector
    [SerializeField] XRSocketInteractor bottomSocket;
    //[SerializeField] GameObject canvasFail;

    // Internal state management variables
    [HideInInspector] public ExecutableBlock currentBlock;
    [HideInInspector] public bool wasIfBlock = false;
    [HideInInspector] public bool ifConditionChecked = false;
    [HideInInspector] public bool allCorrect = false;

    Coroutine currentCoroutine;
    [HideInInspector] public bool activeIf = false;
    [HideInInspector] public IfBlock ifBlock;
    public UnityEvent onStart = new UnityEvent();

    private GameManager_V2 gameManager;

    bool activeFor = false;
    ForBlock forBlock;

    int forBlocks = 0;  // Track the number of active "For" blocks
    int endForBlocks = 0; // Track the number of completed "For" blocks
    int ifBlocks = 0;  // Track the number of active "If" blocks
    int endIfBlocks = 0;  // Track the number of completed "If" blocks

    public void InitDebugFunction()
    {
        Debug.Log("Se ha a�adido algo al socket");
    }

    // Get the bottom socket for connecting blocks
    public XRSocketInteractor getBottomSocket()
    {
        return bottomSocket;
    }

    public void setGameManager(GameManager_V2 gameManager)
    {
        this.gameManager = gameManager;
    }

    // Initialization logic
    public override void Start()
    {
        base.Start();

        /*if (canvasFail != null)
        {
            canvasFail.SetActive(false);  // Hide failure message by default
        }*/
    }

    /* Start executing the blocks from the beginning.
     * This function checks for pausing, initializes variables, and starts the execution coroutine.
     */
    public void Execute()
    {
        // Primero se comprueba que la secuencia no tiene ningún error (que no falta ninguna cosa tipo If-EndIf, etc)
        if (gameManager.CheckIfLocalSecuenceIsCorrect())
        {
            gameManager.setXRInteractionNewState(false);
            onStart.Invoke();
        }
    }

    // Coroutine to handle the block execution process step-by-step with delays
    public List<string> getString()
    {
        List<string> cadena = new List<string>();
        currentBlock = (ExecutableBlock)getSocketBlock(bottomSocket);  // Get the first block connected to the socket

        // Main loop to read each block in the sequence
        while (currentBlock != null && !error)
        {
            string currentBlockName = currentBlock.name.Replace("(Clone)", "").Trim();
            cadena.Add(currentBlockName);

            //Aqui se van a tratar los casos especiales
            if(currentBlockName == "For")
            {
                string possibleRightSocket = currentBlock.gameObject.GetComponent<ForBlock>().getRightSocketName();

                if(possibleRightSocket != null)
                {
                    cadena.Add(possibleRightSocket.Replace("(Clone)", "").Trim());
                }

            }
            else if(currentBlockName == "Turn")
            {
                string possibleRightSocket = currentBlock.gameObject.GetComponent<TurnBlock>().getRightSocketString();

                if (possibleRightSocket != null)
                {
                    cadena.Add(possibleRightSocket.Replace("(Clone)", "").Trim());
                }
            }

            currentBlock = (ExecutableBlock)currentBlock.getSocketBlock(((WithBottomSocket)currentBlock).getBottomSocket());  // Move to the next block 
        }

        return cadena;
    }

    // Coroutine to handle the block execution process step-by-step with delays
    public IEnumerator c_Execute()
    {
        bool executingBlock = false;

        yield return new WaitForSeconds(1);  // Initial wait

        currentBlock = (ExecutableBlock)getSocketBlock(bottomSocket);  // Get the first block connected to the socket

        ResetBlockCounters();

        // Main loop to execute each block in the sequence
        while (currentBlock != null && !error)
        {
            HandleBlockExecution(ref executingBlock);

            yield return new WaitUntil(() => !error);  // Wait until there's no error
            if (executingBlock)
            {
                yield return new WaitUntil(() => currentBlock.IsFinished());  // Wait for the block to finish
                executingBlock = false;
            }

            yield return new WaitForSeconds(0.5f);  // Delay before moving to the next block
            //yield return new WaitUntil(() => GameManager.character.Motionless());  // Wait for character to be still

            currentBlock = (ExecutableBlock)currentBlock.getSocketBlock(((WithBottomSocket)currentBlock).getBottomSocket());  // Move to the next block
        }

        EndExecution();
    }

    public void ClearPreviousBlocks()
    {
        // Recopilar bloques y eliminarlos
        List<GameObject> playerBlocks = new List<GameObject>();

        currentBlock = (ExecutableBlock)getSocketBlock(bottomSocket);  // Get the first block connected to the socket

        // Main loop to read each block in the sequence
        while (currentBlock != null && !error)
        {
            playerBlocks.Add(currentBlock.gameObject);
            string currentBlockName = currentBlock.name.Replace("(Clone)", "").Trim();


            //Aqui se van a tratar los casos especiales
            if (currentBlockName == "For")
            {
                GameObject possibleRightSocket = currentBlock.gameObject.GetComponent<ForBlock>().getRightSocketObject();

                if (possibleRightSocket != null)
                {
                    playerBlocks.Add(possibleRightSocket);
                }

            }
            else if (currentBlockName == "Turn")
            {
                GameObject possibleTurnRightSocket = currentBlock.gameObject.GetComponent<TurnBlock>().getRightSocketObject();

                if (possibleTurnRightSocket != null)
                {
                    playerBlocks.Add(possibleTurnRightSocket);
                }
            }

            currentBlock = (ExecutableBlock)currentBlock.getSocketBlock(((WithBottomSocket)currentBlock).getBottomSocket());  // Move to the next block
        }

        //Se destruyen los cosos
        foreach(GameObject currentObj in playerBlocks)
        {
            Destroy(currentObj);
        }
    }

    // Reset block counters for loops and conditionals
    private void ResetBlockCounters()
    {
        forBlocks = 0;
        endForBlocks = 0;
        ifBlocks = 0;
        endIfBlocks = 0;
    }

    // Handle the execution logic of individual blocks (If, For, Else)
    private void HandleBlockExecution(ref bool executingBlock)
    {
        if (currentBlock is ForBlock)
        {
            activeFor = true;
            forBlocks++;
            forBlock = (ForBlock)currentBlock;
            currentBlock.Execute();
            executingBlock = true;
        }
        else if (currentBlock is IfBlock)
        {
            ifBlocks++;
            wasIfBlock = true;
            ifConditionChecked = false;
            ifBlock = (IfBlock)currentBlock;

            if (ifBlock.conditionChecked())
            {
                ifConditionChecked = true;
                activeIf = true;
            }
            else
            {
                ifBlock.isFinished = true;
            }
        }
        else if (currentBlock.name.Contains("EndIf"))
        {
            activeIf = false;
            ifBlock = null;
            endIfBlocks++;
        }
        else if (currentBlock is EndForBlock)
        {
            activeFor = false;
            endForBlocks++;
            forBlock = null;
        }
        else
        {
            if ((activeIf && ifBlock != null || !activeIf && ifBlock == null) && (forBlock == null && !activeFor))
            {
                currentBlock.Execute();
                executingBlock = true;
            }
        }
    }

    // End execution and handle completion checks
    private void EndExecution()
    {
        if (allCorrect && forBlocks == endForBlocks && ifBlocks == endIfBlocks)
        {
            //todo ok se acaba el nivel
        }
        /*else if (canvasFail != null)
        {
            canvasFail.SetActive(true);  // Show failure canvas
        }*/

        Debug.Log("MainBlock : END");
    }
}


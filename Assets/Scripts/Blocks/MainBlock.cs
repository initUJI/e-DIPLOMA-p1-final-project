using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/*
 * MainBlock is associated with the game object of the same name.
 * This class controls the main logic for executing algorithmic blocks, including loops and conditionals.
 * It handles execution, pausing, and failure handling during algorithm runs.
 */
public class MainBlock : Block, WithBottomSocket
{
    public static bool paused;
    public static bool error;

    // Public variables to be set in the Unity inspector
    [SerializeField] XRSocketInteractor bottomSocket;
    [SerializeField] GameObject canvasFail;

    // Internal state management variables
    [HideInInspector] public ExecutableBlock currentBlock;
    [HideInInspector] public bool wasIfBlock = false;
    [HideInInspector] public bool ifConditionChecked = false;
    [HideInInspector] public bool allCorrect = false;

    Coroutine currentCoroutine;
    [HideInInspector] public bool activeIf = false;
    [HideInInspector] public IfBlock ifBlock;

    bool activeFor = false;
    ForBlock forBlock;

    int forBlocks = 0;  // Track the number of active "For" blocks
    int endForBlocks = 0; // Track the number of completed "For" blocks
    int ifBlocks = 0;  // Track the number of active "If" blocks
    int endIfBlocks = 0;  // Track the number of completed "If" blocks

    // Get the bottom socket for connecting blocks
    public XRSocketInteractor getBottomSocket()
    {
        return bottomSocket;
    }

    // Initialization logic
    public override void Start()
    {
        base.Start();
        paused = false;

        if (canvasFail != null)
        {
            canvasFail.SetActive(false);  // Hide failure message by default
        }
    }

    /* Start executing the blocks from the beginning.
     * This function checks for pausing, initializes variables, and starts the execution coroutine.
     */
    public void Execute()
    {
        if (!paused)
        {
            error = false;  // Reset error flag

            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);  // Stop any previous executions
            }

            currentCoroutine = StartCoroutine(c_Execute());  // Start the coroutine for block execution
        }
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
            yield return new WaitUntil(() => !paused);  // Wait until the game is unpaused

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
            completeLevel();
        }
        else if (canvasFail != null)
        {
            canvasFail.SetActive(true);  // Show failure canvas
        }

        Debug.Log("MainBlock : END");
    }

    // Toggle pause state
    public void TooglePause()
    {
        paused = !paused;
    }

    // Complete the level if all conditions are met
    private void completeLevel()
    {

    }

    // Check if the target object is colliding with any objects in the list
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
            if (obj == null) continue;

            Collider objCollider = obj.GetComponent<Collider>();
            if (objCollider == null)
            {
                Debug.LogWarning($"Object in the list does not have a collider: {obj.name}");
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


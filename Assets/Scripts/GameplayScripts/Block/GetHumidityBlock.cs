
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class GetHumidityBlock : ActionCharacterBlock, WithRightSocket
{
    [SerializeField] protected XRSocketInteractor rightSocket;
    private Plant[] plants;

    public XRSocketInteractor getRightSocket()
    {
        return rightSocket;
    }

    public override void Execute(Dictionary<string, int> variables)
    {
        plants = GameObject.FindObjectsOfType<Plant>();
        StartCoroutine(c_Execute(variables));
    }

    IEnumerator c_Execute(Dictionary<string, int> variables)
    {
        isFinished = false;
        bool solved = true;

        foreach (Plant p in plants)
        {
            if ((SceneManager.GetActiveScene().buildIndex == 0))
            {
                yield return new WaitUntil(() => p.characterInPlant());
            }
            yield return new WaitForSeconds(1f);
            if (p.characterInPlant())
            {
                p.humidityChecked = true;
                character.activeWater();
                if (allPlantsChecked())
                {
                    //Debug.Log(SceneManager.GetActiveScene().buildIndex);
                    if (SceneManager.GetActiveScene().buildIndex == 0)
                    {
                        MainMenuController mMC = FindObjectOfType<MainMenuController>();
                        mMC.dynamicTutorialCompleted = true;
                    }
                    else
                    {
                        if (solved && AreBlocksCorrectlyPaired())
                        {
                            //levelcompleted
                            //LevelManager lm = FindObjectOfType<LevelManager>();
                            //lm.saveCompletedLevel(lm.getActualLevel());
                            FindObjectOfType<MainBlock>().allCorrect = true;
                        }
                    }                   
                }
            }
            else if (!IsCollidingWithAny(character.gameObject, ConvertPlantListToGameObjectList(plants)))
            {
                solved = false;
            }
        }

        yield return new WaitUntil(() => base.IsFinished());
        isFinished = true;
    }

    private bool allPlantsChecked()
    {
        foreach (Plant p in plants)
        {
            if (!p.humidityChecked)
            {
                return false;
            }
        }
        return true;
    }

    public override bool IsFinished()
    {
        return base.IsFinished() && isFinished;
    }

    bool AreBlocksCorrectlyPaired()
    {
        ForBlock[] forBlocks = FindObjectsOfType<ForBlock>();
        EndForBlock[] endForBlocks = FindObjectsOfType<EndForBlock>();
        IfBlock[] ifBlocks = FindObjectsOfType<IfBlock>();
        EndIfBlock[] endIfBlocks = FindObjectsOfType<EndIfBlock>();

        if (forBlocks.Length != endForBlocks.Length)
        {
            Debug.LogError($"Mismatch in number of ForBlocks ({forBlocks.Length}) and EndForBlocks ({endForBlocks.Length})");
            return false;
        }

        if (ifBlocks.Length != endIfBlocks.Length)
        {
            Debug.LogError($"Mismatch in number of IfBlocks ({ifBlocks.Length}) and EndIfBlocks ({endIfBlocks.Length})");
            return false;
        }

        // Further checking could be done here to ensure the blocks are correctly paired in order

        return true;
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

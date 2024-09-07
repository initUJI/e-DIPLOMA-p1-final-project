using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteButtonsController : MonoBehaviour
{
public float deadTime = 1.0f;
private bool _deadTimeActive = false;
    private EventsManager eventsManager;
    private LevelManager levelManager;
    public void onTriggerEnterDelete() 
    {
        transform.parent.parent.gameObject.GetComponent<BouncyScaleScript>().f_ScaleUpOrDown();

        if (eventsManager == null)
        {
            eventsManager = FindFirstObjectByType<EventsManager>();
        }
        if (levelManager == null)
        {
            levelManager = FindFirstObjectByType<LevelManager>();
            if (levelManager != null)
            {
                levelManager.sumNumberOfBlocks(transform.parent.parent.gameObject);
            }
        }

        eventsManager.deleteBlock(transform.parent.parent.gameObject);

        ShelfController[] shelves = FindObjectsOfType<ShelfController>();
        foreach (ShelfController s in shelves)
        {
            if (transform.parent.parent.gameObject.name.Contains(s.blockPrefab.name))
            {
                s.actualiceText();
            }
       
        }
    }

    public void closeDeleteWindow()
    {
        // fermer la fenetre
        gameObject.transform.parent.GetComponent<BouncyScaleScript>().f_ScaleUpOrDown();
        //Destroy(currentWindow);

        if (eventsManager == null)
        {
            eventsManager = FindFirstObjectByType<EventsManager>();
        }

        eventsManager.deleteWindowClose();
    }
}

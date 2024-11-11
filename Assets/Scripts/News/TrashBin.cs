using UnityEngine;

public class TrashBin : MonoBehaviour
{
    private EventsManager eventsManager;
    public GameObject deleteAllBlocks_panel;
    private GameManager_V2 gameManager;

    private void Start()
    {
        deleteAllBlocks_panel.SetActive(false);
        gameManager = FindFirstObjectByType<GameManager_V2>();

    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra en el trigger tiene el tag "Block"
        if (other.CompareTag("Block"))
        {
            // Destruye el objeto con el tag "Block"
            onTriggerEnterDeleteRubish(other.gameObject);
            
        }
    }

    public void onTriggerEnterDeleteRubish(GameObject gameObject)
    {
        // ELIMINACIÓN DEL BLOQUE EN CUESTION
        Debug.Log(gameObject.name);
        GameObject blockShelfController = gameObject.GetComponent<Block>().myShelfController;
        blockShelfController.GetComponent<ShelfController_V2>().blockDeleted();

        //gameObject.GetComponent<BouncyScaleScript>().SetNextScaleDown();
        Destroy(gameObject);

        // Inicializar EventsManager y LevelManager si no están ya asignados
        if (eventsManager == null)
        {
            eventsManager = FindFirstObjectByType<EventsManager>();
        }
        // Notificar al EventsManager
        eventsManager.deleteBlock(gameObject);

        
    }

    public void trashPressed()
    {
        deleteAllBlocks_panel.SetActive(true);
        LeanTween.scale(deleteAllBlocks_panel, Vector3.one, 1f);

    }

    public void Yes_onDeletePanel()
    {
        LeanTween.scale(deleteAllBlocks_panel, Vector3.zero, 1f).setOnComplete(() =>
        {
            deleteAllBlocks_panel.SetActive(false);

        });
        deleteAllBlocks();
    }

    public void No_onDeletePanel()
    {
        LeanTween.scale(deleteAllBlocks_panel, Vector3.zero, 1f).setOnComplete(() =>
        {
            deleteAllBlocks_panel.SetActive(false);

        });

    }

    private void deleteAllBlocks()
    {
        Block[] blocksToDelete = FindObjectsOfType<Block>();

        foreach (Block bl in blocksToDelete)
        {
            if(bl.gameObject.name != "MainBlock") Destroy(bl.gameObject);

        }

        gameManager.refreshLocalAvailableBlocks();
        gameManager.sendLocalSecuenceToServer();
    }


    /*private void UpdateShelves(GameObject parentObject)
    {

        // ACTUALIZAR LOS DATOS EN EL GAME MANAGER Y EN LOS SHELFCONTROLLERS CORRESPONDIENTES
        
        ShelfController[] shelves = FindObjectsOfType<ShelfController>();
        foreach (ShelfController shelf in shelves)
        {
            if (parentObject.name.Contains(shelf.blockPrefab.name))
            {
                if (levelManager != null && levelManager.returnNumberOfBlocks(parentObject) == 1)
                {
                    shelf.callCreateNewBlock();
                }
                shelf.actualiceText();
            }
        }
    }*/
}

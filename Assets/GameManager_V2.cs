using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class GameManager_V2 : MonoBehaviour
{
    public enum GameState
    {
        WAITING_FOR_PLAYERS,  // Esperando a que ambos jugadores se conecten
        CODING,               // Fase en la que los jugadores construyen sus funciones
        EXECUTING             // Fase en la que se ejecutan las funciones y los coches se mueven
    }

    public GameState actualGameState;
    public List<BlockSet> playerBlocksSets;

    private bool localPlayerInitialized = false;

    [Header("Player Scenarios")]
    public List<GameObject> PlayableScenarios;  // Escenarios donde los jugadores pueden interactuar
    public List<GameObject> NonPlayableScenarios;
    public TextMeshProUGUI actuaRoundText;
    public TextMeshProUGUI objectivesAcomplishedText;
    public List<Transform> PlayersInitialTransforms;
    public List<CarController_V2> PlayersCarControllers;
    public GameObject executingPopup_prefab;

    [Header("Local player Setup")]
    public PlayerSettings playerSettings;
    public GameObject local_XRPlayer;
    public XRRayInteractor[] local_XRLineInteractors;
    public PlayerBlacscreenCanvas XRPlayer_FaderSphere;
    public GameSocket_Manager socketManager;
    public MainBlock playerMainBlockController;
    private List<ShelfController_V2> localPlayerShelfsControllers;
    [SerializeField] private ParticleSystem confettiPS;

    [Header("Local player Setup")]
    [SerializeField] public CarController_V2 localCarController;
    [SerializeField] private CarController_V2 partnerCarController;

    [SerializeField] private List<BlockObject> localBlocksSecuence;
    [SerializeField] private List<BlockObject> partnerBlocksSecuence;
    [SerializeField] private PartnerMainBlock_Controller partnerMainBlockController;

    [SerializeField] private int actualGamePhase = 0;
    private int actualRound = 1;
    [SerializeField] private List<BlockObject> blockOptions;
    private bool mustUpdate = false;
    private bool canProcess = false;
    private bool[] isCarsMovementFinished = { false, false };

    //  TESTING INPUT
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            showExecutingPopup();
        }

        /*else if (Input.GetKeyDown(KeyCode.O))
        {
            UpdatePartnerBlocks();
        }else if (Input.GetKeyDown(KeyCode.I))
        {
            List<string> blockNames = playerMainBlockController.getString();
            Debug.Log(string.Join(", ", blockNames));
            sendLocalSecuenceToServer();
        }
        */



        // EL REAL UPDATE (NO TOCAR)
        if (mustUpdate)
        {
            UpdatePartnerBlocks();
            mustUpdate = false;
        }
        if (canProcess)
        {
            localCarController.setCustomPlayerLogText("EXECUTING CODE");
            showExecutingPopup();
            ProcecarSecuences();
            canProcess = false;
        }
    }

    public void showExecutingPopup()
    {
        //GetChild(0).GetChild(0). --> Para acceder desde el XROrigin a la cámara que representa la cabeza
        Vector3 spawnPosition = local_XRPlayer.transform.GetChild(0).GetChild(0).position + local_XRPlayer.transform.GetChild(0).GetChild(0).forward * 0.5f;
        Quaternion spawnRotation = Quaternion.LookRotation(spawnPosition - local_XRPlayer.transform.GetChild(0).GetChild(0).position);

        Instantiate(executingPopup_prefab, spawnPosition, spawnRotation);
    }

    public void showNewRoundPopup()
    {
        //GetChild(0).GetChild(0). --> Para acceder desde el XROrigin a la cámara que representa la cabeza
        Vector3 spawnPosition = local_XRPlayer.transform.GetChild(0).GetChild(0).position + local_XRPlayer.transform.GetChild(0).GetChild(0).forward * 0.5f;
        Quaternion spawnRotation = Quaternion.LookRotation(spawnPosition - local_XRPlayer.transform.GetChild(0).GetChild(0).position);

        GameObject newPopup = Instantiate(executingPopup_prefab, spawnPosition, spawnRotation);
        newPopup.GetComponent<TextMeshPro>().text = "Starting coding phase";
    }

    public void sendLocalSecuenceToServer()
    {
        //List<string> localStringSecuence = Utilities.BlockListToStringList(localBlocksSecuence);
        List<string> localStringSecuence = playerMainBlockController.getString();

        socketManager.sendLocalSecuence(localStringSecuence);

        //Se actualiza localmente
        List<BlockObject> actualBlockSecuence = Utilities.StringListToBlockList(localStringSecuence, blockOptions);
        this.localBlocksSecuence = actualBlockSecuence;

    }

    public void receivePartnerSecuenceFromServer(List<string> partnerSecuence)
    {
        List<BlockObject> blockObjects = Utilities.StringListToBlockList(partnerSecuence, blockOptions);
        partnerBlocksSecuence = blockObjects;


        mustUpdate = true;
    }

    private void UpdatePartnerBlocks()
    {
        partnerMainBlockController.updatePartnerBlocks(partnerBlocksSecuence);
    }

    public void ProcecarSecuences()
    {
        for (int i = 0; i < isCarsMovementFinished.Length; i++)
        {
            isCarsMovementFinished[i] = false;
        }

        StartCoroutine(localCarController.ProcesarSecuences(localBlocksSecuence));
        StartCoroutine(partnerCarController.ProcesarSecuences(partnerBlocksSecuence, true));
        StartCoroutine(WaitForNextRound());
    }

    private IEnumerator WaitForNextRound()
    {
        while (true)
        {
            bool allCarsFinished = true;

            // Comprobamos el array de booleanos
            for (int i = 0; i < isCarsMovementFinished.Length; i++)
            {
                if (!isCarsMovementFinished[i])
                {
                    allCarsFinished = false;
                    break;  // Si uno es false, ya no seguimos comprobando
                }
            }

            if (allCarsFinished)
            {
                // Ejecuta la acción si todos los coches han terminado
                InitializeNextRound();
                yield break;  // Detenemos la corrutina si se ha cumplido la condición
            }

            // Esperamos un segundo antes de volver a comprobar
            yield return new WaitForSeconds(1);
        }
    }

    public void isCarFinished(int carID)
    {
        isCarsMovementFinished[carID] = true;
    }

    public bool CheckIfLocalSecuenceIsCorrect()
    {
        return localCarController.CheckIfSecuenceIsPosible(localBlocksSecuence, 1);
    }

    private void Awake()
    {
        actualGameState = GameState.WAITING_FOR_PLAYERS;
        setXRInteractionNewState(false, true);
    }

    private void CallToProcess()
    {
        EventsManager events = FindObjectOfType<EventsManager>();
        events.playPressed();
        canProcess = true;
    }

    private void Start()
    {
        InitializeLocalPlayer();
        socketManager.onPlayerMove.AddListener(receivePartnerSecuenceFromServer);
        socketManager.onPlayerReady.AddListener(CallToProcess);
        socketManager.onRoomReady.AddListener(playerJoined);
    }

    private void playerJoined()
    {
        // Desactivar fader y activar las interacciones del XR
        Debug.Log("que empiece el juego");
        EventsManager eventsManager = FindObjectOfType<EventsManager>();
        eventsManager.messageOther("PLAYER JOINED THE GAME");
        setXRInteractionNewState(true);
        XRPlayer_FaderSphere.faderSphereNewState(0);
    }

    private void InitializeLocalPlayer()
    {
        if(localPlayerInitialized == false)
        {
            if(socketManager.roomID == null || socketManager.roomID == "")
            {
                //A�n no se ha iniciado el player en el servidor
                StartCoroutine(CheckRoomIDAgain());
            }
            else
            {
                //Ha inicializado correctamente en el servidor
                localPlayerInitialized = true;

                InitializeLocalScenario(socketManager.playerID);
            }
        }
    }

    private IEnumerator CheckRoomIDAgain()
    {
        yield return new WaitForSeconds(1f);

        // Vuelve a comprobar si la roomID sigue siendo null o vac�a
        if (socketManager.roomID == null || socketManager.roomID == "")
        {
            // Si sigue siendo null o vac�a, vuelve a iniciar el proceso
            StartCoroutine(CheckRoomIDAgain());
        }
        else
        {
            // Si ya est� disponible, inicializa el jugador local
            localPlayerInitialized = true;
            InitializeLocalScenario(socketManager.playerID);
        }
    }

    private void InitializeLocalScenario(int playerID)
    {
        if (playerID == 1)
        {
            PlayableScenarios[0].SetActive(true);
            playerMainBlockController = PlayableScenarios[0].GetComponentInChildren<MainBlock>();
            NonPlayableScenarios[1].SetActive(true);
            partnerMainBlockController = PlayableScenarios[0].GetComponentInChildren<PartnerMainBlock_Controller>();
            playerMainBlockController.onStart.AddListener(socketManager.sendPlayerReady);
            playerMainBlockController.setGameManager(this);
            XRPlayer_FaderSphere = local_XRPlayer.GetComponentInChildren<PlayerBlacscreenCanvas>();

            // Se setean los CarControllers segun el PlayerID que haya tocado
            localCarController = PlayersCarControllers[0];
            partnerCarController = PlayersCarControllers[1];
            localCarController.setPlayerText(playerMainBlockController.gameObject.GetComponentInChildren<TextMeshProUGUI>());

            //Se localiza el player en su lugar de Player1
            local_XRPlayer.transform.SetPositionAndRotation(PlayersInitialTransforms[0].position, PlayersInitialTransforms[0].rotation);
            localPlayerShelfsControllers = PlayableScenarios[0].GetComponentsInChildren<ShelfController_V2>().ToList();

            XRPlayer_FaderSphere.setNewText("Player: " + playerID + " joined. Room name: " + socketManager.roomID + ". Waiting for partner.");
            SetLocalAvailableBlocks(playerBlocksSets[actualGamePhase]);
        }else if(playerID == 2)
        {
            PlayableScenarios[1].SetActive(true);
            playerMainBlockController = PlayableScenarios[1].GetComponentInChildren<MainBlock>();
            NonPlayableScenarios[0].SetActive(true);
            partnerMainBlockController = PlayableScenarios[1].GetComponentInChildren<PartnerMainBlock_Controller>();
            playerMainBlockController.onStart.AddListener(socketManager.sendPlayerReady);
            playerMainBlockController.setGameManager(this);
            XRPlayer_FaderSphere = local_XRPlayer.GetComponentInChildren<PlayerBlacscreenCanvas>();

            // Se setean los CarControllers segun el PlayerID que haya tocado
            localCarController = PlayersCarControllers[1];
            partnerCarController = PlayersCarControllers[0];
            localCarController.setPlayerText(playerMainBlockController.gameObject.GetComponentInChildren<TextMeshProUGUI>());

            //Se localiza el player en su lugar de Player2
            local_XRPlayer.transform.SetPositionAndRotation(PlayersInitialTransforms[1].position, PlayersInitialTransforms[1].rotation);
            localPlayerShelfsControllers = PlayableScenarios[1].GetComponentsInChildren<ShelfController_V2>().ToList();

            XRPlayer_FaderSphere.setNewText("Player: " + playerID + " joined. Room name: " + socketManager.roomID + ". Waiting for partner.");

            SetLocalAvailableBlocks(playerBlocksSets[actualGamePhase]);
        }
        else
        {
            Debug.LogError("Player_ID out of bounds!");
        }
    }

    public void incrementGamePhase()
    {
        actualGamePhase++;
        objectivesAcomplishedText.text = actualGamePhase + "/" + playerBlocksSets.Count;

        EventsManager eventsManager = FindObjectOfType<EventsManager>();
        eventsManager.messageOther("START PHASE: " + actualGamePhase);

        if (actualGamePhase >= playerBlocksSets.Count)
        {
            // Hacer el fin del juego (INCOMPLETO)
            localCarController.setCustomPlayerLogText("Game Finished! Congratulations!");
            confettiPS.Play();
            eventsManager.levelCompleted(SceneManager.GetActiveScene().name);
            setXRInteractionNewState(false);
        }
    }

    private void SetLocalAvailableBlocks(BlockSet actualBlockSet)
    {
        foreach(ShelfController_V2 sf in localPlayerShelfsControllers)
        {
            switch (sf.block.blockName)
            {
                case ("MoveForward"):
                    sf.availableBlockQuantity = actualBlockSet.moveForwardQuantity;

                    sf.SetupShelfController();

                    break;
                case ("For"):
                    sf.availableBlockQuantity = actualBlockSet.forQuantity;

                    sf.SetupShelfController();

                    break;
                case ("EndFor"):
                    sf.availableBlockQuantity = actualBlockSet.endForQuantity;

                    sf.SetupShelfController();

                    break;
                case ("If"):
                    sf.availableBlockQuantity = actualBlockSet.ifQuantity;

                    sf.SetupShelfController();

                    break;
                case ("EndIf"):
                    sf.availableBlockQuantity = actualBlockSet.endIfQuantity;

                    sf.SetupShelfController();

                    break;
                case ("Left"):
                    sf.availableBlockQuantity = actualBlockSet.leftQuantity;

                    sf.SetupShelfController();

                    break;
                case ("Right"):
                    sf.availableBlockQuantity = actualBlockSet.rightQuantity;

                    sf.SetupShelfController();

                    break;
                case ("Turn"):
                    sf.availableBlockQuantity = actualBlockSet.turnQuantity;

                    sf.SetupShelfController();

                    break;
                case ("GetHumidity"):
                    sf.availableBlockQuantity = actualBlockSet.getHumidityQuantity;

                    sf.SetupShelfController();

                    break;
                case ("n3"):
                    sf.availableBlockQuantity = actualBlockSet.number3_Quantity;

                    sf.SetupShelfController();

                    break;
                case ("n2"):
                    sf.availableBlockQuantity = actualBlockSet.number2_Quantity;

                    sf.SetupShelfController();

                    break;
                case ("n4"):
                    sf.availableBlockQuantity = actualBlockSet.number4_Quantity;

                    sf.SetupShelfController();

                    break;
                case ("n5"):
                    sf.availableBlockQuantity = actualBlockSet.number5_Quantity;

                    sf.SetupShelfController();

                    break;
                case ("n6"):
                    sf.availableBlockQuantity = actualBlockSet.number6_Quantity;

                    sf.SetupShelfController();

                    break;
                case ("n7"):
                    sf.availableBlockQuantity = actualBlockSet.number7_Quantity;

                    sf.SetupShelfController();

                    break;
                case ("n8"):
                    sf.availableBlockQuantity = actualBlockSet.number8_Quantity;

                    sf.SetupShelfController();

                    break;
                case ("n9"):
                    sf.availableBlockQuantity = actualBlockSet.number9_Quantity;

                    sf.SetupShelfController();

                    break;
            }
        }
    }

    public void InitializeNextRound()
    {
        // Hay que sumar turno
        actualRound++;
        actuaRoundText.text = "Round " + actualRound;

        EventsManager eventsManager = FindObjectOfType<EventsManager>();
        eventsManager.messageOther("START ROUND: " + actualRound);
        showNewRoundPopup();

        // Borrar bloques (los del mainBlock y los de las estanterías para hacer los nuevos)
        partnerMainBlockController.ClearPreviousBlocks();
        playerMainBlockController.ClearPreviousBlocks();

        // Asegurar que se eliminan todos los bloques de la escena (hasta los que están más apartados)
        /*Block[] remainingBlocks = GameObject.FindObjectsOfType<Block>();
        foreach(Block actualBlock in remainingBlocks)
        {
            if(actualBlock.gameObject != playerMainBlockController.gameObject)
            {
                Destroy(actualBlock.gameObject);
            }
        }*/

        // Crear los bloques teniendo en cuenta la fase en la que se encuentra el jugador
        SetLocalAvailableBlocks(playerBlocksSets[actualGamePhase]);
    }

    public void setXRInteractionNewState(bool newState, bool firstIteration = false)
    {
        if(firstIteration)
        {
            foreach (XRRayInteractor xrLine in local_XRLineInteractors)
            {
                xrLine.GetComponent<LineRenderer>().enabled = newState;
                xrLine.enabled = newState;
            }
        }else
        {
            if(!playerSettings.isPlayerRightHanded)
            {
                local_XRLineInteractors[0].GetComponent<LineRenderer>().enabled = newState;
                local_XRLineInteractors[0].enabled = newState;
            }else
            {
                local_XRLineInteractors[1].GetComponent<LineRenderer>().enabled = newState;
                local_XRLineInteractors[1].enabled = newState;
            }
        }
        
    }
}

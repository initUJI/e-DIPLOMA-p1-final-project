using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    public List<Transform> PlayersInitialTransforms;
    public List<CarController_V2> PlayersCarControllers;

    [Header("Local player Setup")]
    public GameObject local_XRPlayer;
    public GameSocket_Manager socketManager;
    public MainBlock playerMainBlockController;
    private List<ShelfController_V2> localPlayerShelfsControllers;

    [Header("Local player Setup")]
    [SerializeField] private CarController_V2 localCarController;
    [SerializeField] private CarController_V2 partnerCarController;

    [SerializeField] private List<BlockObject> localBlocksSecuence;
    [SerializeField] private List<BlockObject> partnerBlocksSecuence;
    [SerializeField] private PartnerMainBlock_Controller partnerMainBlockController;

    [SerializeField] private int actualGamePhase = 0;
    [SerializeField] private List<BlockObject> blockOptions;
    private bool mustUpdate = false;
    private bool canProcess = false;

    //  TESTING INPUT
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ProcecarSecuences();

        }else if (Input.GetKeyDown(KeyCode.O))
        {
            UpdatePartnerBlocks();
        }else if (Input.GetKeyDown(KeyCode.I))
        {
            List<string> blockNames = playerMainBlockController.getString();
            Debug.Log(string.Join(", ", blockNames));
            sendLocalSecuenceToServer();
        }
        
        if (mustUpdate)
        {
            UpdatePartnerBlocks();
            mustUpdate = false;
        }
        if (canProcess)
        {
            ProcecarSecuences();
            canProcess = false;
        }
    }

    public void sendLocalSecuenceToServer()
    {
        //List<string> localStringSecuence = Utilities.BlockListToStringList(localBlocksSecuence);
        socketManager.sendLocalSecuence(playerMainBlockController.getString());
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
        StartCoroutine(localCarController.ProcesarSecuences(localBlocksSecuence));
    }

    private void Awake()
    {
        actualGameState = GameState.WAITING_FOR_PLAYERS;
    }

    private void CallToProcess()
    {
        canProcess = true;
    }

    private void Start()
    {
        InitializeLocalPlayer();
        socketManager.onPlayerMove.AddListener(receivePartnerSecuenceFromServer);
        socketManager.onPlayerReady.AddListener(CallToProcess);

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

            // Se setean los CarControllers segun el PlayerID que haya tocado
            localCarController = PlayersCarControllers[0];
            partnerCarController = PlayersCarControllers[1];

            //Se localiza el player en su lugar de Player1
            local_XRPlayer.transform.SetPositionAndRotation(PlayersInitialTransforms[0].position, PlayersInitialTransforms[0].rotation);
            localPlayerShelfsControllers = PlayableScenarios[0].GetComponentsInChildren<ShelfController_V2>().ToList();
            
            SetLocalAvailableBlocks(playerBlocksSets[actualGamePhase]);
        }else if(playerID == 2)
        {
            PlayableScenarios[1].SetActive(true);
            playerMainBlockController = PlayableScenarios[1].GetComponentInChildren<MainBlock>();
            NonPlayableScenarios[0].SetActive(true);
            partnerMainBlockController = PlayableScenarios[1].GetComponentInChildren<PartnerMainBlock_Controller>();
            playerMainBlockController.onStart.AddListener(socketManager.sendPlayerReady);

            // Se setean los CarControllers segun el PlayerID que haya tocado
            localCarController = PlayersCarControllers[1];
            partnerCarController = PlayersCarControllers[0];

            //Se localiza el player en su lugar de Player2
            local_XRPlayer.transform.SetPositionAndRotation(PlayersInitialTransforms[1].position, PlayersInitialTransforms[1].rotation);
            localPlayerShelfsControllers = PlayableScenarios[1].GetComponentsInChildren<ShelfController_V2>().ToList();

            SetLocalAvailableBlocks(playerBlocksSets[actualGamePhase]);
        }
        else
        {
            Debug.LogError("Player_ID out of bounds!");
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
}

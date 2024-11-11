using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColaLevelsManager : MonoBehaviour
{
    public GameObject arduinoAssemblyZone;
    public GameObject sensorHumidity; // Solo necesario para el nivel Plants
    public GameObject[] levelButtons; // Los botones para elegir el nivel
    public GameObject completionParticles;
    public GameObject buildingArduinoPack;

    public checkCar carCheck; // Referencia al script checkCar

    [Header("Main menu panels")]
    public GameObject titlePanel;
    public GameObject welcomePanel;
    public GameObject carBuildingPanel;

    private string selectedLevel;
    public bool needsHumiditySensor = false;

    void Start()
    {
        LeanTween.scale(titlePanel, new Vector3(0.005482751f, 0.005482751f, 0.005482751f), 1.5f);
    }

    public void titleStartButtonPressed()
    {
        LeanTween.scale(titlePanel, new Vector3(0, 0, 0), 1.5f).setOnComplete(() =>
        {
            welcomePanel.SetActive(true);

            LeanTween.scale(welcomePanel, new Vector3(0.005482751f, 0.005482751f, 0.005482751f), 1.5f).setOnComplete(() => {
                // Inicialmente, ocultar los botones de nivel y la zona de montaje
                foreach (GameObject button in levelButtons)
                {
                    button.SetActive(true);
                }
                arduinoAssemblyZone.SetActive(false);
                sensorHumidity.SetActive(false);
            });
        });


        
    }

    public void OnLevelButtonClicked(string levelName)
    {
        selectedLevel = levelName;

        //Se escala a 0 el menú principal
        LeanTween.scale(welcomePanel, Vector3.zero, 1.5f).setOnComplete(() => {
            //Que aparezca el segundo panel con el tutorial de montaje del coche y también el coche
            welcomePanel.SetActive(false);
            carBuildingPanel.SetActive(true);
            buildingArduinoPack.SetActive(true);
            needsHumiditySensor = selectedLevel == "Plants";
            LeanTween.scale(carBuildingPanel, new Vector3(0.005482751f, 0.005482751f, 0.005482751f), 1.5f).setOnComplete(() => {
                // Configurar si se necesita sensor de humedad
                sensorHumidity.SetActive(needsHumiditySensor);
                Debug.Log("Sensor de humedad activado: " + sensorHumidity.activeSelf);

                // Actualizar checkCar con el valor de humiditySensorRequired
                if (carCheck != null)
                {
                    carCheck.humiditySensorRequired = needsHumiditySensor;
                    Debug.Log("Humidity sensor set en checkCar: " + carCheck.humiditySensorRequired);
                }

                // Mostrar la zona de montaje y ocultar los botones de nivel
                foreach (GameObject button in levelButtons)
                {
                    button.SetActive(false);
                }

                StartCoroutine(CargaZona());
                // Iniciar la secuencia de partículas y carga de escena
            });
            
        });
    }

    public void NotifyAssemblyComplete()
    {
        Debug.Log("Montaje del Arduino completo. Cargando escena...");
        StartCoroutine(WaitAndLoadScene());
    }

    private IEnumerator CargaZona()
    {
        // Activar partículas de confetti o efectos de finalización
        ParticleSystem particleSystem = completionParticles.GetComponent<ParticleSystem>();
        particleSystem.Play();

        // Esperar a que se vean las partículas antes de continuar
        yield return new WaitForSeconds(1f);
        arduinoAssemblyZone.SetActive(true);

        // No es necesario hacer nada más aquí, NotifyAssemblyComplete() se encargará de la carga de escena
    }

    private IEnumerator WaitAndLoadScene()
    {
        // Esperar un tiempo adicional antes de cargar la escena
        yield return new WaitForSeconds(3f);

        // Encontrar y configurar el checkCar
        checkCar carCheck = FindObjectOfType<checkCar>();
        if (carCheck != null)
        {
            carCheck.humiditySensorRequired = needsHumiditySensor;
            Debug.Log("Configuración de sensor de humedad en checkCar: " + carCheck.humiditySensorRequired);
        }
        else
        {
            Debug.LogError("No se encontró checkCar en la escena.");
        }

        // Cargar la escena del nivel seleccionado
        switch (selectedLevel)
        {
            case "Plants":
                Debug.Log("Cargando escena: DefinitiveScene_Plants");
                SceneManager.LoadScene("Plants_CollaborativeLevel");
                break;
            case "Walls":
                Debug.Log("Cargando escena: DefinitiveScene_Walls");
                SceneManager.LoadScene("Zombies_CollaborativeLevel");
                break;
            case "Trash":
                Debug.Log("Cargando escena: DefinitiveScene_Trash");
                SceneManager.LoadScene("Trash_CollaborativeLevel");
                break;
            default:
                Debug.LogError("Nivel seleccionado no válido.");
                break;
        }
    }
}

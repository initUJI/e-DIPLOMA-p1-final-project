using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColaLevelsManager : MonoBehaviour
{
    public GameObject arduinoAssemblyZone;
    public GameObject sensorHumidity; // Solo necesario para el nivel Plants
    public GameObject[] levelButtons; // Los botones para elegir el nivel
    public GameObject completionParticles;

    public checkCar carCheck; // Referencia al script checkCar


    private string selectedLevel;
    public bool needsHumiditySensor = false;

    void Start()
    {
        // Inicialmente, ocultar los botones de nivel y la zona de montaje
        foreach (GameObject button in levelButtons)
        {
            button.SetActive(true);
        }
        arduinoAssemblyZone.SetActive(false);
        sensorHumidity.SetActive(false);
    }

    public void OnLevelButtonClicked(string levelName)
    {
        selectedLevel = levelName;
        Debug.Log("Nivel seleccionado: " + levelName);

        // Configurar si se necesita sensor de humedad
        needsHumiditySensor = selectedLevel == "Plants";
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
       

        // Iniciar la secuencia de partículas y carga de escena
        StartCoroutine(CargaZona());
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

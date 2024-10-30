using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkCar : MonoBehaviour
{
    bool placa =false;
    bool senHUM =false;
    bool senULT =false;
    bool shield=false;
    public GameObject carParticle;

    public GameObject roboCola, roboplaca, robochasis, sensor1, sensor2, roboshield; // El objeto final ensamblado
    public GameObject completionParticles; // Las partículas que aparecen al final
    public float moveSpeed = 0.3f; // Velocidad del movimiento

    public bool humiditySensorRequired;

    public ColaLevelsManager colaLevelsManager; // Referencia al script ColaLevelsManager


    void Start()
    {
        Debug.Log("Inicio del script checkCar. Comprobando componentes.");
        // Comprobar si tenemos referencia a ColaLevelsManager
        if (colaLevelsManager != null)
        {
            // Obtener si el sensor de humedad es necesario desde el ColaLevelsManager
            humiditySensorRequired = colaLevelsManager.needsHumiditySensor;
            Debug.Log("Referencia a ColaLevelsManager recibida. Sensor de humedad requerido: " + humiditySensorRequired);
        }
        else
        {
            Debug.LogError("Referencia a ColaLevelsManager no asignada.");
        }

        EventsManager eventsManager = FindObjectOfType<EventsManager>();
        eventsManager.messageOther("MONTAGE SCENE STARTED");
    }
    void Update()
    {
         // Agregamos logs para revisar el estado de las variables

        // Verifica si todas las piezas están colocadas correctamente
        if (placa && senULT && shield &&(!humiditySensorRequired || senHUM))
        {
            ParticleSystem carPar = carParticle.GetComponent<ParticleSystem>();
            //Debug.Log("Iniciando secuencia de finalización.");

            carPar.Play();

            StartCoroutine(CompleteSequence());
            Debug.Log("Todos los componentes están colocados correctamente.");
        }
        
    }


    public void placaColocated()
    {
        placa = true;
        EventsManager eventsManager = FindObjectOfType<EventsManager>();
        eventsManager.messageOther("ARDUINO BOARD COLOCATED");
    }
    public void placaExit()
    {
        placa = false;
        EventsManager eventsManager = FindObjectOfType<EventsManager>();
        eventsManager.messageOther("ARDUINO BOARD REMOVED");
    }

    public void sensorHUMColocated()
    {
        senHUM = true;
        EventsManager eventsManager = FindObjectOfType<EventsManager>();
        eventsManager.messageOther("HUMIDITY SENSOR COLOCATED");
    }
    public void sensorHUMExit()
    {
        senHUM = false;
        EventsManager eventsManager = FindObjectOfType<EventsManager>();
        eventsManager.messageOther("HUMIDITY SENSOR REMOVED");
    }

    public void senosorULTColocated()
    {
        senULT = true;
        EventsManager eventsManager = FindObjectOfType<EventsManager>();
        eventsManager.messageOther("ULTRASONIC SENSOR COLOCATED");
    }

    public void sensorULTExit()
    {
        senULT = false;
        EventsManager eventsManager = FindObjectOfType<EventsManager>();
        eventsManager.messageOther("ULTRASONIC SENSOR REMOVED");

    }

    public void shieldColocated()
    {
        shield = true;
        EventsManager eventsManager = FindObjectOfType<EventsManager>();
        eventsManager.messageOther("BASE SHIELD COLOCATED");
    }

    public void shieldExit()
    {
        shield = false;
        EventsManager eventsManager = FindObjectOfType<EventsManager>();
        eventsManager.messageOther("BASE SHIELD REMOVED");
    }

    IEnumerator CompleteSequence()
    {
        ParticleSystem completpart = completionParticles.GetComponent<ParticleSystem>();
        Debug.Log("Iniciando secuencia de finalización.");

        completpart.Play();

        // Mueve el objeto a la posición final
        Vector3 startPosition = roboCola.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveSpeed)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(3f); // Espera 3 segundos antes de continuar

        // Desactiva las partículas y el conjunto final
        completpart.Stop();
        roboCola.SetActive(false);
        roboplaca.SetActive(false);
        robochasis.SetActive(false);
        sensor1.SetActive(false);
        sensor2.SetActive(false);
        roboshield.SetActive(false);

        // Notificar a ColaLevelsManager que el ensamblaje está completo
        if (colaLevelsManager != null)
        {
            Debug.Log("Notificando a ColaLevelsManager que el montaje está completo.");
            EventsManager eventsManager = FindObjectOfType<EventsManager>();
            eventsManager.messageOther("CAR MONTAGE COMPLETED");
            colaLevelsManager.NotifyAssemblyComplete();
        }
        else
        {
            Debug.LogError("Referencia a ColaLevelsManager no asignada.");
        }
    }
}

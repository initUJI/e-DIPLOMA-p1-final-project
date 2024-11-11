using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantsController : MonoBehaviour
{
    public GameObject Plant1, Plant2, Plant3, Plant4;
    public bool overPlant1 = false, overPlant2 = false, overPlant3 = false, overPlant4 = false;
    public GameObject PlantParticles1, PlantParticles2, PlantParticles3, PlantParticles4; // Part�culas asignadas en el Inspector
    public GameManager_V2 gameManager;

    public float particleDuration = 3f; // Duraci�n de las part�culas en segundos

    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject == Plant1)
       {
            overPlant1 = true;
        }
        else if (other.gameObject == Plant2)
       {
            overPlant2 = true;
        }
       else if (other.gameObject == Plant3)
       {
            overPlant3 = true;

        }
        else if (other.gameObject == Plant4)
       {
            overPlant4 = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Plant1)
        {
            overPlant1 = false;
        }
        else if (other.gameObject == Plant2)
        {
            overPlant2 = false;
        }
        else if (other.gameObject == Plant3)
        {
            overPlant3 = false;

        }
        else if (other.gameObject == Plant4)
        {
            overPlant4 = false;
        }
    }

    public void tryToGetHumidity()
    {
        // En caso de estar sobre una planta y recibir la instrucci�n de GetHumidity, se producen los cambios necesarios

        if(overPlant1 == true)
        {
            ActivateParticles(PlantParticles1);
            gameManager.incrementGamePhase();

            Plant1.transform.GetChild(1).gameObject.SetActive(false);

            Collider collider = Plant1.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }
            overPlant1 = false;
        }
        else if (overPlant2 == true)
        {
            ActivateParticles(PlantParticles2);
            gameManager.incrementGamePhase();

            Plant2.transform.GetChild(1).gameObject.SetActive(false);

            Collider collider = Plant2.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }
            overPlant2 = false;
        }
        else if (overPlant3 == true)
        {
            ActivateParticles(PlantParticles3);
            gameManager.incrementGamePhase();

            Plant3.transform.GetChild(1).gameObject.SetActive(false);

            Collider collider = Plant3.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }
            overPlant3 = false;
        }
        else if (overPlant4 == true)
        {
            ActivateParticles(PlantParticles4);
            gameManager.incrementGamePhase();

            Plant4.transform.GetChild(1).gameObject.SetActive(false);

            Collider collider = Plant4.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }
            overPlant4 = false;
        }
        else
        {
            // No est� sobre una planta, hay que notificarlo de alguna manera
        }
    }

    private void ActivateParticles(GameObject particleSystemObject)
    {
        //DEBERIAMOS COMPROBAR SI EN EL SENTENCE ESTE EL GET HUMIDITY
        
        particleSystemObject.SetActive(true); // Activar el GameObject de las part�culas

        if (particleSystemObject != null)
        {
            ParticleSystem ps = particleSystemObject.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play(); // Activar el sistema de part�culas
                StartCoroutine(StopParticlesAfterDuration(ps)); // Detener despu�s de 3 segundos

            }
            else
            {
                Debug.LogWarning("No se encontr� el sistema de part�culas en el objeto: " + particleSystemObject.name);
            }
        }
    }
    // Coroutine para detener las part�culas despu�s de 3 segundos
    private IEnumerator StopParticlesAfterDuration(ParticleSystem ps)
    {
        yield return new WaitForSeconds(particleDuration); // Esperar la duraci�n
        ps.Stop(); // Detener el sistema de part�culas
        ps.gameObject.SetActive(false); // Opcional: Desactivar el GameObject de part�culas
    }
}

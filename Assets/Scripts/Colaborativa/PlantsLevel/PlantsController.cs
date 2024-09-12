using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantsController : MonoBehaviour
{
    public GameObject Plant1, Plant2, Plant3, Plant4;
    public GameObject PlantParticles1, PlantParticles2, PlantParticles3, PlantParticles4; // Partículas asignadas en el Inspector

    public float particleDuration = 3f; // Duración de las partículas en segundos

    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject == Plant1)
       {
            ActivateParticles(PlantParticles1);
        }
        else if (other.gameObject == Plant2)
       {
            ActivateParticles(PlantParticles2);
        }
       else if (other.gameObject == Plant3)
       {
            ActivateParticles(PlantParticles3);

        }
        else if (other.gameObject == Plant4)
       {
            ActivateParticles(PlantParticles4);
        }
    }

    private void ActivateParticles(GameObject particleSystemObject)
    {
        //DEBERIAMOS COMPROBAR SI EN EL SENTENCE ESTE EL GET HUMIDITY
        
        particleSystemObject.SetActive(true); // Activar el GameObject de las partículas

        if (particleSystemObject != null)
        {
            ParticleSystem ps = particleSystemObject.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play(); // Activar el sistema de partículas
                StartCoroutine(StopParticlesAfterDuration(ps)); // Detener después de 3 segundos

            }
            else
            {
                Debug.LogWarning("No se encontró el sistema de partículas en el objeto: " + particleSystemObject.name);
            }
        }
    }
    // Coroutine para detener las partículas después de 3 segundos
    private IEnumerator StopParticlesAfterDuration(ParticleSystem ps)
    {
        yield return new WaitForSeconds(particleDuration); // Esperar la duración
        ps.Stop(); // Detener el sistema de partículas
        ps.gameObject.SetActive(false); // Opcional: Desactivar el GameObject de partículas
    }
}

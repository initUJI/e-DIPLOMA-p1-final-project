using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashController : MonoBehaviour
{
    public GameObject Trash1, Trash2, Trash3, Trash4;
    public GameObject TrashParticles1, TrashParticles2, TrashParticles3, TrashParticles4; // Partículas asignadas en el Inspector

    public float particleDuration = 3f; // Duración de las partículas en segundos

    private Vector3 AnimMovement = new Vector3(0, -0.7f, 0);  // La basura baja
    public GameManager_V2 gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Trash1)
        {
            StartCoroutine(ActivateParticles(TrashParticles1, Trash1, AnimMovement));
            gameManager.incrementGamePhase();
            other.transform.GetChild(0).gameObject.SetActive(false);
        }
        else if (other.gameObject == Trash2)
        {
            StartCoroutine(ActivateParticles(TrashParticles2, Trash2, AnimMovement));
            gameManager.incrementGamePhase();
            other.transform.GetChild(0).gameObject.SetActive(false);
        }
        else if (other.gameObject == Trash3)
        {
            StartCoroutine(ActivateParticles(TrashParticles3, Trash3, AnimMovement));
            gameManager.incrementGamePhase();
            other.transform.GetChild(0).gameObject.SetActive(false);

        }
        else if (other.gameObject == Trash4)
        {
            StartCoroutine(ActivateParticles(TrashParticles4, Trash4, AnimMovement));
            gameManager.incrementGamePhase();
            other.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    IEnumerator ActivateParticles(GameObject particleSystemObject, GameObject trash, Vector3 moveDirection)
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
                Vector3 startPosition = trash.transform.position;
                Vector3 endPosition = startPosition + moveDirection;

                float elapsedTime = 0;

                while (elapsedTime < 0.5f)
                {
                    trash.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / 0.5f);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
            }
            else
            {
                Debug.LogWarning("No se encontró el sistema de partículas en el objeto: " + particleSystemObject.name);
            }

            Collider collider = trash.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
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

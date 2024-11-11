using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsController : MonoBehaviour
{
    public GameObject ButtonN, ButtonS, ButtonE, ButtonW;
    public GameObject northWall, southWall, eastWall, westWall;
    private Vector3 wallMovement = new Vector3(0, 0.2f, 0);   // Las paredes suben

    // Velocidad de la animaci�n de movimiento
    public float moveSpeed = 0.8f;

    // Valores de movimiento para las compuertas y paredes
    public Material levelBoxMaterial;
    public GameManager_V2 gameManager;


    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject == ButtonN)
       {
           Debug.Log("Bot�n Norte activado");
           StartCoroutine(MoveObject(northWall, wallMovement,ButtonN));
           gameManager.incrementGamePhase();
        }
        else if (other.gameObject == ButtonS)
       {
           Debug.Log("Bot�n Sur activado");
           StartCoroutine(MoveObject(southWall, wallMovement, ButtonS));
           gameManager.incrementGamePhase();
        }
        else if (other.gameObject == ButtonE)
       {
           Debug.Log("Bot�n Este activado");
           StartCoroutine(MoveObject(eastWall, wallMovement, ButtonE));
           gameManager.incrementGamePhase();
        }
        else if (other.gameObject == ButtonW)
       {
           Debug.Log("Bot�n Oeste activado");
           StartCoroutine(MoveObject(westWall, wallMovement, ButtonW));
           gameManager.incrementGamePhase();
        }
    }

    IEnumerator MoveObject(GameObject obj, Vector3 moveDirection, GameObject button)
    {
        Vector3 startPosition = obj.transform.position;
        Vector3 endPosition = startPosition + moveDirection;

        float elapsedTime = 0;

        while (elapsedTime < moveSpeed)
        {
            obj.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / moveSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = endPosition; // Asegurarse de que llegue a la posici�n final
        
        // Si se proporcion� un bot�n, desact�valo y cambia su material
        if (button != null)
        {
            button.transform.GetChild(0).gameObject.SetActive(false);

            // Desactiva el Collider del bot�n
            Collider collider = button.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }

            // Cambia el material del segundo elemento (�ndice 1) en el MeshRenderer del bot�n
            Renderer renderer = button.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material[] materials = renderer.materials;
                if (materials.Length > 1)
                {
                    materials[1] = levelBoxMaterial; // Cambiar el material del �ndice 1
                    renderer.materials = materials;  // Asignar de nuevo el array de materiales al renderer
                }
            }
        }
    }
}

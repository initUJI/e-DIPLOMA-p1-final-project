using System.Collections;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    // Asignar los botones desde el inspector de Unity
    public GameObject ButtonRed, ButtonBlue, ButtonYellow, ButtonPurple;
    public GameObject ButtonN, ButtonS, ButtonE, ButtonW;

    // Asignar las compuertas y muros desde el inspector
    public GameObject redGate, blueGate, yellowGate, purpleGate;
    public GameObject northWall, southWall, eastWall, westWall;

    // Velocidad de la animaci�n de movimiento
    public float moveSpeed = 1.5f;

    // Valores de movimiento para las compuertas y paredes
    private Vector3 gateMovement = new Vector3(0, -0.7f, 0);  // Las compuertas bajan
    private Vector3 wallMovement = new Vector3(0, 0.2f, 0);   // Las paredes suben

    public Material levelBoxMaterial;


    // Usamos OnTriggerEnter para detectar que el coche toca un bot�n
    void OnTriggerEnter(Collider other)
    {
        // Detectamos si el objeto que entra en el trigger es uno de los botones
        if (other.gameObject == ButtonRed)
        {
            Debug.Log("Bot�n Rojo activado");
            StartCoroutine(MoveObject(redGate, gateMovement, ButtonRed));
        }
        else if (other.gameObject == ButtonBlue)
        {
            Debug.Log("Bot�n Azul activado");
            StartCoroutine(MoveObject(blueGate, gateMovement, ButtonBlue));
        }
        else if (other.gameObject == ButtonYellow)
        {
            Debug.Log("Bot�n Amarillo activado");
            StartCoroutine(MoveObject(yellowGate, gateMovement, ButtonYellow));
        }
        else if (other.gameObject == ButtonPurple)
        {
            Debug.Log("Bot�n P�rpura activado");
            StartCoroutine(MoveObject(purpleGate, gateMovement, ButtonPurple));
        }
        else if (other.gameObject == ButtonN)
        {
            Debug.Log("Bot�n Norte activado");
            StartCoroutine(MoveObject(northWall, wallMovement,ButtonN));
        }
        else if (other.gameObject == ButtonS)
        {
            Debug.Log("Bot�n Sur activado");
            StartCoroutine(MoveObject(southWall, wallMovement, ButtonS));
        }
        else if (other.gameObject == ButtonE)
        {
            Debug.Log("Bot�n Este activado");
            StartCoroutine(MoveObject(eastWall, wallMovement, ButtonE));
        }
        else if (other.gameObject == ButtonW)
        {
            Debug.Log("Bot�n Oeste activado");
            StartCoroutine(MoveObject(westWall, wallMovement, ButtonW));
        }
    }

    // Corutina para mover las compuertas o las paredes
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
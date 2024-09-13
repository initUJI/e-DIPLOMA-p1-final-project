using System.Collections;
using UnityEngine;

public class FenceController : MonoBehaviour
{
    // Asignar los botones desde el inspector de Unity
    public GameObject ButtonRed, ButtonBlue, ButtonYellow, ButtonPurple;

    // Asignar las compuertas y muros desde el inspector
    public GameObject redGate, blueGate, yellowGate, purpleGate;

    // Velocidad de la animaci�n de movimiento
    public float moveSpeed = 0.8f;

    // Valores de movimiento para las compuertas y paredes
    private Vector3 gateMovement = new Vector3(0, -0.7f, 0);  // Las compuertas bajan
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
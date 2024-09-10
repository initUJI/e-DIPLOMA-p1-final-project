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
    public float moveSpeed = 2f;

    // Valores de movimiento para las compuertas y paredes
    private Vector3 gateMovement = new Vector3(0, -0.7f, 0);  // Las compuertas bajan
    private Vector3 wallMovement = new Vector3(0, 0.2f, 0);   // Las paredes suben

    // Usamos OnTriggerEnter para detectar que el coche toca un bot�n
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colisi�n detectada con: " + other.gameObject.name);
        // Detectamos si el objeto que entra en el trigger es uno de los botones
        if (other.gameObject == ButtonRed)
        {
            Debug.Log("Bot�n Rojo activado");
            StartCoroutine(MoveObject(redGate, gateMovement));
        }
        else if (other.gameObject == ButtonBlue)
        {
            Debug.Log("Bot�n Azul activado");
            StartCoroutine(MoveObject(blueGate, gateMovement));
        }
        else if (other.gameObject == ButtonYellow)
        {
            Debug.Log("Bot�n Amarillo activado");
            StartCoroutine(MoveObject(yellowGate, gateMovement));
        }
        else if (other.gameObject == ButtonPurple)
        {
            Debug.Log("Bot�n P�rpura activado");
            StartCoroutine(MoveObject(purpleGate, gateMovement));
        }
        else if (other.gameObject == ButtonN)
        {
            Debug.Log("Bot�n Norte activado");
            StartCoroutine(MoveObject(northWall, wallMovement));
        }
        else if (other.gameObject == ButtonS)
        {
            Debug.Log("Bot�n Sur activado");
            StartCoroutine(MoveObject(southWall, wallMovement));
        }
        else if (other.gameObject == ButtonE)
        {
            Debug.Log("Bot�n Este activado");
            StartCoroutine(MoveObject(eastWall, wallMovement));
        }
        else if (other.gameObject == ButtonW)
        {
            Debug.Log("Bot�n Oeste activado");
            StartCoroutine(MoveObject(westWall, wallMovement));
        }
    }

    // Corutina para mover las compuertas o las paredes
    IEnumerator MoveObject(GameObject obj, Vector3 moveDirection)
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
    }
}

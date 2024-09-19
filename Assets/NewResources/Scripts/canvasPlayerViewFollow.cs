
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canvasPlayerViewFollow : MonoBehaviour
{
    public Transform jugador;
    public Transform interfaz;
    public float velocidadRotacion = 5f;
    public float umbral = 0.1f;

    bool isMoving = false;

    void LateUpdate()
    {
        interfaz.position = jugador.position;

        // Obtener la dirección actual del jugador
        Vector3 direccionJugador = jugador.forward;
        direccionJugador.y = 0f;
        direccionJugador = direccionJugador.normalized;

        // Obtener la dirección actual de la interfaz
        Vector3 direccionInterfaz = interfaz.forward;
        direccionInterfaz.y = 0f;
        direccionInterfaz = direccionInterfaz.normalized;

        // Obtener el ángulo actual entre la dirección del jugador y la dirección de la interfaz
        float angulo = Vector3.SignedAngle(direccionJugador, direccionInterfaz, Vector3.up);
        //Debug.Log("Angulo actual: " + Mathf.Abs(angulo));

        // Solo actualizar la rotación de la interfaz si el ángulo supera el umbral
        if (Mathf.Abs(angulo) > umbral)
        {
            /*
            Vector3 rotacionObjetivo = jugador.eulerAngles;
            rotacionObjetivo.x = 0;
            rotacionObjetivo.z = 0;

            Vector3 rotacionActual = interfaz.eulerAngles;
            rotacionActual.x = 0;
            rotacionActual.z = 0;

            Debug.Log("Angulo actual = " + rotacionActual.y + " |  Angulo objetivo = " + rotacionObjetivo.y);

            float rotacionInterpolada = Mathf.LerpAngle(rotacionActual.y, rotacionObjetivo.y, Time.deltaTime * velocidadRotacion);
            
            interfaz.localEulerAngles = new Vector3(0, rotacionInterpolada, 0);
            */
            if (!isMoving)
            {
                StartCoroutine("moveCanvasToCenteredPosition");
            }
        }
    }

    IEnumerator moveCanvasToCenteredPosition()
    {
        if (isMoving)
            yield break;

        isMoving = true;
        float coroutineAngle = umbral;

        do
        {
            Vector3 rotacionObjetivo = jugador.eulerAngles;
            rotacionObjetivo.x = 0;
            rotacionObjetivo.z = 0;

            Vector3 rotacionActual = interfaz.eulerAngles;
            rotacionActual.x = 0;
            rotacionActual.z = 0;

            //Debug.Log("Angulo actual = " + rotacionActual.y + " |  Angulo objetivo = " + rotacionObjetivo.y);

            float rotacionInterpolada = Mathf.LerpAngle(rotacionActual.y, rotacionObjetivo.y, Time.deltaTime * velocidadRotacion);

            interfaz.localEulerAngles = new Vector3(0, rotacionInterpolada, 0);
            coroutineAngle = Mathf.Abs(rotacionInterpolada - rotacionObjetivo.y);

            yield return new WaitForSeconds(0.01f);
        }
        while (coroutineAngle > 2);

        isMoving = false;

        yield return null;
    }
}
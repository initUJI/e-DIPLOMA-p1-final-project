using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class LimitGrabMoveDistance : MonoBehaviour
{
    public float minDistance = 0.5f; // Distancia mínima permitida desde la posición inicial
    public float maxDistance = 2.0f; // Distancia máxima permitida desde la posición inicial

    private XRGrabInteractable grabInteractable;
    private Vector3 initialPosition; // Posición inicial cuando se agarra
    private XRController activeController; // Controlador que está agarrando el objeto
    private bool isGrabbed = false;

    void Start()
    {
        // Obtiene el XRGrabInteractable y asigna eventos
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void Update()
    {
        if (isGrabbed && activeController != null)
        {
            // Calcula la distancia desde la posición inicial
            float currentDistance = Vector3.Distance(initialPosition, transform.position);

            // Verifica el botón MoveObjectIn y MoveObjectOut en el controlador activo
            if (currentDistance >= maxDistance)
            {
                // Si se ha alcanzado la distancia máxima, desactiva el movimiento hacia adelante
                activeController.moveObjectIn = InputHelpers.Button.None;
            }
            else
            {
                // Si no se ha alcanzado la distancia máxima, activa el movimiento hacia adelante
                activeController.moveObjectIn = InputHelpers.Button.PrimaryAxis2DUp;
            }

            if (currentDistance <= minDistance)
            {
                // Si se ha alcanzado la distancia mínima, desactiva el movimiento hacia atrás
                activeController.moveObjectOut = InputHelpers.Button.None;
            }
            else
            {
                // Si no se ha alcanzado la distancia mínima, activa el movimiento hacia atrás
                activeController.moveObjectOut = InputHelpers.Button.PrimaryAxis2DDown;
            }
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        // Marca el objeto como agarrado, guarda la posición inicial y el controlador activo
        isGrabbed = true;
        initialPosition = transform.position;

        // Determina el controlador que está interactuando
        activeController = args.interactorObject.transform.GetComponent<XRController>();
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        // Restablece el estado al soltar el objeto y reactiva los botones
        isGrabbed = false;

        if (activeController != null)
        {
            activeController.moveObjectIn = InputHelpers.Button.PrimaryAxis2DUp;
            activeController.moveObjectOut = InputHelpers.Button.PrimaryAxis2DDown;
        }

        activeController = null;
    }

    void OnDestroy()
    {
        // Limpia los eventos al destruir el objeto
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }
}

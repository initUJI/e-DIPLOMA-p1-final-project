using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SingleObjectSocket : MonoBehaviour
{
    private XRSocketInteractor socketInteractor;

    void Start()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();

        if (socketInteractor == null)
        {
            Debug.LogError("No XRSocketInteractor found on the GameObject.");
            return;
        }

        // Agregar listeners para los eventos de selección y deselección
        socketInteractor.selectEntered.AddListener(OnSelectEntered);
        socketInteractor.selectExited.AddListener(OnSelectExited);
    }

    private void OnDestroy()
    {
        if (socketInteractor != null)
        {
            // Eliminar listeners cuando el objeto se destruya
            socketInteractor.selectEntered.RemoveListener(OnSelectEntered);
            socketInteractor.selectExited.RemoveListener(OnSelectExited);
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        // Desactivar el socket para evitar que se seleccionen nuevos objetos
        socketInteractor.socketActive = false;
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        // Reactivar el socket cuando el objeto se retire
        socketInteractor.socketActive = true;
    }
}


using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomActionButton : MonoBehaviour
{
    [SerializeField] private InputHelpers.Button customActionButton = InputHelpers.Button.Trigger; // Botón de acción personalizado
    private XRSimpleInteractable interactable;
    private XRController controller;
    private bool isCustomSelected = false;

    private void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();

        if (interactable == null)
        {
            Debug.LogError("XRSimpleInteractable not found on this GameObject.");
            return;
        }
    }

    private void Update()
    {
        if (controller == null) return;

        // Verificar si el botón personalizado está presionado
        bool isButtonPressed = controller.inputDevice.IsPressed(customActionButton, out bool pressed) && pressed;

        if (isButtonPressed && !isCustomSelected)
        {
            // Forzar la selección cuando se presiona el botón personalizado
            isCustomSelected = true;

            // Crear argumentos para el evento de selección
            var selectEnterArgs = new SelectEnterEventArgs
            {
                interactableObject = interactable,
                interactorObject = (IXRSelectInteractor)controller
            };

            interactable.selectEntered.Invoke(selectEnterArgs); // Invocar el evento selectEntered
        }
        else if (!isButtonPressed && isCustomSelected)
        {
            // Forzar la deselección cuando se suelta el botón personalizado
            isCustomSelected = false;

            // Crear argumentos para el evento de deselección
            var selectExitArgs = new SelectExitEventArgs
            {
                interactableObject = interactable,
                interactorObject = (IXRSelectInteractor)controller
            };

            interactable.selectExited.Invoke(selectExitArgs); // Invocar el evento selectExited
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Asigna el controlador cuando un interactor entra en contacto con el objeto
        if (other.TryGetComponent(out XRController xrController))
        {
            controller = xrController;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Desasigna el controlador cuando el interactor deja el objeto
        if (other.TryGetComponent(out XRController xrController) && xrController == controller)
        {
            controller = null;
            if (isCustomSelected)
            {
                isCustomSelected = false;

                // Crear y disparar el evento selectExited al salir
                var selectExitArgs = new SelectExitEventArgs
                {
                    interactableObject = interactable,
                    interactorObject = (IXRSelectInteractor)xrController
                };

                interactable.selectExited.Invoke(selectExitArgs);
            }
        }
    }
}

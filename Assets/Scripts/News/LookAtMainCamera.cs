using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LookAtMainCamera : MonoBehaviour
{
    public float targetYRotation = 0f; // Rotación adicional en el eje Y (0, 90, 180, etc.)

    private Transform mainCameraTransform;
    private Quaternion initialRotation; // Rotación inicial en el mundo cuando se spawnea
    private XRGrabInteractable grabInteractable;
    private bool isGrabbed = false;

    void Start()
    {
        // Obtén la referencia a la cámara principal y guarda la rotación inicial del objeto
        mainCameraTransform = Camera.main.transform;
        initialRotation = transform.rotation;

        // Configura los eventos de agarre en el XRGrabInteractable
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void Update()
    {
        // Solo aplica la rotación hacia la cámara mientras esté agarrado
        if (isGrabbed)
        {
            // Calcula la dirección hacia la cámara en el plano XZ (ignorando el eje Y)
            Vector3 directionToCamera = mainCameraTransform.position - transform.position;
            directionToCamera.y = 0; // Mantiene la altura constante

            // Si la dirección es válida, calcula la rotación hacia la cámara
            if (directionToCamera != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);

                // Aplica la rotación adicional en el eje Y definida en targetYRotation
                targetRotation *= Quaternion.Euler(0, targetYRotation, 0);

                // Mantén la rotación inicial en X y Z, y ajusta solo el eje Y
                transform.rotation = Quaternion.Euler(initialRotation.eulerAngles.x, targetRotation.eulerAngles.y, initialRotation.eulerAngles.z);
            }
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        // Activa el comportamiento de mirar a la cámara al agarrar
        isGrabbed = true;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        // Desactiva el comportamiento manteniendo la rotación final al soltar
        isGrabbed = false;
    }

    void OnDestroy()
    {
        // Limpia los eventos cuando se destruye el objeto
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }
}

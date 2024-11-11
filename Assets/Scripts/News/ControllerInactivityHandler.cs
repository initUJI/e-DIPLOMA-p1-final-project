using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ControllerInactivityHandler : MonoBehaviour
{
    [SerializeField] private XRController leftHandController;
    [SerializeField] private XRController rightHandController;
    [SerializeField] private GameObject leftHandModel;    // Modelo del mando izquierdo
    [SerializeField] private GameObject rightHandModel;   // Modelo del mando derecho
    [SerializeField] private float inactivityTime = 3f;   // Tiempo de inactividad en segundos antes de desactivar el rayo

    private float leftHandIdleTimer = 0f;
    private float rightHandIdleTimer = 0f;

    private Vector3 lastLeftHandPosition;
    private Vector3 lastRightHandPosition;

    private XRRayInteractor leftRayInteractor;
    private XRRayInteractor rightRayInteractor;

    private LineRenderer leftLineRenderer;
    private LineRenderer rightLineRenderer;

    void Start()
    {
        // Obtener los componentes XRRayInteractor y LineRenderer
        if (leftHandController != null)
        {
            leftRayInteractor = leftHandController.GetComponent<XRRayInteractor>();
            leftLineRenderer = leftHandController.GetComponent<LineRenderer>();
            lastLeftHandPosition = leftHandController.transform.position;
        }

        if (rightHandController != null)
        {
            rightRayInteractor = rightHandController.GetComponent<XRRayInteractor>();
            rightLineRenderer = rightHandController.GetComponent<LineRenderer>();
            lastRightHandPosition = rightHandController.transform.position;
        }
    }

    void Update()
    {
        CheckControllerInactivity(leftHandController, leftHandModel, ref leftRayInteractor, ref leftLineRenderer, ref leftHandIdleTimer, ref lastLeftHandPosition);
        CheckControllerInactivity(rightHandController, rightHandModel, ref rightRayInteractor, ref rightLineRenderer, ref rightHandIdleTimer, ref lastRightHandPosition);
    }

    private void CheckControllerInactivity(XRController controller, GameObject handModel, ref XRRayInteractor rayInteractor, ref LineRenderer lineRenderer, ref float idleTimer, ref Vector3 lastPosition)
    {
        if (controller == null || rayInteractor == null || lineRenderer == null || handModel == null)
            return;

        // Verificar si el controlador está encendido
        InputDevice device = controller.inputDevice;
        bool isDeviceActive = device.isValid && device.TryGetFeatureValue(CommonUsages.isTracked, out bool isTracked) && isTracked;

        if (!isDeviceActive)
        {
            // Si el mando no está encendido, ocultar el modelo, rayo y línea
            rayInteractor.enabled = false;
            lineRenderer.enabled = false;
            handModel.SetActive(false);
            return;
        }

        // Obtener la posición actual del controlador
        Vector3 currentPosition = controller.transform.position;

        // Comprobar si el controlador se ha movido
        if (Vector3.Distance(currentPosition, lastPosition) < 0.001f)
        {
            // Si no se ha movido, aumentar el temporizador de inactividad
            idleTimer += Time.deltaTime;

            // Si el tiempo de inactividad supera el tiempo especificado, desactivar el rayo, la línea y el modelo
            if (idleTimer >= inactivityTime)
            {
                rayInteractor.enabled = false;
                lineRenderer.enabled = false;
                handModel.SetActive(false); // Desactivar el modelo del mando
            }
        }
        else
        {
            // Si el controlador se ha movido, reiniciar el temporizador y activar el rayo, la línea y el modelo
            idleTimer = 0f;
            rayInteractor.enabled = true;
            lineRenderer.enabled = true;
            handModel.SetActive(true); // Activar el modelo del mando
        }

        // Actualizar la última posición conocida del controlador
        lastPosition = currentPosition;
    }
}

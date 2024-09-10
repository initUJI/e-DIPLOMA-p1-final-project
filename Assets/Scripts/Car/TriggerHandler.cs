using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
    // Referencia al CarController
    public CarController carController;

    private void OnTriggerEnter(Collider other)
    {
        if (carController != null)
        {
            carController.OnExternalColliderTriggerEnter(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (carController != null)
        {
            carController.OnExternalColliderTriggerStay(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (carController != null)
        {
            carController.OnExternalColliderTriggerExit(other);
        }
    }
}

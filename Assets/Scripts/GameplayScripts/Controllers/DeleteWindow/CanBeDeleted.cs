using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class CanBeDeleted : MonoBehaviour
{
    public XRController principalHandController;
    XRRayInteractor principalRay;
    public XRBaseInteractable interactable;
    public bool primaryButtonValue;
    public bool openWindow;
    public GameObject currentWindow;

    [SerializeField] GameObject deleteWindowPrefab;
    private EventsManager eventsManager;
    private GameManager gameManager;

    private void Start()
    {
        interactable = GetComponent<XRBaseInteractable>();
        currentWindow = null;
        openWindow = false;

        principalHandController = GameObject.Find("RightHand Controller").GetComponent<XRController>();
        if (principalHandController != null)
        {
            principalRay = principalHandController.GetComponent<XRRayInteractor>();
        }

        /*gameManager = FindAnyObjectByType<GameManager>();
        gameManager.InvokeAfterInit(this, () =>
        {
            principalHandController = gameManager.principalHandController;
            if (principalHandController != null)
            {
                principalRay = principalHandController.GetComponent<XRRayInteractor>();
            }   
        });*/
    }

    private void Update()
    {
        if (interactable != null && principalHandController != null && principalRay != null)
        {
            InputDevice device = principalHandController.inputDevice;
            if (device != null && device.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonValue))
            {
                if (principalRay.IsHovering(interactable) && primaryButtonValue && !openWindow)
                {
                    // faire apparaitre la fenetre si elle n'existe pas
                    currentWindow = Instantiate(deleteWindowPrefab, transform);
                    currentWindow.transform.parent = transform;
                    currentWindow.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, -transform.localEulerAngles.y, transform.localEulerAngles.z);
                    currentWindow.transform.localPosition = currentWindow.transform.right * -0.5f;
                    currentWindow.transform.LookAt(principalHandController.gameObject.transform);

                    openWindow = true;
                    //Debug.Log("Ouverture !");

                    if (eventsManager == null)
                    {
                        eventsManager = FindFirstObjectByType<EventsManager>();
                    }

                    eventsManager.deleteWindowOpen();
                } /*else if(!primaryButtonValue && openWindow)
                {
                    closeDeleteWindow();
                }*/
            }
        }
    }

    public void closeDeleteWindow()
    {
        // fermer la fenetre
        currentWindow.GetComponent<BouncyScaleScript>().f_ScaleUpOrDown();
        //Destroy(currentWindow);
        //Debug.Log("Fermeture !");
        openWindow = false;

        if (eventsManager == null)
        {
            eventsManager = FindFirstObjectByType<EventsManager>();
        }

        eventsManager.deleteWindowClose();
    }
}

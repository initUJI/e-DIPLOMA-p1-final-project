using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketsControl : MonoBehaviour
{
    XRSocketInteractor[] sockets;
    XRGrabInteractable _interactable;
    void Start()
    {
        _interactable = GetComponent<XRGrabInteractable>();
        sockets = GetComponentsInChildren<XRSocketInteractor>();

        _interactable.selectEntered.AddListener(f_ToogleSockets);
        _interactable.selectExited.AddListener(f_ActivateSockets);
    }

    public XRSocketInteractor[] getSockets()
    {
        sockets = GetComponentsInChildren<XRSocketInteractor>();
        return sockets;
    }

    public void f_DeactivateSockets()
    {
        foreach (XRSocketInteractor s in sockets)
        {
            s.gameObject.SetActive(false);
        }
    }

    public void f_ActivateSockets(SelectExitEventArgs args)
    {
        foreach (XRSocketInteractor s in sockets)
        {
            s.gameObject.SetActive(true);
        }
    }

    public void f_ToogleSockets(SelectEnterEventArgs args)
    {
        bool setTo = args.interactorObject.transform.GetComponent<XRRayInteractor>() ? false : true;
        foreach (XRSocketInteractor s in sockets)
        {
            s.gameObject.SetActive(setTo);
        }
    }
}
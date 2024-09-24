using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class panelMover : MonoBehaviour
{
    // Variables p�blicas para definir las dos posiciones
    public Vector3 upperPosition;  // Posici�n cuando el panel est� arriba
    public Vector3 lowerPosition;  // Posici�n cuando el panel est� abajo
    public float moveDuration = 0.5f;  // Duraci�n del movimiento

    // Variable para saber si el panel est� en la posici�n superior
    private bool isPanelUp = false;

    // Funci�n p�blica que se activar� desde el bot�n
    public void TogglePanelPosition()
    {
        if (isPanelUp)
        {
            // Mover el panel a la posici�n inferior
            LeanTween.moveLocal(gameObject, lowerPosition, moveDuration).setEase(LeanTweenType.easeInOutQuad);
        }
        else
        {
            // Mover el panel a la posici�n superior
            LeanTween.moveLocal(gameObject, upperPosition, moveDuration).setEase(LeanTweenType.easeInOutQuad);
        }

        // Cambiar el estado del panel
        isPanelUp = !isPanelUp;
    }
}

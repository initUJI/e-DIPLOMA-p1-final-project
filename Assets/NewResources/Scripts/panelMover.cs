using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class panelMover : MonoBehaviour
{
    // Variables públicas para definir las dos posiciones
    public Vector3 upperPosition;  // Posición cuando el panel está arriba
    public Vector3 lowerPosition;  // Posición cuando el panel está abajo
    public float moveDuration = 0.5f;  // Duración del movimiento

    // Variable para saber si el panel está en la posición superior
    private bool isPanelUp = false;

    // Función pública que se activará desde el botón
    public void TogglePanelPosition()
    {
        if (isPanelUp)
        {
            // Mover el panel a la posición inferior
            LeanTween.moveLocal(gameObject, lowerPosition, moveDuration).setEase(LeanTweenType.easeInOutQuad);
        }
        else
        {
            // Mover el panel a la posición superior
            LeanTween.moveLocal(gameObject, upperPosition, moveDuration).setEase(LeanTweenType.easeInOutQuad);
        }

        // Cambiar el estado del panel
        isPanelUp = !isPanelUp;
    }
}

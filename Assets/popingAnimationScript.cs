using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class popingAnimationScript : MonoBehaviour
{
    // Escala m�nima y m�xima entre las cuales el GameObject alternar�
    public Vector3 minScale = new Vector3(1, 1, 1);
    public Vector3 maxScale = new Vector3(2, 2, 2);

    // Duraci�n del tiempo de escalado en segundos
    public float scaleDuration = 1f;

    void Start()
    {
        // Iniciar el escalado hacia el tama�o m�ximo
        ScaleUp();
    }

    void ScaleUp()
    {
        // Escalar hacia el tama�o m�ximo
        LeanTween.scale(gameObject, maxScale, scaleDuration).setEase(LeanTweenType.easeInOutSine)
            .setOnComplete(ScaleDown); // Al completar, llamar a ScaleDown para alternar
    }

    void ScaleDown()
    {
        // Escalar hacia el tama�o m�nimo
        LeanTween.scale(gameObject, minScale, scaleDuration).setEase(LeanTweenType.easeInOutSine)
            .setOnComplete(ScaleUp); // Al completar, llamar a ScaleUp para alternar
    }
}

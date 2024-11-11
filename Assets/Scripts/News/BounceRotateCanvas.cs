using System.Collections;
using UnityEngine;

public class BounceRotateCanvas : MonoBehaviour
{
    public AnimationCurve bounceCurve;    // Curva de animación para el rebote
    public float animationTime = 1f;      // Duración de la animación en segundos
    public float bounceHeight = 5f;       // Altura máxima del rebote, ajustada para un movimiento pequeño
    public float rotationSpeed = 45f;     // Velocidad de rotación en grados por segundo

    public enum RotationAxis { X, Y, Z }  // Enum para seleccionar el eje de rotación
    public RotationAxis rotationAxis = RotationAxis.Z;  // Eje de rotación por defecto

    private Vector3 originalPosition;     // Posición original del Canvas
    private Coroutine animationCoroutine;

    void Start()
    {
        originalPosition = transform.localPosition;
        StartInfiniteBounceRotation();
    }

    public void StartInfiniteBounceRotation()
    {
        if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        animationCoroutine = StartCoroutine(InfiniteBounceRotationRoutine());
    }

    private IEnumerator InfiniteBounceRotationRoutine()
    {
        while (true)
        {
            float elapsedTime = 0f;

            while (elapsedTime < animationTime)
            {
                float progress = elapsedTime / animationTime;

                // Movimiento de rebote pequeño en el eje Y
                float bounceOffset = bounceCurve.Evaluate(progress) * bounceHeight;
                transform.localPosition = originalPosition + new Vector3(0, bounceOffset, 0);

                // Aplicar rotación en el eje seleccionado
                switch (rotationAxis)
                {
                    case RotationAxis.X:
                        transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0);
                        break;
                    case RotationAxis.Y:
                        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
                        break;
                    case RotationAxis.Z:
                        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
                        break;
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}

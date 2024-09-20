using UnityEngine;

public class InfoPopup : MonoBehaviour
{
    public float displayDuration = 3f; // Tiempo que se mostrará el popup
    public Vector3 initialScale = Vector3.zero; // Escala inicial para animación
    public Vector3 targetScale = Vector3.one; // Escala objetivo para animación
    public float scaleDuration = 0.5f; // Duración de la animación de escala

    private void Start()
    {
        // Ajustar la escala inicial
        transform.localScale = initialScale;

        // Animar la escala del popup para que aparezca
        LeanTween.alpha(gameObject, 1, scaleDuration);
        LeanTween.scale(gameObject, targetScale, scaleDuration).setOnComplete(() =>
        {
            // Una vez completada la animación de aparición, esperar y luego desaparecer
            StartCoroutine(WaitAndDisappear());
        });
    }

    // Corutina para esperar antes de desaparecer
    private System.Collections.IEnumerator WaitAndDisappear()
    {
        yield return new WaitForSeconds(displayDuration);

        // Animar la desaparición del popup
        LeanTween.alpha(gameObject, 0, scaleDuration);
        LeanTween.scale(gameObject, Vector3.zero, scaleDuration).setOnComplete(() =>
        {
            Destroy(gameObject); // Destruir el prefab una vez ha desaparecido
        });
    }
}

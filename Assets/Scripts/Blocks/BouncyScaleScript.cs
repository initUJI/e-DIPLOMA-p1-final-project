using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyScaleScript : MonoBehaviour
{
    public AnimationCurve scaleCurve;
    public float animationTime = 0.4f;
    [Header("FPS")]
    public float scaleFrameTime = 24f;
    [Header("Add-ons")]
    public bool destroyWhenScaledToZero = true;
    public bool particleEffect = true;
    [SerializeField]
    private GameObject effect;

    private bool scaleUp = true;
    private float originalAnimationTime;
    private Vector3 objectOriginalScale;
    private Coroutine lastCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        if (particleEffect && effect == null) Debug.LogWarning("，ATENCION! Boolean particleEffect es true\nNecesitas asignar la variable effect (GameObject con ParticleSystem) a la componente BouncyScript del objeto " + gameObject.name + " para que haya un efecto." + "\nATENTION! Boolean particleEffect is set to true\nYou need to assign the variable effect (GameObject with PaticleSystem) to the component BouncyScript of the object " + gameObject.name + " in order to have an effect.");

        objectOriginalScale = transform.localScale;
        originalAnimationTime = animationTime;
        scaleFrameTime = 1 / scaleFrameTime;

        scaleUp = true;
        lastCoroutine = StartCoroutine(BouncyScale(null));
    }

    public void f_ScaleUpOrDown()
    {
        lastCoroutine = StartCoroutine(BouncyScale(lastCoroutine));
    }

    /// <summary>
    /// It scales the object contrary to the last call. If not coroutine was active before, just pass a null
    /// </summary>
    IEnumerator BouncyScale(Coroutine lastActive)
    {
        Vector3 actualScale = objectOriginalScale;
        animationTime = originalAnimationTime;
        if (particleEffect && effect != null && scaleUp) Instantiate(effect, transform);

        if (lastActive != null) // If it was at the middle of the other coroutine, stops it and starts the next scale coorutine from the point it was left
        {
            StopCoroutine(lastActive);

            scaleUp = !scaleUp;
            if ((transform.localScale.x / objectOriginalScale.x) < 1) animationTime = originalAnimationTime * (transform.localScale.x / objectOriginalScale.x); //Calculates new animation time depending of how much it needs to scale

            actualScale = scaleUp ? objectOriginalScale : transform.localScale;
        }

        float elapsedTime = 0f;
        float animTime = 0;
        bool disapearEffect = false;

        while (elapsedTime < animationTime)
        {
            animTime = scaleUp ? elapsedTime / animationTime : 1 - elapsedTime / animationTime;

            transform.localScale = actualScale * scaleCurve.Evaluate(animTime);

            elapsedTime += scaleFrameTime * animationTime;

            if (particleEffect && effect != null && !scaleUp && !disapearEffect && elapsedTime > animationTime * 0.7) //Effect when fading out
            {
                disapearEffect = true;
                Instantiate(effect, transform);
            }

            yield return new WaitForSeconds(scaleFrameTime * animationTime);
        }

        transform.localScale = scaleUp ? objectOriginalScale : Vector3.zero;
        scaleUp = !scaleUp;

        lastCoroutine = null;

        if (scaleUp && destroyWhenScaledToZero) GameObject.Destroy(gameObject); // Destroy if scaled to Zero and inspector Boolean of destroying active
    }

}

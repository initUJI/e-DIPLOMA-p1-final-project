using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class is associated with the character moving through the level. It manages the character's actions and animation.
 */
public class Character : MonoBehaviour
{

    const float ANIMATION_TIME_OBJECT = 1.0f;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private Quaternion transformRotationSave;
    private GameObject child;
    private float unit = 2f;
    private const float speed = 2f;
    private const float rotateSpeed = 200f;
    private float timeAnimationCutOrBreak = ANIMATION_TIME_OBJECT;
    private Transform levelTransform;
    private EventsManager eventsManager;

    public bool isForwarding;
    public bool isBehinding;
    public bool isRotating;
    public bool isAnimated;
    public bool isCutting;
    public bool isBreaking;
    public bool objectIsFalling;
    public Material carGlow;
    public GameObject waterEffect;
    
    private Material initialMaterial;

    public Animator animator;

    private void Start()
    {
        isForwarding = false;
        isBehinding = false;
        isRotating = false;
        isAnimated = false;
        isCutting = false;
        isBreaking = false;
        objectIsFalling = false;
        child = GameObject.FindGameObjectWithTag("Car");
        levelTransform = transform.parent.parent.parent.transform;
        unit *= levelTransform.localScale.x;
        eventsManager = FindObjectOfType<EventsManager>();
        targetPosition = transform.position;
        targetRotation = transform.rotation;
        initialMaterial = child.transform.GetChild(0).transform.GetChild(0).GetComponent<MeshRenderer>().materials[1];
    }

    public void activeWater()
    {
        waterEffect.GetComponent<ParticleSystem>().Play();
        waterEffect.GetComponent<AudioSource>().Play();
    }

    public void activeGlow()
    {
        MeshRenderer renderer = child.transform.GetChild(0).transform.GetChild(0).GetComponent<MeshRenderer>();

        // Obtener una copia del array de materiales
        Material[] materials = renderer.materials;

        // Imprimir el material antes de cambiarlo
       // Debug.Log("Antes: " + materials[1]);

        // Cambiar el segundo material
        materials[1] = carGlow;

        // Asignar el array modificado de vuelta al MeshRenderer
        renderer.materials = materials;

        // Imprimir el material después de cambiarlo
       // Debug.Log("Después: " + materials[1]);
    }

    public void desactiveGlow()
    {
        MeshRenderer renderer = child.transform.GetChild(0).transform.GetChild(0).GetComponent<MeshRenderer>();

        // Obtener una copia del array de materiales
        Material[] materials = renderer.materials;

        // Imprimir el material antes de cambiarlo
        // Debug.Log("Antes: " + materials[1]);

        // Cambiar el segundo material
        materials[1] = initialMaterial;

        // Asignar el array modificado de vuelta al MeshRenderer
        renderer.materials = materials;

        // Imprimir el material después de cambiarlo
        // Debug.Log("Después: " + materials[1]);
    }

    public bool Motionless()
    {
        return !(isForwarding || isRotating /*|| isCutting || isBreaking || isAnimated || objectIsFalling || animator.enabled*/);
    }

    public void Forward()
    {
        GameObject objectInFront = GameManager.objectInFront;
        if (objectInFront == null)
        {
            PlayAnimation("Walking");
            isForwarding = true;
            Debug.Log("Go forward!");
            targetPosition += transform.forward * unit;
        }
        else
        {
            if (GameManager.printScreenTMP != null)
            {
                GameManager.printScreenTMP.text = "Cannot forwarding because there is a " + GameManager.objectInFront.tag;
            }
            
        }
    }

    public void Behind()
    {
        GameObject objectInBehind = GameManager.objectInBehind;
        if (objectInBehind == null)
        {
            PlayAnimation("Walking");
            isBehinding = true;
            Debug.Log("Go behind!");
            targetPosition -= transform.forward * unit;
        }
        else
        {
            GameManager.printScreenTMP.text = "Cannot because there is a " + GameManager.objectInBehind.tag;
        }
    }

    public void Cut()
    {
        if (GameManager.objectInFront != null && GameManager.objectInFront.CompareTag("TREE"))
        {
            PlayAnimation("Cut");
            isCutting = true;
        }
        else
        {
            GameManager.printScreenTMP.text = "There is no tree!";
        }
    }

    public void Break()
    {
        if (GameManager.objectInFront != null && GameManager.objectInFront.CompareTag("ROCK"))
        {
            PlayAnimation("Break");
            isBreaking = true;
        }
        else
        {
            if (GameManager.printScreenTMP != null)
            {
                GameManager.printScreenTMP.text = "There is no rock!";
            }     
        }
    }

    public void TurnRight()
    {
        isRotating = true;
        Debug.Log("Turn right!");
        targetRotation *= Quaternion.Euler(0f, 90f, 0f);
    }

    public void TurnLeft()
    {
        isRotating = true;
        Debug.Log("Turn left!");
        targetRotation *= Quaternion.Euler(0f, -90f, 0f);
    }

    public void Update()
    {

        if (isForwarding)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // Obtiene las posiciones de los objetos, pero establece sus valores de y en 0
            Vector3 position1 = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 position2 = new Vector3(targetPosition.x, 0, targetPosition.z);

            // Calcula la distancia entre los dos puntos en el plano XY y muestra el resultado en la consola
            float distance = Vector3.Distance(position1, position2);

            if (distance < 0.01 && distance > -0.01)
            {
                child = GameObject.FindGameObjectWithTag("Car");
                isForwarding = false;
                isAnimated = false;
                targetPosition = transform.position;
                child.transform.rotation.Set(Quaternion.identity.x, Quaternion.identity.y, Quaternion.identity.z, Quaternion.identity.w);
            }
        }

        if (isBehinding)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if (Mathf.Approximately(transform.position.z, targetPosition.z)
                && Mathf.Approximately(transform.position.y, targetPosition.y)
                && Mathf.Approximately(transform.position.x, targetPosition.x))
            {
                child = GameObject.FindGameObjectWithTag("Car");
                isBehinding = false;
                isAnimated = false;
                targetPosition = transform.position;
                child.transform.rotation.Set(Quaternion.identity.x, Quaternion.identity.y, Quaternion.identity.z, Quaternion.identity.w);
            }
        }

        if (isRotating)
        {
            if (transform.rotation.eulerAngles.y != targetRotation.eulerAngles.y)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            }
            else
            {
                child = GameObject.FindGameObjectWithTag("Car");
                isRotating = false;
                isAnimated = false;
                targetRotation = transform.rotation;
                child.transform.rotation.Set(Quaternion.identity.x, Quaternion.identity.y, Quaternion.identity.z, Quaternion.identity.w);
            }
        }

        /*if (isCutting)
        {
            if (timeAnimationCutOrBreak >= 0f)
            {
                timeAnimationCutOrBreak -= Time.deltaTime;
            }
            else if (timeAnimationCutOrBreak <= 0f)
            {
                isCutting = false;
                isAnimated = false;
                timeAnimationCutOrBreak = ANIMATION_TIME_OBJECT;

                if (GameManager.objectInFront.tag.Equals("TREE"))
                {
                    GameObject go = GameManager.objectInFront;
                    Quaternion goInitialRotation = go.transform.rotation;
                    Vector3 goInitialScale = go.transform.localScale;


                    go.GetComponent<Rigidbody>().detectCollisions = false;
                    objectIsFalling = true;
                    go.GetComponent<Animator>().Play("Tree", -1, 1);
                   

                    StartCoroutine(Tool.c_InvokeAfterWait(1.0f, () =>
                    {
                        Debug.Log(go == null);
                        go.SetActive(false);
                        go.transform.rotation = goInitialRotation;
                        go.transform.localScale = goInitialScale;
                        objectIsFalling = false;
                    }));



                } else
                {
                    GameManager.ReportError(GameManager.currentBlock, "The character can only cut down trees");
                }

            }
        }*/

       /* if (isBreaking)
        {
            if (timeAnimationCutOrBreak >= 0f)
            {

                timeAnimationCutOrBreak -= Time.deltaTime;
            }
            else if (timeAnimationCutOrBreak <= 0f)
            {
                isBreaking = false;
                isAnimated = false;
                timeAnimationCutOrBreak = ANIMATION_TIME_OBJECT;

                if (GameManager.objectInFront.tag.Equals("ROCK"))
                {
                    GameObject go = GameManager.objectInFront;
                    Quaternion goInitialRotation = go.transform.rotation;
                    Vector3 goInitialScale = go.transform.localScale;


                    go.GetComponent<Rigidbody>().detectCollisions = false;
                    objectIsFalling = true;
                    go.GetComponent<Animator>().Play("Rock", -1, 1);


                    StartCoroutine(Tool.c_InvokeAfterWait(1.0f, () =>
                    {
                        Debug.Log(go == null);
                        go.SetActive(false);
                        go.transform.rotation = goInitialRotation;
                        go.transform.localScale = goInitialScale;
                        objectIsFalling = false;
                    }));

                } else
                {
                    GameManager.ReportError(GameManager.currentBlock, "The character can only break rocks");
                }

            }
        }*/

        // This case is useful for keeping the animation fluid. When the isAnimated variable is set to false,
        // we wait until the character has recovered its original position before stopping the animation.
        //if (!isAnimated && Mathf.Abs(child.transform.rotation.z) < 0.01)
        //{
           // animator.enabled = false;
        //}
    }

    public void ResetTheTargetPosition(Vector3 initialTargetPosition, Quaternion initialTargetRotation)
    {
        targetPosition = initialTargetPosition;
        targetRotation = initialTargetRotation;
    }

    private void PlayAnimation(string animation)
    {
        isAnimated = true;
        if (animator != null)
        {
            animator.enabled = true;
            animator.Play(animation);
        }
        
    }

}
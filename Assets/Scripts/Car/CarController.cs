using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    // Campos expuestos en el inspector
    public Collider frontCollider;         // Collider frontal asignado desde el inspector
    //public GameObject waterEffect;         // Efecto de agua asignado desde el inspector
    public Material carGlow;               // Material para el efecto de "glow" del coche asignado desde el inspector

    // Variable privada para almacenar el objeto con el que el coche está colisionando
    private GameObject currentCollidingObject;

    // Booleans usados para gestionar estados de movimiento
    [HideInInspector] public bool isForwarding;
    [HideInInspector] public bool isBehinding;
    [HideInInspector] public bool isAnimated;
    [HideInInspector] public bool isRotating;

    // Material inicial del coche (para restaurar después de activar el brillo)
    private Material initialMaterial;

    // Variables para gestionar posiciones y rotaciones del coche
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private Transform levelTransform;      // Transform del nivel para obtener su escala
    private GameObject child;              // Objeto hijo que representa el coche

    // Variables para gestionar la velocidad de movimiento y rotación
    private float unit = 2f;               // Unidad de movimiento (modificado por el tamaño del nivel)
    private const float speed = 2f;        // Velocidad de movimiento del coche
    private const float rotateSpeed = 200f; // Velocidad de rotación del coche

    void Start()
    {
        // Inicialización de los estados
        isForwarding = false;
        isBehinding = false;
        isAnimated = false;
        isRotating = false;

        // Posición y rotación objetivo iniciales (iguales a la posición y rotación actuales)
        targetPosition = transform.position;
        targetRotation = transform.rotation;

        // Obtener el transform del nivel
        levelTransform = transform.parent.parent.transform;
        unit *= levelTransform.localScale.x; // Ajustar la unidad de movimiento según la escala del nivel

        // Encontrar el objeto hijo del coche mediante la etiqueta "Car"
        child = GameObject.FindGameObjectWithTag("Car");

        // Validar si el collider frontal está asignado
        if (frontCollider == null)
        {
            Debug.LogError("El collider frontal no está asignado en el inspector.");
        }
        else
        {
            // Asegurarse de que el collider actúe como trigger
            frontCollider.isTrigger = true;

            // Añadir el componente TriggerHandler al objeto que contiene el collider
            TriggerHandler triggerHandler = frontCollider.gameObject.AddComponent<TriggerHandler>();

            // Pasar la referencia del CarController al TriggerHandler
            triggerHandler.carController = this;
        }
    }

    // Método que indica si el coche está quieto (sin moverse ni rotar)
    public bool Motionless()
    {
        return !(isForwarding || isRotating);
    }

    // Movimiento hacia adelante, solo si no hay objeto colisionando enfrente
    public IEnumerator Forward()
    {
        isForwarding = true;
        targetPosition = transform.position + transform.forward * 2f; // Ejemplo de movimiento hacia adelante

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            // Mover el personaje hacia adelante
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 2f); // Velocidad ajustable
            yield return null; // Esperar al siguiente frame
        }

        // Finaliza el movimiento
        isForwarding = false;
    }

    // Movimiento hacia atrás, solo si no hay objeto colisionando detrás
    public void Behind()
    {
        GameObject objectInBehind = GetCollidingObject();
        if (objectInBehind == null)
        {
            isBehinding = true;
            Debug.Log("Go behind!");
            targetPosition -= transform.forward * unit; // Calcular la nueva posición objetivo
        }
    }

    // Activar el efecto de agua (partículas y sonido)
    public void activeWater()
    {
        /*if (waterEffect != null)
        {
            waterEffect.GetComponent<ParticleSystem>().Play();
            waterEffect.GetComponent<AudioSource>().Play();
        }*/
    }

    // Método unificado para activar o desactivar el "glow"
    public void SetGlow(bool activateGlow)
    {
        MeshRenderer renderer = child.transform.GetChild(0).transform.GetChild(0).GetComponent<MeshRenderer>();

        Material[] materials = renderer.materials;        // Obtener una copia del array de materiales

        if (activateGlow)
        {
            materials[1] = carGlow;            // Activar el "glow", asignando el material del brillo
        }
        else
        {
            materials[1] = initialMaterial;            // Desactivar el "glow", restaurando el material original
        }

        renderer.materials = materials;        // Asignar el array modificado de vuelta al MeshRenderer
    }


    // Rotación del coche hacia la derecha
    public void TurnRight()
    {
        isRotating = true;
        Debug.Log("Turn right!");
        targetRotation *= Quaternion.Euler(0f, 90f, 0f); // Actualizar la rotación objetivo
    }

    // Rotación del coche hacia la izquierda
    public void TurnLeft()
    {
        isRotating = true;
        Debug.Log("Turn left!");
        targetRotation *= Quaternion.Euler(0f, -90f, 0f); // Actualizar la rotación objetivo
    }

    // Resetear la posición y rotación objetivo
    public void ResetTheTargetPosition(Vector3 initialTargetPosition, Quaternion initialTargetRotation)
    {
        targetPosition = initialTargetPosition;
        targetRotation = initialTargetRotation;
    }

    // Actualización en cada frame
    public void Update()
    {
        // Gestionar el movimiento hacia adelante
        if (isForwarding)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // Comprobar si el coche ha alcanzado la posición objetivo
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isForwarding = false;
                isAnimated = false;
                targetPosition = transform.position;
            }
        }

        // Gestionar el movimiento hacia atrás
        if (isBehinding)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if (Mathf.Approximately(transform.position.z, targetPosition.z)
                && Mathf.Approximately(transform.position.x, targetPosition.x))
            {
                isBehinding = false;
                isAnimated = false;
                targetPosition = transform.position;
            }
        }

        // Gestionar la rotación del coche
        if (isRotating)
        {
            if (transform.rotation.eulerAngles.y != targetRotation.eulerAngles.y)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            }
            else
            {
                isRotating = false;
                isAnimated = false;
                targetRotation = transform.rotation;
            }
        }
    }

    // Métodos para gestionar colisiones con el trigger
    public void OnExternalColliderTriggerEnter(Collider other)
    {
        currentCollidingObject = other.gameObject;
    }

    public void OnExternalColliderTriggerStay(Collider other)
    {
        currentCollidingObject = other.gameObject;
    }

    public void OnExternalColliderTriggerExit(Collider other)
    {
        if (currentCollidingObject == other.gameObject)
        {
            currentCollidingObject = null;
        }
    }

    // Devuelve el objeto con el que el coche está colisionando actualmente
    public GameObject GetCollidingObject()
    {
        return currentCollidingObject;
    }
}

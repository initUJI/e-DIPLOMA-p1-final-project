using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterWallController : MonoBehaviour
{
    [SerializeField] private GameObject arrowDown;
    [SerializeField] private GameObject arrowUp;
    private Animator animator;

    [HideInInspector] public bool wallUP;

    // Start is called before the first frame update
    void Start()
    {
        arrowDown.SetActive(true);
        arrowUp.SetActive(false);
        wallUP = false;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void centerWallDown()
    {
        StartCoroutine(WaitForAnimation("CenterWallDown"));
    }

    public void centerWallUp()
    {
        StartCoroutine(WaitForAnimation("CenterWallUp"));
    }

    IEnumerator WaitForAnimation(string animation)
    {
        animator.Play(animation);
        AnimatorStateInfo animationState = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(animationState.length);
        //Debug.Log("La animación ha terminado!");
        // Coloca aquí el código que quieres ejecutar al finalizar la animación

        switch (animation) {
            case "CenterWallDown": arrowUp.SetActive(true); arrowDown.SetActive(false); wallUP = false; break;
            case "CenterWallUp": arrowUp.SetActive(false); arrowDown.SetActive(true); wallUP = true; break;
            default: break;
        }
    }
}

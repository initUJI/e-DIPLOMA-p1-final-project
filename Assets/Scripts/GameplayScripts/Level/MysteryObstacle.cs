using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryObstacle : MonoBehaviour
{
    [SerializeField] GameObject[] obstacles;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("CharacterCollider"))
        {
            GameObject mysteryObstacle = Instantiate(obstacles[Random.Range(0, obstacles.Length)], transform.position, transform.rotation);
            mysteryObstacle.transform.parent = transform.parent;

            GameManager.executionObstacles.Add(mysteryObstacle);

            gameObject.SetActive(false);
        }
    }
}

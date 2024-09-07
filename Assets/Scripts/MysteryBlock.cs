using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryBlock : MonoBehaviour
{
    public GameObject[] obstacle;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("CharacterCollider"))
        {
            GameObject mysteryObstacle = Instantiate(obstacle[Random.Range(0, obstacle.Length)], transform.position, transform.rotation);
            mysteryObstacle.transform.parent = transform.parent;
            Destroy(gameObject);
        }
    }
}

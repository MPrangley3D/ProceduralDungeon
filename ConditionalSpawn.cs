using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalSpawn : MonoBehaviour
{
    public GameObject[] objects;

    private void Start()
    {
        int roll = Random.Range(1, 101);

        if (roll < 65)
        {
            int rand = Random.Range(0, objects.Length);
            GameObject newTile = Instantiate(objects[rand], transform.position, Quaternion.identity);
            newTile.transform.parent = this.gameObject.transform;
        }
    }
}

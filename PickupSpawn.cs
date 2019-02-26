using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawn : MonoBehaviour
{
    public GameObject[] objects;

    private void Start()
    {
        int roll = Random.Range(1, 101);

        if (roll < 25)
        {
            int rand = Random.Range(0, objects.Length);
            GameObject newPickup = Instantiate(objects[rand], transform.position, Quaternion.identity);
            newPickup.transform.parent = this.gameObject.transform;
        }
    }
}

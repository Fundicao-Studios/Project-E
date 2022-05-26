using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject dungeonSpawnPoint;

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameObject.Find("Player").transform.position = dungeonSpawnPoint.transform.position;
        }
    }
}

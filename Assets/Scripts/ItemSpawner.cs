using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour, Interactible
{
    public GameObject itemToSpawn;
    public Transform location;

    public void PerformAction()
    {
        Instantiate(itemToSpawn, location.position, Quaternion.identity);
    }
}

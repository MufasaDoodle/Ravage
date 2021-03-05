using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour, Interactible
{
    public GameObject itemToSpawn;
    public Transform location;

    public int cost = 100;

    public void PerformAction()
    {
        if(cost < 0)
        {
            return;
        }

        if (Player.Instance.EconomyController.SpendMoney(cost))
        {
            Instantiate(itemToSpawn, location.position, Quaternion.identity);
        }
    }
}

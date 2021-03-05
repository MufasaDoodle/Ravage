using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_Destroyer : MonoBehaviour
{
    void OnTriggerEnter(Collider collision)
    {
        Destroy(collision.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractController : MonoBehaviour
{
    public float radius = 3f;
    public Camera playerCam;

    public delegate void PickupChanged(Pickup pickup);
    public PickupChanged pickupChangedCB;

    public delegate void InteractChanged(string text);
    public InteractChanged interactChangedCB;

    private Pickup currentItem;
    private string currentInteractible;

    // Update is called once per frame
    void Update()
    {
        var ray = playerCam.ViewportPointToRay(Vector3.one * 0.5f);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, radius))
        {
            var pickupable = hit.transform.GetComponent<Pickup>();

            if (pickupable != null) //if item has the pickup component
            {
                SetFocusedPickup(pickupable);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    Player.Instance.PickUpItem(pickupable);
                    Destroy(hit.transform.gameObject);
                    SetFocusedPickup(null);
                    return;
                }
            }
            else //if object is not pickupabble
            {
                if (currentItem != null)
                {
                    SetFocusedPickup(null);
                }
            }

            var interactible = hit.transform.GetComponent<Interactible>();

            if (interactible != null)
            {
                SetInteractible("Mag Spawner");
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactible.PerformAction();
                    return;
                }
            }
            else
            {
                if(currentInteractible != null)
                {
                    SetInteractible(null);
                }
            }
        }
        else //if found nothing
        {
            if (currentItem != null)
            {
                SetFocusedPickup(null);
            }

            if (currentInteractible != null)
            {
                SetInteractible(null);
            }
        }
    }

    private void SetInteractible(string text)
    {
        currentInteractible = text;

        if (interactChangedCB != null)
        {
            interactChangedCB.Invoke(text);
        }
    }

    private void SetFocusedPickup(Pickup item)
    {
        currentItem = (Pickup)item;

        if (pickupChangedCB != null)
        {
            pickupChangedCB.Invoke(item);
        }
    }
}

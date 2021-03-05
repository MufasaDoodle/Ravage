using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractUI : MonoBehaviour
{
    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {        

        Player.Instance.InteractController.pickupChangedCB += UpdateUI;
        Player.Instance.InteractController.interactChangedCB += UpdateUIInteractible;
    }

    void UpdateUI(Pickup pickup)
    {
        if(pickup == null)
        {
            text.transform.gameObject.SetActive(false);
        }
        else
        {
            text.transform.gameObject.SetActive(true);
            text.text = $"Press E to pick up {pickup.itemName}";
        }
    }

    void UpdateUIInteractible(string text)
    {
        if (text == null)
        {
            this.text.transform.gameObject.SetActive(false);
        }
        else
        {
            this.text.transform.gameObject.SetActive(true);
            this.text.text = $"Press E to use {text}";
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Slider healthBar;

    bool recentlyChanged;
    private float countdown = 5f;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = Player.Instance;
        recentlyChanged = false;
        player.updateHealthCB += UpdateHealthBar;
    }

    // Update is called once per frame
    void Update()
    {
        if (recentlyChanged && player.CurrentHealth != player.MaxHealth)
        {
            countdown = Mathf.Clamp(countdown -= Time.deltaTime, 0, 5);

            if (countdown == 0)
            {
                recentlyChanged = false;
                gameObject.SetActive(false);
            }
        }
        else
        {
            countdown = 5f;
        }
    }

    private void UpdateHealthBar()
    {
        countdown = 5f;
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        healthBar.maxValue = player.MaxHealth;
        healthBar.value = player.CurrentHealth;
        recentlyChanged = true;
    }
}

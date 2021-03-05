using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SprintBarUI : MonoBehaviour
{
    public Slider sprintBar;
    public GameObject player;

    public bool RecentlyChanged { get; set; }

    private SprintController playerSprintController;
    private float countdown = 5f;

    // Start is called before the first frame update
    void Start()
    {
        sprintBar = GetComponent<Slider>();
        playerSprintController = player.GetComponent<SprintController>();
        sprintBar.maxValue = playerSprintController.maxSprintCharge;
        sprintBar.value = playerSprintController.SprintCharge;
        RecentlyChanged = false;
    }

    void Update()
    {
        if (RecentlyChanged)
        {
            countdown = Mathf.Clamp(countdown -= Time.deltaTime, 0, 5);

            if(countdown == 0)
            {
                RecentlyChanged = false;
                gameObject.SetActive(false);
            }            
        }
        else
        {
            countdown = 5f;
        }
    }

    public void UpdateSprintBar()
    {
        countdown = 5f;
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        sprintBar.value = playerSprintController.SprintCharge;
        RecentlyChanged = true;
    }
}

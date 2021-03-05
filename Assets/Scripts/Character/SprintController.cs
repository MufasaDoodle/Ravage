using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Hold data pertaining to character sprinting.
/// Not a huge fan of this implementation, consider revising
/// </summary>
public class SprintController : MonoBehaviour
{
    public float maxSprintCharge = 100f;
    public float sprintSubtractionAmount = 10f;
    public float sprintRechargeAmount = 5f;

    public GameObject SprintBarUI;

    public float SprintCharge { get; set; }
    public bool IsSprinting { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        SprintCharge = maxSprintCharge;
        IsSprinting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsSprinting)
        {
            if(SprintCharge <= maxSprintCharge)
            {
                SprintCharge += sprintRechargeAmount * Time.deltaTime;
                UpdateSprintBar();
            }
        }
        else
        {
            if(SprintCharge > 0)
            {
                SprintCharge -= sprintSubtractionAmount * Time.deltaTime;
                UpdateSprintBar();
            }
        }
    }

    void UpdateSprintBar()
    {
        if(SprintBarUI != null)
        {
            SprintBarUI.GetComponent<SprintBarUI>().UpdateSprintBar();
        }
    }
}

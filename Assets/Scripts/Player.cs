using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public GunController GunController { get; private set; }
    public InteractController InteractController { get; private set; }

    public int startingHealth = 100;

    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; private set; }

    public delegate void UpdateHealth();
    public UpdateHealth updateHealthCB;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        GunController = GetComponent<GunController>();
        InteractController = GetComponent<InteractController>();
    }

    void Start()
    {
        MaxHealth = startingHealth;
        CurrentHealth = MaxHealth;
        if(updateHealthCB != null)
        {
            updateHealthCB.Invoke();
        }
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
        if (updateHealthCB != null)
        {
            updateHealthCB.Invoke();
        }

        if(CurrentHealth <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void PickUpItem(Pickup pickup)
    {
        if(pickup.type.Contains("Ammo"))
        {
            AddMagazine(pickup);
        }
        else if(pickup.type == "Health")
        {
            AddHealth(25);
        }
    }

    private void AddHealth(int amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 1, 100);

        if (updateHealthCB != null)
        {
            updateHealthCB.Invoke();
        }
    }

    private void AddMagazine(Pickup pickup)
    {
        GunController.AddMagazine(pickup);
    }
}

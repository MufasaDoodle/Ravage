using System;
using System.Collections;
using UnityEngine;

public enum GunTypes
{
    Handgun,
    AssaultRifle
}

public class Gun : MonoBehaviour
{
    public GunTypes gunType;
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 3f;
    public int magAmmo = 30;
    public int magazines = 5;

    public AudioSource audioSource;

    public AudioClip fireClip;
    public AudioClip reloadEmpty;
    public AudioClip reloadAmmoLeft;
    public AudioClip takeOut;
    public AudioClip[] bulletCasings;

    public GameObject magazinePrefab;
    public GameObject casingPrefab;

    public Transform casingEjectLocation;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    private float nextTimeToFire = 0f;
    private Animator animator;

    public bool IsReloading { get; private set; }
    public int totalAmmo { get; private set; }

    public int CurrentAmmo { get; private set; }

    public delegate void UpdateAmmo();
    public UpdateAmmo updateAmmoCallback;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        CurrentAmmo = magAmmo;
        totalAmmo = magazines * magAmmo;        

        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.25f);
        if (updateAmmoCallback != null)
        {
            updateAmmoCallback.Invoke();
        }
    }

    public void Shoot()
    {
        if (IsReloading)
        {
            return;
        }

        if (!(Time.time >= nextTimeToFire))
        {
            return;
        }

        if (CurrentAmmo <= 0)
        {
            return;
        }

        nextTimeToFire = Time.time + 1f / fireRate;

        CurrentAmmo--;
        totalAmmo = Mathf.Clamp(totalAmmo - 1, 0, 999);
        animator.Play("Fire");
        audioSource.PlayOneShot(fireClip, 0.5f);
        muzzleFlash.Play();

        if (updateAmmoCallback != null)
        {
            updateAmmoCallback.Invoke();
        }

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            var target = hit.transform.GetComponent<Target>();
            var enemy = hit.transform.GetComponent<Enemy>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
            else if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.velocity = transform.forward * impactForce;
            }

            var impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 2f);
        }
    }

    // called by an animation event
    public void Reload()
    {
        //IsReloading = true;

        if (totalAmmo < 1)
        {
            return;
        }

        if (CurrentAmmo > 1)
        {
            animator.Play("Reload Ammo Left");
            audioSource.PlayOneShot(reloadAmmoLeft);
        }
        else
        {
            animator.Play("Reload Out Of Ammo");
            audioSource.PlayOneShot(reloadEmpty);
        }

        //totalAmmo += CurrentAmmo;
        CurrentAmmo = 0;

        if (updateAmmoCallback != null)
        {
            updateAmmoCallback.Invoke();
        }

        //IsReloading = false;
    }

    public void SetAmmoToMax()
    {
        if (totalAmmo >= magAmmo)
        {
            CurrentAmmo = magAmmo;
        }
        else
        {
            CurrentAmmo = totalAmmo;
        }

        if (updateAmmoCallback != null)
        {
            updateAmmoCallback.Invoke();
        }
    }

    public void SetIsReloading(int toggle)
    {
        IsReloading = Convert.ToBoolean(toggle);
    }

    public void InstantiateUsedMagazine()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.y -= 0.25f;
        var go = Instantiate(magazinePrefab, spawnPos, Quaternion.identity);
        Vector3 ejectionVector = transform.TransformDirection(Vector3.left) * 0.25f;
        var ejectionRotation = Quaternion.Euler(new Vector3(0, 0, 40f));
        go.GetComponent<Rigidbody>().AddForce(ejectionVector, ForceMode.Impulse);
        go.transform.rotation = ejectionRotation;
    }

    public void AddMagazine()
    {
        totalAmmo += magAmmo;

        if (updateAmmoCallback != null)
        {
            updateAmmoCallback.Invoke();
        }
    }

    public IEnumerator SpawnBulletCasing()
    {
        var go = Instantiate(casingPrefab, casingEjectLocation.position, casingPrefab.transform.rotation);
        Vector3 ejectionVector = transform.TransformDirection(Vector3.right) * 0.3f;

        go.GetComponent<Rigidbody>().AddForce(ejectionVector, ForceMode.Impulse);
        yield return new WaitForSeconds(0.5f);

        if (bulletCasings != null)
        {
            var clipToPlay = UnityEngine.Random.Range(0, bulletCasings.Length);
            audioSource.PlayOneShot(bulletCasings[clipToPlay]);
        }

        yield return new WaitForSeconds(10f);
        Destroy(go);
    }

    public void PlayTakeoutSound()
    {
        audioSource.PlayOneShot(takeOut);
    }
}

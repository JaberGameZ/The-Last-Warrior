using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Gun : MonoBehaviour
{
    //Weapon Properties
    [Header("Weapon Properties")]
    [SerializeField] float FireRate;
    [SerializeField] float AimSpeed;
    [SerializeField] float RotationSpeed;
    [SerializeField] float BulletRange;
    [SerializeField] float NextFireTime = 0f;
    [SerializeField] float WeaponDamage;
    [SerializeField] float RecoilAmount;
    [SerializeField] float GunMovementSpeed;
    [SerializeField] float TimeToReload = 5f;
    //[SerializeField] float CurrentTime = 0f;
    public float WeaponCost;
    public bool WeaponPurchased;

    //Ammo Variables
    [Header("Ammo")]
    public float AmmoInThePack;
    public float ExtraAmmoAmount;
    [SerializeField] float MaxInAmmoInClip;
    public float CurrentAmmo;

    //Vectors
    [Header("Vectors")]
    [SerializeField] Vector3 AimPosition;
    [SerializeField] Vector3 OriginalPos;
    [SerializeField] Vector3 RecoilDirection;
    [SerializeField] Vector3 MovingDirection;

    //Sound Effects
    [Header("Sound Effects")]
    [SerializeField] AudioSource GunShot;


    [Header("Weapon Components")]
    [SerializeField] GameObject Bullet;
    [SerializeField] Player Player;
    [SerializeField] Transform BulletOrigin;
    [SerializeField] TextMeshProUGUI CurrentAmmoUI;
    [SerializeField] TextMeshProUGUI MaxAmmoUI;
    [SerializeField] ParticleSystem ShotFx;
    public GameObject Panel;

    void Start()
    {
        CurrentAmmo = MaxInAmmoInClip;
        Player = GameObject.FindAnyObjectByType<Player>();


    }

    // Update is called once per frame
    void Update()
    {
        Reload();
        UI();

        if (CurrentAmmo == 0)
        {
            StartCoroutine("Reload");
        }
    }


    public void Shooting()
    {
        if (Time.time >= NextFireTime && CurrentAmmo > 0)
        {
            GameObject Projectile = Instantiate(Bullet, BulletOrigin.position, Quaternion.identity);
            Projectile.GetComponent<Rigidbody>().velocity = BulletOrigin.transform.forward * BulletRange;
            Projectile.transform.localRotation = BulletOrigin.rotation;

            Destroy(Projectile, 6f);
            NextFireTime = Time.time + FireRate;
            Recoil();
            CurrentAmmo--;
            ShotFx.Play();
            GunShot.Play();


        }
    }

    void UI()
    {
        //CurrentAmmoUI.text = CurrentAmmo.ToString();
        //MaxAmmoUI.text = AmmoInThePack.ToString();
    }

   

    void Recoil()
    {
        //transform.position -= RecoilDirection * Time.deltaTime * RecoilAmount;
        float interpolationFactor = Time.deltaTime * RecoilAmount;

        if (Input.GetButton("Fire1"))
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, RecoilDirection, interpolationFactor);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, OriginalPos, interpolationFactor);
        }

    }

    IEnumerator Reload()
    {
            yield return new WaitForSeconds(TimeToReload);
        float AmmoNeededToReload = MaxInAmmoInClip - CurrentAmmo;

        if (AmmoInThePack >= AmmoNeededToReload)
        {
            AmmoInThePack -= AmmoNeededToReload;
            CurrentAmmo += AmmoNeededToReload;
        }

        else
        {
            CurrentAmmo += AmmoInThePack;
            AmmoInThePack = 0;

        }

    }



    void Aim()
    {
        float interpolationFactor = Time.deltaTime * AimSpeed;

        if (Input.GetKey(KeyCode.Mouse1))
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, AimPosition, interpolationFactor);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, OriginalPos, interpolationFactor);
        }
    }
}

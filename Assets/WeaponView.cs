using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponView : MonoBehaviour
{
    public int currentMagazin;
    public int magazinSize;
    public int totalRounds;
    public int roundPerMagazin;
    public float maxDistance;
    public float spread;
    public Vector2Int bulletPerShoot;
    public float rpm;
    public ParticleSystem particlesFire;
    public GameObject trail;
    public float reloadTime = 0.5f;

    public GameObject impactRock;
    public GameObject impactMetal;
    public GameObject impactBlood;

    public bool reloading = false;

    private int mask;

    private AudioSource audioSource;

    public AudioClip fireClip;
    public AudioClip reloadClip;
    public AudioClip getOutWeapon;

    private float lastShot = 0;
    private float nextReload = 0;
    public int damagePerBullet = 15;

    public float fireVolume = 0.2f;

    public bool CanReload
    {
        get
        {
            return (currentMagazin != magazinSize && totalRounds > 0);
        }
    }

    public void AddAmmo(int i)
    {
        totalRounds += i;
        GUI_Controller.Instance.ammo.UpdateView(currentMagazin, totalRounds);
    }

    private void OnEnable()
    {
        audioSource.volume = fireVolume;
        audioSource.pitch = Random.Range(0.85f, 1.15f);
        audioSource.PlayOneShot(getOutWeapon);
    }

    private void Awake()
    {
        mask = LayerMask.GetMask("Default" , "Floor");
        audioSource = GetComponent<AudioSource>();
        GUI_Controller.Instance.ammo.UpdateView(currentMagazin, totalRounds);
    }

    private void Update()
    {
        if (reloading && nextReload < Time.time)
        {
            reloading = false;
            GUI_Controller.Instance.ammo.UpdateView(currentMagazin, totalRounds);
        }
    }

    public void Fire(Vector3 origin, Vector3 forward, Vector3 right, Vector3 up, bool isPlayer)
    {
        if(Time.time > lastShot && currentMagazin > 0 && !reloading)
        {
            currentMagazin--;
            lastShot = Time.time + (1f / (rpm / 60f));
            audioSource.volume = fireVolume;
            audioSource.pitch = Random.Range(0.85f, 1.15f);
            audioSource.PlayOneShot(fireClip);
            particlesFire.Play();

            if(isPlayer)
                GUI_Controller.Instance.ammo.UpdateView(currentMagazin, totalRounds);

            int bc = Random.Range(bulletPerShoot.x, bulletPerShoot.y);
            for (int i = 0; i < bc; i++)
            {
                Vector2 c = Random.insideUnitCircle;
                Vector3 f = (forward + ((right * c.x * spread) + (up * c.y * spread))).normalized;
                RaycastHit hit;
                bool b = Physics.Raycast(origin, f, out hit, maxDistance, mask);

                if (b)
                {
                    GameObject g = Instantiate(trail, hit.point, Quaternion.LookRotation(f));
                    g.GetComponent<Trail>().Init(hit.distance);

                    if(hit.collider!= null)
                    {
                        Lock l = hit.collider.GetComponent<Lock>();
                        if(l != null)
                        {
                            l.Shoot();
                            Instantiate(impactMetal, hit.point, Quaternion.LookRotation(hit.normal));
                        }
                        else
                        {
                            EnnemyMotor ennemy = hit.collider.GetComponent<EnnemyMotor>();
                            PlayerMotor pm = hit.collider.GetComponent<PlayerMotor>();

                            if (ennemy != null || pm != null)
                            {
                                if(pm != null)
                                    pm.SubHealth(damagePerBullet);
                                else
                                    ennemy.SubHealth(damagePerBullet);
                                Instantiate(impactBlood, hit.point, Quaternion.LookRotation(hit.normal));
                            }
                            else
                            {
                                Instantiate(impactRock, hit.point, Quaternion.LookRotation(hit.normal));
                            }
                        }
                    }
                    else
                    {
                        Instantiate(impactRock, hit.point, Quaternion.LookRotation(hit.normal));
                    }
                }
                else
                {
                    GameObject g = Instantiate(trail, origin + (maxDistance * f), Quaternion.LookRotation(f));
                    g.GetComponent<Trail>().Init(maxDistance);
                }
            }
        }
    }

    public void Reload()
    {
        if(currentMagazin != magazinSize && totalRounds > 0 && !reloading)
        {
            currentMagazin++;
            totalRounds--;
            nextReload = Time.time + reloadTime;
            audioSource.volume = 0.2f;
            audioSource.pitch = Random.Range(0.85f, 1.15f);
            audioSource.PlayOneShot(reloadClip);
            reloading = true;
        }
    }
}

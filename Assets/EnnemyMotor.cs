using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyMotor : MonoBehaviour
{
    [SerializeField]
    private int currentHealth;
    [SerializeField]
    private int maxHealth;
    private EnnemyViewController ennemyViewController;
    private EnnemySoundController ennemySoundController;
    private GameObject player;
    public GameObject ammoDrop;
    public Transform muzzle;
    private Rigidbody rb;

    public Vector2 minDist = new Vector2 (8, 15);
    public float safeDistanceThreshold = 1;
    private float safeDistance;
    public float speed;

    public float lookLerpPower;
    private Vector3 lookRot;
    public WeaponView weapon;

    public float detectRange = 20;
    public bool inCombat = false;

    private bool canJump = true;
    private bool grounded = false;
    private float gravity;
    private int physicMask = 0;
    private float frameGravity;
    private bool raycast = false;
    private Vector3 moveDir;
    public Vector3 move;

    private float shootTimer = 0;
    public Vector2 shotDelayLimit = new Vector2(5, 15);
    private float shotDelay = 10;

    public float rangeShot;
    public float angleShot;
    private EnnemySpawner ennemySpawner;

    private void Awake()
    {
        ennemySoundController = GetComponent<EnnemySoundController>();
        ennemyViewController = GetComponent<EnnemyViewController>();
        player = GameController.Instance.player;
        safeDistance = Random.Range(minDist.x, minDist.y);
        rb = GetComponent<Rigidbody>();
        shotDelay = Random.Range(shotDelayLimit.x, shotDelayLimit.y);
        frameGravity = Physics.gravity.y * Time.fixedDeltaTime;
        physicMask = LayerMask.GetMask("Default", "Floor");
        gravity += frameGravity;
    }

    private void OnDisable()
    {
        inCombat = false;
        ennemyViewController.UpdateCombat(false);
        shootTimer = Time.time + shotDelay;
    }

    public void AddSpawner(EnnemySpawner es)
    {
        ennemySpawner = es;
    }

    public void Shot()
    {
        shootTimer = Time.time + shotDelay;
        weapon.Fire(muzzle.position, muzzle.forward, muzzle.right, muzzle.up,false);
    }

    public void SubHealth(int i)
    {
        currentHealth -= i;
        ennemySoundController.Hit();
        if (currentHealth <= 0)
        {
            enabled = false;
            ennemySpawner.Death(gameObject);
            ennemyViewController.Death(player);
            GameObject g = Instantiate(ammoDrop, transform.position, Quaternion.LookRotation(Random.insideUnitSphere * 360f));
            g.GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * 5, ForceMode.Impulse);
        }

        ennemyViewController.UpdateLife(currentHealth, maxHealth);
    }

    private void FixedUpdate()
    {
        if (rb)
        {
            if (gravity < 0)
                gravity += frameGravity * 1.5f;
            else if (gravity > 0)
                gravity += frameGravity;

            rb.velocity = new Vector3(0, gravity, 0);

            RaycastHit hit;
            raycast = Physics.Raycast(transform.position, Vector3.down, out hit, 0.1f, physicMask);

            if (raycast)
            {
                if ((!grounded || gravity < 0) && canJump)
                {
                    gravity = 0;
                    grounded = true;
                }
            }
            else
            {
                if (grounded)
                {
                    grounded = false;
                    gravity += frameGravity;
                }
            }

            gravity = Mathf.Clamp(gravity, -102, 102);
            rb.velocity = moveDir + new Vector3(0, gravity, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Vehicul")
        {
            Vector3 v = collision.rigidbody.velocity;
            SubHealth((int)(v.magnitude * 4));
        }
    }

    private void Update()
    {
        if(player != null)
        {
            Vector3 v = transform.position - player.transform.position;
            float dist = v.magnitude;

            if (inCombat)
            {
                move = v;
                v.y = 0;
                v.Normalize();

                lookRot = Vector3.Lerp(lookRot, -v, lookLerpPower);

                if (dist < safeDistance)
                {
                    moveDir = v * speed;
                }
                else if (dist > safeDistance + safeDistanceThreshold)
                {
                    move = -move;
                    moveDir = -v * speed;
                }
                else  
                {
                    move = Vector3.zero;
                    moveDir = Vector3.zero;
                }

                transform.rotation = Quaternion.LookRotation(lookRot);

                float f = Vector3.Dot(-v, transform.forward);


                if (f > (1 - angleShot))
                {
                    if (dist < rangeShot)
                    {
                        if (Time.time > shootTimer)
                        {
                            Shot();
                        }
                    }
                }
            }
            else
            {
                if (dist < detectRange)
                {
                    inCombat = true;
                    ennemyViewController.UpdateCombat(true);
                }
            }
        }
    }
}

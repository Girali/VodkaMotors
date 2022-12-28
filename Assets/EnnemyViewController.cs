using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyViewController : MonoBehaviour
{
    public SkinnedMeshRenderer render;
    public Animator animator;
    public DUI_Health health;
    public Material[] materials;

    private EnnemyMotor ennemyMotor;
    private Vector3 walkDir;

    public float lerpWalkPower;
    public float lerpRotationPower; 
    
    public float walkPower;
    public bool autoRotation = true;
    public GameObject[] ragdoll;
    public Rigidbody torso;

    public void Death(GameObject player)
    {
        Ragdoll(true);
        Destroy(gameObject, 5);
        StartCoroutine(CRT_AddForceDeath(player));
    }

    IEnumerator CRT_AddForceDeath(GameObject player)
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        torso.AddForce((transform.position - player.transform.position).normalized * 200f, ForceMode.Impulse);
    }

    public void Ragdoll(bool b)
    {
        animator.enabled = !b;
        GetComponent<Collider>().enabled = !b;
        if(b)
            Destroy(GetComponent<Rigidbody>());

        foreach (GameObject g in ragdoll)
        {
            Collider c = g.GetComponent<Collider>();
            Rigidbody r = g.GetComponent<Rigidbody>();
            c.enabled = b;
            r.isKinematic = !b;
        }
    }

    private void Awake()
    {
        Ragdoll(false);
        ennemyMotor = GetComponent<EnnemyMotor>();
        render.material = materials[Random.Range(0, materials.Length)];
        walkDir = Vector3.zero;
    }

    public void UpdateCombat(bool b)
    {
        animator.SetBool("Combat", b);
    }

    public void Fire()
    {
        animator.SetTrigger("Fire");
    }

    public void UpdateLife(int current, float total)
    {
        health.Fill(current / (float)total);
    }


    private void Update()
    {
        Vector3 v = ennemyMotor.move;
        v.y = 0;
        v.Normalize();

        Vector3 f = transform.InverseTransformDirection(v);

        walkDir = Vector3.Lerp(walkDir, v, lerpWalkPower);
        animator.SetFloat("X", f.x);
        animator.SetFloat("Y", f.z);
    }
}

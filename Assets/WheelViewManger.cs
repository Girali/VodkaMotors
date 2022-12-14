using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelViewManger : MonoBehaviour
{
    [SerializeField] 
    private GameObject render;
    private WheelCollider wheel;

    public Vector3 v;
    public Quaternion q;

    private void Start()
    {
        wheel= GetComponent<WheelCollider>();
    }

    private void Update()
    {
        wheel.GetWorldPose(out v, out q);

        render.transform.position = v;
        render.transform.rotation = q;
    }
}

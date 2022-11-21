using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelViewManger : MonoBehaviour
{
    [SerializeField] 
    private GameObject render;
    private WheelCollider wheel;
    [SerializeField] 
    private AxeViewManager axe;
    private Vector3 initPosition;
    private Quaternion initRotation;

    public Vector3 v;
    public Quaternion q;
    public Vector3 locaPosition;

    private void Start()
    {
        wheel= GetComponent<WheelCollider>();
        initPosition = Vector3.zero;//render.transform.localPosition;
        initRotation = Quaternion.identity;// render.transform.localRotation;
    }

    private void Update()
    {
        wheel.GetWorldPose(out v, out q);

        render.transform.position = v + initPosition;
        render.transform.rotation = q * initRotation;
    }

    private void OnDisable()
    {
        locaPosition = axe.vehicul.InverseTransformDirection(v - axe.transform.position);
    }
}

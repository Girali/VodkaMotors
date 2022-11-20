using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiculMotor : Motor
{
    [SerializeField]
    private GameObject[] frontWheels;
    [SerializeField]
    private GameObject[] rearWheels;

    [SerializeField]
    private GameObject[] stearAxes;
    private float[] stearAxesAngles;

    [SerializeField]
    private float maxStearAngle = 10;
    private float currentStearAngle = 0;
    [SerializeField, Range(0f, 0.1f)]
    private float lerpStearSpeed = 0;
    [SerializeField]
    private float turnForce = 5;
    [SerializeField]
    private float turnTorque = 5;

    [SerializeField]
    private Transform forceAxe;

    [SerializeField]
    private float maxSpeedTarget = 5;
    private float currentSpeed = 0;
    [SerializeField, Range(0f,0.01f)]
    private float lerpSpeedSpeed = 0;
    [SerializeField]
    private float acceleration = 5;

    protected override void Start()
    {
        base.Start();
        stearAxesAngles = new float[2];
        stearAxesAngles[0] = stearAxes[0].transform.localRotation.eulerAngles.y;
        stearAxesAngles[1] = stearAxes[1].transform.localRotation.eulerAngles.y;
    }

    public override void Move(bool forward, bool backward, bool left, bool right, bool jump, bool sprint, float yaw, float pitch)
    {
        base.Move(forward, backward, left, right, jump, sprint, yaw, pitch);

        float t2 = 0;
        if (forward ^ backward)
            t2 = forward ? maxSpeedTarget : -maxSpeedTarget;

        currentSpeed = Mathf.Lerp(currentSpeed, t2, lerpSpeedSpeed);

        float t1 = 0;
        if (left ^ right)
            t1 = left ? maxStearAngle : -maxStearAngle;

        currentStearAngle = Mathf.Lerp(currentStearAngle, t1, lerpStearSpeed);

        stearAxes[0].transform.localRotation = Quaternion.Euler(0, currentStearAngle + stearAxesAngles[0], 0);
        stearAxes[1].transform.localRotation = Quaternion.Euler(0, currentStearAngle + stearAxesAngles[1], 0);

        frontWheels[0].transform.Rotate(Vector3.forward * currentSpeed);
        frontWheels[1].transform.Rotate(-Vector3.forward * currentSpeed);

        rearWheels[0].transform.Rotate(Vector3.forward * currentSpeed);
        rearWheels[1].transform.Rotate(-Vector3.forward * currentSpeed);

        //Vector3 moveDir = Vector3.forward * currentSpeed;
        //moveDir.y = rb.velocity.y;
        //rb.velocity = moveDir;
        //rb.AddForce(transform.forward * currentSpeed * acceleration, ForceMode.Impulse);
        //rb.AddTorque(transform.up * currentSpeed * currentStearAngle * turnTorque, ForceMode.Impulse);
        //rb.AddForceAtPosition(forceAxe.right * currentSpeed * currentStearAngle * turnForce, forceAxe.position, ForceMode.Impulse);
    }
}

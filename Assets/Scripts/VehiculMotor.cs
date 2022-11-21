using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiculMotor : Motor
{
    [SerializeField]
    private WheelCollider[] frontWheels;
    [SerializeField]
    private WheelCollider[] rearWheels;

    private WheelFrictionCurve[] rearWheelFrictionCurves;

    [SerializeField]
    private float maxStearAngle = 10;
    private float currentStearAngle = 0;
    [SerializeField, Range(0f, 0.1f)]
    private float lerpStearSpeed = 0;

    [SerializeField]
    private float maxSpeedTarget = 5;
    private float currentSpeed = 0;
    [SerializeField, Range(0f,0.1f)]
    private float lerpSpeedSpeed = 0;

    [SerializeField]
    private float driftForce = 0.1f;

    [SerializeField]
    private float breakForce = 100;
    [SerializeField]
    private float motorBreak = 100;
    private bool inUse = false;

    public void EnterVehicul()
    {
        inUse = true;
    }

    public void ExitVehicul()
    {
        inUse = false;
    }

    protected override void Start()
    {
        base.Start();
        rearWheelFrictionCurves = new WheelFrictionCurve[2];
        rearWheelFrictionCurves[0] = rearWheels[0].sidewaysFriction;
        rearWheelFrictionCurves[1] = rearWheels[1].sidewaysFriction;
    }

    public void Update()
    {
        if (!inUse)
        {
            frontWheels[0].brakeTorque = breakForce;
            frontWheels[1].brakeTorque = breakForce;
            rearWheels[0].brakeTorque = breakForce;
            rearWheels[1].brakeTorque = breakForce;
        }
        else
        {
            frontWheels[0].brakeTorque = motorBreak;
            frontWheels[1].brakeTorque = motorBreak;
            rearWheels[0].brakeTorque = motorBreak;
            rearWheels[1].brakeTorque = motorBreak;
        }
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
            t1 = left ? -maxStearAngle : maxStearAngle;

        currentStearAngle = Mathf.Lerp(currentStearAngle, t1, lerpStearSpeed);

        if (jump)
        {
            rearWheelFrictionCurves[0].stiffness = driftForce;
            rearWheelFrictionCurves[1].stiffness = driftForce;
        }
        else
        {
            rearWheelFrictionCurves[0].stiffness = 1;
            rearWheelFrictionCurves[1].stiffness = 1;
        } 

        rearWheels[0].sidewaysFriction = rearWheelFrictionCurves[0];
        rearWheels[1].sidewaysFriction = rearWheelFrictionCurves[1];

        frontWheels[0].motorTorque = currentSpeed;
        frontWheels[1].motorTorque = currentSpeed;

        rearWheels[0].motorTorque = currentSpeed;
        rearWheels[1].motorTorque = currentSpeed;

        frontWheels[0].steerAngle = currentStearAngle;
        frontWheels[1].steerAngle = currentStearAngle;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiculMotor : Motor
{
    public WheelCollider[] frontWheels;
    public WheelCollider[] rearWheels;

    [SerializeField]
    private MusicController musicController;
    private VehiculPartController vehiculPartController;

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
    [SerializeField]
    private float thresholdAutoCorrect = 20;
    [SerializeField]
    private float powerAutoCorrect = 20;

    [SerializeField]
    private float powerRedirect = 1;
    [SerializeField]
    private float powerSubVelocity = 1;


    public void EnterVehicul()
    {
        inUse = true;
        frontWheels[0].brakeTorque = 0;
        frontWheels[1].brakeTorque = 0;
        rearWheels[0].brakeTorque = 0;
        rearWheels[1].brakeTorque = 0;
    }

    public void ExitVehicul()
    {
        inUse = false;
        frontWheels[0].brakeTorque = breakForce;
        frontWheels[1].brakeTorque = breakForce;
        rearWheels[0].brakeTorque = breakForce;
        rearWheels[1].brakeTorque = breakForce;
    }

    public void Init()
    {
        vehiculPartController = GetComponent<VehiculPartController>();
        rearWheelFrictionCurves = new WheelFrictionCurve[2];
        rearWheelFrictionCurves[0] = rearWheels[0].sidewaysFriction;
        rearWheelFrictionCurves[1] = rearWheels[1].sidewaysFriction;

        frontWheels[0].brakeTorque = breakForce;
        frontWheels[1].brakeTorque = breakForce;
        rearWheels[0].brakeTorque = breakForce;
        rearWheels[1].brakeTorque = breakForce;
    }

    public void CheckDrift()
    {
        Vector3 v1 = rb.velocity;
        Vector3 v2 = transform.forward;

        v1.y = 0;
        v2.y = 0;

        v1.Normalize();
        v2.Normalize();

        float d = Vector3.Dot(v1.normalized, v2.normalized);

        if(d < 0.8f)
        {
            rb.AddForce(-v1 * powerSubVelocity, ForceMode.Acceleration);
            rb.AddForce(v2 * powerRedirect, ForceMode.Acceleration);
        }
    }

    public float Speed
    {
        get
        {
            return rb.velocity.magnitude;
        }
    }

    public void Update()
    {
        if (!inUse)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0, lerpSpeedSpeed);
            currentStearAngle = Mathf.Lerp(currentStearAngle, 0, lerpStearSpeed);

            frontWheels[0].motorTorque = currentSpeed;
            frontWheels[1].motorTorque = currentSpeed;

            rearWheels[0].motorTorque = currentSpeed;
            rearWheels[1].motorTorque = currentSpeed;

            frontWheels[0].steerAngle = currentStearAngle;
            frontWheels[1].steerAngle = currentStearAngle;
        }


        float upTilt = transform.eulerAngles.z;
        float frontTilt = transform.eulerAngles.x;

        if (frontTilt > 180)
            frontTilt -= 360;

        if (Mathf.Abs(frontTilt) > thresholdAutoCorrect)
        {
            rb.AddRelativeTorque(Vector3.right * frontTilt * powerAutoCorrect, ForceMode.Force);
        }

        if (upTilt > 180)
            upTilt -= 360;

        if(Mathf.Abs(upTilt) > thresholdAutoCorrect)
        {
            rb.AddRelativeTorque(Vector3.forward * upTilt * powerAutoCorrect, ForceMode.Force);
        }
    }



    public override void Move(bool forward, bool backward, bool left, bool right, bool jump, bool sprint, float yaw, float pitch, RaycastHit hit, bool interact)
    {
        base.Move(forward, backward, left, right, jump, sprint, yaw, pitch, hit, interact);

        float t2 = 0;
        if (forward ^ backward)
            t2 = forward ? maxSpeedTarget : -maxSpeedTarget;

        currentSpeed = Mathf.Lerp(currentSpeed, t2, lerpSpeedSpeed);
        
        float t1 = 0;
        if (left ^ right)
            t1 = left ? -maxStearAngle : maxStearAngle;

        vehiculPartController.UpdateView(((currentStearAngle / maxStearAngle) + 1f) / 2f);

        currentStearAngle = Mathf.Lerp(currentStearAngle, t1, lerpStearSpeed);

        if(forward)
            CheckDrift();

        if (shiftDown)
            musicController.StartNewMusic();

        if (!(forward || backward || left || right))
        {

            if (jump)
            {
                frontWheels[0].brakeTorque = breakForce;
                frontWheels[1].brakeTorque = breakForce;
                rearWheels[0].brakeTorque = breakForce;
                rearWheels[1].brakeTorque = breakForce;
                rearWheelFrictionCurves[0].stiffness = 1;
                rearWheelFrictionCurves[1].stiffness = 1;
            }
            else
            {
                frontWheels[0].brakeTorque = motorBreak;
                frontWheels[1].brakeTorque = motorBreak;
                rearWheels[0].brakeTorque = motorBreak;
                rearWheels[1].brakeTorque = motorBreak;
                rearWheelFrictionCurves[0].stiffness = 1;
                rearWheelFrictionCurves[1].stiffness = 1;
            }
        }
        else
        {
            frontWheels[0].brakeTorque = 0;
            frontWheels[1].brakeTorque = 0;
            rearWheels[0].brakeTorque = 0;
            rearWheels[1].brakeTorque = 0;

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

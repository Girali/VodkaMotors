using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootController : MonoBehaviour
{
    private Motor currentMotorUsed;
    private PlayerMotor playerMotor;
    public  float sensitivity;
    private bool forward;
    private bool backward;
    private bool left;
    private bool right;
    private bool jump;
    private bool sprint;
    private float yaw;
    private float pitch;
    private float xMouse;
    private float yMouse;

    private void Start()
    {
        Time.fixedDeltaTime = 1f / 60f;
        Application.targetFrameRate = 60;

        playerMotor = GetComponent<PlayerMotor>();
        currentMotorUsed = playerMotor;
    }

    private float AngleModulo(float value)
    {
        if (value < 0)
            return value + 360f;
        return value % 360f;
    }

    private void PoolKeys()
    {
        xMouse = Input.GetAxisRaw("Mouse X");
        yMouse = Input.GetAxisRaw("Mouse Y");

        forward = Input.GetKey(KeyCode.W);
        backward = Input.GetKey(KeyCode.S);
        left = Input.GetKey(KeyCode.A);
        right = Input.GetKey(KeyCode.D);
        jump = Input.GetKey(KeyCode.Space);
        sprint = Input.GetKey(KeyCode.LeftShift);

        yaw += xMouse * sensitivity;
        yaw = AngleModulo(yaw);
        pitch += -yMouse * sensitivity;
    }

    private void Update()
    {
        PoolKeys();
        currentMotorUsed.Move(forward, backward, left, right, jump, sprint, yaw, pitch);
    }
}

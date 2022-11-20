using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : Motor
{
    public Camera cam;
    public float jumpForce = 5f;
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    private Vector3 movingDir;
    private bool canJump = true;
    private bool grounded = false;
    private bool jumpPressed = false;
    private float gravity;
    private int physicMask = 0;
    private float frameGravity;
    private bool raycast = false;
    private bool spherecast = false;
    private Vector3 camInitialPos;
    private bool usePhysicsInterpolate = true;
    private Vector3 lastCamPos;
    private Vector3 lastVelocity;
    private bool fixedWasCalled = false;

    protected override void Start()
    {
        base.Start();
        frameGravity = Physics.gravity.y * Time.fixedDeltaTime;
        physicMask = LayerMask.GetMask("Default");
    }

    public void NextFramePos(Vector3 v)
    {
        if (v.magnitude < 1f)
        {
            cam.transform.position += v;
        }
    }

    public void ResetFramePos()
    {
        cam.transform.localPosition = camInitialPos;
    }

    private void Update()
    {
        if (usePhysicsInterpolate)
        {
            if (fixedWasCalled)
            {
                fixedWasCalled = false;
            }
            else
            {
                NextFramePos(lastVelocity * 0.5f);
            }
        }
    }

    private void FixedUpdate()
    {
        if (usePhysicsInterpolate)
        {
            lastVelocity = transform.position - lastCamPos;
            lastCamPos = transform.position;
            if (!fixedWasCalled)
            {
                ResetFramePos();
                fixedWasCalled = true;
            }
        }

        if (gravity < 0)
            gravity += frameGravity * 1.5f;
        else if (gravity > 0)
            gravity += frameGravity;

        rb.velocity = new Vector3(0, gravity, 0);

        RaycastHit hit;
        raycast = Physics.Raycast(transform.position, Vector3.down, out hit, 2f, physicMask);

        Collider[] colliders = Physics.OverlapSphere(transform.position + new Vector3(0, -0.56f, 0), 0.48f, physicMask);
        spherecast = colliders.Length > 1;

        if (spherecast && raycast)
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
        rb.velocity = movingDir + new Vector3(0, gravity, 0);
    }

    public void Jump()
    {
        canJump = false;
        jumpPressed = true;
        StartCoroutine(StartJump());

        if (gravity < 0)
            gravity *= 0.5f;

        gravity = 0;

        grounded = false;

        gravity += jumpForce;
    }

    IEnumerator StartJump()
    {
        yield return new WaitForSeconds(0.1f);
        canJump = true;
    }

    public override void Move(bool forward, bool backward, bool left, bool right, bool jump, bool sprint, float yaw, float pitch)
    {
        base.Move(forward, backward, left, right, jump, sprint, yaw, pitch);
        //gravity
        movingDir = Vector3.zero;

        if (forward ^ backward)
        {
            movingDir += forward ? transform.forward : -transform.forward;
        }

        if (left ^ right)
        {
            movingDir += right ? transform.right : -transform.right;
        }

        movingDir.Normalize();

        float speed = 0;

        if (sprint)
            speed = sprintSpeed;
        else
            speed = walkSpeed;

        movingDir *= speed;

        //jumping
        if (jump && jumpForce > 0)
        {
            if (jumpPressed == false && canJump)
            {
                if(grounded)
                    Jump();
            }
        }
        else
        {
            if (jumpPressed)
                jumpPressed = false;
        }

        cam.transform.localEulerAngles = new Vector3(pitch, 0f, 0f);
        transform.rotation = Quaternion.Euler(0, yaw, 0);
    }
}

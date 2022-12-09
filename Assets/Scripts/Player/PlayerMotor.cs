using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerMotor : Motor
{
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private GameObject render;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float walkSpeed = 5f;
    [SerializeField]
    private float sprintSpeed = 10f;
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

    [SerializeField]
    private PlayerAnimation playerAnimation;

    [SerializeField]
    private RotationConstraint cameraRotationConstraint;

    [SerializeField]
    private PlayerFootController playerController;

    public bool inVehicul = false; 

    protected override void Start()
    {
        base.Start();
        frameGravity = Physics.gravity.y * Time.fixedDeltaTime;
        physicMask = LayerMask.GetMask("Default");
        gravity += frameGravity;
        camInitialPos = render.transform.localPosition;
    }

    public void EnterVehicul(Transform t)
    {
        Destroy(rb);
        Destroy(cldr);
        transform.parent = t;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        playerAnimation.Driving(true);
        inVehicul = true;
        cameraRotationConstraint.rotationAxis = Axis.Z;
    }

    public void ExitVehicul()
    {
        rb = gameObject.AddComponent<Rigidbody>();

        rb.mass = 60;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        CapsuleCollider c = gameObject.AddComponent<CapsuleCollider>();
        c.height = 2;
        c.radius = 0.4f;
        cldr = c;
        transform.parent = null;

        playerAnimation.Driving(false);
        inVehicul = false;
        cameraRotationConstraint.rotationAxis = Axis.Z | Axis.Y;
    }

    public void NextFramePos(Vector3 v)
    {
        if (v.magnitude < 1f)
        {
            render.transform.position += v;
        }
    }

    public void ResetFramePos()
    {
        render.transform.localPosition = camInitialPos;
    }

    private void Update()
    {
        if (!inVehicul)
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
    }

    private void FixedUpdate()
    {
        if (!inVehicul)
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
                    playerAnimation.Fall(false);
                }
            }
            else
            {
                if (grounded)
                {
                    grounded = false;
                    playerAnimation.Fall(true);
                    gravity += frameGravity;
                }
            }

            gravity = Mathf.Clamp(gravity, -102, 102);
            rb.velocity = movingDir + new Vector3(0, gravity, 0);
        }
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
        playerAnimation.Fall(true);

        gravity += jumpForce;
    }

    IEnumerator StartJump()
    {
        yield return new WaitForSeconds(0.1f);
        canJump = true;
    }

    public override void Move(bool forward, bool backward, bool left, bool right, bool jump, bool sprint, float yaw, float pitch, RaycastHit hit, bool interact)
    {
        base.Move(forward, backward, left, right, jump, sprint, yaw, pitch,hit, interact);

        if (!inVehicul)
        {
            //gravity
            movingDir = Vector3.zero;

            Vector2 v = Vector2.zero;

            if (forward ^ backward)
            {
                movingDir += forward ? transform.forward : -transform.forward;
                v = new Vector2(v.x, forward ? 1 : -1);
            }

            if (left ^ right)
            {
                movingDir += right ? transform.right : -transform.right;
                v = new Vector2(right ? 1 : -1, v.y);
            }

            playerAnimation.Run(v);

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
                    if (grounded)
                        Jump();
                }
            }
            else
            {
                if (jumpPressed)
                    jumpPressed = false;
            }

            transform.rotation = Quaternion.Euler(0, yaw, 0);
            cam.transform.localEulerAngles = new Vector3(pitch, 0, 0f);

            if (interact)
            {
                if (hit.collider)
                {
                    VehiculSit vs = hit.collider.GetComponent<VehiculSit>();
                    EnterVehicul(vs.sitPosition);
                    playerController.SetNewMotor(vs.motor);
                }
            }
        }
        else
        {
            cam.transform.localEulerAngles = new Vector3(pitch, yaw, 0f);

            if (interact)
            {
                ExitVehicul();
                playerController.SetNewMotor(null);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootController : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    private VehiculMotor vehiculMotor;
    private PlayerMotor playerMotor;
    private PlayerInteractController playerInteractController;
    private PlayerItemController playerItemController;
    public float sensitivity;
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
    private bool interact;
    private bool reload;
    private bool mouse1Down;
    private bool mouse2Down;
    private bool mouse1;
    private bool mouse2;
    private bool mouse1Up;
    private bool mouse2Up;
    private bool vodka;
    private int interactLayer;
    private float yawStart;
    private bool inUse = false;

    public void SetNewMotor(VehiculMotor m)
    {
        if (m == null)
        {
            vehiculMotor.ExitVehicul();
        }
        else
        {
            m.EnterVehicul();
        }

        vehiculMotor = m;
    }

    public void StopUse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        inUse = false;
    }

    public void StartUse()
    {
        yaw = yawStart;
        pitch = 0;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inUse = true;
    }

    private void Start()
    {
        Time.fixedDeltaTime = 1f / 60f;
        Application.targetFrameRate = 60;

        yawStart = transform.eulerAngles.y;

        interactLayer = LayerMask.GetMask("Interactable");
        playerInteractController = GetComponent<PlayerInteractController>();
        playerMotor = GetComponent<PlayerMotor>();
        playerItemController = GetComponent<PlayerItemController>();
    }

    private float AngleModulo(float value)
    {
        if (value < 0)
            return value + 360f;
        return value % 360f;
    }

    private void PoolKeys()
    {
        if (inUse)
        {
            xMouse = Input.GetAxisRaw("Mouse X");
            yMouse = Input.GetAxisRaw("Mouse Y");

            forward = Input.GetKey(AppController.Instance.forward);
            backward = Input.GetKey(AppController.Instance.backward);
            left = Input.GetKey(AppController.Instance.left);
            right = Input.GetKey(AppController.Instance.right);
            vodka = Input.GetKeyDown(KeyCode.V);
            reload = Input.GetKeyDown(KeyCode.R);
            interact = Input.GetKeyDown(AppController.Instance.interact);
            jump = Input.GetKey(AppController.Instance.jump);
            sprint = Input.GetKey(AppController.Instance.sprint);

            mouse1Down = Input.GetMouseButtonDown(0);
            mouse2Down = Input.GetMouseButtonDown(1);
            mouse1 = Input.GetMouseButton(0);
            mouse2 = Input.GetMouseButton(1);
            mouse1Up = Input.GetMouseButtonUp(0);
            mouse2Up = Input.GetMouseButtonUp(1);

            yaw += xMouse * sensitivity;
            yaw = AngleModulo(yaw);
            pitch += -yMouse * sensitivity;
        }
        else
        {
            xMouse = 0;
            yMouse = 0;

            forward = false;
            backward = false;
            left = false;
            right = false;
            vodka = false;
            interact = false;
            jump = false;
            sprint = false;

            mouse1Down = false;
            mouse2Down = false;
            mouse1 = false;
            mouse2 = false;
            mouse1Up = false;
            mouse2Up = false;
        }
    }

    private void Update()
    {
        PoolKeys();

        RaycastHit hit;
        Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, 4f, interactLayer);

        playerMotor.Move(forward, backward, left, right, jump, sprint, yaw, pitch, hit, interact);

        playerInteractController.Motor(mouse1Down, mouse1Up, hit);
        playerItemController.Motor(vodka, mouse1Down, reload);


        if (vehiculMotor != null)
            vehiculMotor.Move(forward, backward, left, right, jump, sprint, yaw, pitch, hit, interact);
    }
}

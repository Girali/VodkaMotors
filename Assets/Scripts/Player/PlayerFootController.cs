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
    private PlayerVodkaController playerVodkaController;
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
    private bool mouse1Down;
    private bool mouse2Down;
    private bool mouse1;
    private bool mouse2;
    private bool mouse1Up;
    private bool mouse2Up;
    private bool vodka;
    private int interactLayer;

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

    private void Start()
    {
        Time.fixedDeltaTime = 1f / 60f;
        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        interactLayer = LayerMask.GetMask("Interactable");
        playerInteractController = GetComponent<PlayerInteractController>();
        playerMotor = GetComponent<PlayerMotor>();
        playerVodkaController = GetComponent<PlayerVodkaController>();
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
        vodka = Input.GetKeyDown(KeyCode.V);
        interact = Input.GetKeyDown(KeyCode.E);
        jump = Input.GetKey(KeyCode.Space);
        sprint = Input.GetKey(KeyCode.LeftShift);

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

    private void Update()
    {
        PoolKeys();

        RaycastHit hit;
        Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, 4f, interactLayer);

        playerMotor.Move(forward, backward, left, right, jump, sprint, yaw, pitch, hit, interact);

        playerInteractController.Motor(mouse1Down, mouse1Up, hit);
        playerVodkaController.Motor(vodka);


        if (vehiculMotor != null)
            vehiculMotor.Move(forward, backward, left, right, jump, sprint, yaw, pitch, hit, interact);
    }
}

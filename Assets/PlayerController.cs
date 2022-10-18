using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController _instance = null;
    public static PlayerController Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<PlayerController>();
            return _instance;
        }
    }

    [SerializeField]
    private CharacterJoint hand;
    private FurniturePiece grabed;
    private Vector3 lastHandPosition;
    private Vector3 velocityAtFrame;

    [SerializeField]
    private Transform movePlan;
    [SerializeField]
    private Transform camPivot;
    [SerializeField]
    private Transform furnitureOrbit;
    [SerializeField]
    private Transform orbitTempX;
    [SerializeField]
    private Transform orbitTempY;
    [SerializeField]
    private Camera cam;
    private float speed = .5f;

    private int planLayer;

    private void Awake()
    {
        planLayer = LayerMask.GetMask("Ignore Raycast");
        orbitTempX.rotation = cam.transform.rotation;
        orbitTempY.rotation = cam.transform.rotation;
    }

    IEnumerator AddNextFramePhysFrameForce(Rigidbody rb)
    {
        yield return new WaitForFixedUpdate();
        rb.velocity = velocityAtFrame * 30;

        //Debug.DrawLine(lastHandPosition, lastHandPosition + (velocityAtFrame * 10), Color.red);
        //Debug.DrawLine(lastHandPosition, lastHandPosition + Vector3.up, Color.green);
    }

    public void Update()
    {
        Vector3 v = Input.mousePosition;
        bool usingHand = false;
        v.x /= Screen.width;
        v.y /= Screen.height;

        Ray r = cam.ViewportPointToRay(v);
        RaycastHit hit;

        if (grabed == null) 
        {
            if (Physics.Raycast(r, out hit, 20f))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.rigidbody)
                    {
                        if (hit.rigidbody.GetComponent<FurniturePiece>())
                        {
                            grabed = hit.rigidbody.GetComponent<FurniturePiece>();
                            hand.transform.position = hit.point;
                            movePlan.position = hit.point;
                            grabed.StartGrab();
                        }
                    }
                }

                if (Input.GetMouseButtonDown(1))
                {
                    if (hit.collider)
                    {
                        if (hit.collider.GetComponent<FurniturePiece>())
                        {
                            FurniturePiece f = hit.collider.GetComponent<FurniturePiece>();
                            f.DetachePiece(hit.point);
                        }
                    }
                }
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                usingHand = true;

                Physics.Raycast(r.origin, r.direction, out hit, 20f, planLayer);

                if(hand.connectedBody == null)
                    hand.connectedBody = grabed.GetComponent<Rigidbody>();

                lastHandPosition = hand.transform.position;
                velocityAtFrame = (hit.point - lastHandPosition);
                hand.transform.position = hit.point;

                float f = Input.GetAxis("Mouse ScrollWheel") * speed;
                movePlan.position += movePlan.forward * f;
            }

            if (Input.GetMouseButtonUp(0))
            {
                StartCoroutine(AddNextFramePhysFrameForce(hand.connectedBody));
                ReleaseCommand();
                usingHand = false;
            }
        }


        if (!usingHand)
        {
            float f = Input.GetAxis("Mouse ScrollWheel") * speed;
            cam.transform.position += cam.transform.forward * f;

            if (Input.GetMouseButton(1))
            {
                camPivot.Rotate(Vector3.up, -Input.GetAxis("Mouse X"), Space.World);
            }

            if (Input.GetMouseButton(2))
            {
                furnitureOrbit.Rotate(orbitTempX.up, -Input.GetAxis("Mouse X"), Space.World);
                furnitureOrbit.Rotate(orbitTempY.right, Input.GetAxis("Mouse Y"), Space.World);

                orbitTempX.Rotate(cam.transform.up, Input.GetAxis("Mouse X"), Space.World);
                orbitTempY.Rotate(cam.transform.right, -Input.GetAxis("Mouse Y"), Space.World);
            }
        }
    }

    public void ReleaseCommand()
    {
        if (grabed)
        {
            grabed.StopGrab();
            grabed = null;
            hand.connectedBody = null;
        }
    }
}

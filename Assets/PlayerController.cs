using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private ToolType currentTool = ToolType.Hand;

    [SerializeField]
    private CharacterJoint hand;
    private FurniturePiece grabed;

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
    private float speed = .2f;

    private int planLayer;

    private void Awake()
    {
        planLayer = LayerMask.GetMask("Ignore Raycast");
        orbitTempX.rotation = cam.transform.rotation;
        orbitTempY.rotation = cam.transform.rotation;
    }

    public void Update()
    {
        switch (currentTool)
        {
            case ToolType.Hand:
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

                        hand.transform.position = hit.point;

                        float f = Input.GetAxis("Mouse ScrollWheel") * speed;
                        movePlan.position += movePlan.forward * f;
                    }

                    if (Input.GetMouseButtonUp(0))
                    {
                        grabed = null;
                        hand.connectedBody = null;
                        usingHand = true;
                    }
                }


                if (usingHand)
                {
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
                break;
            case ToolType.Glue:
                break;
            case ToolType.Screwdriver:
                break;
            case ToolType.Hexkey:
                break;
            default:
                break;
        }


    }

    public enum ToolType
    {
        Hand,
        Glue,
        Screwdriver,
        Hexkey
    }
}

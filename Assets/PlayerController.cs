using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    [SerializeField]
    private Transform furnitureOrbit;
    [SerializeField]
    private Transform orbitTempX;
    [SerializeField]
    private Transform orbitTempY;
    [SerializeField]
    private Transform cam;
    [SerializeField]
    private float speed = 5f;

    private void Awake()
    {
        orbitTempX.rotation = cam.rotation;
        orbitTempY.rotation = cam.rotation;
    }

    public void Update()
    {
        if (Input.GetMouseButton(0))
        {
            furnitureOrbit.Rotate(orbitTempX.up, -Input.GetAxis("Mouse X"), Space.World);
            furnitureOrbit.Rotate(orbitTempY.right, Input.GetAxis("Mouse Y"), Space.World);

            orbitTempX.Rotate(cam.up, Input.GetAxis("Mouse X"), Space.World);
            orbitTempY.Rotate(cam.right, -Input.GetAxis("Mouse Y"), Space.World);
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

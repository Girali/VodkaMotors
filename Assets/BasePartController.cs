using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

public class BasePartController : MonoBehaviour
{
    public List<VehiculPartObject> partStored = new List<VehiculPartObject>();
    public VehiculPartController vehiculPartController;
    public int variantIndex = 0;
    public VehiculParts currentPart;

    public Camera cam;
    public Transform vehiculTarget;
    public Transform target;
    public Transform[] targets;

    public UnityEvent onPieceAdded;

    public bool IsCurrentPartEmpty()
    {
        return vehiculPartController.GetPart(currentPart, variantIndex) == null;
    }

    public void AddObject(VehiculPartObject ip)
    {
        if (ip != null)
        {
            if (ip.prefab != null)
            {
                partStored.Add(ip);
            }
        }
    }

    public void RemoveObject(VehiculPartObject ip)
    {
        if (ip != null) 
        { 
            if (ip.prefab != null)
            {
                partStored.Remove(ip);
            }
        }
    }

    public void NextIndex()
    {
        variantIndex++;

        if (currentPart == VehiculParts.Wheel)
            variantIndex = variantIndex % 4;
        else if (currentPart == VehiculParts.Door)
            variantIndex = variantIndex % 2;

        GotTo(currentPart);
    }

    public void PrevIndex() 
    {
        variantIndex--;

        if (currentPart == VehiculParts.Wheel)
            variantIndex = variantIndex % 4;
        else if (currentPart == VehiculParts.Door)
            variantIndex = variantIndex % 2;

        variantIndex = Mathf.Abs(variantIndex);

        GotTo(currentPart);
    }

    public void SetTarget(VehiculParts vp)
    {
        variantIndex = 0;
        GotTo(vp);
    }

    public void GotTo(VehiculParts vp)
    {
        currentPart = vp;
        switch (vp)
        {
            case VehiculParts.Engine:
                GUI_Controller.Instance.partsPanel.ShowVariant(false);
                target = targets[(int)vp - 1];
                break;
            case VehiculParts.Wheel:
                GUI_Controller.Instance.partsPanel.ShowVariant(true);
                target = targets[(int)vp - 1 + variantIndex];
                break;
            case VehiculParts.Seat:
            case VehiculParts.Roof:
            case VehiculParts.Armature:
            case VehiculParts.SteeringWheel:
            case VehiculParts.Back:
            case VehiculParts.Carrige:
            case VehiculParts.Spoiler:
            case VehiculParts.Sidebar:
            case VehiculParts.Bumper:
            case VehiculParts.Exhaust:
            case VehiculParts.Bonnet:
                GUI_Controller.Instance.partsPanel.ShowVariant(false);
                target = targets[(int)vp - 1 + 3];
                break;
            case VehiculParts.Door:
                GUI_Controller.Instance.partsPanel.ShowVariant(true);
                target = targets[(int)vp - 1 + 3 + variantIndex];
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if(target != null)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, target.position, 0.05f);
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, target.rotation, 0.05f);
        }
    }

    public void Enter()
    {
        AppController.Instance.blockPause = true;
        partStored = partStored.OrderBy((t) => t.part).ToList();
        cam.gameObject.SetActive(true);
        vehiculPartController.transform.position = vehiculTarget.position;
        vehiculPartController.transform.rotation = vehiculTarget.rotation;
        vehiculPartController.GetComponent<Rigidbody>().isKinematic = true;
        GUI_Controller.Instance.partsPanel.gameObject.SetActive(true);
        GUI_Controller.Instance.gui.gameObject.SetActive(false);
        FindObjectOfType<PlayerFootController>().StopUse();
    }

    public void Exit()
    {
        AppController.Instance.blockPause = false;
        cam.gameObject.SetActive(false);
        target = null;
        vehiculPartController.GetComponent<Rigidbody>().isKinematic = false;
        GUI_Controller.Instance.partsPanel.gameObject.SetActive(false);
        GUI_Controller.Instance.gui.gameObject.SetActive(true);
        FindObjectOfType<PlayerFootController>().StartUse();
    }

    public VehiculPartObject ReplacePiece(VehiculPartObject vpo)
    {
        return vehiculPartController.ReplacePiece(vpo, variantIndex);
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody)
        {
            InteractablePart interactablePart = other.attachedRigidbody.GetComponent<InteractablePart>();
            if (interactablePart != null && interactablePart.collectablePart && !interactablePart.added)
            {
                interactablePart.added = true;
                onPieceAdded.Invoke();
                partStored.Add(interactablePart.vehiculPartObject);
                GUI_Controller.Instance.AddNotify(interactablePart.vehiculPartObject);
                Destroy(interactablePart.gameObject);
            }
        }
    }
}

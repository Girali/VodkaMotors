using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerInteractController : MonoBehaviour
{
    [SerializeField]
    private Transform cam;

    [SerializeField]
    private CharacterJoint hand;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private LineRenderer lineRenderer;

    private float lerpTargetSpeed = 0.05f;

    private Interactable interactable;
    private InteractableObject interactableObject;

    [SerializeField]
    private float powerReleaseForce = -10;

    private Interactable lastInteractable;

    public void Motor(bool mouseLeftDown, bool mouseLeftUp, RaycastHit hit)
    {
        if (hit.collider)
        {
            Interactable i = hit.collider.GetComponent<Interactable>();

            if (hit.rigidbody)
            {
                InteractableObject io = hit.rigidbody.GetComponent<InteractableObject>();

                if(io != lastInteractable)
                {
                    lastInteractable = io;
                    GUI_Controller.Instance.SetInteractText(lastInteractable);
                }

                io = hit.collider.GetComponent<InteractableObject>();

                if (io != lastInteractable)
                {
                    lastInteractable = io;
                    GUI_Controller.Instance.SetInteractText(lastInteractable);
                }
            }
            else if(i != null)
            {
                if (i != lastInteractable)
                {
                    lastInteractable = i;
                    GUI_Controller.Instance.SetInteractText(lastInteractable);
                }
            }
            else if(lastInteractable)
            {
                lastInteractable = null;
                GUI_Controller.Instance.SetInteractText(lastInteractable);
            }
        }
        else
        {
            if(lastInteractable != null)
            {
                lastInteractable = null;
            }

            GUI_Controller.Instance.SetInteractText(lastInteractable);
        }

        if (mouseLeftDown)
        {
            if (hit.collider)
            {
                if (hit.rigidbody)
                {
                    InteractableObject io = hit.rigidbody.GetComponent<InteractableObject>();
                    if (io != null)
                    {
                        io.StartInteract();
                        hand.transform.position = hit.point;
                        hand.connectedBody = io.rb;
                        hand.transform.parent = null;
                        interactableObject = io;
                        lineRenderer.enabled = true;
                    }
                    else
                    {
                        Interactable i = hit.collider.GetComponent<Interactable>();
                        if (i != null)
                        {
                            i.StartInteract();
                        }
                    }
                }
                else
                {
                    Interactable io = hit.collider.GetComponent<Interactable>();
                    if (io != null)
                    {
                        io.StartInteract();
                    }
                }
            }
        }

        if(mouseLeftUp)
        {
            if (interactableObject != null)
            {
                interactableObject.StopInteract();
                hand.connectedBody = null;
                hand.transform.parent = cam.transform;
                //interactableObject.rb.AddRelativeForce(hand.transform.position - target.transform.position * powerReleaseForce, ForceMode.Force);
                lineRenderer.enabled = false;
                interactableObject = null;
            }
        }

        if(interactableObject != null)
        {
            lineRenderer.SetPosition(0, hand.transform.position);
            lineRenderer.SetPosition(1, target.transform.position);
            hand.transform.position = Vector3.Lerp(hand.transform.position, target.transform.position, lerpTargetSpeed);
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
}

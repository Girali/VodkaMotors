using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeViewManager : MonoBehaviour
{
    public Transform vehicul;

    public WheelViewManger left;
    public WheelViewManger right;

    public GameObject axeRender;

    void Update()
    {
        if (axeRender.gameObject.activeSelf) 
        {
            if (!left.gameObject.activeSelf && !right.gameObject.activeSelf)
            {
                axeRender.SetActive(false);
            }
        }
        else
        {
            if (left.gameObject.activeSelf || right.gameObject.activeSelf)
            {
                axeRender.SetActive(true);
            }
        }

        if (left.gameObject.activeSelf && right.gameObject.activeSelf)
        {
            transform.position = left.v + ((right.v - left.v) * 0.5f);
            transform.rotation = Quaternion.LookRotation(right.v - left.v);
        }
        else if (left.gameObject.activeSelf)
        {
            transform.position = left.v + (((transform.position + vehicul.TransformDirection(right.locaPosition)) - left.v) * 0.5f);
            transform.rotation = Quaternion.LookRotation((transform.position + vehicul.TransformDirection(right.locaPosition)) - left.v);
        }
        else if (right.gameObject.activeSelf)
        {
            transform.position = right.v + (((transform.position + vehicul.TransformDirection(left.locaPosition)) - right.v) * 0.5f);
            transform.rotation = Quaternion.LookRotation((transform.position + vehicul.TransformDirection(left.locaPosition)) - right.v);
        }
    }
}

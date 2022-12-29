using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DUI_InterestPoint : MonoBehaviour
{
    private GameObject player;
    private GameObject interestPoint;
    [SerializeField]
    private float distance;

    [SerializeField]
    private bool followInterestPoint;

    public void Init(GameObject p, GameObject ip)
    {
        player = p;
        interestPoint = ip;
    }

    public void DestoryNow(Interactable io)
    {
        if(io)
            io.onInteractStart -= DestoryNow;

        Destroy(gameObject);
    }

    public void DestoryNow()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        if(player != null)
        {
            if(interestPoint == null)
                Destroy(gameObject);

            if(followInterestPoint)
                transform.position = interestPoint.transform.position;

            float f = Vector3.Distance(player.transform.position, transform.position);

            if(f < distance)
            {
                Destroy(gameObject);
            }
        }
    }
}

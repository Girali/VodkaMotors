using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateContentManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] prefabs;
    [SerializeField]
    private Transform[] targets;

    [SerializeField]
    private int itemGenCount = 5;

    [SerializeField]
    private GameObject topClosed;
    [SerializeField]
    private GameObject topOpened;


    [SerializeField]
    private int lockState = 0;
    private int currentLockState = 0;

    private InteractableObject interactableObject;
    private DUI_InterestPoint interestPoint;

    private void Awake()
    {
        interactableObject = GetComponent<InteractableObject>();
        GUI_Controller.Instance.compass.InitMission(this);
        interestPoint = DUI_Controller.Instance.AddItemPoint(gameObject);
        interactableObject.onInteractStart += interestPoint.DestoryNow;
    }

    public void Complete()
    {
        GameController.Instance.CompleteMission();
        Destroy(gameObject);
    }

    private void Generate()
    {
        for (int i = 0; i < itemGenCount; i++)
        {
            Transform t = targets[Random.Range(0, targets.Length)];
            GameObject g = prefabs[Random.Range(0, prefabs.Length)];
            Instantiate(g, t.position, g.transform.rotation);
        }
    }

    public void BreakLock()
    {
        currentLockState++; 
        if (lockState == currentLockState)
            Open();
    }

    public void Open()
    {
        Generate();
        topClosed.SetActive(false);
        topOpened.SetActive(true);
    }
}

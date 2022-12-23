using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    private static TutorialManager instance;
    public static TutorialManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TutorialManager>();
            }

            return instance;
        }
    }

    public TutorialStep[] steps;
    private int indexStep = -1;

    public GameObject prefabPart;
    public GameObject missiongFirst;
    public GameObject grageInteract;

    private DUI_InterestPoint interestPoint;

    public void SpawnFirstPart()
    {
        GameObject g = Instantiate(prefabPart);
        g.transform.position = missiongFirst.transform.position + (Vector3.up * 5f);
        g.GetComponent<InteractablePart>().collectablePart = true;

        interestPoint = DUI_Controller.Instance.AddItemPoint(GameController.Instance.spawnPoints.gameObject);

        DUI_InterestPoint ip = DUI_Controller.Instance.AddItemPoint(g);
        g.GetComponent<InteractablePart>().onInteractStart += ip.DestoryNow;
    }

    public void UseGarage()
    {
        interestPoint = DUI_Controller.Instance.AddItemPoint(grageInteract);
    }

    private void Start()
    {
        NextStep();
    }

    public void NextStep()
    {
        if (interestPoint)
            interestPoint.DestoryNow(null);

        indexStep++;
        steps[indexStep].Begin();
    }

}

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
    public Interactable shotgun;
    public AudioSource radio;
    public AudioSource car;

    private DUI_InterestPoint interestPoint;

    public void SpawnFirstPart()
    {
        GameObject g = Instantiate(prefabPart);
        g.transform.position = missiongFirst.transform.position + (Vector3.up * 5f);
        g.GetComponent<InteractablePart>().StartInteraction();

        interestPoint = DUI_Controller.Instance.AddItemPoint(GameController.Instance.spawnPoints.gameObject);

        MusicController.Instance.audioSourceDialogue = missiongFirst.GetComponent<MissionStructure>().source;
        missiongFirst.GetComponent<MissionStructure>().system.Play();

        DUI_InterestPoint ip = DUI_Controller.Instance.AddItemPoint(g);
        g.GetComponent<InteractablePart>().onInteractStart += ip.DestoryNow;
    }

    public void UseGarage()
    {
        MusicController.Instance.audioSourceDialogue = radio;
        interestPoint = DUI_Controller.Instance.AddItemPoint(grageInteract);
    }

    public void AddShotgun()
    {
        GameController.Instance.player.GetComponent<PlayerItemController>().AddShotgun();
    }

    public void Shotgun()
    {
        DUI_InterestPoint ip = DUI_Controller.Instance.AddItemPoint(shotgun.gameObject);
        shotgun.onInteractStart += ip.DestoryNow;
        shotgun.canInteract = true;

        EnnemySpawner es = GameController.Instance.GetNearMajorStruct().GetComponent<EnnemySpawner>();
        es.during = false;
        es.spawned = false;
        es.ennemiesToSpawn = new Vector2Int(2, 2);
        es.spawns = new List<GameObject>();
        es.allDead += steps[5].End;
        es.totalDead = 0;

        interestPoint = DUI_Controller.Instance.AddItemPoint(es.gameObject);
        es.allDead += interestPoint.DestoryNow;
    }

    public void NewDelivery()
    {
        MusicController.Instance.audioSourceDialogue = car;
        GameController.Instance.StartNewMission();
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

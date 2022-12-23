using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DUI_Controller : MonoBehaviour
{
    private static DUI_Controller _instance;
    public static DUI_Controller Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DUI_Controller>();
            }

            return _instance;
        }
    }

    [SerializeField]
    private GameObject pointInterestQuest;
    [SerializeField]
    private GameObject pointInterestItem;

    public DUI_InterestPoint AddItemPoint(GameObject ip)
    {
        DUI_InterestPoint g = Instantiate(pointInterestItem, ip.transform.position, Quaternion.identity, transform).GetComponent<DUI_InterestPoint>();
        g.gameObject.SetActive(true);
        g.Init(GameController.Instance.player, ip);
        return g;
    }

    public DUI_InterestPoint AddQuestPoint(GameObject ip)
    {
        DUI_InterestPoint g = Instantiate(pointInterestQuest, ip.transform.position, Quaternion.identity , transform).GetComponent<DUI_InterestPoint>();
        g.gameObject.SetActive(true);
        g.Init(GameController.Instance.player, ip);
        return g;
    }
}

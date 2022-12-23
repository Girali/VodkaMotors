using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class UI_PartSeparator : MonoBehaviour
{
    public VehiculParts part;
    public TMP_Text text;
    public GameObject parent;
    public GameObject cell;
    private UI_PartsPanel panel;

    public bool opened = false;
    private bool hasEmpty = false;
    public VehiculPartObject emptyPlace;

    private List<VehiculPartObject> vehiculPartObjects = new List<VehiculPartObject>();

    public void AddVPO(VehiculPartObject vpo)
    {
        vehiculPartObjects.Add(vpo);
    }

    public void OnClick(VehiculPartObject vpo)
    {
        if(vpo.part == VehiculParts.None)
        {
            vpo = (VehiculPartObject)ScriptableObject.CreateInstance(typeof(VehiculPartObject));
            vpo.part = part;
        }

        VehiculPartObject v = panel.OnClick(vpo);

        if(v != null)
            vehiculPartObjects.Add(v);

        if(vpo.prefab != null)
            vehiculPartObjects.Remove(vpo);

        EmptySection();

        FillSection();
    }

    public void Open()
    {
        panel.OpenPart(this);
    }

    public void FillSection()
    {
        opened = true;

        if (hasEmpty && !panel.basePartController.IsCurrentPartEmpty())
            AddCell(emptyPlace);

        foreach (VehiculPartObject vpo in vehiculPartObjects)
        {
            AddCell(vpo);
        }

        StartCoroutine(CRT_UpdateTransform());
    }

    private IEnumerator CRT_UpdateTransform()
    {
        Vector2 v = transform.GetComponent<RectTransform>().sizeDelta;
        yield return new WaitForEndOfFrame();
        transform.GetComponent<RectTransform>().sizeDelta = v + Vector2.up;
    }

    public void EmptySection()
    {
        int i = 0;
        GameObject[] allChildren = new GameObject[parent.transform.childCount];

        foreach (Transform child in parent.transform)
        {
            allChildren[i] = child.gameObject;
            i += 1;
        }

        foreach (GameObject child in allChildren)
        {
            Destroy(child.gameObject);
        }

        opened = false;
        StartCoroutine(CRT_UpdateTransform());
    }

    private void AddCell(VehiculPartObject vpo)
    {
        GameObject g = Instantiate(cell, parent.transform);
        g.SetActive(true);
        g.GetComponent<UI_PartCell>().Init(vpo, this);
    }

    public void Init(VehiculParts v, UI_PartsPanel p, bool he)
    {
        hasEmpty = he;
        panel = p;
        part = v;
        text.text = v.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering;

public class PlayerItemController : MonoBehaviour
{
    [SerializeField]
    private float regenRate = 0.2f;
    [SerializeField]
    private float recoverRate = 0.2f;

    private float drinkAmount = 1f;
    private float alcoolAmount = 1f;
    private float alcoolAmountTarget = 0f;
    private float drink = 0.2f;
    private float drinkingTime = 3f;

    private float currentState = 2f;
    [SerializeField]
    private Volume volume;
    [SerializeField]
    private RotationConstraint constraint;
    private bool drinking = false;
    private float stopDrinkTimer = 0;

    private ItemType currentItem = ItemType.Shotgun;

    public GameObject[] items;

    private PlayerAnimation playerAnimation;
    private PlayerMotor playerMotor;
    public WeaponView weapon;
    public Camera cam;

    private bool reloading = false;
    private bool switchable = false;

    public void AddShotgun()
    {
        switchable = true;
        currentItem = ItemType.Shotgun;
        GUI_Controller.Instance.ammo.gameObject.SetActive(true);
        GUI_Controller.Instance.vodka.gameObject.SetActive(false);
        SwitchItem();
    }

    public void AddAmmo(int i)
    {
        weapon.AddAmmo(i);
    }

    private void Start()
    {
        playerMotor = GetComponent<PlayerMotor>();
        playerAnimation = GetComponent<PlayerAnimation>();
        currentItem = ItemType.Vodka;
        SwitchItem();
    }

    public enum ItemType
    {
        Vodka,
        Shotgun
    }

    public void SwitchItem()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (currentItem == (ItemType)i)
                items[i].SetActive(true);
            else
                items[i].SetActive(false);
        }

        playerAnimation.Stance((int)currentItem);
    }

    public void Motor(bool vodka, bool leftClick, bool reload, bool switchItem)
    {
        if(switchItem)
        {
            if(currentItem == ItemType.Vodka)
            {
                currentItem = ItemType.Shotgun;
                GUI_Controller.Instance.ammo.gameObject.SetActive(true);
                GUI_Controller.Instance.vodka.gameObject.SetActive(false);
            }
            else
            {
                currentItem = ItemType.Vodka;
                GUI_Controller.Instance.vodka.gameObject.SetActive(true);
                GUI_Controller.Instance.ammo.gameObject.SetActive(false);
            }

            SwitchItem();
        }

        switch (currentItem)
        {
            case ItemType.Vodka:
                if (!drinking)
                {
                    if (vodka && drinkAmount > drink)
                    {
                        stopDrinkTimer = Time.time + drinkingTime;
                        drinking = true;
                        drinkAmount -= drink;
                        alcoolAmountTarget += drink;
                        GUI_Controller.Instance.vodka.Empty(drinkAmount, drinkingTime);
                        playerAnimation.Drink();
                        playerMotor.AddHealth(20);
                    }
                }
                break;
            case ItemType.Shotgun:
                if (!weapon.reloading)
                {
                    if (leftClick)
                    {
                        weapon.Fire(cam.transform.position, cam.transform.forward, cam.transform.right, cam.transform.up,true);
                        playerAnimation.Fire();
                    }

                    if(reload && weapon.CanReload)
                    {
                        weapon.Reload();
                        playerAnimation.Reload();
                    }
                }
                break;
            default:
                break;
        }

        if (!drinking)
        {
            drinkAmount += regenRate * Time.deltaTime;
            alcoolAmountTarget -= recoverRate * Time.deltaTime;

            if (drinkAmount > 1)
                drinkAmount = 1;

            if (alcoolAmountTarget < 0)
                alcoolAmountTarget = 0;

            GUI_Controller.Instance.vodka.UpdateView(drinkAmount);
        }
        else
        {
            if (stopDrinkTimer < Time.time)
            {
                drinking = false;
            }
        }

        alcoolAmount = Mathf.Lerp(alcoolAmount, alcoolAmountTarget, 0.01f);

        volume.weight = alcoolAmount;
        constraint.weight = alcoolAmount;
    }
}

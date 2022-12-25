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

    private PlayerAnimation playerAnimation;

    private void Start()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    public enum ItemType
    {
        Vodka,
        Shotgun
    }

    public void Motor(bool vodka, bool leftClick, bool reload)
    {
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
            }

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

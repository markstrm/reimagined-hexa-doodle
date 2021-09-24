using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldBar : MonoBehaviour
{

    public Slider slider;


    public void SetMaxHealth(int health)
    {
        slider.maxValue = health - 100;
        slider.value = health - 100;
    }
    public void SetShield(int health)
    {

        slider.value = health - 100;
    }
}

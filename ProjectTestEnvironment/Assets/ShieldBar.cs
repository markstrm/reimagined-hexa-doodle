using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldBar : MonoBehaviour
{

    public Slider slider;


    public void SetMaxHealth(int health)
    {
        slider.maxValue = health ;
        slider.value = health;
    }
    public void SetShield(int health)
    {

        slider.value = health;
    }
}

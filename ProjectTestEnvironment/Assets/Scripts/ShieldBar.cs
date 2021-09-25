using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldBar : MonoBehaviour
{

    public Slider slider;
    Color color;

    float flashing;
    public Image shieldBar;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health ;
        slider.value = health;
    }
    public void SetShield(int health)
    {

        slider.value = health;

        
    }

    private void Update()
    {
        if (slider.value <= 100)
        {
            flashing += Time.deltaTime * 20f;
        }
        else
        {
            flashing = 0;
        }

        var sinedFlasing = Mathf.Sin(flashing - Mathf.PI * 0.5f) * 0.5f + 0.5f;

        shieldBar.color = Color.Lerp(Color.white, Color.red, sinedFlasing);
    }

}


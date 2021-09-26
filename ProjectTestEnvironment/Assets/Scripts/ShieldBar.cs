using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldBar : MonoBehaviour
{

    public Slider Slider;
    Color color;

    float flashing;
    public Image shieldBar;

    public void SetMaxHealth(int health)
    {
        Slider.maxValue = health;
        Slider.value = health;
    }
    public void SetShield(int health)
    {
        Slider.value = health;
    }

    private void Update()
    {
        if (Slider.value <= 100)
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


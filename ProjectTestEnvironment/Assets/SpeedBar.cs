using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBar : MonoBehaviour
{

    public Slider slider;

    public float speedTime = -10;

    //private bool stopTimer;

    private void Start()
    {
        slider.value = 0f;
    }

    public void SetBarTimer()
    {
        speedTime = Time.time;
    }

    private void Update()
    {
        slider.value = Mathf.Clamp01(1f - (Time.time - speedTime) / 10f);
    }

    //private void Update()
    //{
    //float time = speedTime - Time.time;

    //slider.value = time;
    //}
}

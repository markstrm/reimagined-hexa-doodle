using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeCounter : MonoBehaviour
{

    //public Image[] Life;
    public int livesRemaining = 3;

    public void LoseLife()
    {
        livesRemaining--;
    }

    private void Update()
    {
       
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i < livesRemaining);
        }
        }
}





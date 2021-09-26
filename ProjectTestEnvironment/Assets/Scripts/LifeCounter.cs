using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeCounter : MonoBehaviour
{

    public int LivesRemaining = 3;

    public void LoseLife()
    {
        LivesRemaining--;
    }

    private void Update()
    {
       
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i < LivesRemaining);
        }
        }
}





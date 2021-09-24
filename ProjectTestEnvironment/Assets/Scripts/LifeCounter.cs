using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeCounter : MonoBehaviour
{

    //public Image[] Life;
    public int livesRemaining = 2;

    public void LoseLife()
    {
        livesRemaining--;
    }

    private void Update()
    {
        Debug.Log(livesRemaining);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i < livesRemaining);
        }
        }
}





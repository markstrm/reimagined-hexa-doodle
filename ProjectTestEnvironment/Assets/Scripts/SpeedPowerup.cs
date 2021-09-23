using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerup : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody2D rb;
    PlayerMovement movement;

    private void Awake()
    {

        rb=GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();

    }
    private void OnTriggerEnter2D(Collider2D other)
    {


        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Speedy");
            Pickup();
            

        }

    }

    void Pickup()
    {
        


        movement.speed = 24f;
        

        Destroy(gameObject);

    }


}

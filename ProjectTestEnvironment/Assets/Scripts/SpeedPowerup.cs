using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerup : MonoBehaviour
{
    // Start is called before the first frame update

    public PlayerMovement player;

    [SerializeField]
    private float speedboost;
    

    Rigidbody2D rb;

    private void Awake()
    {

        rb=GetComponent<Rigidbody2D>();

    }
    private void OnTriggerEnter2D(Collider2D other)
    {


        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Speedy");
            Pickup(other.gameObject.GetComponent<PlayerMovement>());
        }

    }

    void Pickup(PlayerMovement player)
    {

        player.speed = speedboost;
        player.Speedtime = true;
        player.Speedtimer();

        

        Destroy(gameObject);

    }

 



}

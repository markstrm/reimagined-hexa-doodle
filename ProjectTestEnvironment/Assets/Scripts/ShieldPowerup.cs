using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerup : MonoBehaviour
{
    public PlayerMovement player;
    public float maxLifetime = 10f;

    Rigidbody2D rb;

    private void Awake()
    {

        rb = GetComponent<Rigidbody2D>();

    }

    private void Start()
    {
        Destroy(this.gameObject, this.maxLifetime); //destroy the bullet after 10s
    }

    private void OnTriggerEnter2D(Collider2D other) // If Player collides with powerup
    {


        if (other.gameObject.tag == "Player")
        {
            Debug.Log("More Shields");
            Pickup(other.gameObject.GetComponent<PlayerMovement>());
        }

    }

    void Pickup(PlayerMovement player) // the powerup speed buff given to the player and also starts a timer in the Playermovement script. Also destroys the powerup object
    {

        player._health = 300;
        Destroy(gameObject);

    }
}

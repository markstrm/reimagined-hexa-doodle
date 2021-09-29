using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerup : MonoBehaviour
{
    public PlayerMovement player;
    public float MaxLifetime = 10f;
   // private float time;

    public AudioClip _speedBoost;
    public float _speedBoostVol = 0.7f;

    [SerializeField]
    private float _speedboost;
    
    Rigidbody2D _rb;
   // public Animator animator;

    private void Awake()
    {
        _rb=GetComponent<Rigidbody2D>();
       // animator = GetComponent<Animator>();
    }
    private void Start()
    {

        //animator = gameObject.GetComponent<Animator>();
        Destroy(this.gameObject, this.MaxLifetime); //destroy the bullet after 10s
     

       // if (MaxLifetime < 3f)
      //  {

         //   animator.Play("Speed_Powerup_Expire");


      //  }


    }

   // private void Update()
   // {

        //time -= Time.deltaTime;
       // time = MaxLifetime;


  //  }

    private void OnTriggerEnter2D(Collider2D other) // If Player collides with powerup
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Speedy");
            Pickup(other.gameObject.GetComponent<PlayerMovement>());
            AudioSource.PlayClipAtPoint(_speedBoost, transform.position, _speedBoostVol);
        }
    }

    void Pickup(PlayerMovement player) // the powerup speed buff given to the player and also starts a timer in the Playermovement script. Also destroys the powerup object
    {
        player.speed = _speedboost;
        player.Speedtime = true;
        player.Speedtimer();
        Destroy(gameObject);
    }

 
    


}

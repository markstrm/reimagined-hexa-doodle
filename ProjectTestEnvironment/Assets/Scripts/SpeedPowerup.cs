using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class SpeedPowerup : MonoBehaviour
{
    public PlayerMovement player;
    public float MaxLifetime = 10f;
   // private float time;

    public AudioClip _speedBoost;
    public float _speedBoostVol = 0.7f;

    [SerializeField]
    private float _speedboost;

    [SerializeField] private float _timeUntilExpiration; //time until start anim

    private bool _hasPlayedAnimation;

    Rigidbody2D _rb;
    // public Animator animator;

    Animator anim;

    private float _timeOfCreation;

    private void Awake()
    {
        _timeOfCreation = Time.time; //saves the time when object was created
        _rb=GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
       // animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Time.time - _timeOfCreation > _timeUntilExpiration && _hasPlayedAnimation == false)
        {
             _hasPlayedAnimation = true;
            anim.SetTrigger("OnDespawn");
        }
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
       // player.Speedtime = true;
        player.Speedtimer();
        Destroy(gameObject);
    }

 
    


}

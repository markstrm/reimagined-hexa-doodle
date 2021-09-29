using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ShieldPowerup : MonoBehaviour
{
    public PlayerMovement player;
    public float MaxLifetime = 10f;

    public AudioClip _shieldRecharge;
    public float _shieldRechargeVol = 0.7f;

    [SerializeField] private float _timeUntilExpiration; //time until start anim
    private bool _hasPlayedAnimation;
    Animator anim;

    private float _timeOfCreation;

    Rigidbody2D rb;

    private void Awake()
    {
        _timeOfCreation = Time.time; //saves the time when object was created
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

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
        Destroy(this.gameObject, this.MaxLifetime); //destroy the bullet after 10s
    }

    private void OnTriggerEnter2D(Collider2D other) // If Player collides with powerup
    {


        if (other.gameObject.tag == "Player")
        {
            Debug.Log("More Shields");
            Pickup(other.gameObject.GetComponent<PlayerMovement>());
            AudioSource.PlayClipAtPoint(_shieldRecharge, transform.position, _shieldRechargeVol);
        }

    }

    void Pickup(PlayerMovement player) // the powerup speed buff given to the player and also starts a timer in the Playermovement script. Also destroys the powerup object
    {

        player._health = 300;
        Destroy(gameObject);

    }
}

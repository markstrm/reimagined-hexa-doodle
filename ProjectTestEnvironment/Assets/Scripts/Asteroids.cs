using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroids : MonoBehaviour
{

    public float _Hitpoints;
    public float _MaxHitpoints = 2;

    private float _Rotation;
    public float _RotationSpeed;
    public bool _ClockwiseRotation;

    private Rigidbody2D _Rigidbody;


    private void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _Hitpoints = _MaxHitpoints;
    }

    void Update()
    {
        if (_ClockwiseRotation == false)
        {
            _Rotation += Time.deltaTime * _RotationSpeed;
        }
        else
        {
            _Rotation += -Time.deltaTime * _RotationSpeed;
        }

        transform.rotation = Quaternion.Euler(0, 0, _Rotation);

    }

    public void TakeHit(float damage)
    {
        _Hitpoints -= damage;
        if(_Hitpoints <= 0)
        {
            // here we can put a explosion sfx
            Destroy(gameObject);
           
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) // Can collide with player, bullet and possibly enemy?
    {
        if(collision.gameObject.tag == "Bullet")
        {

        }
    }

}

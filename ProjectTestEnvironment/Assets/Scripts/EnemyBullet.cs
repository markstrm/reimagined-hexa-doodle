using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    GameObject _target;
    public float _speed;
    Rigidbody2D _rigidbody;
    public float maxLifetime = 1f;
    private float _searchCountdown = 0.5f;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>(); //reference to the rigidbody
        _target = GameObject.FindGameObjectWithTag("Player"); //when spawn, will look at the player
        Vector2 _moveDirection = (_target.transform.position - transform.position).normalized * _speed;
        _rigidbody.velocity = _moveDirection * _speed;
        

        transform.eulerAngles = new Vector3(0, 0, Vector2ToAngle(_moveDirection) - 90f);
        
    }

    public void Update()
    {
        if(_BulletExists() == true)
        {
            Destroy(this.gameObject, this.maxLifetime); //destroy the bullet after 0.5s
        }
    }

    bool _BulletExists()
    {
        _searchCountdown -= Time.deltaTime;
        if (_searchCountdown <= 0f) //for performance, checks every second if there are any bullets in existance, instead of every frame.
        {
            _searchCountdown = 0.5f; //if searchCountdown reaches 0 and there still is bullets existing
            if (GameObject.FindGameObjectWithTag("Enemy Projectile") == null) //checks if there are still any bullets existing
            {
                return false; //returns false if no bullets are found
            }
        }
        return true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }

    public static float Vector2ToAngle(Vector2 vect)
    {
        return Mathf.Atan2(vect.y, vect.x) * Mathf.Rad2Deg;
    }
}

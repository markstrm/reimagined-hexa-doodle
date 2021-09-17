using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    GameObject _target;
    public float _speed;
    Rigidbody2D _rigidbody;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>(); //reference to the rigidbody
        _target = GameObject.FindGameObjectWithTag("Player"); //when spawn, will look at the player
        Vector2 _moveDirection = (_target.transform.position - transform.position).normalized * _speed;
        _rigidbody.velocity = new Vector2(_moveDirection.x, _moveDirection.y);
        Destroy(this.gameObject, 2);
    }

}

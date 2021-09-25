using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private Rigidbody2D _Rigidbody;

    public float speed = 4000f;
    public float maxLifetime = 8f;

    private void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Project(Vector2 direction)
    {
        _Rigidbody.AddForce(direction * this.speed); //force of the bullet

        Destroy(this.gameObject, this.maxLifetime); //destroy the bullet after 8s
    }

}

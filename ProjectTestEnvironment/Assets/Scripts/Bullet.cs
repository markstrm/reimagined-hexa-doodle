
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private Rigidbody2D _Rigidbody;

    public float speed = 100f;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var enemy = collision.collider.GetComponent<Asteroids>();
        if (enemy)
        {
            enemy.TakeHit(1);
        }
        Destroy(gameObject);
    }

}

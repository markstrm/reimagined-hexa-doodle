
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private Rigidbody2D _rigidbody;

    public float Speed = 4000f;
    public float MaxLifetime = 8f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Project(Vector2 direction)
    {
        _rigidbody.AddForce(direction * this.Speed); //force of the bullet

        Destroy(this.gameObject, this.MaxLifetime); //destroy the bullet after 8s
    }


}

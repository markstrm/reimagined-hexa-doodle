using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    public float _movementSpeed;
    public float _lineOfSight;
    public float _shootingRange;
    public GameObject _bullet; //the bullet that the enemy will shoot
    public GameObject _bulletParent; //the place where the bullet will be shot from
    private Transform _player; //target the player

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {

        float _distanceFromPlayer = Vector2.Distance(_player.position, transform.position); //distance between the player and the enemy
        RotateTowards(_player.position);
        if (_distanceFromPlayer < _lineOfSight && _distanceFromPlayer>_shootingRange) 
        {
            transform.position = Vector2.MoveTowards(this.transform.position, _player.position, _movementSpeed * Time.deltaTime); //our position, player position
        }   
    }

    private void OnDrawGizmosSelected() //draws a circle with a size that we can decide
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _lineOfSight);
        Gizmos.DrawWireSphere(transform.position, _shootingRange);
    }

    private void RotateTowards(Vector2 target)
    {
        var offset = 90f;
        Vector2 direction = target - (Vector2)transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
    }
}

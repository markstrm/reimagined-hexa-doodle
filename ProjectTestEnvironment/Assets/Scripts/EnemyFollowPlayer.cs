using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    public float _movementSpeed;
    public float _lineOfSight;
    public float _shootingRange;
    public float _stationaryRange;
    public float _fireRate = 0.4f;
    public int _scoreValue = 100;
    public float _health = 100;
    public GameObject _deathVFX;
    public float _durationOfExplosion = 1f;

    private float _nextFireTime;

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
        if (_distanceFromPlayer < _lineOfSight && _distanceFromPlayer > _stationaryRange) 
        {
            transform.position = Vector2.MoveTowards(this.transform.position, _player.position, _movementSpeed * Time.deltaTime); //our position, player position
        }   
        if(_distanceFromPlayer < _shootingRange && _nextFireTime <Time.time)
        {
            Instantiate(_bullet, _bulletParent.transform.position, Quaternion.identity);
            _nextFireTime = Time.time + _fireRate;
        }
    }

    private void RotateTowards(Vector2 target)//rotates to face the player continuosly
    {
        var offset = 90f;
        Vector2 direction = target - (Vector2)transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
    }

    private void OnTriggerEnter2D(Collider2D other)//receives damage from player bullet
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)//calculates damage received
    {
        _health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()//enemy death
    {
        FindObjectOfType<GameSession>().AddToScore(_scoreValue);
        Destroy(gameObject);
        GameObject explosion = Instantiate(_deathVFX, transform.position, transform.rotation);//explosion vfx
        Destroy(explosion, _durationOfExplosion);
    }

    private void OnDrawGizmosSelected() //draws a circle with a size that we can decide
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _lineOfSight);
        Gizmos.DrawWireSphere(transform.position, _shootingRange);
        Gizmos.DrawWireSphere(transform.position, _stationaryRange);
    }

}

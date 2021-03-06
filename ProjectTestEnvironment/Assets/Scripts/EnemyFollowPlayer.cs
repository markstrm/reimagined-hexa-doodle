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

    private int randomNumber;

   // public PlayerMovement player;
    public GameObject speedPickUp;
    public GameObject healthPickUp;

    GameSession gameSession;
    public GameObject spawnPoint; //array with all the possible spawn locations for enemy2

    // public GameObject _BulletHolder;
    public float _durationOfExplosion = 1f;

    private float _nextFireTime;

    public float _shieldDuration = 1f;
    public float _shieldDelay = 1f;
    public Color _shieldColor;

    SpriteRenderer sr;
    Color defaultColor;

    public float _respawnInvulnerabilityTime = 3.0f;
    public Animator animator;

    public AudioClip _deathSFX;
    public float _deathSFXVol = 0.7f;

    public AudioClip _bulletSFX;
    public float _bulletSFXVol = 0.7f;

    public AudioClip _shieldHitSFX;
    public float _shieldHitSFXVol = 0.7f;

    public GameObject _bullet; //the bullet that the enemy will shoot
    public GameObject _bulletParent; //the place where the bullet will be shot from
    private Transform _player; //target the player

    private PlayerMovement player;
    private GameObject _EnemyHolder;
    public Transform _enemy; //reference to the prefab that we want to instantiate
    public Transform _enemytwo; //reference to the prefab that we want to instantiate

    // Start is called before the first frame update
    void Start()
    {
        //_player = GameObject.FindGameObjectWithTag("Player").transform;

        gameSession = GameObject.Find("Game Session").GetComponent<GameSession>();
        sr = GetComponent<SpriteRenderer>();
        defaultColor = sr.color;//saves default sprite color

        _movementSpeed = Random.Range(10, 15); //random movementspeed to try mitigate the clumping

        //animator.SetBool("EnemyRespawn", true);
        this.GetComponent<PolygonCollider2D>().enabled = false;
        this.gameObject.SetActive(true);

        animator = gameObject.GetComponent<Animator>();

        animator.SetTrigger("EnemyRespawn2");
        Invoke(nameof(TurnOnCollisions), _respawnInvulnerabilityTime);

    }

    private void TurnOnCollisions()
    {
        this.GetComponent<PolygonCollider2D>().enabled = true;

        // animator.SetBool("EnemyRespawn", true);
    }

    // Update is called once per frame
    void Update()
    {
        if(_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if(_player != null)
        {

            float _distanceFromPlayer = Vector2.Distance(_player.position, transform.position); //distance between the player and the enemy
            RotateTowards(_player.position);
            if (_distanceFromPlayer < _lineOfSight && _distanceFromPlayer > _stationaryRange) 
            {
                transform.position = Vector2.MoveTowards(this.transform.position, _player.position, _movementSpeed * Time.deltaTime); //our position, player position
            }   
            if(gameSession.isAlive && _distanceFromPlayer < _shootingRange && _nextFireTime < Time.time)
            {
                Instantiate(_bullet, _bulletParent.transform.position, Quaternion.identity);
                AudioSource.PlayClipAtPoint(_bulletSFX, transform.position, _bulletSFXVol);
                _nextFireTime = Time.time + _fireRate;
            }
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
        if (!damageDealer)
        {
            return;
        }

        _health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (_health <= 0)
        {
            Die();
        }
        else
        {
            AudioSource.PlayClipAtPoint(_shieldHitSFX, transform.position, _shieldHitSFXVol);
            StartCoroutine(_DamageEffectSequence());
        }
    }

    IEnumerator _DamageEffectSequence()
    {

        // tint the sprite with damage color
        sr.color = _shieldColor;

        // you can delay the animation
        yield return new WaitForSeconds(_shieldDelay);

        // lerp animation with given duration in seconds
        for (float t = 0; t < 1.0f; t += Time.deltaTime / _shieldDuration)
        {
            sr.color = Color.Lerp(_shieldColor, defaultColor, t);

            yield return null;
        }

        // restore origin color
        sr.color = defaultColor;
    }

    private void Die()//enemy death
    {
        FindObjectOfType<GameSession>().AddToScore(_scoreValue);
        GameObject explosion = Instantiate(_deathVFX, transform.position, transform.rotation);

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        _EnemyHolder = GameObject.FindGameObjectWithTag("EnemyHolder");

        randomNumber = Random.Range(0, 100);
        Debug.Log(randomNumber);

        if(transform.position.x < gameSession.GetBoundsWidth() && transform.position.x > -gameSession.GetBoundsWidth()
            && transform.position.y < gameSession.GetBoundsHeight() && transform.position.y > -gameSession.GetBoundsHeight())
        {
            if (randomNumber >= 85)
            {
                if (randomNumber >= 85 && gameSession.isAlive && player._health == 100)
                {
                    //spawn health pickup
                    Instantiate(healthPickUp, transform.position, Quaternion.identity);
                    Debug.Log(randomNumber);
                }
                else if (randomNumber >= 90 && gameSession.isAlive && player._health == 200)
                {
                    Instantiate(healthPickUp, transform.position, Quaternion.identity);
                }
                else
                {
                    //spawn speed pickup
                    Instantiate(speedPickUp, transform.position, Quaternion.identity);
                    Debug.Log(randomNumber);
                }
            }

            if (randomNumber >= 95)
            {
                if (gameSession.score > 10000 && gameSession.score < 25000)
                {
                    var spawnPoints = GameObject.FindGameObjectsWithTag("E2S");
                    Debug.Log("Spawn points " + spawnPoints.Length);

                    Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)].transform;
                    Instantiate(_enemy, _sp.position, _sp.rotation, _EnemyHolder.transform);  //spawn enemy
                }
                else if (gameSession.score >= 25000 && gameSession.score <= 40000)
                {
                    var spawnPoints = GameObject.FindGameObjectsWithTag("E3S");
                    Debug.Log("Spawn points " + spawnPoints.Length);

                    Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)].transform;
                    Instantiate(_enemytwo, _sp.position, _sp.rotation, _EnemyHolder.transform);  //spawn enemy   
                }
                else if (gameSession.score > 40000)
                {
                    var spawnPoints = GameObject.FindGameObjectsWithTag("E3S");
                    Debug.Log("Spawn points " + spawnPoints.Length);

                    Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)].transform;
                    Instantiate(_enemytwo, _sp.position, _sp.rotation, _EnemyHolder.transform);  //spawn enemy   

                    var spawnPointstwo = GameObject.FindGameObjectsWithTag("E2S");
                    Debug.Log("Spawn points " + spawnPoints.Length);

                    Transform _sptwo = spawnPoints[Random.Range(0, spawnPoints.Length)].transform;
                    Instantiate(_enemy, _sptwo.position, _sptwo.rotation, _EnemyHolder.transform);  //spawn enemy
                }
                else if (randomNumber >= 97)
                {
                    var spawnPoints = GameObject.FindGameObjectsWithTag("E2S");
                    Debug.Log("Spawn points " + spawnPoints.Length);

                    Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)].transform;
                    Instantiate(_enemy, _sp.position, _sp.rotation, _EnemyHolder.transform);  //spawn enemy
                }
            }
        }
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(_deathSFX, transform.position, _deathSFXVol);
    }

    private void OnDrawGizmosSelected() //draws a circle with a size that we can decide
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _lineOfSight);
        Gizmos.DrawWireSphere(transform.position, _shootingRange);
        Gizmos.DrawWireSphere(transform.position, _stationaryRange);
    }

}

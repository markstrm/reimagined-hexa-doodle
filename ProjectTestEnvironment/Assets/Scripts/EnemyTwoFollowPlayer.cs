using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTwoFollowPlayer : MonoBehaviour
{

    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float fireRate;
    [SerializeField]
    private float shootingDistance;
    [SerializeField] private string _waypointName;
    [SerializeField] private float _rotationOffset;

    private int _currentWaypoint;
    private bool _isInRange;
    private Transform _playerTransform;
    private float _previousShootTime;
    private Transform[] _wayPoints;
    private GameObject _waypointsGO;
    public GameObject healthPickUp;
    public GameObject speedPickUp;

    public float _respawnInvulnerabilityTime = 3.0f;
    public Animator animator;

    public int _health = 600;
    public int _scoreValue = 500;

    public float _shieldDuration = 1f;
    public float _shieldDelay = 1f;
    public Color _shieldColor;
    public float _durationOfExplosion = 1f;

    SpriteRenderer sr;
    Color defaultColor;

    public GameObject _deathVFX;
    public AudioClip _deathSFX;
    public float _deathSFXVol = 0.7f;
    public AudioClip _bulletSFX;
    public float _bulletSFXVol = 0.7f;
    public AudioClip _shieldHitSFX;
    public float _shieldHitSFXVol = 0.7f;
    public EnemyHealthBar healthBar;

    private GameSession _gameSession;
    public GameObject _bullet; //the bullet that the enemy will shoot
    public GameObject _bulletParent; //the place where the bullet will be shot from
    private PlayerMovement player;
    

    //private int spawnedEnemiesAmount = 0;

    private void Start()
    {
        movementSpeed = Random.Range(8, 12); //random movementspeed to try mitigate the clumping
        _gameSession = FindObjectOfType<GameSession>();
        sr = GetComponent<SpriteRenderer>();
        defaultColor = sr.color;//saves default sprite color
        GameObject go = GameObject.FindWithTag("Player");
        if (go == null) return;
        _playerTransform = go.transform;
        CreateWaypoints();
        
        //healthBar.SetMaxHealth(_health);

        //this.GetComponent<PolygonCollider2D>().enabled = false;
        this.GetComponent<BoxCollider2D>().enabled = false;
        this.gameObject.SetActive(true);

        animator = gameObject.GetComponent<Animator>();

        animator.SetTrigger("Enemy2Respawn");
        Invoke(nameof(TurnOnCollisions), _respawnInvulnerabilityTime);
    }
        private void TurnOnCollisions()
    {
        //this.GetComponent<PolygonCollider2D>().enabled = true;
        this.GetComponent<BoxCollider2D>().enabled = true;
    }

    private void CreateWaypoints()
    {
        _waypointsGO = GameObject.Find(_waypointName);
        _wayPoints = new Transform[_waypointsGO.transform.childCount];
        for (int i = _waypointsGO.transform.childCount - 1; i >= 0; i--)
        {
            _wayPoints[i] = _waypointsGO.transform.GetChild(i);
        }
    }

    private void Update()
    {
        _waypointsGO.transform.position = _playerTransform.position;
        RotateTowardsPlayer();
        float distance = Vector2.Distance(_playerTransform.position, transform.position);
        //healthBar.SetHealth((int)_health);

        if (_isInRange)
        {
            if (Vector2.Distance(_wayPoints[_currentWaypoint].position, transform.position) < 0.1f)
            {
                GetNextWayPoint();
            }
            else
            {
                FollowWaypoints();
            }          
            
            if (_gameSession.isAlive)
            {
                if (Time.time - _previousShootTime >= fireRate)
                {
                    Shoot();
                }
            }
            
        }
        else
        {
            if (distance > shootingDistance)
            {
                MoveTowardsPlayer();
            }
            else
            {
                _isInRange = true;
                Shoot();
                SetClosestWayPointToCurrent();
            }
        }
        
    }

    private void GetNextWayPoint()
    {
        

        if (_currentWaypoint < _wayPoints.Length - 1)
        {
            _currentWaypoint++;
        }
        else
        {
            _currentWaypoint = 0;
        }
        
    }

    private void SetClosestWayPointToCurrent()
    {
        // Set minDistans to absolut maximum to get the first distans saved
        float minDistance = float.MaxValue;
        // The closest waypoint
        int closestWaypoint = -1;
        for (var i = 0; i < _wayPoints.Length; i++)
        {
            Transform wayPoint = _wayPoints[i];
            // Calculate the distance from our currentposition to the current invesitaged Waypoint
            float distance = Vector2.Distance(wayPoint.position, transform.position);
            // Check if the investigated distance is larger then the minimum distance
            if (minDistance > distance)
            {
                // If the current waypoint is shorter then the minumum distance then save it as the closest one
                minDistance = distance;
                closestWaypoint = i;
            }
        }
        // When done save the closest waypoint to the class variable
        _currentWaypoint = closestWaypoint;
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
        //healthBar.SetHealth(_health);
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
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        FindObjectOfType<GameSession>().AddToScore(_scoreValue);
        GameObject explosion = Instantiate(_deathVFX, transform.position, transform.rotation);//explosion vfx
        //Destroy(explosion, _durationOfExplosion);

        if (InsideBounds(transform.position)) 
        {

            if (player._health < 300) // if player is damaged, the enemy will drop health refill pickup, else if player is full health it will drop a speed pick up.
            {
                Instantiate(healthPickUp, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(speedPickUp, transform.position, Quaternion.identity);
            }
        }       
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(_deathSFX, transform.position, _deathSFXVol);
    }

    private bool InsideBounds(Vector2 position)
    {
        return
        position.x < _gameSession.GetBoundsWidth() && position.x > -_gameSession.GetBoundsWidth()
        && position.y < _gameSession.GetBoundsHeight() && position.y > -_gameSession.GetBoundsHeight();

    }

    private void Shoot()
    {
        // TODO Add Instantiate bullet
        Instantiate(_bullet, _bulletParent.transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(_bulletSFX, transform.position, _bulletSFXVol);
        _previousShootTime = Time.time;
    }

    private void FollowWaypoints()
    {
        if (InsideBounds(_wayPoints[_currentWaypoint].position))
        { 
            transform.position = Vector2.MoveTowards(
            transform.position,
            _wayPoints[_currentWaypoint].position,
            movementSpeed * Time.deltaTime
            );

        }
        else
        {
            GetNextWayPoint();
        }
    }

    private void RotateTowardsPlayer()
    { 
        Vector2 direction = (Vector2)_playerTransform.position - (Vector2)transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + _rotationOffset));
    }

    private void MoveTowardsPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, _playerTransform.position, movementSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, shootingDistance);
    }
}



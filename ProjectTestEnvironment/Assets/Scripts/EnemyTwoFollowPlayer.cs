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

    private int _currentWaypoint;
    private bool _isInRange;
    private Transform _playerTransform;
    private float _previousShootTime;
    private Transform[] _wayPoints;
    private GameObject _waypointsGO;

    public float _health = 600;
    public int _scoreValue = 300;

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

    GameSession gameSession;
    public GameObject _bullet; //the bullet that the enemy will shoot
    public GameObject _bulletParent; //the place where the bullet will be shot from

    private int spawnedEnemiesAmount = 0;

    private void Start()
    {
        gameSession = GameObject.Find("Game Session").GetComponent<GameSession>();
        sr = GetComponent<SpriteRenderer>();
        defaultColor = sr.color;//saves default sprite color
        GameObject go = GameObject.FindWithTag("Player");
        if (go == null) return;
        _playerTransform = go.transform;
        CreateWaypoints();
    }

    private void CreateWaypoints()
    {
        _waypointsGO = GameObject.Find("Waypoints");
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
        if (_isInRange)
        {
            if (Vector2.Distance(_wayPoints[_currentWaypoint].position, transform.position) < 0.1f)
            {
                GetNextWayPoint();
            }
            FollowWaypoints();
            // TODO Fix alive check
            if (gameSession.isAlive)
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
        GameObject explosion = Instantiate(_deathVFX, transform.position, transform.rotation);//explosion vfx
        //Destroy(explosion, _durationOfExplosion);
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(_deathSFX, transform.position, _deathSFXVol);

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
        transform.position = Vector2.MoveTowards(
            transform.position,
            _wayPoints[_currentWaypoint].position,
            movementSpeed * Time.deltaTime
            );
    }

    private void RotateTowardsPlayer()
    {
        var offset = 90f;
        Vector2 direction = (Vector2)_playerTransform.position - (Vector2)transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
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



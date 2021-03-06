using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    [SerializeField] Camera _Camera;

    PlayerInputActions _Input;
    Rigidbody2D _Rigidbody;
    public Bullet bulletPrefab;
    public Laser PlayerLaser;
    public GameObject centralFireLoc;
    public GameObject leftFireLoc;
    public GameObject rightFireLoc;

    private Vector2 movement;
    private Vector2 _MousePos;

   // public bool Speedtime = false;
    public bool canShoot = true;
    public bool canShootL = true;
    private float _oldPos;
    // [SerializeField] private float minTolerance;

    public int _health = 300;
    public GameObject _deathVFX;
    public float _durationOfExplosion = 2f;

    public float _shieldDuration = 1f;
    public float _shieldDelay = 1f;
    public Color _shieldColor;

    public ShieldBar shieldBar;
    public LifeCounter lifeCounter;
    public SpeedBar speedBar;

    public AudioClip _deathSFX;
    public float _deathSFXVol = 0.7f;

    public AudioClip _bulletSFX;
    public float _bulletSFXVol = 0.7f;

    public AudioClip _shieldHitSFX;
    public float _shieldHitSFXVol = 0.7f;

    public AudioClip _laserSFX;
    public float _laserSFXVol = 0.7f;

    public AudioClip _boostEndSFX;
    public float _boostEndSFXVol = 0.7f;

    public ParticleSystem PlayerTrail;
    public ParticleSystem PlayerTrailGreen;

    private GameSession _gameSession;

    SpriteRenderer sr;
    Color defaultColor;

    private Coroutine _speedBoostCR;

    private void Awake()
    {
        _gameSession = FindObjectOfType<GameSession>();
        _Input = new PlayerInputActions();
        _Rigidbody = GetComponent<Rigidbody2D>();
        SetUpSingleton();
        _oldPos = transform.position.x;
        sr = GetComponent<SpriteRenderer>();
        defaultColor = sr.color;//saves default sprite color
        shieldBar.SetMaxHealth(_health);
    }


    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime * movement.normalized.x;
        transform.position += Vector3.up * speed * Time.deltaTime * movement.normalized.y;

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, - _gameSession.GetBoundsWidth(), _gameSession.GetBoundsWidth()), Mathf.Clamp(transform.position.y, -_gameSession.GetBoundsHeight(), _gameSession.GetBoundsHeight()));
        shieldBar.SetShield(_health);
    }

    private void FixedUpdate()
    {

        Vector2 globalMousePosition = _Camera.ScreenToWorldPoint(_MousePos);
        Vector2 facingDirection = globalMousePosition - _Rigidbody.position; //_Rigidbody


        //if (Mathf.Abs(facingDirection.x) > minTolerance && Mathf.Abs(facingDirection.y) > minTolerance)
        // {
        float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg - 90;
        //_Rigidbody.MoveRotation(angle); 
        transform.rotation = Quaternion.AngleAxis(angle,Vector3.forward);
        //}
        
    }

    private void OnEnable()
    {
        _Input.Enable();
        _Input.Player.MousePos.performed += OnMousePos;
    }

    private void OnDisable()
    {
        _Input.Disable();
    }

    public void Move(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();

        if (movement.magnitude > 0) //checks for player movement to activate trail
        {
            PlayerTrail.Play();
        }
        if (movement.magnitude == 0)
        {
            PlayerTrail.Stop();
            PlayerTrailGreen.Stop();
        }
        if (movement.magnitude > 0 && speed > 20f)
        {
            PlayerTrailGreen.Play();
            PlayerTrail.Stop();
        }
    }

    public void OnMousePos(InputAction.CallbackContext context)
    {
        _MousePos = (context.ReadValue<Vector2>());
        //_MousePos = _Camera.ScreenToWorldPoint(context.ReadValue<Vector2>());
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!canShoot) return;

            Bullet bullet = Instantiate(bulletPrefab, centralFireLoc.transform.position, transform.rotation); //spawns the bullet and gives it a position and rotation. Will spawn at the players positon and in the same rotation as the player.
            bullet.Project(this.transform.up); //projects in the same positon as the player
            AudioSource.PlayClipAtPoint(_bulletSFX, transform.position, _bulletSFXVol);
            StartCoroutine(CanShoot());

        }
    }

    IEnumerator CanShoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(1.25f);
        canShoot = true;
    }

    public void Shoot2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!canShootL) return;

            {
                var laser = Instantiate(PlayerLaser, leftFireLoc.transform.position, leftFireLoc.transform.rotation);
                laser.Project(leftFireLoc.transform.up);
                AudioSource.PlayClipAtPoint(_laserSFX, transform.position, _bulletSFXVol);
                StartCoroutine(CanShootL());
            }
            {
                var laser = Instantiate(PlayerLaser, rightFireLoc.transform.position, rightFireLoc.transform.rotation);
                laser.Project(rightFireLoc.transform.up);
                AudioSource.PlayClipAtPoint(_laserSFX, transform.position, _bulletSFXVol);
                StartCoroutine(CanShootL());
            }
        }
    }
    
    IEnumerator CanShootL()
    {
        canShootL = false;
        yield return new WaitForSeconds(0.1f);
        canShootL = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        ProcessHit(damageDealer);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Die();
        }
        if (collision.gameObject.CompareTag("E2S"))
        {
            Die();
        }
        if (collision.gameObject.CompareTag("E3S"))
        {
            Die();
        }

    }

    private void ProcessHit(DamageDealer damageDealer)//calculates damage received
    {
        if(!damageDealer)
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
        shieldBar.SetShield(_health);
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

    private void Die()//player death
    {

        FindObjectOfType<GameSession>().PlayerDied(); //slow and costly function

        GameObject explosion = Instantiate(_deathVFX, transform.position, transform.rotation);//explosion vfx
        AudioSource.PlayClipAtPoint(_deathSFX, transform.position, _deathSFXVol);
        Destroy(explosion, _durationOfExplosion);
        this.gameObject.SetActive(false);
        lifeCounter.LoseLife();
        sr.color = defaultColor;
        speedBar.speedTime = -10f;
    }

    private void SetUpSingleton()
    {
        int numberGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numberGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            //DontDestroyOnLoad(gameObject);
        }
    }


    public void Speedtimer() // to indentify if the bool is true so that it can start the timer 
    {

        if(_speedBoostCR == null)
        {
            _speedBoostCR = StartCoroutine(SpeedboostDuration());   
        }
        else
        {
            StopCoroutine(_speedBoostCR);
            _speedBoostCR = StartCoroutine(SpeedboostDuration());
        }
        //if(Speedtime == true){   
        //}
    }

    IEnumerator SpeedboostDuration() // The timer that has the duration of the boost before turning the player back to it's orginal speed
    {
        //GetComponent<PlayerMovement>();
        PlayerTrailGreen.Play();
        PlayerTrail.Stop();
        speedBar.SetBarTimer();
        yield return new WaitForSeconds(10f);
        AudioSource.PlayClipAtPoint(_boostEndSFX, transform.position, _boostEndSFXVol);
        speed = 20f;
        //Speedtime = false;
        PlayerTrailGreen.Stop();
        PlayerTrail.Play();
        yield break;

    }


}

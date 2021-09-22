using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] Camera _Camera;

    PlayerInputActions _Input;
    Rigidbody2D _Rigidbody;
    public Bullet bulletPrefab;

    private Vector2 movement;
    private Vector2 _MousePos;

    private bool canShoot = true;
    private float _oldPos;

    [SerializeField] private float minTolerance;

    public int _health = 300;
    public GameObject _deathVFX;
    public float _durationOfExplosion = 2f;

    public float _shieldDuration = 1f;
    public float _shieldDelay = 1f;
    public Color _shieldColor;

    public AudioClip _deathSFX;
    public float _deathSFXVol = 0.7f;

    public AudioClip _bulletSFX;
    public float _bulletSFXVol = 0.7f;

    public AudioClip _shieldHitSFX;
    public float _shieldHitSFXVol = 0.7f;

    public ParticleSystem PlayerTrail;

    SpriteRenderer sr;
    Color defaultColor;

    private void Awake()
    {
        _Input = new PlayerInputActions();
        _Rigidbody = GetComponent<Rigidbody2D>();
        SetUpSingleton();
        _oldPos = transform.position.x;
        sr = GetComponent<SpriteRenderer>();
        defaultColor = sr.color;//saves default sprite color

    }


    void Update()
    {
        transform.localPosition += Vector3.right * speed * Time.deltaTime * movement.normalized.x;
        transform.localPosition += Vector3.up * speed * Time.deltaTime * movement.normalized.y;


    }

    private void FixedUpdate()
    {
        Vector2 facingDirection = _MousePos - _Rigidbody.position;
   
        if(Mathf.Abs(facingDirection.x) > minTolerance && Mathf.Abs(facingDirection.y) > minTolerance)
        {
            float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg - 90;
            _Rigidbody.MoveRotation(angle);
        }
        
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
        if (movement.magnitude == 0 && PlayerTrail.isPlaying)
        {
            PlayerTrail.Stop();
        }
    }

    private void OnMousePos(InputAction.CallbackContext context)
    {
        _MousePos = _Camera.ScreenToWorldPoint(context.ReadValue<Vector2>());
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!canShoot) return;

            Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position, this.transform.rotation); //spawns the bullet and gives it a position and rotation. Will spawn at the players positon and in the same rotation as the player.
            bullet.Project(this.transform.up); //projects in the same positon as the player
            AudioSource.PlayClipAtPoint(_bulletSFX, transform.position, _bulletSFXVol);
            StartCoroutine(CanShoot());
        }
    }

    IEnumerator CanShoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(0.35f);
        canShoot = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        ProcessHit(damageDealer);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Die();
        }
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

    private void Die()//player death
    {

        FindObjectOfType<GameSession>().PlayerDied(); //slow and costly function

        GameObject explosion = Instantiate(_deathVFX, transform.position, transform.rotation);//explosion vfx
        AudioSource.PlayClipAtPoint(_deathSFX, transform.position, _deathSFXVol);
        Destroy(explosion, _durationOfExplosion);
        this.gameObject.SetActive(false);
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroids : MonoBehaviour
{

    private int _scoreValue = 100;
    [SerializeField] private float _health = 500;

    GameSession gameSession;
    SpriteRenderer sr;
    Color defaultColor;

    public float _durationOfExplosion = 1f;
    public float _shieldDuration = 1f;
    public float _shieldDelay = 1f;
    public Color _shieldColor;

    public AudioClip _shieldHitSFX;
    public float _shieldHitSFXVol = 0.7f;

    public AudioClip _deathSFX;
    public float _deathSFXVol = 0.7f;

    public GameObject _deathVFX;

    private Rigidbody2D _rigidbody;


    private void Awake()
    {
        gameSession = GameObject.Find("Game Session").GetComponent<GameSession>();
        _rigidbody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        defaultColor = sr.color;//saves default sprite color
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
        Destroy(gameObject);
    }
}
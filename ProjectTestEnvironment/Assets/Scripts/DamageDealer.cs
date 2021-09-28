using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour

{

    [SerializeField] int _damage = 100;
    [SerializeField] AudioClip _ionBurstSFX;
    [SerializeField] float _ionBurstSFXVol;
    [SerializeField] ParticleSystem _burstVFX;

    public int GetDamage()
    {
        return _damage;
    }

    public void Hit()
    {
       Destroy(gameObject);
        if (gameObject.tag == "Bullet")
        {
           // var explosion = Instantiate(_burstVFX, transform.position, transform.rotation);//ion burst vfx
            AudioSource.PlayClipAtPoint(_ionBurstSFX, transform.position, _ionBurstSFXVol);
        }
    }
}

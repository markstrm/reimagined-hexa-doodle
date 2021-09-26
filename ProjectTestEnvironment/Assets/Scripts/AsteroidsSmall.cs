using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsSmall : MonoBehaviour
{
    private float _Rotation;
    public float _RotationSpeed;
    public bool _ClockwiseRotation;

    private Rigidbody2D _Rigidbody;


    private void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (_ClockwiseRotation == false)
        {
            _Rotation += Time.deltaTime * _RotationSpeed;
        }
        else
        {
            _Rotation += -Time.deltaTime * _RotationSpeed;
        }

        transform.rotation = Quaternion.Euler(0, 0, _Rotation);

    }
}

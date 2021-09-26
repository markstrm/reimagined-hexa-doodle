using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsSmall : MonoBehaviour
{
    private float _rotation;
    public float RotationSpeed;
    public bool ClockwiseRotation;

    private Rigidbody2D _Rigidbody;


    private void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (ClockwiseRotation == false)
        {
            _rotation += Time.deltaTime * RotationSpeed;
        }
        else
        {
            _rotation += -Time.deltaTime * RotationSpeed;
        }

        transform.rotation = Quaternion.Euler(0, 0, _rotation);

    }
}

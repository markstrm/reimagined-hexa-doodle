using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    [SerializeField] private float _maxRotSpeed;
    [SerializeField] private float _minRotSpeed;

    private float _speed;
    private bool _clockwiseRotation;

    // Start is called before the first frame update
    void Start()
    {
        _speed = Random.Range(_minRotSpeed, _maxRotSpeed);
       if(Random.Range(0,2) == 1)
        {
            _clockwiseRotation = true;
        }
        else
        {
            _clockwiseRotation = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_clockwiseRotation)
        {
          transform.Rotate(Vector3.forward * -_speed * Time.deltaTime);
        }
        else
        {
            transform.Rotate(Vector3.forward * _speed * Time.deltaTime);
        }

        
    }
}

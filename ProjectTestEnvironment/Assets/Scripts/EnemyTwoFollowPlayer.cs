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
    private void Start()
    {
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
            if (Time.time - _previousShootTime >= fireRate)
            {
                Shoot();
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

    private void Shoot()
    {
        // TODO Add Instantiate bullet
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



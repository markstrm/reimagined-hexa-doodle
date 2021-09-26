using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{

    public Transform target;
 
    void Update()
    {
        transform.position = new Vector2(target.position.x, transform.position.y); //minimap camera will follow the player
    }
}

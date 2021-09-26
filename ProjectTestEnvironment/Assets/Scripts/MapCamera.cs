using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{

    public Transform Target;
 
    void Update()
    {
        transform.position = new Vector2(Target.position.x, transform.position.y); //minimap camera will follow the player
    }
}

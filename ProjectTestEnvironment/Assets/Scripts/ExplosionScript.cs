using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    void Start()
    {
        Invoke("destroy", 1f);

    }

    void destroy()
    {
        Destroy(gameObject);
    }
}

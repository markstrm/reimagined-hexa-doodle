using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("destroy", 1f);

    }



    // Update is called once per frame
    void destroy()
    {
        Destroy(gameObject);
    }
}

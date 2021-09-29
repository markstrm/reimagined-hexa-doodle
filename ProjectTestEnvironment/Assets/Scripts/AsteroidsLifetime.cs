using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class AsteroidsLifetime : MonoBehaviour
{
    public float MaxLifetime = 30f;

    // Start is called before the first frame update
    private void Start()
    {
        Destroy(this.gameObject, this.MaxLifetime); //destroy the bullet after 10s
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    GameObject target;
    public float speed;
    Rigidbody2D bulletRB;

    // Start is called before the first frame update
    void Start()
    {
        bulletRB = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        Vector2 moveDir = (target.transform.position - transform.position).normalized;
        bulletRB.velocity = moveDir * speed;

        transform.eulerAngles = new Vector3(0, 0, Vector2ToAngle(moveDir) - 90f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static float Vector2ToAngle(Vector2 vect)
    {
        return Mathf.Atan2(vect.y, vect.x) * Mathf.Rad2Deg;
    }
}

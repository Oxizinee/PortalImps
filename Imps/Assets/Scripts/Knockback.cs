using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float Speed;
    public float Life = 10;
    private float _timer;
   
    private void Update()
    {
        _timer += Time.deltaTime;

        transform.position += transform.forward * Time.deltaTime * Speed;

        if (_timer >= Life)
        {
            Destroy(this.gameObject);
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Imp"))
    //    {
    //        Vector3 dir = collision.rigidbody.ClosestPointOnBounds(transform.position) - transform.position;
    //        dir = -dir.normalized;

    //            collision.transform.position -= dir * 12 * Time.deltaTime;
    //    }
    //}
}

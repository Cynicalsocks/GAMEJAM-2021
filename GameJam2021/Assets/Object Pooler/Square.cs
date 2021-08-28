//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Square : MonoBehaviour, IPooledObject
//{
//    public float upForce = 1f;
//    public float sideForce = .1f;

//    public void OnObjectSpawn()
//    {
//        float xForce = Random.Range(-sideForce, sideForce);
//        float yForce = Random.Range(upForce / 2f, upForce);

//        Vector2 force = new Vector2(xForce, yForce);

//        GetComponent<Rigidbody2D>().velocity = force;
//    }
//}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed;
    public Rigidbody2D myRigidbody;

    // Start is called before the first frame update
    // void Start()
    // {
    //     lifetimeCounter = lifetime;
    // }

    // private void Update()
    // {
    //     lifetimeCounter -= Time.deltaTime;
    //     if(lifetimeCounter <= 0)
    //     {
    //         Destroy(this.gameObject);
    //     }
    // }

    public void Setup(Vector2 velocity, Vector3  direction)
    {
        myRigidbody.velocity = velocity.normalized * speed;
        transform.rotation = Quaternion.Euler(direction);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("enemy"))
        {
            Destroy(this.gameObject);
        }
    }

}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody m_Rigidbody;
    public float maxSpeed = 6.0f;
    public float speedAdd = 0.01f;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private int hit = 0;
    private void OnCollisionExit(Collision other)
    {
        var velocity = m_Rigidbody.velocity;
        
        //check if we are not going totally vertically as this would lead to being stuck, we add a little vertical force
        var dir = Vector3.Dot(velocity.normalized, Vector3.up);
        if (dir < 0.1f || dir > 0.9f)
        {
            // velocity += velocity.y > 0 ? Vector3.up * 0.5f : Vector3.down * 0.5f;
            Vector3 add = Vector2.zero;
            add.x = Math.Sign(velocity.x) * 0.1f;
            add.y = Math.Sign(velocity.y) * 0.1f;
            velocity += add;
        }

        // if (!other.gameObject.CompareTag("Player"))
        //     return;
        
        if (++hit < 10)
            return;

        hit = 0;
        
        //after a collision we accelerate a bit
        velocity += velocity.normalized * speedAdd;
        
        //max velocity
        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }

        m_Rigidbody.velocity = velocity;
    }
}

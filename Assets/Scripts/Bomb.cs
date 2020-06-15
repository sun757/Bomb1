using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bomb : MonoBehaviour
{
    //[SerializeField]
    //MovingSphere movingSphere;
    public Vector3 desireVelocity;
    float duration = 0.5f;
    bool updateTimer;
    public float timer = 0.0f;
    private void Awake()
    {
        //movingSphere = GetComponentInParent<MovingSphere>();
        //desireVelocity = new Vector3(0,0,0);
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
    public void ThrowBomb(Vector3 desireVelocity)
    {
        Rigidbody body = this.GetComponent<Rigidbody>();
        Vector3 bombVelocity = body.velocity;
        bombVelocity.x = desireVelocity.x;
        bombVelocity.y = desireVelocity.y;
        bombVelocity.z = desireVelocity.z;
        body.velocity = bombVelocity;
    }
    private void Update()
    {
        if (updateTimer)
        {
            if (timer >= duration)
            {
                updateTimer = false;
                this.gameObject.SetActive(false);
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }
    private void FixedUpdate()
    {
    }
    void OnCollisionEnter(Collision collision)
    {
        updateTimer = true;
        //Debug.Log(Time.time);
    }


}

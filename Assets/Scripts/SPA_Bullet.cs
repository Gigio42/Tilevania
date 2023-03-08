using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPA_Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 20f;

    Rigidbody2D myRigidbody;
    SPA_PlayerMovement player;
    float xSpeed;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<SPA_PlayerMovement>();
        xSpeed = player.transform.localScale.x * bulletSpeed;
    }

    void Update()
    {
        myRigidbody.velocity = new Vector2 (xSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Enemy")
        {
            Destroy(other.gameObject);
        }      
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        Destroy(gameObject);   
    }
}

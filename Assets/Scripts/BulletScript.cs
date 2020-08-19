using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed;
    public float lifetime;
    public int killPoints;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        //Get the rigidbody component
        rb = GetComponent<Rigidbody>();

        //Life time of the bullet
        Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        //Move the bullet forward based on it's rotation
        rb.velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            //Gain points for killing enemy
            GameManager.instance.gameScore += killPoints;

            //Destroy the enemy
            Destroy(other.gameObject);

            //Destory itself
            Destroy(gameObject);

        }
    }
}

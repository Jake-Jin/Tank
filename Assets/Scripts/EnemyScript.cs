using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float speed;
    public float lifetime;
    public bool hasLife;
    public bool canMove;
    public bool isBoss;

    [Header("SHOOTING")]
    public float fireRate;
    public float duration;
    public float power;

    [Header("FOLLOWING")]
    public bool isFollowing;
    public float reverseTime;

    

    [Header("CONNECTIONS")]
    public Transform fp;
    public GameObject bullet;
    public Transform bulletSpawn;

    Rigidbody rb;
    Transform playerPos;
    bool canShoot = true;

    // Start is called before the first frame update
    void Start()
    {
        //Get out Rigidbody
        rb = GetComponent<Rigidbody>();

        //Get the player location
        playerPos = GameObject.Find("Player").transform;

        //If we have life then use the lifetime to destroy the enemy
        if(lifetime > 0)
            Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        //If the follow switch is active
        if(isFollowing && playerPos)
        {
            transform.LookAt(playerPos);
        }
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.forward * speed;

        //Shotting by pressing jump button on controller
        if (isBoss && canShoot)
        {
            StartCoroutine("Shoot");
        }
    }

    //If the enemy gets stuck
    IEnumerator Reverse()
    {
        //Stop moving forward
        canMove = false;

        //Half the current speed
        speed /= 2;

        //Start the timer from 0
        float t = 0;

        //While our timer is under our set reverse time
        while(t < reverseTime)
        {
            //This will move our enemy backward
            rb.velocity = -transform.forward * speed;

            //Increase t* time
            t += Time.deltaTime;

            //Wait 1 frame and run again
            yield return null;
        }

        //Set our speed back to it's original number
        speed *= 2;
        canMove = true;
    }

    IEnumerator Shoot()
    {
        // Making sure we can't fire until the firerate is over
        canShoot = false;

        /*---------------------------------------------------------------------------*/
        // Spawning the bullet into the scene based on the position of a bullet spawn object
       /*
        GameObject missile = Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
        float t = 0;
        while (t < duration)
        {
            Vector3 damping = new Vector3(0, missile.transform.position.y * 0.1f, 0);
            missile.transform.position = damping;
            t += Time.deltaTime;
        }
        */
        // Wait for the cooldown
        yield return new WaitForSeconds(fireRate);
        /*---------------------------------------------------------------------------*/
        
        // Now we can shoot again
        canShoot = true;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.gameScore -= 500;

            // Remove 1 life
            GameManager.instance.isAttacked = true;
            GameManager.instance.lives--;
            

            // Destory this object
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Barrier"))
        {
            //If a boss hits any barrier then destroy and reverse
            if (isBoss)
                Destroy(other.gameObject);
            StartCoroutine("Reverse");
        }

        // If the enemy collides with the shield destroy the enemy
        if (other.gameObject.CompareTag("Shield"))
        {
            Destroy(gameObject);
        }
    }
}

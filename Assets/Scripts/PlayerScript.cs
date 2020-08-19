//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("CAMERA")]
    public float offsetX;
    public float offsetZ;
    public float camSpeed;
    public float duration;
    public float magnitude;

    [Header("SPEEDS")]
    public float speed;         
    public float turnSpeed;     
    public float bodyLag;       

    [Header("SHOOTING")]
    public float fireRate;

    [Header("SHIELD")]
    public float shieldCD = 5;

    [Header("AUDIO")]
    public AudioClip bulletSFX;
    public AudioClip collisionSFX;

    [Header("CONNECTIONS")]
    public Transform playerCamera;
    public Transform fp;
    public Transform cannon;
    public Transform body;
    public GameObject bullet;
    public Transform bulletSpawn;
    public GameObject shield;

    AudioSource playerAudio;
    Rigidbody rb;
    bool canShoot = true;
    bool hasShield = false;

    //Not update yet
    Transform leftWheel;
    Transform rightWheel;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //Get the audio source component
        playerAudio = GetComponent<AudioSource>();

        //Get gameObject of child
        leftWheel = this.gameObject.transform.GetChild(1);
        rightWheel = this.gameObject.transform.GetChild(1);

        

        //Set the default rotation
        //m_rb.MoveRotation(Quaternion.Euler(new Vector3(0, -90, 0)));
    }

    void FixedUpdate()
    {
        // Get a forward input
        float vert = Input.GetAxis("Vertical");
        // Set up a movement speed based on input
        float moveSpeed = vert * speed;

        // Avoid flying
        Vector3 followXZ = new Vector3(fp.position.x, transform.position.y, fp.position.z);

        // Get a direction the player will go to
        Vector3 dir = followXZ - transform.position;

        // Get Vector to move
        Vector3 movement = new Vector3(dir.x * moveSpeed, rb.velocity.y, dir.z * moveSpeed);

        // Move the player

        rb.velocity = movement;
    }

    // Update is called once per frame
    void Update()
    {
        //Get inputs
        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        

        //Get the current turn speed based on input
        float rotY = hori * turnSpeed;

        //Rotate the cannon based on input
        if(hori != 0)
        {
            cannon.Rotate(new Vector3(0, rotY, 0) * Time.deltaTime);
        }

        // When pressing forward input
        if(vert != 0)
        {
            body.rotation = Quaternion.Lerp(body.rotation, cannon.rotation, bodyLag * Time.deltaTime);
        }

        //Shotting by pressing jump button on controller
        if(Input.GetButton("Jump") && canShoot)
        {
            StartCoroutine("Shoot");
        }

        //Camera movement with control by pushing and dragging mouse button
        CameraFollow();

        //Camera shakes when losing a life
        if (GameManager.instance.isAttacked)
        {
            StartCoroutine(CameraShake(duration, magnitude));
            GameManager.instance.isAttacked = false;
        }


        if (GameManager.instance.gameOver)
        {
            Destroy(gameObject);
        }
    }
    void CameraFollow()
    {
        //Make the camera follow the player only on the x and z direction
        playerCamera.transform.position = new Vector3(transform.position.x + offsetX, playerCamera.transform.position.y, transform.position.z + offsetZ);
        if(Input.GetMouseButton(1))
        {
            playerCamera.transform.Rotate(0, camSpeed * Input.GetAxis("Mouse X") * Time.deltaTime, 0);
        }
        
    }

    IEnumerator CameraShake(float duration, float magnitude)
    {
        Vector3 originalPos = playerCamera.transform.localPosition;
        float elapsed = 0.0f;

        //Play the collision sound effect
        playerAudio.PlayOneShot(collisionSFX);

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            playerCamera.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        playerCamera.localPosition = originalPos;
    }

    IEnumerator Shoot()
    {
        // Making sure we can't fire until the firerate is over
        canShoot = false;

        //Play the bullet sound effect
        playerAudio.PlayOneShot(bulletSFX);

        // Spawning the bullet into the scene based on the position of a bullet spawn object
        Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);

        // Wait for the cooldown
        yield return new WaitForSeconds(fireRate);

        // Now we can shoot again
        canShoot = true;
    }

    IEnumerator Shield()
    {
        // Tell the script we have a shield on
        hasShield = true;

        // Set the timer to start at 0
        float t = 0;

        // Spawn the shield and make sure it doesn't parent to the player
        //GameObject spawnShield = Instantiate(shield, transform);
        
        GameObject spawnShield = Instantiate(shield, transform.position, transform.rotation);
        //spawnShield.transform.parent = gameObject.transform;
        
        while (t < shieldCD)
        {
            // Updating to equal time
            t += Time.deltaTime;

            // Making sure the shield follows the player
            //Vector3 pos = transform.position + new Vector3(0, 1, 0);
            //spawnShield.transform.position = pos;
            spawnShield.transform.position = transform.position;

            // Run every
            yield return null;
        }

        Destroy(spawnShield);

        hasShield = false;
    }

    void OnTriggerEnter(Collider other)
    {
        // If we run into the shield pickup
        if (other.gameObject.CompareTag("ShieldPickup") && !hasShield)
        {
            // Activate the shield
            StartCoroutine("Shield");

            // Destroy the other object
            Destroy(other.gameObject);
        }
    }


}

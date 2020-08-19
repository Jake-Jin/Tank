using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject[] enemies;
    public float minT;
    public float maxT;
    public Transform[] spawners;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("SpawnEnemy");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemy()
    {
        while (!GameManager.instance.gameOver)
        {
            // choose randomly which spawner to use
            int sc = Random.Range(0, spawners.Length);
            // choose randomly the next spawn time
            float t = Random.Range(minT, maxT);
            // Choose randomly between the enemies based on percentage
            int ec = Random.Range(0, 101);

            //Spawn boss 10% and 1mins later
            if (ec < 10 && GameManager.instance.CountTime > 60)
            {
                // spawn an enemy to one of the spawners locations
                Instantiate(enemies[3], spawners[sc].position, spawners[sc].rotation);
                
            }
            //Spawn enemy1 40% of the time - spawn enemy2 30% of the time - enemy3 20%
            else if (ec >= 10 && ec < 50)   //40%
            {
                // spawn an enemy to one of the spawners locations
                Instantiate(enemies[2], spawners[sc].position, spawners[sc].rotation);
            }
            else if(ec >= 50 && ec < 80)    //30%
            {
                Instantiate(enemies[1], spawners[sc].position, spawners[sc].rotation);
            }
            else
            {
                // spawn an enemy to one of the spawners locations
                
                Instantiate(enemies[0], spawners[sc].position, spawners[sc].rotation);
            }

            // wait for a random time between the two number we chose
            yield return new WaitForSeconds(t);
        }
    }
}

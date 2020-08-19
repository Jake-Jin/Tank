using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    //General variables
    public bool gameOver = false;
    public int lives;
    public int gameScore = 0;
    public float CountTime = 0;
    public bool isAttacked = false;

    //Private variables
    int ogLives;
    int currentTime;
    


    [Header("STAGE1")]
    public int iBox;
    public int iMine;
    public int iBarrier;
    public int iShield;

    [Header("CONNECTIONS")]
    public Transform plane;
    public GameObject box;
    public GameObject mine;
    public GameObject barrier;

    void Awake()
    {
        //Check if there is a GameManager already set
        if(instance == null)
        {
            //Make this the game manager
            instance = this;
        }
        // If we load a new level and this is not the current instance of the game manager
        else if(instance != this)
        {
            //Destroy the new script
            Destroy(gameObject);
        }

        // Don't destroy this gameobject the script is attached to on load
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        SetBarriers();
    }

    // Update is called once per frame
    void Update()
    {
        //Check if our lives is above 0
        if (lives > 0)
        {
            //If so, not game over
            gameOver = false;
            
            //Count time
            CountTime += Time.deltaTime;
        }
        else
            //If not, game over
            gameOver = true;

        
    }

    void SetBarriers()
    {        
        for (int i=0; i<iBox; i++)
        {
            Vector3 planeSize = Vector3.Scale(plane.localScale, plane.GetComponent<MeshFilter>().mesh.bounds.size);
            Vector3 posSpawn = new Vector3(Random.Range((-planeSize.x / 2), (planeSize.x / 2)), 0, Random.Range((-planeSize.z / 2), (planeSize.z / 2)));
            Instantiate(box, posSpawn, box.transform.rotation);
        }
        for (int i = 0; i < iMine; i++)
        {
            Vector3 planeSize = Vector3.Scale(plane.localScale, plane.GetComponent<MeshFilter>().mesh.bounds.size);
            Vector3 posSpawn = new Vector3(Random.Range((-planeSize.x / 2), (planeSize.x / 2)), 0, Random.Range((-planeSize.z / 2), (planeSize.z / 2)));
            Instantiate(mine, posSpawn, mine.transform.rotation);
        }
        for (int i=0; i<iBarrier; i++)
        {
            Vector3 planeSize = Vector3.Scale(plane.localScale, plane.GetComponent<MeshFilter>().mesh.bounds.size);
            Vector3 posSpawn = new Vector3(Random.Range((-planeSize.x / 2), (planeSize.x / 2)), 0, Random.Range((-planeSize.z / 2), (planeSize.z / 2)));
            Instantiate(barrier, posSpawn, barrier.transform.rotation);
        }
    }
}

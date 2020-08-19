using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public Text score;
    public Text time;
    public Text gameOver;

    // Start is called before the first frame update
    void Start()
    {
        gameOver.transform.position = transform.position + new Vector3(0, 500, 0);
    }

    // Update is called once per frame
    void Update()
    {
        score.text = GameManager.instance.gameScore.ToString();

        //Time format
        string minutes = Mathf.Floor(GameManager.instance.CountTime / 60).ToString("00");
        string seconds = (GameManager.instance.CountTime % 60).ToString("00");
        time.text = string.Format("{0}:{1}", minutes, seconds);
        if(GameManager.instance.lives == 0)
            gameOver.transform.position = transform.position;
    }
}

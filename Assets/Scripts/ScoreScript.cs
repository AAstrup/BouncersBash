using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {

    private static ScoreScript scoreInstance;
    GameObject Score_red;
    GameObject Score_green;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);

        if (scoreInstance == null)
        {
            scoreInstance = this;
        }
        else
        {
            DestroyObject(gameObject);
        }

        Score_red = GameObject.Find("Score_red");
        Score_green = GameObject.Find("Score_green");
    }
    
    void Start () {
		Score_red.GetComponent<Text>().text = "0";
        Score_green.GetComponent<Text>().text = "0";
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour {

    float mapStartTime;
    public bool randomlyDestroyFloor;
    List<GameObject> gmjToFalling;
    GameObject[] gmjToFall;
    List<GameObject> gmjStillAlive;
    int amountGmjFound;
    public float fallSpeed = 3f;
    public float mapTimeInSeconds = 60f; 

    // Use this for initialization
    void Start () {
        mapStartTime = Time.time;
        gmjToFall = new GameObject[10];
        gmjToFalling = new List<GameObject>();
        gmjStillAlive = new List<GameObject>();
        for (int i = 0; i <= 9; i++)
        {
            var transform = base.transform.Find("Destructable" + i.ToString());
            if (transform != null)
            {
                gmjStillAlive.Add(transform.gameObject);
                gmjToFall[i] = transform.gameObject;
                amountGmjFound++;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        CheckForRandomGmjToDestroy();

        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown(i.ToString()) && gmjToFall[i] != null)
            {
                Destroy(i);
            }
        }
        foreach (var item in gmjToFalling)
        {
            item.transform.position = new Vector2(item.transform.position.x, item.transform.position.y - fallSpeed * Time.deltaTime);
        }
	}

    private void CheckForRandomGmjToDestroy()
    {
        if (!randomlyDestroyFloor || gmjStillAlive.Count == 0)
            return;
        float timePrFall = mapTimeInSeconds / amountGmjFound;
        if ((Time.time - mapStartTime) > (timePrFall * (amountGmjFound - gmjStillAlive.Count + 1)))//+1 such that a piece is not falling from the start
        {
            int index = UnityEngine.Random.Range(0, gmjStillAlive.Count);
            GameObject chosenGmjToDestroy = gmjStillAlive[index];
            gmjStillAlive.RemoveAt(index);
            gmjToFalling.Add(chosenGmjToDestroy);
        }
    }

    private void Destroy(int i)
    {
        if(!gmjToFalling.Contains(gmjToFall[i]))
            gmjToFalling.Add(gmjToFall[i]);
    }
}

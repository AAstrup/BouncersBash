using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour {

    public bool randomlyDestroyFloor;
    List<GameObject> gmjToFalling;
    GameObject[] gmjToFall;
    List<GameObject> gmjStillAlive;
    int amountGmjFound;
    public float fallSpeed = 3f;
    public float mapTimeInSeconds = 60f; 

    // Use this for initialization
    void Start () {
        gmjToFall = new GameObject[10];
        gmjToFalling = new List<GameObject>();
        gmjStillAlive = new List<GameObject>();
        for (int i = 0; i <= 9; i++)
        {
            var gmj = transform.Find("Destructable" + i.ToString()).gameObject;
            if (gmj != null)
            {
                gmjStillAlive.Add(gmj);
                gmjToFall[i] = gmj;
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
        if (!randomlyDestroyFloor)
            return;
        float timePrFall = amountGmjFound / mapTimeInSeconds;
        if (Time.time > (timePrFall * gmjStillAlive.Count))
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

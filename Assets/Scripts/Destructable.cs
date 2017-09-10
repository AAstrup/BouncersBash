using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour {

    float mapStartTime;
    public bool randomlyDestroyFloor;
    List<GameObject> gmjFalling;
    GameObject[] gmjToFall;
    List<GameObject> gmjStillAlive;
    int amountGmjFound;
    public float fallSpeed = 3f;
    public float mapTimeInSeconds = 60f;
    int? gmjNumberQueuedDestroyTarget = null;

    // Use this for initialization
    void Start () {
        mapStartTime = Time.time;
        gmjToFall = new GameObject[10];
        gmjFalling = new List<GameObject>();
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
                //Destroy(i); //Destroys immediately
                SetDestroyTarget(i); //Sets the next block to be destroyed
            }
        }
        foreach (var item in gmjFalling)
        {
            item.transform.position = new Vector2(item.transform.position.x, item.transform.position.y - fallSpeed * Time.deltaTime);
        }
	}

    private void SetDestroyTarget(int i)
    {
        if (!gmjFalling.Contains(gmjToFall[i]))
            gmjNumberQueuedDestroyTarget = i;
    }

    private void CheckForRandomGmjToDestroy()
    {
        if (!randomlyDestroyFloor || gmjStillAlive.Count == 0)
            return;
        float timePrFall = mapTimeInSeconds / amountGmjFound;
        if ((Time.time - mapStartTime) > (timePrFall * (amountGmjFound - gmjStillAlive.Count + 1)))//+1 such that a piece is not falling from the start
        {
            GameObject toDestroy;
            int index;
            if (gmjNumberQueuedDestroyTarget.HasValue) //Queued gmj will be destroyed
            {
                index = gmjNumberQueuedDestroyTarget.Value;
                toDestroy = gmjToFall[gmjNumberQueuedDestroyTarget.Value];
                gmjStillAlive.Remove(gmjToFall[index]);
                gmjNumberQueuedDestroyTarget = null;
            }
            else //Random gmj will be destroyed
            {
                index = UnityEngine.Random.Range(0, gmjStillAlive.Count);
                toDestroy = gmjStillAlive[index];
                gmjStillAlive.RemoveAt(index);
            }
            gmjFalling.Add(toDestroy);
        }
    }

    private void Destroy(int i)
    {
        if(!gmjFalling.Contains(gmjToFall[i]))
            gmjFalling.Add(gmjToFall[i]);
    }
}

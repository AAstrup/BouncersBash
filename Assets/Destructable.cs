using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour {

    List<GameObject> gmjToFall;
    public float fallSpeed = 3f;

    // Use this for initialization
    void Start () {
        gmjToFall = new List<GameObject>();

    }
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
                Destroy(i);
        }
        foreach (var item in gmjToFall)
        {
            item.transform.position = new Vector2(item.transform.position.x, item.transform.position.y - fallSpeed * Time.deltaTime);
        }
	}

    private void Destroy(int i)
    {
        Debug.Log("TESTING FOR " + i);
        if (transform.Find("Destructable" + i.ToString()) != null)
            if(!gmjToFall.Contains(transform.Find("Destructable" + i.ToString()).gameObject))
                gmjToFall.Add(transform.Find("Destructable" + i).gameObject);
    }
}

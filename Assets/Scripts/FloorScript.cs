using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorScript : MonoBehaviour {

    private Color originalColour;
    private float speed;
    private float floatSpan;
    private Vector2 originalPosition;

    public void Start()
    {
        originalColour = this.GetComponent<SpriteRenderer>().color;
        speed = 10f;
        floatSpan = 0.5f;
        originalPosition = transform.position;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        InvokeRepeating("Bounce", 0, 0.01f);
        Invoke("StopBouncing", 0.15f);
        StartCoroutine(ChangeColour());
    }
    
    public void Bounce()
    {
        gameObject.GetComponent<Transform>().position = new Vector2(originalPosition.x, originalPosition.y + Mathf.Sin(Time.time * speed) * floatSpan / 2.0f);
    }

    void StopBouncing()
    {
        CancelInvoke("Bounce");
        transform.position = originalPosition;
    }

    IEnumerator ChangeColour()
    {
        float h;
        float s;
        float v;
        Color.RGBToHSV(originalColour, out h, out s, out v);
        GetComponent<SpriteRenderer>().color = Color.HSVToRGB(h, s, v + 0.1f);
        yield return new WaitForSeconds(0.1f);
        this.GetComponent<SpriteRenderer>().color = originalColour;
    }
}

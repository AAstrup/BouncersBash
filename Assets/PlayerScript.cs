using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour {

    public string leftButton;
    public string rightButton;
    public bool continuousInput;
    public float continuousMoveSpeed = 0.5f;
    public float noncontinuousMoveSpeed = 2f;
    Vector2 velocityFromLastFrame;
    Rigidbody2D body;
    public float maxHorizontalSpeedInput = 4;
    public float minVerticalSpeed = 8;
    public float maxVerticalSpeed = 8;

    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody2D>();
    }

    private void UseNoncontinuousInput()
    {
        if (Input.GetKeyDown(rightButton))
        {
            if(body.velocity.x < maxHorizontalSpeedInput)
                body.velocity = new Vector2(body.velocity.x + noncontinuousMoveSpeed, body.velocity.y);
        }
        else if (Input.GetKeyDown(leftButton))
        {
            if(body.velocity.x > -maxHorizontalSpeedInput)
                body.velocity = new Vector2(body.velocity.x - noncontinuousMoveSpeed, body.velocity.y);
        }
    }

    private void UseContinuousInput()
    {
        if (Input.GetKey(rightButton))
        {
            if(body.velocity.x < maxHorizontalSpeedInput)
                body.velocity = new Vector2(body.velocity.x + continuousMoveSpeed, body.velocity.y);
        }
        else if (Input.GetKey(leftButton))
        {
            if(body.velocity.x > -maxHorizontalSpeedInput)
                body.velocity = new Vector2(body.velocity.x - continuousMoveSpeed, body.velocity.y);
        }
    }

    // Update is called once per frame
    void Update () {
        if (transform.position.y < -10f)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (continuousInput)
            UseContinuousInput();
        else
            UseNoncontinuousInput();
        velocityFromLastFrame = body.velocity;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Vector2 n = col.contacts[0].normal;
        Vector2 newDir = Vector2.Reflect(velocityFromLastFrame, n);
        Vector2 correctMomentum = newDir;
        if (correctMomentum.y > maxVerticalSpeed)
            correctMomentum.y = maxVerticalSpeed;

        if (col.contacts[0].point.y < transform.position.y)
            correctMomentum.y = minVerticalSpeed;

        body.velocity = correctMomentum;
    }

    public Vector2 Rotate(Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }
}

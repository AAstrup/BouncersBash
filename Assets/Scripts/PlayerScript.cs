using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

    public float playerHitEachOtherImpact = 1.5f;
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

    Vector3 originalCameraPosition;
    float shakeAmt = 0;
    public Camera mainCamera;

    private AudioSource audioSource;
    public AudioClip groundBounceSound;
    public AudioClip playerBounceSound;
    public AudioClip deathSound;

    void Start () {
        originalCameraPosition = mainCamera.transform.position;
        body = GetComponent<Rigidbody2D>();
        audioSource = GameObject.Find("Score").GetComponent<AudioSource>();
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
    
    void Update() {
        if (transform.position.y < -18f) {
            if (this.name.Equals("Player_red"))
            {
                Text score = GameObject.Find("Score_green").GetComponent<Text>();
                score.text = (int.Parse(score.text) + 1).ToString();
            }
            else if (this.name.Equals("Player_green"))
            {
                Text score = GameObject.Find("Score_red").GetComponent<Text>();
                score.text = (int.Parse(score.text)+1).ToString();
            }

            audioSource.PlayOneShot(deathSound, 1f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (continuousInput)
            UseContinuousInput();
        else
            UseNoncontinuousInput();
        velocityFromLastFrame = body.velocity;
    }

    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.tag.Equals("Player"))
        {
            audioSource.PlayOneShot(playerBounceSound, 1f);
            PlayerCollision(col);
            return;
        } else
        {
            audioSource.PlayOneShot(groundBounceSound, 1f);
        }
        Vector2 n = col.contacts[0].normal;
        Vector2 newDir = Vector2.Reflect(velocityFromLastFrame, n);
        Vector2 correctMomentum = newDir;
        if (correctMomentum.y > maxVerticalSpeed)
            correctMomentum.y = maxVerticalSpeed;

        if (col.contacts[0].point.y < transform.position.y)
            correctMomentum.y = minVerticalSpeed;

        body.velocity = correctMomentum;
    }

    float lastPlayerCollisionTime;
    private void PlayerCollision(Collision2D col)
    {
        if (transform.position.y < col.gameObject.transform.position.y)
            return;
        if (Time.time < (lastPlayerCollisionTime + 0.1f))
            return;

        var otherScript = col.gameObject.GetComponent<PlayerScript>();
        lastPlayerCollisionTime = Time.time;
        otherScript.UpdateLastPlayerCollisionTime();

        var otherVel = otherScript.GetVelocity() * playerHitEachOtherImpact;
        otherScript.SetVelocity(velocityFromLastFrame * playerHitEachOtherImpact);
        body.velocity = otherVel;
        
        shakeAmt = col.relativeVelocity.magnitude * .01f;
        InvokeRepeating("CameraShake", 0, .01f);
        Invoke("StopShaking", 0.15f);
    }

    private void UpdateLastPlayerCollisionTime()
    {
        lastPlayerCollisionTime = Time.time;
    }

    private void SetVelocity(Vector2 newVel)
    {
        body.velocity = newVel;
    }

    private Vector2 GetVelocity()
    {
        return velocityFromLastFrame;
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

    void CameraShake()
    {
        if (shakeAmt > 0)
        {
            float quakeAmtY = UnityEngine.Random.value * shakeAmt * 2 - shakeAmt;
            float quakeAmtX = UnityEngine.Random.value * shakeAmt * 2 - shakeAmt;
            Vector3 pp = mainCamera.transform.position;
            pp.y += quakeAmtY;
            pp.x += quakeAmtX;
            mainCamera.transform.position = pp;
        }
    }

    void StopShaking()
    {
        CancelInvoke("CameraShake");
        mainCamera.transform.position = originalCameraPosition;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class car_script : MonoBehaviour
{
    [Header("Object References")]
    public Transform wall;
    public GameObject explosion;
    public TMP_Text timeTakeToHit;

    [Header("Kinematic Variable Text")]
    public TMP_Text timeTilCollision;
    public TMP_Text displacementText;
    public TMP_Text velocityText;

    [Header("Kinematic Variables")]
    public float movement_speed_per_second = 5.0f;
    public float s;
    public float v;
    public float t;
    public bool isRunning;

    bool timeCheck1;
    bool timeCheck2;
    float startTime;
    float endTime;
    float totalTime;
    Vector3 startPos;


    void Start()
    {
        explosion.SetActive(false);
        isRunning = false;
        timeCheck1 = true;
        timeCheck2 = true;
        startPos = transform.position;

        // "HOW LONG UNTIL CAR HITS WALL?"
        //Set-up for "SUVAT" Kinematic Equation
        // S = ((U + V) / 2)T

        // Vector from car to wall, this is our displacement (S) variable
        Vector3 carToWall = wall.position - transform.position;
        s = carToWall.magnitude;

        // U + V is one in the same as there is no acceleration therefore velocity will stay the same for initial & final
        v = movement_speed_per_second;

        // now solve for T.
        // "S = ((U + V) / 2)T" becomes "T = S / (((U + V) / 2))"
        t = s / ((v + v) / 2);

        // how to round to float
        //source code: https://forum.unity.com/threads/how-to-round-a-float.315809/
        timeTilCollision.text = "Estimated time until collision: " + Mathf.Round(t * 10.0f) * 0.1f + " seconds";
        displacementText.text = "Displacement (S): " + Mathf.Round(s * 10.0f) * 0.1f;
        velocityText.text = "Velocity (V & U): " + v;
        print(t); // print to console
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) isRunning = true;

        if (isRunning)
        {
            if (timeCheck1)
            {
                startTime = Time.time;
                timeCheck1 = false;
            }

            Vector3 carToWall = wall.position - transform.position; // track vector length
            Vector3 velocityPerSecond = carToWall.normalized * movement_speed_per_second; // determine speed in which to move towards
            Vector3 velocityThisFrame = velocityPerSecond * Time.deltaTime; // speed over frame

            // if car hits wall
            if (carToWall.magnitude < velocityThisFrame.magnitude)
            {
                transform.position = wall.position;
                explosion.SetActive(true);

                if (timeCheck2)
                {
                    endTime = Time.time;
                    timeCheck2 = false;
                }

                totalTime = endTime - startTime;
                timeTakeToHit.text = "Time Taken: " + Mathf.Round(totalTime * 10.0f) * 0.1f + " seconds";
                print(totalTime);
            }
            else
            {
                transform.position += velocityThisFrame; // Move towards wall
            }
        }

        //RESET
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = startPos;
            timeCheck1 = true;
            timeCheck2 = true;
            startTime = 0;
            endTime = 0;
            totalTime = 0;
            isRunning = false;
            explosion.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    [Header("Component References")]
    public Rigidbody2D rb;
    public GameObject finishLine;

    [Header("SUVAT Variables")]
    public float acceleration; // a
    public float initialVelocity; // u
    public float finalVelocity; // v
    public float tTime; // t

    [Header("Forces")]
    Vector3 iVelocity = new Vector3(0,0,0);
    Vector3 fVelocity =new Vector3(0,0,0);
    Vector3 accel =new Vector3(0,0,0);

    [Header("Time & Force Variables")]
    public float levelTime;
    public float timeTaken;
    public float currentVelocity;

    [Header("Positions")]
    public Vector3 startPosition;
    public Vector3 endPosition;

    public bool isRunning;
    public bool checkTime;
    public float timeStart;
    public float timeEnd;

    void Start()
    {

        // Set initial velocity variables
        iVelocity = new Vector3(initialVelocity, 0, 0);
        currentVelocity = acceleration;

        // set start position
        startPosition = transform.position;

        isRunning = false;
        checkTime = false;
    }

    void Update()
    {
        // Track Time
        levelTime = Time.time;

        //Intial update Finish line S = ((U + V) / 2)T
        finishLine.transform.position = new Vector3((((initialVelocity + finalVelocity) / 2) * tTime) + startPosition.x, // needs to be displaced
                                                    finishLine.transform.position.y,
                                                    finishLine.transform.position.x);
        // start simulation
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isRunning = true;
        }

        if (isRunning)
        {
            // Calculate V = U + AT
            finalVelocity = initialVelocity + (acceleration * tTime);

            if (currentVelocity < finalVelocity)
            {
                //START moving object
                if (transform.position.x < -10)
                {
                    //Move object with inital velocty 
                    transform.position += iVelocity * Time.deltaTime;
                }
                else // once hits tracking border
                {
                    //acceleration A = (V - U) / T
                    acceleration = (finalVelocity - initialVelocity) / tTime;

                    //calculate acceleration over time
                    currentVelocity += acceleration * Time.deltaTime;
                    accel = new Vector3((currentVelocity), 0, 0);

                    //Apply acceleration
                    transform.position += accel * Time.deltaTime;
                }

                // TRACK TIME BETWEEN START AND FINISH
                if (transform.position.x > -10)
                {
                    // Track Time
                    if (!checkTime)
                    {
                        //check time when bullet hits startline.
                        timeStart = Time.time;
                        checkTime = true;
                    }

                }

                // update final position & final time
                endPosition = transform.position;
                timeEnd = Time.time;
            }
            else
            {
                isRunning = false;
                // update time taken
                timeTaken = timeEnd - timeStart;
            }
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = startPosition;
            currentVelocity = initialVelocity; 
            levelTime = 0;
            timeTaken = 0;
            finalVelocity = 0;
            isRunning = false;
            checkTime = false;
        }
    }
}

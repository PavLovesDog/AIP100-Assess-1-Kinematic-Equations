using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Car : MonoBehaviour
{
    [Header("Component References")]
    public Transform startSkid;
    public GameObject brakeText;
    public TMP_Text dTravelled;
    public TMP_Text edTravelled;
    public TMP_Text acellText;
    public TMP_Text iVText;
    public TMP_Text fVText;
    public TMP_Text CurrVelText;

    [Header("SUVAT Variables")]
    public float decceleration; // A
    public float initialVelocity; // U
    public float finalVelocity; // V

    [Header("Forces")]
    Vector3 iVelocity = new Vector3(0, 0, 0);
    Vector3 fVelocity = new Vector3(0, 0, 0);
    Vector3 deccel = new Vector3(0, 0, 0);

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
    public float estimatedDistance;

    //tracking distance Variables
    Vector3 lastPos;
    public float distanceTravelled;

    void Start()
    {
        brakeText.SetActive(false);

        // Set initial velocity variables
        iVelocity = new Vector3(initialVelocity, 0, 0);
        currentVelocity = initialVelocity;

        // set start position
        startPosition = transform.position;
        lastPos = startSkid.position; // for travel
        isRunning = false;
        checkTime = false;

        //HOW FAR WILL THE CAR SLIDE TO A STOP?
        // Using Kinematic E: V^2 = U^2 + 2AS
        float A = decceleration;
        float U = initialVelocity;
        float V = 0; // as we know the car will come to a stop

        //V^2 drops. Then we can rearrange the formula to:
        // S = (-U^2) / (2 * A)
        float S = (U * -U) / (2 * A);

        print(S);
        estimatedDistance = S;
    }

    void Update()
    {
        HandleDisplay();

        // Track Time
        levelTime = Time.time;


        // start simulation
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isRunning = true;
        }

        if (isRunning)
        {
            //If car is still breaking
            if (currentVelocity > finalVelocity)
            {
                //START moving object
                if (transform.position.x < startSkid.position.x)
                {
                    //Move object with inital velocty 
                    transform.position += iVelocity * Time.deltaTime;
                }
                else // once hits tracking border
                {
                    //announce brake
                    brakeText.SetActive(true);

                    //Track distance travelled
                    float distance = Vector3.Distance(lastPos, transform.position);
                    distanceTravelled += distance;
                    lastPos = transform.position;

                    //update current velocity
                    currentVelocity += decceleration * Time.deltaTime;

                    //calculate decceleration over time
                    deccel = new Vector3((currentVelocity), 0, 0);

                    //Apply decceleration
                    transform.position += deccel * Time.deltaTime;
                }

                // TRACK TIME BETWEEN START AND FINISH
                if (transform.position.x > startSkid.position.x)
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
            lastPos = startSkid.position;
            distanceTravelled = 0;
            finalVelocity = 0;
            isRunning = false;
            checkTime = false;
            brakeText.SetActive(false);
        }
    }

    void HandleDisplay()
    {
        acellText.text = "Acceleration (A): " + decceleration;
        iVText.text = "Initial Velocity (U): " + initialVelocity;
        fVText.text = "Final Velocity (V): " + finalVelocity;

        dTravelled.text = "Distance Travelled: " + Mathf.Round(distanceTravelled * 10.0f) * 0.1f;
        edTravelled.text = "Estimated Distance (S): " + Mathf.Round(estimatedDistance * 10.0f) * 0.1f;
        CurrVelText.text = "Current Velocity: " + Mathf.Round(currentVelocity * 10.0f) * 0.1f;
    }
}
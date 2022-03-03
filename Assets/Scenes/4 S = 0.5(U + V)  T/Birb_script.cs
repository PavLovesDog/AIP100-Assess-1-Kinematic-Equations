using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Birb_script : MonoBehaviour
{
    [Header("Object References")]
    public Transform landPoint;
    public GameObject arriveText;
    public TMP_Text dTravelled;
    public TMP_Text dRemaining;
    public TMP_Text iV;
    public TMP_Text fV;
    public TMP_Text cV;
    public TMP_Text timeV;
    public TMP_Text estDistance;
    public TMP_Text timeTaken;

    public float movement_speed_per_second;
    public float acceleration_magnitude_per_second;

    // Vectors for VELOCITY & ACCELERATION
    Vector3 acceleration_per_second = Vector3.zero;
    Vector3 velocity_per_second = Vector3.zero;
    Vector3 lastPos;

[Header("Kinematic Variables")]
    public float T; // Time
    public float U; // Initial Velocity
    public float V; // Final Velocity
    public float A; // Acceleration
    public float S; // Displacement

    [Header("Display Variables")]
    public float currentVelocity;
    public float trackDistance;
    public float distanceTravelled;
    public float time_taken;

    // Start is called before the first frame update
    void Start()
    {
        arriveText.SetActive(false);

        // vector from Bird to Land & start/end positions
        Vector3 Bird_to_Land = landPoint.position - transform.position;
        lastPos = transform.position;

        // Initialise velocity * acceleration per second
        acceleration_per_second = Bird_to_Land.normalized * acceleration_magnitude_per_second; // normalised reduces the vector magnitude to max 1
        velocity_per_second = Bird_to_Land.normalized * movement_speed_per_second;
        

        //HOW FAR MUST BIRD FLY FROM ONE LAND TO ANOTHER LAND ?
        // Set-up evaluation of Kinematic Equation
        // Use: S = 1/2(U + V) * T
        // set variables
        U = velocity_per_second.magnitude; // U (initial velocity)
        A = acceleration_per_second.magnitude; // A

        currentVelocity = U;
        //Find Final Velocity with V = U + AT
        V = U + A * T;

        // solve for displacement
        // S = 1/2(U + V) * T
        S = 0.5f * (U + V) * T;

        print(S);
    }


    void Update()
    {
        // Display text & Timers
        dRemaining.text = "Distance Remaining: " + trackDistance;
        dTravelled.text = "Distance Travelled: " + Mathf.Round(distanceTravelled * 10.0f) * 0.1f;
        iV.text = "Initial Velocity (U): " + U;
        fV.text = "Final Velocity (V): " + Mathf.Round(V * 10.0f) * 0.1f;
        timeV.text = "Time (T): " + Mathf.Round(T * 10.0f) * 0.1f;
        estDistance.text = "Estimated Distance To Travel " + Mathf.Round(S * 10.0f) * 0.1f;
        cV.text = "Current Velocity: " + Mathf.Round(currentVelocity * 10.0f) * 0.1f;

        if (transform.position.x < landPoint.position.x)
        {
            time_taken = Time.time;
        }
        
        timeTaken.text = "Time Taken: " + Mathf.Round(time_taken * 10.0f) * 0.1f + " seconds";

        //Track and note distance travelled
        // Code sourced: https://answers.unity.com/questions/1494589/how-to-measure-the-overall-distance-travelled.html
        float distance = Vector3.Distance(lastPos, transform.position);
        distanceTravelled += distance;
        lastPos = transform.position;
        trackDistance = landPoint.position.x - transform.position.x; // count down

        

        //Update velocity with acceleration over time
        velocity_per_second += acceleration_per_second * Time.deltaTime;

        // constantly track the vector between bird and land
        Vector3 Bird_to_Land = landPoint.position - transform.position;
        Vector3 velocity_this_frame = velocity_per_second * Time.deltaTime;

        // if the bird has reached the land
        if (Bird_to_Land.magnitude < velocity_this_frame.magnitude)
        {
            transform.position = landPoint.position;
            print(Time.time);
            arriveText.SetActive(true);
            //Debug.Break(); // Pause to editor.
        }
        else
        {
            //update position / Move Bird
            transform.position += velocity_this_frame;
            //track velocity
            currentVelocity += A * Time.deltaTime;
        }
    }
}
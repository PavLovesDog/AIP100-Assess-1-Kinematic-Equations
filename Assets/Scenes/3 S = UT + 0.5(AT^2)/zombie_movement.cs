using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class zombie_movement : MonoBehaviour
{
    public Transform player;
    public TMP_Text ZombieETA;
    public TMP_Text EstimatedTime;
    public TMP_Text TimeTaken;
    public TMP_Text LevelTime;
    public GameObject deathText;

    public float movement_speed_per_second = 2.0f;
    public float acceleration_magnitude_per_second = 0.2f;

    // Vectors for VELOCITY & ACCELERATION
    Vector3 acceleration_per_second = Vector3.zero;
    Vector3 velocity_per_second = Vector3.zero;
    float time_to_intercept;
    float level_time;
    float time_taken;

    // Start is called before the first frame update
    void Start()
    {
        deathText.SetActive(false);

        // vector from zombie to player
        Vector3 Z_to_P = player.position - transform.position;

        // Initialise velocity * acceleration per second
        acceleration_per_second = Z_to_P.normalized * acceleration_magnitude_per_second; // normalised reduces the vector magnitude to max 1
        velocity_per_second = Z_to_P.normalized * movement_speed_per_second;

        //HOW LONG UNTIL ZOMBIE REACHES VICTIM?
        // Set-up evaluation of Kinematic Equation
        // known SUVAT variables for: S = UT + 1/2(AT^2)
        float dis = Z_to_P.magnitude; // S
        float vel = velocity_per_second.magnitude; // U (initial velocity)
        float acc = acceleration_per_second.magnitude; // A

        // "S = UT + 1/2(AT^2)" can rearrange to "0 = UT + 1/2(AT^2) - S" :: 0.5AT^2 + UT - S = 0 (ax^2 + bx +c = 0)
        // to form a quadratic.
        // we can then use Quadratic Equation to solve with known variables
        

        //0.5AT^2 + UT - S = 0
        //ax^2    + bx + c = 0
        //acceleration = a, initial velocity = b, displacement = c
        float a = acc * 0.5f;
        float b = vel;
        float c = -dis;

        // put known variables into Quadratic formula
        //QF = x = (-b +- sqrt(b^2 - 4(ac)) / 2a
        float t0 = (-b + Mathf.Sqrt((b * b) - (4 * a * c))) / (2 * a);
        float t1 = (-b - Mathf.Sqrt((b * b) - (4 * a * c))) / (2 * a);

        if (t0 < 0.0f) // if first possible point is negative
            time_to_intercept = t1; // the other must be our time
        else if (t1 < 0.0f)
            time_to_intercept = t0;
        else
            time_to_intercept = t0 < t1 ? t0 : t1; // if t0 is < t1 then(?) i'll take t0, otherwise(:) t1;

        print(time_to_intercept); // to console

        // display numbers on screen!
        EstimatedTime.text = "Estimated ETA: " + Mathf.Round(time_to_intercept * 10.0f) * 0.1f + " seconds";
    }

    
    void Update()
    {
        // Display text & Timers
        level_time = Time.time;
        LevelTime.text = "Scene Time " + Mathf.Round(level_time * 10.0f) * 0.1f + " seconds";

        float ETA = 0.0f;
        if (transform.position != player.transform.position)
        {
            ETA = time_to_intercept - Time.time;
        }
        else deathText.SetActive(true);

        ZombieETA.text = "Zombie gets fed in: " + Mathf.Round(ETA * 10.0f) * 0.1f + " seconds";


        if (level_time <= time_to_intercept)
        {
            time_taken = Time.time;
        }

        TimeTaken.text = "Time Taken: " + Mathf.Round(time_taken * 10.0f) * 0.1f + " seconds";

        //Update velocity with acceleration over time
        velocity_per_second += acceleration_per_second * Time.deltaTime;

        // constantly track the vector between zombie and player
        Vector3 Z_to_P = player.position - transform.position;
        Vector3 velocity_this_frame = velocity_per_second * Time.deltaTime;

        // if the zombie has reached the player
        if (Z_to_P.magnitude < velocity_this_frame.magnitude)
        {
            transform.position = player.position;
            print(Time.time);
            //Debug.Break(); // Pause to editor.
        }
        else
        {
            transform.position += velocity_this_frame;
        }
    }
}

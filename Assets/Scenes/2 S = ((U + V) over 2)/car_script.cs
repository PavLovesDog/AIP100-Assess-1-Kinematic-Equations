using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class car_script : MonoBehaviour
{
    public Transform wall;
    public GameObject explosion;
    public TMP_Text timeTilCollision;
    public TMP_Text timeTakeToHit;

    public float movement_speed_per_second = 5.0f;

    void Start()
    {
        explosion.SetActive(false);

        // "HOW LONG UNTIL CAR HITS WALL?"
        //Set-up for "SUVAT" Kinematic Equation
        // S = ((U + V) / 2)T

        // Vector from car to wall, this is our displacement (S) variable
        Vector3 carToWall = wall.position - transform.position;
        float s = carToWall.magnitude;

        // U + V is one in the same as there is no acceleration therefore velocity will stay the same for initial & final
        float v = movement_speed_per_second;

        // now solve for T.
        // "S = ((U + V) / 2)T" becomes "T = S / (((U + V) / 2))"
        float t = s / ((v + v) / 2);

        timeTilCollision.text = "Estimated time until collision: " + t;
        print(t); // print to console
    }

    void Update()
    {
        Vector3 carToWall = wall.position - transform.position; // track vector length
        Vector3 velocityPerSecond = carToWall.normalized * movement_speed_per_second; // determine speed in which to move towards
        Vector3 velocityThisFrame = velocityPerSecond * Time.deltaTime; // speed over frame

        // if car hits wall
        if (carToWall.magnitude < velocityThisFrame.magnitude)
        {
            transform.position = wall.position;
            explosion.SetActive(true);
            timeTakeToHit.text = "Time Taken: " + Time.time;
            print(Time.time);
            //Debug.Break(); // Pause to editor
        }
        else
        {
            transform.position += velocityThisFrame; // Move towards wall
        }
    }
}

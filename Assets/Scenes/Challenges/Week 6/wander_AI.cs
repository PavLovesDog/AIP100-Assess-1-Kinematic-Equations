using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wander_AI : MonoBehaviour
{
    // Boundry Variables
    float boundry_X = 4.5f;
    float boundry_Y = 4.5f;

    Vector3 velocity_per_second = Vector3.zero;
    Vector3 wander_direction = Vector3.zero; // delcare
    float time_until_random_changes = 0.0f;
    public float time_between_random_change = 5.0f;
    public float speed_per_second = 1.0f;
    public float wander_clamp = 1.0f;
    public float steering_speed = 1.0f;
    public bool canMove = true;

    public Transform wanderCircle;


    void Start()
    {
        canMove = true;

        /*SET A RANDOM POSITION FOR WANDER CIRCLE*/

        // generate random position on circles radius using MATHS
        // get radius of wander circle
        float radius = wanderCircle.localScale.x / 2;

        // generate random arc length number of circle (can be 0 - 90)
        float arcLength = Random.Range(0, 90); // limit total direction it can choose

        //use arc length and radius to find/create an angle in radians (Arclength = Radius * Angle)
        //float randAngle = arcLength / (radius * 20 - 0.25f); // adjust radius so radian number cannot exceed 2! CLAMP HERE with 
        float randomAngle = arcLength / (radius * 7 + 1); // 360 RANGE HERE radian number cannot exceed 6!
        print("Angle in Radians = " + randomAngle);

        // convert radians to degrees
        float degreeAngle = Mathf.Rad2Deg * randomAngle;
        print("Angle in Degrees = " + degreeAngle);

        //return sine of angle
        print("X: " + Mathf.Sin(degreeAngle));
        print("Y: " + Mathf.Cos(degreeAngle));
        // Use these -1.0 to 1.0 values for rand direction
        float x_Dir = Mathf.Sin(degreeAngle);
        float y_Dir = Mathf.Cos(degreeAngle);

        /* All above can be substituted for below code.... 
         * BUT, it has helped me understand ways in which to use angle and degrees 
         * within game programming better. So I have chosen to keep it here for
         * better memory retention.
         */

        //x = Random.Range(-1.0f, 1.0f);
        //y = Random.Range(-1.0f, 1.0f);

        // use sine and cosine to generate random position vector
        Vector3 randomWanderDirection = new Vector3(x_Dir, y_Dir, 0.0f);
        randomWanderDirection.Normalize();

        //reposition wander circle in front of wanderer to new desired direction
        wanderCircle.transform.position = transform.position + randomWanderDirection * 2.5f;
    }

    void Update()
    {

        time_until_random_changes -= Time.deltaTime;
        if (time_until_random_changes <= 0.0f)
        {
            //Get some random values for slight direction change
            float x_change = Random.Range(wander_direction.x - wander_clamp, wander_direction.x + wander_clamp);
            float y_change = Random.Range(wander_direction.y - wander_clamp, wander_direction.y + wander_clamp);
            Vector3 direction_adjustment = new Vector3(wander_direction.x + x_change, wander_direction.y + y_change); // add values to current direction vector into new vector
            direction_adjustment.Normalize(); // normalize for direction

            // set wander circle at a new location based on its current (plus or minus a few degrees), to give natural wander effect
            wanderCircle.transform.position = transform.position + direction_adjustment * 2.0f; // multiply to position circle ahead of wanderer

            time_between_random_change = Random.Range(1, 4);
            time_until_random_changes = time_between_random_change;

        }

        // check if wander cirlce is within bounds                       // bring it back within border
        if (wanderCircle.position.x > boundry_X) wanderCircle.position = new Vector3(boundry_X - 1, wanderCircle.position.y, wanderCircle.position.z); // RIGHT WALL
        if (wanderCircle.position.x < -boundry_X) wanderCircle.position = new Vector3(-boundry_X + 1, wanderCircle.position.y, wanderCircle.position.z); // LEFT WALL
        if (wanderCircle.position.y > boundry_Y) wanderCircle.position = new Vector3(wanderCircle.position.x, boundry_Y - 1, wanderCircle.position.z); // TOP WALL
        if (wanderCircle.position.y < -boundry_Y) wanderCircle.position = new Vector3(wanderCircle.position.x, -boundry_Y + 1, wanderCircle.position.z); // BOTTOM WALL

        // Get desired wander direction from NEW position of wander circle
        wander_direction = wanderCircle.position - transform.position; // get direction to go
        wander_direction.Normalize(); // normalize it
        wander_direction *= speed_per_second; // add speed

        // Compute Steering Vector For This Frame (steering vector to add to direction to smoothly turn actor)
        Vector3 steering_vector = (wander_direction - velocity_per_second).normalized * steering_speed;
        Vector3 steering_vector_pf = steering_vector * Time.deltaTime;

        // Determine New Velocity Direction with Added Steering (add steering vector to current vector)
        Vector3 steering_and_current = velocity_per_second + steering_vector_pf;
        steering_and_current.Normalize(); // get direction for added vector

        // Determine New Velocity With Speed
        velocity_per_second = steering_and_current * speed_per_second;

        transform.position += velocity_per_second * Time.deltaTime;

    }
}

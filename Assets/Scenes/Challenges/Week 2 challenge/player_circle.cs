using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_circle : MonoBehaviour
{

    [SerializeField] float gravityMagnitude = -0.5f;
    [SerializeField] float impulseForce_X = 0.0f;
    [SerializeField] float impulseForce_Y = 0.0f;
    [SerializeField] float mass;
    public float friction;
    public float decelleration;

    Vector3 gravityPerSecond; // Gravity force constantly tries to push things down
    public Vector3 VelocityPerSecond = new Vector3(0, 0, 0); // velocity
    Vector3 jumpImpulseForce;    //impulse for jump
    Vector3 startingPos;

    void Start()
    {
        //get starting position
        startingPos = transform.position;
    }

    void Update()
    {
        // set gravity & jump impulse
        gravityPerSecond = new Vector3(0, gravityMagnitude, 0); /* gravity applied to velocity, in the -y direction. 
                                                                          * NOTE REMOVED: Multiply the gravitational force by a fraction of mass.
                                                                          * The heavier an object is, the harsher effects of gravity*/
        jumpImpulseForce = new Vector3((impulseForce_X / mass), (impulseForce_Y / mass), 0.0f); // add mass varibale to affect impulse
        /*ABOVE F = M / A kinetic formula applied to jump 'Force'*/

        // Jump!
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Apply jump impulse!
            VelocityPerSecond += jumpImpulseForce;
        }

        //Reset position for debugging
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = startingPos;
            VelocityPerSecond.y = 0.0f;
            VelocityPerSecond.x = 0.0f;
        }

        // apply gravity to the velocity
        VelocityPerSecond += gravityPerSecond * Time.deltaTime;

        //update position by coded velocity
        transform.position += VelocityPerSecond * Time.deltaTime;

        // if object gets below the "floor"
        if (transform.position.y < 0)
        {
            // re-set position to above floor
            transform.position = new Vector3(transform.position.x, 0.0f, 0.0f);

            // stop velocity build up on floor
            VelocityPerSecond.y = 0.0f;

            // simulate friction to slow down object
            if (VelocityPerSecond.x > 0)
            {
                VelocityPerSecond.x -= Time.deltaTime;
                friction = VelocityPerSecond.x / mass; // friction = force / mass
                decelleration = VelocityPerSecond.x -= Time.deltaTime;
            }
        }

        // find future position & draw line
        Vector3 futurePositionInOneSecond = transform.position + VelocityPerSecond;
        Debug.DrawLine(transform.position, futurePositionInOneSecond, Color.magenta, 0.1f);




        //// change speed over time by adding acceleration value
        //speedPerSecond += accelerationPerSecond * Time.deltaTime;
        //
        //// update velocity direction
        //Vector3 velocityDirection = new Vector3(0.1f * Time.deltaTime, 0.1f * Time.deltaTime, 0).normalized;
        //
        //float speedThisFrame = speedPerSecond * Time.deltaTime;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chase_AI : MonoBehaviour
{
    Vector3 velocity_per_second = Vector3.zero;
    public float speed_per_second;
    public float steering_speed;


    void Update()
    {
        /*
         * A steering behaviour adds a steering vector per frame
         * which allows a agent to, over time, steer in the direction
         * that they want to be heading in, creating more organic 
         * and natural movement
         * 
         * Intuative movements, like moving directly towards mouse position
         * result in inorganic looking moevements.
         */


        // Compute Mouse Position In World Position
        // Vector for mouse position     (below) converts screen position of ouse point to world position
        Vector3 mouse_position_in_world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse_position_in_world.z = 0.0f; // always have z position visible in 2D plane

        // Compute Desired Velocity (vector from actor to mouse)
        Vector3 desired_dir_velocity_ps = mouse_position_in_world - transform.position;
        desired_dir_velocity_ps.Normalize(); // normalize for direction only
        desired_dir_velocity_ps *= speed_per_second; // add speed

        // Compute Steering Vector For This Frame (steering vector to add to direction to smoothly turn actor)
        Vector3 steering_vector_ps = (desired_dir_velocity_ps - velocity_per_second).normalized * steering_speed;
        Vector3 steering_vector_pf = steering_vector_ps * Time.deltaTime;

        // Determine New Velocity Direction with Added Steering (add steering vector to current vector)
        Vector3 steering_and_current = velocity_per_second + steering_vector_pf;
        steering_and_current.Normalize(); // get direction for added vector

        // Determine New Velocity With Speed
        velocity_per_second = steering_and_current * speed_per_second;

        transform.position += velocity_per_second * Time.deltaTime;
    }
}

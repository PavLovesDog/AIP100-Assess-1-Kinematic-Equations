using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intercept_AI : MonoBehaviour
{
    public Vector3 velocity_per_second = Vector3.zero;
    public float speed_per_second;
    public float steering_speed;
    public Transform player;

    // Update is called once per frame
    void Update()
    {
        Player_Movement_2 p_movement = player.GetComponent<Player_Movement_2>();

        //COmpute difference between speeds
        Vector3 relative_velocity = p_movement.velocity_per_second - velocity_per_second;

        //calculate distance between player & goblin
        float distance = (transform.position - player.position).magnitude;

        //Compute how long until intercept
        float time_until_intercept = distance / (relative_velocity.magnitude);

        //compute where the player will be at the time of intersection
        Vector3 predicted_player_pos = player.position + p_movement.velocity_per_second * time_until_intercept;

        Vector3 desired_dir_velocity_ps = predicted_player_pos - transform.position;
        desired_dir_velocity_ps.Normalize();
        desired_dir_velocity_ps *= speed_per_second;

        /* ADD IN TURNING */

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

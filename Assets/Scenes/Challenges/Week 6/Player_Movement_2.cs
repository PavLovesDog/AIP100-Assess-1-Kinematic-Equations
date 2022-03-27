using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement_2 : MonoBehaviour
{
    public Vector3 velocity_per_second = Vector3.zero;
    public float speed_per_second;

    void Update()
    {
        // movement controls
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // compute and set movement
        Vector3 desired_velocity = new Vector3(horizontal, vertical, 0.0f); // listen for controls
        desired_velocity.Normalize(); // normalise for direction
        desired_velocity *= speed_per_second; // multiply by its movement speed
        velocity_per_second = desired_velocity; // apply
        transform.position += velocity_per_second * Time.deltaTime; // update transform position per frame
    }
}

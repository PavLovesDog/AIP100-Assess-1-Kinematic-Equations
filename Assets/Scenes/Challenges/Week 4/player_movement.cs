using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement : MonoBehaviour
{

    public float speed = 1.0f;

    void Start()
    {
        
    }

    void Update()
    {
        // movement controls
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //create vector with movement control variables
        Vector3 movement_direction = new Vector3(horizontal, vertical, 0.0f);
        movement_direction.Normalize(); // normalize the vector so all directions are same

        // add all movement to a 'per frame' vector to govern movement
        Vector3 move_per_frame = movement_direction * speed * Time.deltaTime;

        // move player
        transform.position += move_per_frame;

    }
}

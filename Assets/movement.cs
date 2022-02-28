using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    float gravity_magnitude = -1f; // This is usually a global/project value
    bool is_on_ground = false;
    public float mass = 1.0f;
    public float jump_speed = 6.0f;
    public float movement_speed_per_second = 3.0f;

    Vector3 velocity_per_second = Vector3.zero;
    Vector3 gravity_per_second = Vector3.zero;

    private void Start()
    {
        gravity_per_second = new Vector3(0, gravity_magnitude, 0);
    }

    void Update()
    {
        // Challenge: Make movement in the horizontal an acceleration force,
        // which can be a once off impulse (jump), to achieve this you need
        // air-friction and platform-friction which will apply acceleration in the opposite
        // direction to movement.

        // Apply acceleration for this frame.
        Vector3 acceleration_this_frame = (gravity_per_second/mass) * Time.deltaTime;
        velocity_per_second += acceleration_this_frame;

        // Input (AI).
        float horizontal = Input.GetAxis("Horizontal"); // -1 for left, 1 for right
        float jump = Input.GetAxis("Jump"); // 0 for no input, 1 for input

        Vector3 horizontal_velocity_direction = new Vector3(horizontal, 0.0f, 0.0f).normalized;
        Vector3 horizontal_velocity_this_frame = horizontal_velocity_direction * (movement_speed_per_second * Time.deltaTime);

        bool is_jumping = jump > 0.0f;
        bool should_apply_jump_velocity = is_jumping && is_on_ground;
        if(should_apply_jump_velocity)
        {
            Vector3 jump_velocity_this_frame = new Vector3(0.0f, jump_speed, 0.0f);
            velocity_per_second += jump_velocity_this_frame;
        }

        // Apply velocity for this frame.
        Vector3 velocity_this_frame = (velocity_per_second * Time.deltaTime) + horizontal_velocity_this_frame;
        transform.position += velocity_this_frame;

        // Apply collision logic (AI).
        bool is_below_ground = transform.position.y < -2.5f;
        bool should_place_on_ground = is_below_ground;
        if (should_place_on_ground)
        {
            transform.position = new Vector3(transform.position.x, -2.5f, 0.0f);
            velocity_per_second = new Vector3(velocity_per_second.x, 0.0f, 0.0f);
            is_on_ground = true;
        }
        else
        {
            is_on_ground = false;
        }
    }
}



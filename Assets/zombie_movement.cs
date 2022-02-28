using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombie_movement : MonoBehaviour
{
    public Transform player;
    public float movement_speed_per_second = 2.0f;
    public float acceleration_magnitude_per_second = 0.2f;

    Vector3 acceleration_per_second = Vector3.zero;
    Vector3 velocity_per_second = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 Z_to_P = player.position - transform.position;
        acceleration_per_second = Z_to_P.normalized * acceleration_magnitude_per_second;
        velocity_per_second = Z_to_P.normalized * movement_speed_per_second;

        float dis = Z_to_P.magnitude;
        float vel = velocity_per_second.magnitude;
        float acc = acceleration_per_second.magnitude;

        float a = acc * 0.5f;
        float b = vel;
        float c = -dis;

        float t0 = (-b + Mathf.Sqrt((b * b) - (4 * a * c))) / (2 * a);
        float t1 = (-b - Mathf.Sqrt((b * b) - (4 * a * c))) / (2 * a);

        float time_to_intercept = 0.0f;
        if (t0 < 0.0f)
            time_to_intercept = t1;
        else if (t1 < 0.0f)
            time_to_intercept = t0;
        else
            time_to_intercept = t0 < t1 ? t0 : t1;

        print(time_to_intercept);
    }

    
    void Update()
    {
        velocity_per_second += acceleration_per_second * Time.deltaTime;

        Vector3 Z_to_P = player.position - transform.position;
        Vector3 velocity_this_frame = velocity_per_second * Time.deltaTime;

        if (Z_to_P.magnitude < velocity_this_frame.magnitude)
        {
            transform.position = player.position;
            print(Time.time);
            Debug.Break(); // Pause editor.
        }
        else
        {
            transform.position += velocity_this_frame;
        }
    }
}

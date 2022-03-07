using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class physics_manager : MonoBehaviour
{
    public enum Type
    {
        Circle,
        Square,
        Rectangle
    };

    public Type type;

    public float radius;

    void Start()
    {
        // find radius
        radius = transform.localScale.x / 2;
        print(radius);
    }

    void Update()
    {
        // for all objects that have colliders, detect collision
        collision[] collisions = FindObjectsOfType<collision>();
        //gos_with_colliders[0].gameObject

       // for (int index = 0; index < collisions.Length; index++)

        foreach(collision this_collision in collisions)
        {
                foreach (collision other_collision in collisions)
                {
                    // do not detect collision with self
                    if (this_collision == other_collision) continue;

                    // detect collision between this_ & other_ collision

                }
            }

        if (type == Type.Circle)
        {

        }
    }
}

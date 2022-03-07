using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class timer_Script_1 : MonoBehaviour
{
    [Header("Script References")]
    public bulletScript bullet;

    [Header("Text References")]
    public TMP_Text accelerationText;
    public TMP_Text iVelocityText;
    public TMP_Text fVelocityText;
    public TMP_Text timeText;
    public TMP_Text timeTakenText;
    public TMP_Text cVelocityText;


    //[Header("Times & forces References")]
    //public float decelleration;
    //public float friction;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // SUVAT Variable Text
        accelerationText.text = "Acceleration: " + bullet.acceleration;
        iVelocityText.text = "Initial Veloctiy: " + bullet.initialVelocity;
        fVelocityText.text = "Final Velocity: " + bullet.finalVelocity;
        timeText.text = "Time: " + bullet.tTime;

        // Level Specific Variables
        timeTakenText.text = "Time Taken: " + Mathf.Round(bullet.timeTaken * 10.0f) * 0.1f;
        cVelocityText.text = "Current Velocity: " + bullet.currentVelocity;

        //timerText.text = Mathf.Round(gameTimeRemaining) + " seconds untill Nightfall...";
        //frictionText.text = "Friction: " + player_circle.friction;
        //decellText.text = "Time until ball stops: " + player_circle.decelleration;
        //
        //if (player_circle.friction < 0) player_circle.friction = 0;
        //if (player_circle.decelleration < 0) player_circle.decelleration = 0;
    }
}

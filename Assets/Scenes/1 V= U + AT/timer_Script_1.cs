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
    }
}

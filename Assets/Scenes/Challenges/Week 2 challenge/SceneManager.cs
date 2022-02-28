using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneManager : MonoBehaviour
{
    [Header("Script References")]
    public player_circle player_circle;

    [Header("Text References")]
    public TMP_Text frictionText;
    public TMP_Text decellText;

    [Header("Times & forces References")]
    public float decelleration;
    //public float friction;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //timerText.text = Mathf.Round(gameTimeRemaining) + " seconds untill Nightfall...";
        frictionText.text = "Friction: " + player_circle.friction;
        decellText.text = "Time until ball stops: " + player_circle.decelleration;

        if (player_circle.friction < 0) player_circle.friction = 0;
        if (player_circle.decelleration < 0) player_circle.decelleration = 0;
    }
}

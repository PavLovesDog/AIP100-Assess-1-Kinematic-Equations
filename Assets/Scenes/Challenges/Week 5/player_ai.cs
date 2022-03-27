using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_ai : MonoBehaviour
{
    public enum State
    {
        IDLE,
        RUNNING,
        STUNNED,
        DEAD
    };

    public State state = State.IDLE;
    public float speed = 1.0f;
    public float health = 50.0f;
    public Transform lava;
    public Transform spikeBall;
    public bool playerCanMove;
    public float stunnedFor;
    public float stunTime;
    public float spikeDamage;

    void TransitionTo(State transitionTo)
    {
        //On Exit event
        OnExit(transitionTo);

        State currentState = state;
        state = transitionTo;

        // On Enter event
        OnEnter(currentState);
    }

    void OnExit(State entering)
    {
        print("Exiting: " + state.ToString() + " to: " + entering.ToString());
    }

    void OnEnter(State exiting)
    {
        print("Entering: " + state.ToString() + " from: " + exiting.ToString());

        switch(state)
        {
            case State.STUNNED:
                {
                    SpriteRenderer sprite = GetComponent<SpriteRenderer>();
                    sprite.color = Color.yellow;
                    break;
                }
            case State.IDLE:
                {
                    SpriteRenderer sprite = GetComponent<SpriteRenderer>();
                    sprite.color = Color.green;
                    break;
                }
            case State.RUNNING:
                {
                    SpriteRenderer sprite = GetComponent<SpriteRenderer>();
                    sprite.color = Color.cyan;
                    break;
                }
            case State.DEAD:
                {
                    SpriteRenderer sprite = GetComponent<SpriteRenderer>();
                    sprite.color = Color.red;
                    break;
                }
        }
    }

    void OnUpdate()
    {
        // track which state is active in console
        print("Current State: " + state.ToString());


        switch (state)
        {
            case State.IDLE:
                {
                    HandleInLava();
                    HandleSpikeBall();
                    // movement controls
                    float horizontal = Input.GetAxis("Horizontal");
                    float vertical = Input.GetAxis("Vertical");

                    bool PlayerGivenInput = horizontal != 0.0f || vertical != 0.0f;
                    bool playerIsDead = health <= 0.0f;

                    if (playerIsDead)
                    {
                        TransitionTo(State.DEAD);
                    }
                    else if (PlayerGivenInput)
                    {
                        TransitionTo(State.RUNNING);
                    }
                    else
                    {
                        // Play IDLE animation
                    }

                    break;
                }
            case State.RUNNING:
                { 
                    HandleInLava();
                    HandleSpikeBall();

                    float horizontal = Input.GetAxis("Horizontal");
                    float vertical = Input.GetAxis("Vertical");

                    bool PlayerGivenInput = horizontal != 0.0f || vertical != 0.0f;
                    bool playerIsDead = health <= 0.0f;

                    if (playerIsDead)
                    {
                        TransitionTo(State.DEAD);
                    }
                    else if (PlayerGivenInput)
                    {
                        //Movement Controls
                        if (playerCanMove)
                        {
                            Vector3 movement_direction = new Vector3(horizontal, vertical, 0.0f);
                            movement_direction.Normalize();
                            Vector3 move_per_frame = movement_direction * speed * Time.deltaTime;
                            transform.position += move_per_frame;
                        }
                    }
                    else
                    {
                        TransitionTo(State.IDLE);
                    }

                    break;
                }
            case State.STUNNED:
                {

                    if (stunnedFor > 0.0f)
                    {
                        playerCanMove = false;
                        health -= spikeDamage * Time.deltaTime; // deduct health

                        // watch for death!
                        bool playerIsDead = health <= 0.0f;
                        if (playerIsDead)
                        {
                            TransitionTo(State.DEAD);
                        }

                        Vector3 thisToSpikeBall = spikeBall.transform.position - transform.position; // find vector
                        float distanceBetween = thisToSpikeBall.magnitude; // find magnitude
                        float intersection_depth = (this.transform.localScale.x / 2) + (spikeBall.transform.localScale.x / 2) - (distanceBetween - 0.2f);
                        this.transform.position -= thisToSpikeBall.normalized * intersection_depth; // move player so hit trigger is NOT always triggered
                    }
                    else
                    {
                        TransitionTo(State.IDLE);
                        playerCanMove = true;
                    }
                    break;
                }
        }
    }

    // plays on start up
    private void Start()
    {
        playerCanMove = true;
        OnEnter(state);
    }

    void HandleInLava()
    {
        Vector3 thisToLava = lava.transform.position - transform.position;
        bool isInLava = thisToLava.magnitude < (this.transform.localScale.x / 2) + (lava.transform.localScale.x / 2);
        if (isInLava)
        {
            print("AAAAARRRGHGHGHHGGGGGGGGGGGGHHHHHHHHHHHHHHH, IT BURNNNSSSS!!!!!");
            health -= 10.0f * Time.deltaTime;
        }
    }

    void HandleSpikeBall()
    {
        Vector3 thisToSpikeBall = spikeBall.transform.position - transform.position;
        float distanceBetween = thisToSpikeBall.magnitude;
        bool hasHitSpikeBall = distanceBetween < (this.transform.localScale.x / 2) + (spikeBall.transform.localScale.x / 2);
        if(hasHitSpikeBall)
        {
            print("OOOOH, SPIKEY!");
            TransitionTo(State.STUNNED);

            stunnedFor = stunTime; // set time to wait
        }
    }

    void Update()
    {
        OnUpdate();

        if(!playerCanMove)
        {
            stunnedFor -= Time.deltaTime; // begin countdown of time
            if (stunnedFor < 0.0f) stunnedFor = 0.0f;
        }
    }
}

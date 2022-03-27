using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goblin_ai : MonoBehaviour
{
    public enum State
    {
        IDLE,
        CHASING,
        FLEEING,
        SEEKING,
        STUNNED,
        DEAD
    };
    
    [Header("Current State")]
    public State state = State.IDLE;
    
    [Header("Actor variables")]
    public float speed = 1.0f;
    public float health = 50.0f;
    public Transform lava;
    public Transform spikeBall;
    public Transform Player;
    public float timeStandingStill = 5.0f;
    public float timeSeeking = 5.0f;
    public bool resetSeekTimer;
    public bool seeking;
    public bool idling;

    [Header("Stunned Variables")]
    public bool actorCanMove;
    public float stunnedFor;
    public float stunTime;
    public float spikeDamage;

    public Vector3 randDirection;
    float boundry_X = 4.5f;
    float boundry_Y = 4.5f;

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
        print("Goblin Exiting: " + state.ToString() + " to: " + entering.ToString());
    }

    void OnEnter(State exiting)
    {
        print("Goblin Entering: " + state.ToString() + " from: " + exiting.ToString());

        switch (state)
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
            case State.SEEKING:
                {
                    SpriteRenderer sprite = GetComponent<SpriteRenderer>();
                    sprite.color = Color.green;
                    break;
                }
            case State.CHASING:
                {
                    SpriteRenderer sprite = GetComponent<SpriteRenderer>();
                    sprite.color = Color.magenta;
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
        HandleBorders(); // make sure NO ONE LEAVES


        switch (state)
        {
            case State.IDLE:
                {
                    HandleSpikeBall();
                    HandleInLava();
                    HandleSeekTimer();

                    idling = true;

                    //While idle generate random directions, "think about where to walk"
                    float xRand = Random.Range(-1, 2);
                    float yRand = Random.Range(-1, 2);
                    randDirection = new Vector3(xRand, yRand, 0.0f);

                    // watch for player power/health

                    //watch for player distance
                    Vector3 ActorToPlayer = Player.transform.position - this.transform.position;
                    float distanceToPlayer = ActorToPlayer.magnitude;
                    
                    //STate BOOLS
                    bool ActorIsDead = health <= 0.0f;
                    bool lowHealth = health <= 10.0f;
                    bool detectedPlayer = distanceToPlayer < 3.0f;
                    bool stoodStillTooLong = timeStandingStill < 0.0f;

                    //handle states
                    if (ActorIsDead)
                    {
                        TransitionTo(State.DEAD);
                    }
                    else if (detectedPlayer)
                    {
                        TransitionTo(State.CHASING);
                    }
                    else if (lowHealth) /*player has powerup && nearby or health is low*/
                    {
                        TransitionTo(State.FLEEING);
                    }
                    else if (stoodStillTooLong)
                    {
                        //idling = false;
                        TransitionTo(State.SEEKING);
                    }

                    break;
                }
            case State.CHASING:
                {
                    HandleInLava();
                    HandleSpikeBall();

                    // find vector of self to player
                    Vector3 actorToPlayer = Player.transform.position - this.transform.position;
                    float distanceToPlayer = actorToPlayer.magnitude;
                    actorToPlayer.Normalize(); //normalize for direction

                    bool ActorIsDead = health <= 0.0f;
                    bool isChasing = distanceToPlayer < 3.0f;
                    bool lostPlayer = distanceToPlayer > 5.0f;

                    if (ActorIsDead)
                    {
                        TransitionTo(State.DEAD);
                    }
                    else if (isChasing)
                    {
                        // start at certain speed, accelerate over time
                        //speed += Time.deltaTime / 4; // add acceleration
                        Vector3 moveTowards = actorToPlayer * speed * Time.deltaTime;
                        transform.position += moveTowards; // move towards
                    }
                    else if (lostPlayer)
                    {
                        //CHANGE TO SEEKING
                        TransitionTo(State.IDLE);
                    }
                
                    break;
                }
            case State.FLEEING:
                {
                    HandleSpikeBall();
                    HandleInLava();


                    // find direction vector to player
                    Vector3 actorToPlayer = Player.transform.position - this.transform.position;
                    float distanceToPlayer = actorToPlayer.magnitude;
                    actorToPlayer.Normalize(); //normalize for direction

                    bool ActorIsDead = health <= 0.0f;
                    bool isFleeing = distanceToPlayer < 10.0f;
                    bool lostPlayer = distanceToPlayer > 10.0f;

                    if (ActorIsDead)
                    {
                        TransitionTo(State.DEAD);
                    }
                    else if (isFleeing)
                    {
                        Vector3 moveAway = -actorToPlayer * speed * Time.deltaTime;
                        transform.position += moveAway; // move towards
                    }
                    else if (lostPlayer)
                    {
                        TransitionTo(State.IDLE);
                    }

                    // move away from player at set speed.
                    break;
                }
            case State.SEEKING:
                {
                    seeking = true;
                    HandleSeekTimer();
                    HandleSpikeBall();
                    HandleInLava();

                    // find vector of self to player
                    Vector3 actorToPlayer = Player.transform.position - this.transform.position;
                    float distanceToPlayer = actorToPlayer.magnitude;

                    bool seekedTooLong = timeSeeking < 0.0f;
                    bool foundPlayer = distanceToPlayer < 3.0f; 
                    bool ActorIsDead = health <= 0.0f;
                    
                    // Varibales to help avoid Lava
                    Vector3 thisToLava = lava.transform.position - transform.position;
                    Vector3 moveAway;
                    float distanceBetween = thisToLava.magnitude;
                    float touchingLava = (lava.transform.localScale.x / 2) + (this.transform.localScale.x / 2) - distanceBetween;

                    if (ActorIsDead)
                    {
                        TransitionTo(State.DEAD);
                    }
                    else if (touchingLava > -0.2f)
                    {
                        // run away from lava
                        moveAway = -thisToLava * speed * Time.deltaTime;
                        transform.position += moveAway; // move towards

                        //calculate new direction!
                        float xRand = Random.Range(-1, 2);
                        float yRand = Random.Range(-1, 2);
                        randDirection = new Vector3(xRand, yRand, 0.0f);
                    }
                    else if (seekedTooLong)
                    {
                        //seeking = false;
                        TransitionTo(State.IDLE);
                    }
                    else if (foundPlayer)
                    {
                        TransitionTo(State.CHASING);
                    }
                    else
                    {
                        // create random vector direction and move towards it
                        transform.position += randDirection * Time.deltaTime;
                    }

                    break;
                }
            case State.STUNNED:
                {
                    if (stunnedFor > 0.0f)
                    {
                        actorCanMove = false;
                        health -= spikeDamage * Time.deltaTime; // deduct health

                        Vector3 thisToSpikeBall = spikeBall.transform.position - transform.position; // find vector
                        float distanceBetween = thisToSpikeBall.magnitude; // find magnitude
                        float intersection_depth = (this.transform.localScale.x / 2) + (spikeBall.transform.localScale.x / 2) - (distanceBetween - 0.2f);
                        this.transform.position -= thisToSpikeBall.normalized * intersection_depth; // move player so hit trigger is NOT always triggered
                    }
                    else
                    {
                        TransitionTo(State.IDLE);
                        actorCanMove = true;
                    }
                    break;
                }
        }
    }

    // plays on start up
    private void Start()
    {
        actorCanMove = true;
        resetSeekTimer = false;
        idling = true;
        seeking = false;
        OnEnter(state);

        // generate random seed
        int randSeed = (int)System.DateTime.Now.Ticks;
        Random.InitState(randSeed);
    }

    void HandleSeekTimer()
    {
        if (idling)
        {
            if (resetSeekTimer)
            {
                timeStandingStill = 5.0f;
                resetSeekTimer = false;
            }

            if (timeStandingStill >= 0.0f)
            {
                timeStandingStill -= Time.deltaTime;
            }
            else if (timeStandingStill < 0.0f)
            {
                resetSeekTimer = true;
                idling = false;
            }
        }
        else if (seeking)
        {
            if (resetSeekTimer)
            {
                timeSeeking = 5.0f;
                resetSeekTimer = false;
            }

            if (timeSeeking >= 0.0f)
            {
                timeSeeking -= Time.deltaTime;
            }
            else if (timeSeeking <= 0.0f)
            {
                resetSeekTimer = true;
                seeking = false;
            }
        }

    }

    void HandleInLava()
    {
        Vector3 thisToLava = lava.transform.position - transform.position;
        bool isInLava = thisToLava.magnitude < (this.transform.localScale.x / 2) + (lava.transform.localScale.x / 2);
        if (isInLava)
        {
            print("MY GOBLIN SKIN IT BURNINS!");
            health -= 10.0f * Time.deltaTime;
        }
    }

    void HandleSpikeBall()
    {
        Vector3 thisToSpikeBall = spikeBall.transform.position - transform.position;
        float distanceBetween = thisToSpikeBall.magnitude;
        bool hasHitSpikeBall = distanceBetween < (this.transform.localScale.x / 2) + (spikeBall.transform.localScale.x / 2);
        if (hasHitSpikeBall)
        {
            print("GOBLIN OUCH");
            TransitionTo(State.STUNNED);

            stunnedFor = stunTime; // set time to wait
        }
    }

    void HandleBorders()
    {
        if (transform.position.x > boundry_X) transform.position = new Vector3(boundry_X, transform.position.y, 0.0f); // right wall
        if (transform.position.x < -boundry_X) transform.position = new Vector3(-boundry_X, transform.position.y, 0.0f); // left wall
        if (transform.position.y < -boundry_Y) transform.position = new Vector3(transform.position.x, -boundry_Y, 0.0f); // bottom wall
        if (transform.position.y > boundry_Y) transform.position = new Vector3(transform.position.x, boundry_Y, 0.0f); // top wall
    }

    

    void Update()
    {
        OnUpdate();

        stunnedFor -= Time.deltaTime; // begin countdown of time
        if (stunnedFor < 0.0f) stunnedFor = 0.0f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ********************************************
 *      Moves the player left/right/jump
*********************************************** */

public class Movement : MonoBehaviour
{

    public GameObject RStep0, RStep1, LStep0, LStep1;
    public float instantiateTimer;

    public float speed, jumpForce, facingRight;
    public string jumpSoundVivi = "event:/Voice Overs/VO_Vivi_Jump";
    public string runSoundVivi = "event:/Voice Overs/VO_Vivi_StartRunning";
    public string viviIdleHumSound = "event:/Voice Overs/VO_Vivi_Humming";
    public string viviIdleYawnSound = "event:/Voice Overs/VO_Vivi_Yawn";
    public string climbSoundPath = "event:/Vivi_MovementSFX/SFX_Vivi_ClimbIvy";
    public string runSoundPath = "event:/Vivi_MovementSFX/SFX_Vivi_Walking_Pavement";

    public string footstepPath = "event:/Vivi_MovementSFX/Footsteps/Wood Footsteps";

    FMOD.Studio.EventInstance climbSoundEvent;
    FMOD.Studio.EventInstance runSoundEvent;
    FMOD.Studio.EventInstance footstepsEvent;

    public bool canClimb;
    public bool canMove;
    public bool bgMove; //Moves the background as the player moves
    public bool Run, Climb, Jump, moveleft, moveright, climbUp, jumpMove; //Animator
    public bool firstSpiritPuzzle;
    public bool secondSpiritPuzzle;
    public bool inPuzzleZone, sayCast;

    public float timer, jmpTimer;
    private bool check, fireFlag; //to fix niken's bug with the background
    private float fireTimer, wallChecker; // same above
    private Vector3 initPoint;

    bool climbSound;
    bool runSound;

    SpeechRecognition01 speech;

    // Use this for initialization
    void Start()
	{
        bgMove = true; 
        facingRight = 1f;
        canMove = true;
        Climb = false;
        fireFlag = false;
        moveleft = false;
        moveright = false;
        jumpMove = false;
        climbUp = false;
        sayCast = false;
        fireTimer = 0f;

        climbSound = false;
        climbSoundEvent = FMODUnity.RuntimeManager.CreateInstance(climbSoundPath);

        runSound = false;
        runSoundEvent = FMODUnity.RuntimeManager.CreateInstance(runSoundPath);

        footstepsEvent = FMODUnity.RuntimeManager.CreateInstance(footstepPath);

        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        check = true;
    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        if (canMove)
        {
            if (moveleft || moveright || Run)
            {
                //if move left, play sounds
                if (moveleft)
                {
                    instantiateTimer += Time.deltaTime;
                    if (instantiateTimer > 0.4f)
                    {
                        GameObject L0 = Instantiate(LStep0, transform.position - new Vector3(0, 1f, 0), transform.rotation);
                        GameObject L1 = Instantiate(LStep1, transform.position - new Vector3(0, 1f, 0), transform.rotation);
                        Destroy(L0, 3.0f);
                        Destroy(L1, 3.0f);
                        instantiateTimer = 0;
                    }
                }
                //if move right, play sounds
                if (moveright)
                {
                    instantiateTimer += Time.deltaTime;
                    if (instantiateTimer > 0.4f)
                    {
                        GameObject R0 = Instantiate(RStep0, transform.position - new Vector3(0, 1f, 0), transform.rotation);
                        GameObject R1 = Instantiate(RStep1, transform.position - new Vector3(0, 1f, 0), transform.rotation);
                        Destroy(R0, 3.0f);
                        Destroy(R1, 3.0f);
                        instantiateTimer = 0;
                    }
                }
                if (!runSound)
                {
                    //print("Running sound on");
                    //runSoundEvent.start();
                    //runSound = true;
                }
            }
            else if (!moveleft && !moveright && !Run)
            {
                if (runSound)
                {
                    //print("Running sound off");
                    //runSoundEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    //runSound = false;
                }
            }
            Run = false;
            //Debug.Log(Mathf.Abs(gameObject.GetComponent<Rigidbody2D>().velocity.y));
            //Witch moves left/right

            //if player says "left", turn on left switch
            if (speech.word == "move left" || 
                speech.word == "left" ||
                speech.word == "run left" ||
                speech.word == "walk left")
            {
                moveleft = true;
                moveright = false;
                initPoint = gameObject.transform.position;
                wallChecker = 0f;
            }

            //if player says "right", turn on right switch
            if (speech.word == "move right" || 
                speech.word == "right" ||
                speech.word == "run right" ||
                speech.word == "walk right")
            {
                moveleft = false;
                moveright = true;
                initPoint = gameObject.transform.position;
                wallChecker = 0f;
            }

            //turns player around when they say "turn"
            if (speech.word == "turn")
            {
                facingRight *= -1;
                if (facingRight > 0)
                {
                    gameObject.GetComponent<SpriteRenderer>().flipX = false;
                }

                else
                {
                    gameObject.GetComponent<SpriteRenderer>().flipX = true;
                }

                speech.word = "";
            }

            //turns on climb switch if player says "climb"
            if (speech.word == "climb") { climbUp = true; }

            //tells vivi to move left/right
            if (moveleft) { MoveLeft(); }
            if (moveright) { MoveRight(); }

            ////lets player move with keyboard...for now
            //if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            //{
            //    transform.Translate(Vector3.left * Time.deltaTime * speed);
            //    gameObject.GetComponent<SpriteRenderer>().flipX = true;
            //    facingRight = -1f;
            //    timer = 0.0f;
            //    Run = true;
            //    moveleft = false;
            //    moveright = false;
            //}
            //if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            //{
            //    transform.Translate(Vector3.right * Time.deltaTime * speed);
            //    gameObject.GetComponent<SpriteRenderer>().flipX = false;
            //    facingRight = 1f;
            //    timer = 0.0f;
            //    Run = true;
            //    moveleft = false;
            //    moveright = false;
            //}
            GetComponent<Animator>().SetBool("Run", Run);
        }

    }

    void Update()
    {
        //Fix niken's bug for background
        if (fireFlag)
        {
            fireTimer += Time.deltaTime;
            if (fireTimer > 1.5f)
            {
                bgMove = true;
                fireFlag = false;
            }
        }
        /*//... If Vivi collides unto something, set the speed of the camera to 0
        if (bgMove == false)
        {
            GameObject.Find("AreaOfMovement").GetComponent<Background>().speedOfCamera3 = 0f;
            GameObject.Find("AreaOfMovement").GetComponent<Background>().speedOfCamera2 = 0f;
            //GameObject.Find("HillMovement").GetComponent<bgHill>().speedOfCamera4 = 0f;           //Only on Level 2
        }
        //... If Vivi doesn't collide unto something, set the speed of the camera back
        else if (bgMove == true)
        {
            GameObject.Find("AreaOfMovement").GetComponent<Background>().speedOfCamera3 = 0.018f;
            GameObject.Find("AreaOfMovement").GetComponent<Background>().speedOfCamera2 = 0.006f;
            //GameObject.Find("HillMovement").GetComponent<bgHill>().speedOfCamera4 = .2f;          //Only on Level 2
        }
        */
        //Plays idle sound
        timer += Time.deltaTime;

        if (inPuzzleZone)
        {
            //moveleft = false;
           // moveright = false;
        }
        
        /*if (timer > 10.0f && timer < 10.01f)
        {
            FMODUnity.RuntimeManager.PlayOneShot(viviIdleYawnSound);
        }*/

        //canMove checks if the player can move and isn't in a dialougue/cutscene
        if (canMove == true)
        {
            if (timer > 10.0f && timer < 10.01f)
            {
                FMODUnity.RuntimeManager.PlayOneShot(viviIdleHumSound);
            }
            //Debug.Log(Mathf.Abs(gameObject.GetComponent<Rigidbody2D>().velocity.y));
            //Witch moves left/right

            /* MOVED CODE TO "FIXED UPDATE" TO REMOVE COLLIDING IN WALL MOVEMENT
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * Time.deltaTime * speed);
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
                facingRight = -1f;
                timer = 0.0f;
                Run = true;
                Idle = false;
            }
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * Time.deltaTime * speed);
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
                facingRight = 1f;
                timer = 0.0f;
                Run = true;
                Idle = false;
            }
            */


            /*///Plays the sound for Running////
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                FMODUnity.RuntimeManager.PlayOneShot(runSoundVivi);
            }*/

            //Jump
            if (Jump == true)
            {
                jmpTimer += Time.deltaTime;
                if (jmpTimer >= 0.5f)
                {
                    Jump = false;
                    jmpTimer = 0f;
                }
            }                           //Animator
            if (Jump && canJump() && !canClimb)
            {
                //if (speech.word == "jump") { jumpMove = true; }
                //speech.word = "";
                
                if (check)
                {
                    gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
                    FMODUnity.RuntimeManager.PlayOneShot(jumpSoundVivi);
                    timer = 0.0f;
                    jumpMove = true;
                    Jump = true;
                    check = false;
                }
            }
            
            //if player says "jump", vivi jumps forward
            if (Jump && facingRight > 0 && jumpMove)
            {
                MoveRight();
                moveleft = false;
                moveright = false;
            }
            else if (Jump && facingRight < 0 && jumpMove)
            {
                MoveLeft();
                moveleft = false;
                moveright = false;
            }
            else if (!Jump) { jumpMove = false; check = true; }
            GetComponent<Animator>().SetBool("Jump", Jump);

            /*----For climbing--------*/

            //moving up
            GetComponent<Animator>().SetBool("Climb", Climb);

            if (GetComponent<Animator>().GetBool("Climb") == false)
            {
                climbSoundEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                climbUp = false;
            }

            //lets player climb with keyboard input...for now
            if (climbUp)
            {
                if (canClimb == true)
                {
                    transform.Translate(Vector3.up * Time.deltaTime * speed);
                    timer = 0.0f;
                    Climb = true;
                    if (facingRight > 0) { moveright = true; }
                    else { moveleft = true; }
                    gameObject.GetComponent<Animator>().speed = 1;

                    if (!climbSound)
                    {
                        print("Climbing");
                        climbSoundEvent.start();
                        climbSound = true;
                    }
                }
                else {
                    
                }

            }
            //moving down
            //if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            //{
            //    if (canClimb == true)
            //    {
            //        transform.Translate(Vector3.down * Time.deltaTime * speed);
            //        timer = 0.0f;
            //        Climb = true;
            //        gameObject.GetComponent<Animator>().speed = 1;

            //        if (!climbSound)
            //        {
            //            print("Climbing");
            //            climbSoundEvent.start();
            //            climbSound = true;
            //        }
            //    }
            //    else
            //    {

            //    }
            //}
            
        }
	}

    private bool canJump()
    {
        //checks if player is touching the ground 
        return Mathf.Abs(gameObject.GetComponent<Rigidbody2D>().velocity.y) <= .5f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if something is interactable, vivi ui tells player to say "cast", else it tells player to say "vivi"
        if (collision.tag != "TravelPoint" && collision.tag != "vines" && 
            collision.tag != "Colliding" && collision.tag != "bgAllowedToMove")
        { sayCast = true; }
           
        //lets player climb only if theres a vine
        if(collision.tag == "vines")
        {
            canClimb = true;
            Climb = true;

            //gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
			gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector3 (0, 0, 0);
            sayCast = false;
        }
        //checs if you enter the trigger for the first spirit puzzle (burned house)
        if (collision.tag == "puzzleSpirit1")
        {
            firstSpiritPuzzle = true;
        }
        //checs if you enter the trigger for the second spirit puzzle (Church Bell)
        if (collision.tag == "puzzleSpirit2")
        {
            secondSpiritPuzzle = true;
        }

        //turns off movement once player reaches a travelpoint
        if (collision.tag == "TravelPoint")
        //if (collision.gameObject.layer != 10)
        {
            moveleft = false;
            moveright = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //helps vivi climb a vine
        if(collision.tag == "vines")
        {
            canClimb = true;
            Climb = true;
            gameObject.GetComponent<Animator>().speed = 0;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        }

        //If Vivi collides with something, don't move the background
        if (collision.tag == "Colliding" || collision.tag == "shrine")
        {
            //Debug.Log("COLLISION IS WORKING");
            bgMove = false;
        }
        if (speech.word == "fire") {
            fireFlag = true;
            fireTimer = 0f;
        }
        //checs if you are still in the trigger for the first spirit puzzle (Burned House)
        if (collision.tag == "puzzleSpirit1")
        {
            firstSpiritPuzzle = true;
        }
        //checs if you are still in the trigger for the second spirit puzzle (Church BEll)
        if (collision.tag == "puzzleSpirit1")
        {
            secondSpiritPuzzle = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "TravelPoint") { sayCast = false; }
        if (collision.tag == "vines")
        {
            canClimb = false;
            Climb = false;
            Run = false;
            gameObject.GetComponent<Animator>().speed = 1;
            //gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            gameObject.GetComponent<Rigidbody2D>(). gravityScale = 2;
        }
        //If Vivi isn't colliding anything
        if (collision.tag == "Colliding" || collision.tag == "shrine")
        {
            //Debug.Log("You are out of collision");
            bgMove = true;
        }

        //checs if you left the trigger for the first spirit puzzle (Burned House)
        if (collision.tag == "puzzleSpirit1")
        {
            firstSpiritPuzzle = false;
        }
        //checs if you left the trigger for the second spirit puzzle (Bell)
        if (collision.tag == "puzzleSpirit2")
        {
            secondSpiritPuzzle = false;
        }
    }

    //makes vivi move left
    private void MoveLeft()
    {
        transform.Translate(Vector3.left * Time.deltaTime * speed);
        gameObject.GetComponent<SpriteRenderer>().flipX = true;
        facingRight = -1f;
        timer = 0.0f;
        Run = true;
        wallChecker += Time.deltaTime;
        if (wallChecker > .5f)
        {
            if (Vector2.Distance(transform.position,initPoint) < .5f) { moveleft = false; Run = false; }
            else { initPoint = transform.position; }
            wallChecker = 0f;
        }
    }

    //makes vivi move right
    private void MoveRight()
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed);
        gameObject.GetComponent<SpriteRenderer>().flipX = false;
        facingRight = 1f;
        timer = 0.0f;
        Run = true;
        wallChecker += Time.deltaTime;
        if (wallChecker > .5f)
        {
            if (Vector2.Distance(transform.position, initPoint) < .5f) { moveright = false; Run = false; }
            else { initPoint = transform.position; }
            wallChecker = 0f;
        }
    }

    public void Footfall()
    {
        footstepsEvent.start();
    }

    public void ResetFootstepSound(string path)
    {
        footstepPath = path;
        footstepsEvent = FMODUnity.RuntimeManager.CreateInstance(footstepPath);
    }
}
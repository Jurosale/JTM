using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ********************************************
 *      Checks for spells that directly
 *      affect Vivi's attributes/state
 *      Current Spells: Shrink, Grow, Astral
*********************************************** */

public class AvatarSpells : MonoBehaviour {

    private float weight, posX, posY, posZ, dimX, dimY, dimZ;
    //private float vel, jump;

    public GameObject camera;
    public GameObject growPar;
    public GameObject shrinkPar;

    public float speed;

    //Timer
    public float timer;
    private float clock;

    //use to control astral
    private bool astralOn;
    private float limitX, limitY;

    //use to control shrink and grow
    private int sizeState;
    public bool canShrink;
    public bool canGrow;
    public bool isShrink, isGrow;
    public int tasks, roots;

    GameObject vivi;
    SpeechRecognition01 speech;
    public Vector3 initPos;

    //For the giggle sound
    public string giggleSound = "event:/Voice Overs/VO_Vivi_Giggle";

    // Use this for initialization
    void Start () {
        vivi = GameObject.Find("Witch character");
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        //gets Vivi's inital weight (jump purposes), speed, starting position, and inital size
        weight = GetComponent<Rigidbody2D>().mass;
        //vel = gameObject.GetComponent<Movement>().speed;
        //jump = gameObject.GetComponent<Movement>().jumpForce;
        posX = transform.localPosition.x;
        posY = transform.localPosition.y;
        posZ = transform.localPosition.z;
        dimX = transform.localScale.x;
        dimY = transform.localScale.y;
        dimZ = transform.localScale.z;
        initPos = gameObject.transform.position;

        sizeState = 0;
        canShrink = true;
        canGrow = true;
        clock = 0;
        timer = 0.5f;
        tasks = 0;
        roots = 0;
        isShrink = false;
        isGrow = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (growPar.activeInHierarchy == true || shrinkPar.activeInHierarchy == true)
        {
            clock += Time.deltaTime;
        }
        //if player is not using astral, camera will follow Vivi
        if (astralOn == false)
        {
            //camera position is vivi's position
            camera.transform.position = new Vector3(transform.position.x, GetComponent<Collider2D>().bounds.min.y + 2f, camera.transform.position.z);
            limitX = transform.position.x;
            limitY = transform.position.y;
        }

        //else the player can freely move the camera to scout the area
        else
        {
            //if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && camera.transform.localPosition.x > limitX - 50f)
            //{
            //    camera.transform.Translate(Vector3.left * Time.deltaTime * speed);
            //}
            //if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && camera.transform.localPosition.x < limitX + 50f)
            //{
            //    camera.transform.Translate(Vector3.right * Time.deltaTime * speed);
            //}
            //if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && camera.transform.localPosition.y < limitY + 15f)
            //{
            //    camera.transform.Translate(Vector3.up * Time.deltaTime * speed);
            //}
            //if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && camera.transform.localPosition.y > limitY - 15f)
            //{
            //    camera.transform.Translate(Vector3.down * Time.deltaTime * speed);
            //}
        }
        
        //shrinks Vivi when player casts shrink
        if (canShrink && (speech.word == "shrink" || isShrink))
        {
            isShrink = false;
            sizeState--;
            updateSize();
            canShrink = false;
            shrinkPar.transform.position = new Vector3(vivi.transform.position.x + 4f, vivi.transform.position.y,
                vivi.transform.position.z);
            shrinkPar.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Clear();
            shrinkPar.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Clear();
            shrinkPar.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
            shrinkPar.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Play();
        }

        //this is here to fix a shrink bug
        else if (speech.word != "shrink" && !canShrink) { canShrink = true; isShrink = false; }

        //grows Vivi when player casts grow
        if (canGrow && (speech.word == "grow" || isGrow))
        {
            isGrow = false;
            sizeState++;
            updateSize();
            canGrow = false;
            growPar.transform.position = new Vector3(vivi.transform.position.x + 4f, vivi.transform.position.y,
                vivi.transform.position.z);
            growPar.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Clear();
            growPar.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Clear();
            growPar.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
            growPar.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Play();
        }

        //this is here to fix a grow bug
        else if (speech.word != "grow" && !canGrow) { canGrow = true; isGrow = false; }

        //if player casts astral, prevent Vivi from moving
        if (speech.word == "astral")
        {
            //astralOn = true;
            //gameObject.GetComponent<Movement>().enabled = false;
            //gameObject.GetComponent<Movement>().speed = 0f;
            //gameObject.GetComponent<Movement>().jumpForce = 0f;
        }

        //If player casts return, camera returns to Vivi and she can move again 
        if (astralOn)
        {
            astralOn = false;
            gameObject.GetComponent<Movement>().enabled = true;
            //gameObject.GetComponent<Movement>().speed = vel;
            //gameObject.GetComponent<Movement>().jumpForce = jump;
        }
        timeReset();

        //Vivi giggles and blush when player says cute
        if(speech.word == "cute")
        {
            FMODUnity.RuntimeManager.PlayOneShot(giggleSound);
        }
    }

    //everytime the player casts shrink, the size num decreases by 1 and everytime the player
    //casts grow, the size num increases by 1. if size num is negative, Vivi will be small,
    //if positive she'll be big, and if its 0, she'll be normal size.
    void updateSize()
    {
        if (sizeState < 0) { sizeState = -1; }
        else if (sizeState > 0) { sizeState = 1; }

        switch (sizeState)
        {
            case 0:
                transform.localScale = new Vector3(dimX, dimY, dimZ);
                gameObject.GetComponent<Rigidbody2D>().mass = weight;
                break;
            case -1:
                transform.localScale = new Vector3(dimX / 2f, dimY / 2f, dimZ);
                gameObject.GetComponent<Rigidbody2D>().mass = weight * 1.1f;
                break;
            case 1:
                transform.localScale = new Vector3(dimX * 1.5f, dimY * 1.5f, dimZ);
                gameObject.GetComponent<Rigidbody2D>().mass = weight / 1.3f;
                break;
        }
    }

    void timeReset()
    {
        if (clock >= timer)
        {
            clock = 0;
            //growPar.SetActive(false);
            //shrinkPar.SetActive(false);
        }
    }
}
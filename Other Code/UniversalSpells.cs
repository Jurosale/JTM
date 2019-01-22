using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ********************************************
 *      Checks for spells that can be
 *      universally applied to all objects
 *      Current Spells: Restore, Fire, Hint
*********************************************** */

public class UniversalSpells : MonoBehaviour
{

    public bool canFire;
    public bool canRestore;
    public float distance;
    public bool restoreOverride;

    private float posX, posY, posZ, dimX, dimY, dimZ, fireTimer;

    private Vector4 colorObject;

    float clock;

    bool fireFlag, fireAnim;

    SpeechRecognition01 speech;
    GameObject animFire, vivi;
    UIHistory uiH;

    // Use this for initialization
    void Start()
    {
        posX = transform.localPosition.x;
        posY = transform.localPosition.y;
        posZ = transform.localPosition.z;
        dimX = transform.localScale.x;
        dimY = transform.localScale.y;
        dimZ = transform.localScale.z;

        //Fire animation stuff
        colorObject = new Vector4(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, 
            GetComponent<SpriteRenderer>().color.b, GetComponent<SpriteRenderer>().color.a);

        fireAnim = false;
        fireTimer = 0f;
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        animFire = GameObject.Find("FireAnim");
        vivi = GameObject.Find("Witch character");
        uiH = GameObject.Find("UIH1").GetComponent<UIHistory>();

        restoreOverride = false;
    }

    // When Vivi gets close enough to object...
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //...If it can be affected by fire and the player casted fire, will destroy object
            if (canFire && (speech.word == "fire" || uiH.isFire))
            {
                fireAnim = true;
                animFire.GetComponent<SpriteRenderer>().sortingOrder = 10;
                animFire.transform.position = new Vector3(transform.position.x, GetComponent<Collider2D>().bounds.min.y + 3.5f, transform.position.z);
                fireFlag = false;
            }
        }
    }

    private void Update()
    {
        if (!fireAnim && !fireFlag)
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= 1.2)
            {
                fireAnim = false;
                uiH.isFire = false;
                fireTimer = 0f;
            }
        }
    }

    void FixedUpdate()
    {
        distance = Mathf.Pow((Camera.main.transform.position.x - gameObject.transform.position.x), 2);

        if (distance <= 100)
        {
            if (!restoreOverride)
                canRestore = true;
        }
        else
        {
            canRestore = false;
        }

        if ((speech.word == "fire" || uiH.isFire) && fireFlag)
        {
            //Sets the fire animation at the right position by the object
            animFire.GetComponent<SpriteRenderer>().sortingOrder = 10;
            animFire.transform.position = new Vector3(vivi.transform.position.x + (3f* vivi.GetComponent<Movement>().facingRight),
                vivi.GetComponent<Collider2D>().bounds.min.y + 4f, gameObject.transform.position.z);
            fireFlag = false;
        }
        
        //...If the player casts restore, sets all objects at their initial state (can be cast at any time)
        if (canRestore && (speech.word == "restore" || uiH.isRestore))
        {
            uiH.isRestore = false;
            transform.localScale = new Vector3(dimX, dimY, dimZ);
            transform.localPosition = new Vector3(posX, posY, posZ);
            GetComponent<SpriteRenderer>().color = colorObject;
        }

        if (!uiH.isFire && !fireFlag) {
            fireFlag = true;
        }

        if (fireAnim)
        {
            uiH.isFire = false;
            float timePass = Time.deltaTime;
            fireTimer += timePass;
            GetComponent<SpriteRenderer>().color = new Vector4(GetComponent<SpriteRenderer>().color.r - timePass/1.2f, 
                GetComponent<SpriteRenderer>().color.g - timePass / 1.2f, GetComponent<SpriteRenderer>().color.b - timePass / 1.2f, 
                GetComponent<SpriteRenderer>().color.a);
            if (fireTimer >= 1.2)
            {
                fireAnim = false;
                transform.localScale = new Vector3(dimX, 0, dimZ);
                fireTimer = 0f;
            }
        }
    }
}
        

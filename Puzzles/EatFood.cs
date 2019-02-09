using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatFood : MonoBehaviour
{
    //inkan: for the whale mouth
    public GameObject mouth;
    //inkan: for the root object
    public GameObject root;
    //inkan: for the root object
    public GameObject rootRestored;

    public GameObject food, spirit;
    public RuntimeAnimatorController happyAnim;
    public RuntimeAnimatorController whaleEat;
    bool whaleAnim, startTimer;
    float yPos;
    float timer;
    // Use this for initialization
    void Start()
    {
        //Inkan: turn off the animation in the whale mouth in the beginning
        mouth.GetComponent<Animator>().enabled = false;
        //inkan: turn off the root
        //root.SetActive(false);
        //Inkan: turn off NPC script initially
        spirit.GetComponent<NPCSpells>().enabled = false;
        //Inkan: turn off canRestore to the root
        rootRestored.GetComponent<Root>().canRestore = false;
        whaleAnim = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (startTimer && !whaleAnim)
        {
            timer += Time.deltaTime;
        }
        if (timer > 1.0f)
        {
            timer = 0;
            whaleAnim = true;
        }
        if (whaleAnim)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - Time.deltaTime, transform.position.z);
            //Inkan: turn on the animator
            mouth.GetComponent<Animator>().enabled = true;
            //Inkan: Add animation for whale eating 
            GetComponentInChildren<Animator>().runtimeAnimatorController = whaleEat;

            if (transform.position.y < yPos - 2f)
            {
                //inkan: turn on the root and set canRestore to true.
                //root.SetActive(true);
                rootRestored.GetComponent<Root>().canRestore = true;


                whaleAnim = false;
                spirit.GetComponentInChildren<Animator>().runtimeAnimatorController = happyAnim;
                spirit.GetComponent<NPCSpells>().isPuzzle = true;
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "food")
        {
            startTimer = true;
            yPos = transform.position.y;
            food.GetComponent<Animator>().enabled = true;
            Destroy(food, 1.9f);
            //Inkan: turn on NPC script so ghost can be prayered
            spirit.GetComponent<NPCSpells>().enabled = true;
            //Inkan: turn off cutscenes
            spirit.GetComponent<Spirit_Interaction>().enabled = false;
        }
    }
}

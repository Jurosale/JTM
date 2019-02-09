using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gardenIsland_Puzzle : MonoBehaviour {
    
    //Speech Recogntion Object
    public GameObject speechRec;

    //dead tree object
    public GameObject deadTree;

    //live tree object
    public GameObject liveTree;

    //dead Purple Flowers
    public GameObject purpleWiltedFlowers1;
    public GameObject purpleWiltedFlowers2;

    //live Purple flowers
    public GameObject purpleLiveFlowers1;
    public GameObject purpleLiveFlowers2;

    //dead Red Flowers
    public GameObject redWiltedFlowers1;
    public GameObject redWiltedFlowers2;

    //live red flowers
    public GameObject redLiveFlowers1;
    public GameObject redLiveFlowers2;

    //wilted white flowers
    public GameObject whiteLiveFlowers1;
    public GameObject whiteLiveFlowers2;

    //dead white flowers
    public GameObject whiteDeadFlowers1;
    public GameObject whiteDeadFlowers2;

    //root object
    public GameObject root;

    public RuntimeAnimatorController happyAnim;

    //dead platform
    //public GameObject deadPlatform;

    //live platform
    //public GameObject livePlatform;

    //bool to check if you are in the trigger or not
    public bool inPlantTrigger;

    //bool to say that the puzzle is finished
    public bool puzzleDone;

    //Spirit
    public GameObject spirit;

    // Use this for initialization
    void Start () {
        //turn off the live plants
        liveTree.SetActive(false);
        purpleLiveFlowers1.SetActive(false);
        purpleLiveFlowers2.SetActive(false);
        redLiveFlowers1.SetActive(false);
        redLiveFlowers2.SetActive(false);
        whiteLiveFlowers1.SetActive(false);
        whiteLiveFlowers2.SetActive(false);
        //livePlatform.SetActive(false);
        root.SetActive(false);

        //set the boolean to false
        puzzleDone = false;

        //Turn off the NPC Spell script initially
        spirit.GetComponent<NPCSpells>().enabled = false;

	}
	
	// Update is called once per frame
	void Update () {
		
        if((speechRec.GetComponent<SpeechRecognition01>().word == "water" || speechRec.GetComponent<SpeechRecognition01>().word == "restore") && inPlantTrigger == true && liveTree.activeInHierarchy == false)
        {
            //Turn on all the plants
            liveTree.SetActive(true);
            purpleLiveFlowers1.SetActive(true);
            purpleLiveFlowers2.SetActive(true);
            redLiveFlowers1.SetActive(true);
            redLiveFlowers2.SetActive(true);
            whiteLiveFlowers1.SetActive(true);
            whiteLiveFlowers2.SetActive(true);
            root.SetActive(true);

            //set the puzzleDone to true
            puzzleDone = true;

            //move the root up
            //root.transform.position = Vector3.Lerp(root.transform.position, liveTree.transform.position, Time.deltaTime * 0.2f);

            //destroy all the dead plants
            deadTree.SetActive(false);
            Destroy(purpleWiltedFlowers1);
            Destroy(purpleWiltedFlowers2);
            Destroy(redWiltedFlowers1);
            Destroy(redWiltedFlowers2);
            Destroy(whiteDeadFlowers1);
            Destroy(whiteDeadFlowers2);
        }

        //if the puzzle is done then let the psirit be happy
        if(puzzleDone == true)
        {
            //Turn on the NPCSpells script
            spirit.GetComponent<NPCSpells>().enabled = true;

            //Turn the NPC into yellow when puzzle solved
            spirit.GetComponent<NPCSpells>().isPuzzle = true;

            //Turn off the spirit interaction script
            spirit.GetComponent<Spirit_Interaction>().enabled = false;

            //Replace with happy anim
            spirit.GetComponentInChildren<Animator>().runtimeAnimatorController = happyAnim;
        }

	}

    //checs if they are in the trigger
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            inPlantTrigger = true;
        }
    }

    //checs if they left the trigger
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            inPlantTrigger = false;
        }
    }

}

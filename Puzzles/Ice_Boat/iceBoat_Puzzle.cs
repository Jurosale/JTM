using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iceBoat_Puzzle : MonoBehaviour
{

    //Speech Recognition object
    SpeechRecognition01 speech;
    //puzzle spirit
    public GameObject spirit;
    //happy spirit animimation
    public RuntimeAnimatorController happyAnim;
    //Vivi character
    public GameObject vivi;
    //ice blocK for the bridge
    public GameObject ice;
    //the ruinedBoat object
    public GameObject ruinedBoat;
    //the fixed boat object
    public GameObject fixedBoat;
    //Still sail for boat
    public GameObject sail;
    //Animated sail for boat
    public GameObject sailAni;

    /*
    //IN THE FIXED BOAT HIERARCHY
    //Moving the boat left
    public GameObject moveLeft;
    //Moving the boat right
    public GameObject moveRight;
    //Collider on the left
    public GameObject leftSideCollider;
    //Collider on the right
    public GameObject rightSideCollider;
    //Trigger to turn off left boat collider when near the bridge
    public GameObject bridgeCollider;
    //Trigger to turn off right boat collider when by the island
    public GameObject islandCollider;
    */

    //So Restore doesn't teleport the boat back
    bool restored;

    // Use this for initialization
    void Start()
    {
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        //set the boat false in the beginning
        fixedBoat.SetActive(false);

        //turn off ruined boat colliders
        ruinedBoat.GetComponent<BoxCollider2D>().enabled = false;
        ruinedBoat.GetComponent<PolygonCollider2D>().enabled = false;

        //turns off animation for sail in particular
        sailAni.SetActive(false);

        //turn off script
        spirit.GetComponent<NPCSpells>().enabled = false;

        restored = false;
    }

    // Update is called once per frame
    void Update()
    {

        //checks if the ice disappeared
        if (ice.transform.localScale.y == 0 && ice.activeInHierarchy == true)
        {
            //set ice off
            ice.SetActive(false);
            //set the boat active (their colliders)
            ruinedBoat.GetComponent<BoxCollider2D>().enabled = true;
            ruinedBoat.GetComponent<PolygonCollider2D>().enabled = true;

        }
        //Restore the ruined boat back to its original form (restore must be casted/ice has to be gone/the runined boat must be enabled)
        if (speech.word == "restore" && ice.activeInHierarchy == false && ruinedBoat.activeSelf && !restored)
        {
            //remove previous boat
            ruinedBoat.SetActive(false);

            //set active the fixed Boat
            fixedBoat.SetActive(true);
            fixedBoat.GetComponent<UniversalSpells>().restoreOverride = true;
            restored = true;
            //make spirit happy
            spirit.GetComponentInChildren<Animator>().runtimeAnimatorController = happyAnim;
            spirit.GetComponent<NPCSpells>().enabled = true;
            spirit.GetComponent<NPCSpells>().isPuzzle = true;
            spirit.GetComponent<Spirit_Interaction>().enabled = false;

        }

        //brings back ice if ice is casted
        if(speech.word == "ice" && ice.activeInHierarchy == false && (ruinedBoat.GetComponent<BoxCollider2D>().isActiveAndEnabled))
        {
            //reset ice object
            ice.SetActive(true);
            ice.transform.localScale = new Vector3(0.2825065f, 0.3586008f, 1f);
            //turn off boats
            fixedBoat.SetActive(false);

            //set the boat active (their colliders)
            ruinedBoat.GetComponent<BoxCollider2D>().enabled = false;
            ruinedBoat.GetComponent<PolygonCollider2D>().enabled = false;

            //turns bac cutscne
            spirit.GetComponent<Spirit_Interaction>().enabled = true;
            spirit.GetComponent<NPCSpells>().enabled = false;
        }

        /*//Turn off the spirit interaction script once the puzzle is done
        if (spirit.GetComponent<NPCSpells>().isPuzzle == true && spirit.GetComponent<NPCSpells>().followAnim == true)
        {
            spirit.GetComponent<Spirit_Interaction>().enabled = false;
        }*/

    }


}

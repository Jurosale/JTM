using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixArtist : MonoBehaviour {

    SpeechRecognition01 speech;
    public GameObject pads, fish, artist, ice;
    public RuntimeAnimatorController happyAnim;
    RuntimeAnimatorController angryAnim;
    bool check;
    public bool isPondRestored;

    // Use this for initialization
    void Start () {
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        pads.SetActive(false);
        fish.SetActive(false);
        angryAnim = artist.GetComponentInChildren<Animator>().runtimeAnimatorController;
        check = true;
    }
	
	// Update is called once per frame
	void Update () {
        // if water reaches max height, add pond and fish
		if (transform.localPosition.y > -35 && check && ice.transform.localScale.y == 0)
        {
            pads.SetActive(true);
            fish.SetActive(true);
            artist.GetComponent<NPCSpells>().isPuzzle = true;
            artist.GetComponentInChildren<Animator>().runtimeAnimatorController = happyAnim;
            check = false;
            isPondRestored = true;
        }
        //dont let players reset water height 
        if(isPondRestored == true)
        {
            GetComponent<UniversalSpells>().canRestore = false;
        }
        //if water hasnt reached max height, dont add pond/fish
        else if (!check && (speech.word == "restore" || ice.transform.localScale.y != 0))
        {
            pads.SetActive(false);
            fish.SetActive(false);
            artist.GetComponent<NPCSpells>().isPuzzle = false;
            artist.GetComponentInChildren<Animator>().runtimeAnimatorController = angryAnim;
            check = true;
        }
	}
}

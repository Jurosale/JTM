using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnedHouse_SpiritPuzzle : MonoBehaviour {

    /*-----------------------------*/
    /*This script is specific to the first spirit puzzle in the human city*/
    /*-----------------------------*/

    //Objects for the first puzzle
    public GameObject childGhost;
    public GameObject puzzleSpirit;
    public GameObject nearestShrine;
    public GameObject water4;
    GameObject house;
    ParentSpeech ps;

    //can pray the spirit away
    public bool atShrine;
    public bool houseGone;
    public bool moveParent;

    public bool firstSpirit;

    private bool houseBurned;

    // Use this for initialization
    void Start () {

        childGhost.SetActive(false);
        atShrine = false;
        houseGone = false;
        houseBurned = false;
        house = GameObject.Find("House");
        ps = GameObject.Find("spirit_Puzzle1").GetComponent<ParentSpeech>();
        house.GetComponent<UniversalSpells>().restoreOverride = true;
        moveParent = false;
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(water4.transform.position.y);
        //Turns on the child and change the position of the puzzle spirit
        if (house.transform.localScale.y == 0 && !houseBurned)
        {
            gameObject.GetComponent<PuzzleComponent>().conditionMet = true;
            childGhost.SetActive(true);
            ps.enabled = true;
            houseGone = true;
            houseBurned = true;
        }

        //Change the position of the puzzle spirit
        if (houseGone == true && atShrine == false && moveParent)
        {
            //Animates the spirit to move by the shrine
            if (Vector2.Distance(puzzleSpirit.transform.position, nearestShrine.transform.position) < 5f &&
                Vector2.Distance(puzzleSpirit.transform.position, nearestShrine.transform.position) > 3f &&
                puzzleSpirit.transform.position.x > nearestShrine.transform.position.x) { atShrine = true; }
            puzzleSpirit.transform.position = Vector3.Lerp(puzzleSpirit.transform.position,
                nearestShrine.transform.position + new Vector3(2, 0, 0), Time.deltaTime * 0.3f);
            //atShrine = true;
            // houseGone = false;
        }
        
    }
}

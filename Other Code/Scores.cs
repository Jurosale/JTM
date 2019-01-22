using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Scores : MonoBehaviour {

    public int hPoints, hPages, hSpells;

	// Use this for initialization
	void Start () {
        string[] stuff = System.IO.File.ReadAllLines("Assets/HighScores!.txt");

        hPoints = Convert.ToInt32(stuff[0]);
        hPages = Convert.ToInt32(stuff[1]);
        hSpells = Convert.ToInt32(stuff[2]);
        //Debug.Log("hpoints: " + hPoints + "\nhpages: " + hPages);
    }
	
	// Update is called once per frame
    //keeps scores saved in internal file for permanent save
	void Update () {
		
	}

    public void keepScores ()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void updateScoreFile()
    {
        System.IO.File.WriteAllText("Assets/HighScores!.txt", string.Empty);
        string[] newstuff = { hPoints.ToString(), hPages.ToString(), hSpells.ToString() };
        System.IO.File.WriteAllLines("Assets/HighScores!.txt", newstuff);
    }
}

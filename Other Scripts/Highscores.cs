using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highscores : MonoBehaviour {

    public int highestScore, highestPages, highestSpells;
    UniversialVariables variables;

	// Use this for initialization
	void Start () {
        variables = GameObject.Find("Variables").GetComponent<UniversialVariables>();
        highestScore = GameObject.Find("Variables").GetComponent<Scores>().hPoints;
        highestPages = GameObject.Find("Variables").GetComponent<Scores>().hPages;
        highestSpells = GameObject.Find("Variables").GetComponent<Scores>().hSpells;
    }
	
	// Update is called once per frame
	void Update () {

    }

    //gets called when new game starts, game is completed, or player exits
    //updates highest scores
    public void UpdateScores ()
    {
        if (variables.currWordPnts > highestScore)
        {
            highestScore = variables.currWordPnts;
            GameObject.Find("Variables").GetComponent<Scores>().hPoints = highestScore;
            variables.currWordPnts = 0;
        }

        if (variables.currPages > highestPages)
        {
            highestPages = variables.currPages;
            GameObject.Find("Variables").GetComponent<Scores>().hPages = highestPages;
            variables.currPages = 0;
        }

        if (variables.currDiscovered.Count > highestSpells)
        {
            highestSpells = variables.currDiscovered.Count;
            GameObject.Find("Variables").GetComponent<Scores>().hSpells = highestSpells;
            variables.currDiscovered.Clear();
            variables.currType.Clear();
            variables.currCalled.Clear();
        }
        GameObject.Find("Variables").GetComponent<Scores>().updateScoreFile();
    }
}

//rip this idea

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;



public class CraftingSystem : MonoBehaviour {

    public Text prompt01;
    public Text promptAskFor2Spells;
	public bool readyNaming;
	public bool pass;

	public string[][] mixes;
	public float clock;
	public string reco;
    public int index;           //record the position of the 2 spells in the list
    //public const int spellNum = 2;
    //public const int spellNum2 = 3;

	public string word1 = "", word2 = "",word3 = ""; //The spells being mixed

	public DictationRecognizer dict;
	// Use this for initialization
	void Start () {
        //list of 2 spells can be mixed
        mixes = new string[][]
        {
            new string[]{"water","fire"}, 
            new string[]{"wind","wind"}
        };
        promptAskFor2Spells.enabled = false;
		dict = new DictationRecognizer ();
        dict.DictationResult += onDictationResult;
        dict.DictationHypothesis += onDictationHypothesis;
        dict.DictationComplete += onDictationComplete;
        dict.DictationError += onDictationError;
    }
    void onDictationResult(string text, ConfidenceLevel confidence)
    {
        // write your logic here
        Debug.LogFormat("Dictation result: " + text);
		reco = text;
		Debug.Log (reco);
    }

    void onDictationHypothesis(string text)
    {
        // write your logic here
        Debug.LogFormat("Dictation hypothesis: {0}", text);
    }

    void onDictationComplete(DictationCompletionCause cause)
    {
        // write your logic here
		if (cause != DictationCompletionCause.Complete) {
			Debug.LogErrorFormat ("Dictation completed unsuccessfully: {0}.", cause);
		}
    }

    void onDictationError(string error, int hresult)
    {
        // write your logic here
        Debug.LogErrorFormat("Dictation error: {0}; HResult = {1}.", error, hresult);
    }

    //check if the 2 words are in the list
	bool MatchWords(string w1, string w2)
	{
        index = 0;
        bool contain = false;
        for (int i = 0; i < mixes.Length; i++)
        {
            if (w1 == mixes[i][0])
            {
                if (w2 == mixes[i][1])
                {
                    contain = true;
                    index = i;
                    break;
                }
            }
            if (w2 == mixes[i][0])
            {
                if (w1 == mixes[i][1])
                {
                    contain = true;
                    index = i;
                    break;
                }
            }
        }
        return contain;
	}

    // Update is called once per frame
    void Update () {
		//test feature
		/*if (PhraseRecognitionSystem.Status.ToString() == "Running")
		{
			PhraseRecognitionSystem.Shutdown ();
			dict.Start ();
		}*/
        clock += Time.deltaTime;
        //Introduction Text
       
        if (clock > 2f && word1 == "")
        {
            prompt01.enabled = false;
            //Prompt player to say 2 spells they want to mix
            promptAskFor2Spells.enabled = true;
            promptAskFor2Spells.text = "Cast 2 spells that you want to mix.";
        }
        //  Make sure they have already "discovered" the spell
        if (GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().word != "" && word1 == "" && clock > 1f)
        {
            string thisWord = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().word;
            for (int i = 0; i < GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().discovered.Count; i++)
            {
                if (thisWord == GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().discovered[i])
                {
                    word1 = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().type[i];
                    clock = 0;
                    promptAskFor2Spells.text = "Cast 2 spells that you want to mix.\n " + word1 + " and ?";
                }
            }
        }
        else if (GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().word != "" && clock > 1f && word2 == "")
        {
            string thisWord = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().word;
            for (int i = 0; i < GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().discovered.Count; i++)
            {
                if (thisWord == GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().discovered[i])
                {
                    word2 = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().type[i];
                    Debug.Log(MatchWords(word1, word2));
                }
            }
        }
        
		else if (word1 != "" && word2 != "" && readyNaming == false)
        {
            //Check if the 2 spells are in the list
            promptAskFor2Spells.text = "Did you mix " + word1 + " and " + word2 + "?\n Yes or no?";
            if (GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().word == "yes")
            {
                //You mixed stuff
				if (MatchWords(word1,word2)) {
					readyNaming = true;
					clock = 0;
				} 
				else {
					word1 = "";
					word2 = "";
					promptAskFor2Spells.text = "Failed!";
				}

            }
            else if (GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().word == "no")
            {
                word1 = "";
                word2 = "";
                promptAskFor2Spells.text = "Cast 2 spells that you want to mix.";
            }
        }

        //Name the new spell, check if the name exists already
		if (readyNaming == true && word3 == "" && clock > 1f) {
			promptAskFor2Spells.text = "Name your new spell.";
			if (PhraseRecognitionSystem.Status.ToString() == "Running")
			{
				PhraseRecognitionSystem.Shutdown ();
				dict.Start ();
			}
			if (reco != "") {
				if (dict.Status.ToString () == "Running") {
					dict.Stop();
				}
				word3 = reco;

			}
		} else if (readyNaming == true && word3 != "") {
			int i = 0;
			while (i < GameObject.Find ("SpeechRecognition").GetComponent<SpeechRecognition01> ().keywords.Length) {
				if (word3 == GameObject.Find ("SpeechRecognition").GetComponent<SpeechRecognition01> ().keywords [i]) {
					promptAskFor2Spells.text = "\"" + word3 + "\""+  " already exists, Please choose another word.";
					clock = 0;
					reco = "";
					word3 = "";
					break;
				} else {
					if (i == GameObject.Find ("SpeechRecognition").GetComponent<SpeechRecognition01> ().keywords.Length - 1)
						pass = true;
					i++;
				}
			}
		}

        //change the name of this spell and enable using
		if (pass){
			if (dict.Status.ToString () == "Stopped") {
				PhraseRecognitionSystem.Restart ();
			}
			promptAskFor2Spells.text = "Did you say: \n" + word3 + "?";
			if (GameObject.Find ("SpeechRecognition").GetComponent<SpeechRecognition01> ().word == "yes") {
				//You mixed stuff
				GameObject.Find ("SpeechRecognition").GetComponent<SpeechRecognition01> ().keywords[119 + index] = word3;
				promptAskFor2Spells.text = "You made " + word3;
				word1 = "";
				word2 = "";
				word3 = "";
				reco = "";
				GameObject.Find ("SpeechRecognition").GetComponent<SpeechRecognition01> ().word = "";
				readyNaming = false;
				pass = false;
				GameObject.Find ("SpeechRecognition").GetComponent<SpeechRecognition01> ().crafted1 = true;
			} else if (GameObject.Find ("SpeechRecognition").GetComponent<SpeechRecognition01> ().word == "no") {
				word3 = "";
				reco = "";
				GameObject.Find ("SpeechRecognition").GetComponent<SpeechRecognition01> ().word = "";
				pass = false;
			}
		}
        //Debug.Log(word1);
       // Debug.Log(word2);
		//Debug.Log (word3);
		//Debug.Log (readyNaming);
		//Debug.Log (clock);
		//Debug.Log(dict.Status);
		//Debug.Log (PhraseRecognitionSystem.Status);
		//Debug.Log (pass);
        //Find that they have made a spell
        //Prompt them to name it using 1 word
        //  Make sure that it's not already a spell word
        //  Is this what you want to name it? Y/N
        //Congratulations

    }
}

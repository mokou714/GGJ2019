using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour {
    public int tutorialNum;
    public GameObject player;
    public GameObject playerIntro;
    public GameObject finger;
    public GameObject thumbup;
    public GameObject dustIntro;

    public GameObject firstPlanet;
    public GameObject secondPlanet;
    public GameObject thirdPlanet;
    public GameObject hints;
    public GameObject hint3;
    public GameObject tangent;
    private float dist_ratio;

    public GameObject obstacle;

    //Indicate if the current game is frozen
    private bool stopped =false;

    //Used to freeze the game
    private spacecraft player_sc;

    //The index of the hint on the second planet
    private int hintNum = 0;

    //CheckedList is to mark the steps that are already done, so next to it won't repeat
    private int checkedMark = 0;
    private string saveName = "";


    public Text textfield;
    private Part_1 part_1;
    Dictionary<string, hint> inst;


	// Use this for initialization
	void Start () {
        player_sc = player.transform.GetChild(0).GetComponent<spacecraft>();
        //Debug.Log(player_sc);
        saveName = "tu" + (tutorialNum + 1).ToString();
        checkedMark = GameStates.instance.GetTutorialData(saveName);

        Debug.Log("Checkmark: " + checkedMark);
        inst = new Dictionary<string, hint>();


        switch(tutorialNum){
            case 0:
                //Start page
                //printList();
                part_1 = new Part_1(player_sc, firstPlanet, secondPlanet, textfield, this);
                if(checkedMark < 1){
                    StartCoroutine(waitToShowStuff(playerIntro, 1f));
                }
                break;
            case 1:
                //Tutorial level 1
                //Debug.Log(dist_ratio);

                break;
            case 2:
                inst["energy"] = new hint("You loss energy while flying and\n your size indicates your energy", false, 100, 200);
                inst["dust"] = new hint("The dust around a planet gives\n you the energy.", false, 0, -200);
                break;
            case 3:
                inst["obstacle"] = new hint("The 'Space Eaters' will eat\n your energy!", false, -100, 200);
                break;
        }

	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(checkedMark);

        if(tutorialNum == 0){
            part_1.runtimeRoutine();
        }else if(player_sc.won && (tutorialNum == 1 || tutorialNum == 2 || tutorialNum == 3)){
            showText(null);
            if (tutorialNum == 1 || tutorialNum == 3)
            {
                player_sc.end.transform.Find("arrow").gameObject.SetActive(false);
            }
        }
        //Debug.Log(dist_ratio + ", " + player_ratio);
        if ( Input.GetKeyDown(KeyCode.Space) || (Input.touches.Length > 0 && Input.touches[0].phase == TouchPhase.Ended)){
            if(stopped){
                if(tutorialNum == 0){
                    if(playerIntro.activeSelf){
                        playerIntro.SetActive(false);
                    }

                }else if(tutorialNum == 3){
                    if (obstacle.activeSelf)
                        obstacle.SetActive(false);
                }
                stopped = false;
            }
        }

        if(player_sc.rotatingPlanet != null){
            if(tutorialNum == 0){
                part_1.onPlanetRoutine();
            }else if(tutorialNum == 1){
                if(checkedMark < 1){
                    hint start_campagin = new hint("Get here to start the campaign!", false, 250, -100);
                    showText(start_campagin);
                    player_sc.end.transform.Find("arrow").gameObject.SetActive(true);
                    checkedMark++;
                    GameStates.instance.SaveTutorialData(checkedMark, saveName);
                }

            }else if(tutorialNum == 2){
                if (player_sc.rotatingPlanet == secondPlanet && checkedMark < 1){
                    showText(inst["energy"]);
                    checkedMark++;
                    GameStates.instance.SaveTutorialData(checkedMark, saveName);
                }
                if (player_sc.rotatingPlanet == thirdPlanet &&  checkedMark < 2)
                {
                    showText(inst["dust"]);
                    checkedMark++;
                    GameStates.instance.SaveTutorialData(checkedMark, saveName);
                }
            } else if(tutorialNum == 3){
                if (player_sc.rotatingPlanet == firstPlanet && checkedMark < 1)
                {
                    showText(inst["obstacle"]);
                    player_sc.end.transform.Find("arrow").gameObject.SetActive(true);
                    checkedMark++;
                    GameStates.instance.SaveTutorialData(checkedMark, saveName);
                }
            }

        }



	}

    private void showHints(){
        //Debug.Log("Show hints");
        if (hintNum >= hints.transform.childCount)
            return;
        if(hintNum > 0)
            hints.transform.GetChild(hintNum-1).gameObject.SetActive(false);
        hints.transform.GetChild(hintNum).gameObject.SetActive(true);
        hintNum++;
    }

    IEnumerator waitToHints(float time)
    {
        /*
        Todo: this coroutine to show instruction after the player lands on the second planet
        */
        yield return new WaitForSeconds(time);
        stopped = true;
        player_sc.requiredSleep = true;
        showHints();
    }


    IEnumerator waitToShowStuff(GameObject obj, float time)
    {
        /*
        Todo: this coroutine is used wait to show warning for obstacles
        */
        yield return new WaitForSeconds(time);
        obj.SetActive(true);
        stopped = true;
        checkedMark++;
    }

    IEnumerator waitToShowEnd(GameObject obj, float time)
    {
        /*
        Todo: this coroutine is used wait to show warning for obstacles
        */
        yield return new WaitForSeconds(time);

    }


    IEnumerator waitToEnableTap()
    {
        /*
        Todo: this coroutine is used enable tap detection after some time the instruction dialog is gone so that the player does not overclick
        */
        yield return new WaitForSeconds(0.4f);
        player_sc.requiredSleep = false;
    }

    IEnumerator loadText(string text, int i, float time_interval)
    {
        yield return new WaitForSeconds(time_interval);
        textfield.text = textfield.text + text[i];
        if (i < text.Length - 1)
            StartCoroutine(loadText(text, i + 1, time_interval));
    }

    private void showText(hint text, float time_interval = 0.01f){
        if (text == null){
            textfield.text = "";
            return;
        }
        if (text.showed)
            return;
        Debug.Log(text.content + "," + text.x + "," + text.y);

        textfield.rectTransform.localPosition = new Vector2(text.x, text.y);
        textfield.text = "";
        StartCoroutine(loadText(text.content, 0, time_interval));
        text.showed = true;
    }

}


class hint{
    public string content;
    public bool showed;
    public float x;
    public float y;

    public hint(string content, bool showed = false, float x = -200, float y = 5){
        this.content = content;
        this.showed = showed;
        this.x = x;
        this.y = y;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour {
    public GameObject player;
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

    public GameObject obstable;

    //Indicate if the current game is frozen
    private bool stopped =false;

    //Used to freeze the game
    private float savedTimeScale;
    private spacecraft player_sc;

    //The index of the hint on the second planet
    private int hintNum = 0;

    //CheckedList is to mark the steps that are already done, so next to it won't repeat
    private bool []checkedList;
    private int checkCursor = 0;
    private int tutorialNum = 0;

	// Use this for initialization
	void Start () {
        savedTimeScale = Time.timeScale;
        player_sc = player.transform.GetChild(0).GetComponent<spacecraft>();
        if(finger != null && thumbup != null){
            dist_ratio = (secondPlanet.transform.position.y - firstPlanet.transform.position.y) / (secondPlanet.transform.position.x - firstPlanet.transform.position.x);
            checkedList = new bool[4];
            for (int i = 0; i < checkedList.Length; i++)
            {
                checkedList[i] = false;
            } 
        }else{
            tutorialNum = 1;
            StartCoroutine(waitToShowObstable());
        }



	}
	
	// Update is called once per frame
	void Update () {
        float player_ratio = player.GetComponent<Rigidbody2D>().velocity.y/player.GetComponent<Rigidbody2D>().velocity.x;

        //Debug.Log(dist_ratio + ", " + player_ratio);
        if ( Input.GetKeyDown(KeyCode.Space))
        {
            if(stopped){
                if(tutorialNum == 0){
                    if (finger.activeSelf)
                    {
                        //Actions for the first step
                        finger.SetActive(false);
                        tangent.SetActive(false);
                        stopped = false;
                        resume();
                        player_sc.requiredStop = false;
                        checkedList[checkCursor] = true;
                        checkCursor++;
                    }
                    else if (dustIntro.activeSelf)
                    {
                        //Actions for the third step
                        resume();
                        dustIntro.SetActive(false);
                        checkedList[checkCursor] = true;
                        checkCursor++;
                        StartCoroutine(waitToEnableTap());

                    }

                    if (0 < hintNum && hintNum < hints.transform.childCount)
                    {
                        //Actions for after each small step in the second step
                        stopped = true;
                        showHints();
                        return;
                    }

                    if (hintNum >= hints.transform.childCount)
                    {
                        //Actions for after the second step
                        //Debug.Log("Unlock hints" + hints.transform.childCount);
                        hints.transform.GetChild(hintNum - 1).gameObject.SetActive(false);
                        resume();

                        StartCoroutine(waitToEnableTap());
                    }
                    Debug.Log("Check " + checkCursor);

                }else if(tutorialNum == 1){
                    if (obstable.activeSelf)
                        obstable.SetActive(false);
                    resume();
                }
                stopped = false;
            }
        }

        if(player_sc.rotatingPlanet != null){
            if(tutorialNum == 0){
                if (player_sc.rotatingPlanet.name == secondPlanet.name && !thumbup.activeSelf && !checkedList[1])
                {
                    //Detect when the player lands on the second planet
                    checkedList[checkCursor] = true;
                    checkCursor++;
                    StartCoroutine(waitToHints());
                    //thumbup.SetActive(true);
                    //pause();
                    //stopped = true;
                    //player_sc.requiredStop = true;
                    //showHints(hintNum);
                    //waitToHints();
                }
                else if (player_sc.rotatingPlanet == firstPlanet && !finger.activeSelf && !checkedList[0])
                {
                    //Detect when the player on the first planet is on a good position to shoot onto the second planet
                    if (Mathf.Abs(-(player_ratio - dist_ratio)) < 0.01f)
                    {
                        finger.SetActive(true);
                        tangent.SetActive(true);
                        //spacecraft.GetComponent<Rigidbody2D>().freezeRotation;
                        pause();
                        stopped = true;
                        //Debug.Log("Show hint");
                    }
                }
                if ((player_sc.rotatingPlanet != secondPlanet || player_sc.rotatingPlanet == null) && thumbup.activeSelf)
                {
                    //After the player leaves the second planet
                    thumbup.SetActive(false);
                }
                else if (player_sc.rotatingPlanet == thirdPlanet && !dustIntro.activeSelf && !checkedList[2])
                {
                    pause();
                    stopped = true;
                    dustIntro.SetActive(true);
                    player_sc.requiredStop = true;
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

    IEnumerator waitToHints()
    {
        /*
        Todo: this coroutine to show instruction after the player lands on the second planet
        */
        yield return new WaitForSeconds(0.8f);
        stopped = true;
        player_sc.requiredStop = true;
        pause();
        showHints();
    }

    IEnumerator waitToShowObstable()
    {
        /*
        Todo: this coroutine is used wait to show warning for obstables
        */
        yield return new WaitForSeconds(1f);
        pause();
        obstable.SetActive(true);
        stopped = true;
    }


    IEnumerator waitToEnableTap()
    {
        /*
        Todo: this coroutine is used enable tap detection after some time the instruction dialog is gone so that the player does not overclick
        */
        Debug.Log("Wait to hint");
        yield return new WaitForSeconds(0.4f);
        player_sc.requiredStop = false;
    }

    public void pause()
    {
        //Debug.Log("Pause");
        //transform.GetChild(0).GetComponent<TrailRenderer>().time = Mathf.Infinity;
        Time.timeScale = 0;
    }

    public void resume()
    {
        Time.timeScale = savedTimeScale;
    }

    public static bool TouchRelease()
    {
        bool b = false;
        for (int i = 0; i < Input.touches.Length; i++)
        {
            b = Input.touches[i].phase == TouchPhase.Ended;
            if (b)
                break;
        }
        return b;
    }
}

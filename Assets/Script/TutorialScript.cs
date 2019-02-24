using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private float savedTimeScale;
    private spacecraft player_sc;

    //The index of the hint on the second planet
    private int hintNum = 0;

    //CheckedList is to mark the steps that are already done, so next to it won't repeat
    private int checkedMark = 0;
    private int checkCursor = 0;
    private string saveName = "";

    private bool init = true;

	// Use this for initialization
	void Start () {
        savedTimeScale = Time.timeScale;
        player_sc = player.transform.GetChild(0).GetComponent<spacecraft>();
        //Debug.Log(player_sc);
        saveName = "tu" + (tutorialNum + 1).ToString();
        checkedMark = GameStates.instance.GetTutorialData(saveName);

        switch(tutorialNum){
            case 0:
                //Start page
                //printList();
                if(checkedMark < 1){
                    StartCoroutine(waitToShowStuff(playerIntro, 1f));
                }
                break;
            case 1:
                //Tutorial level 1
                dist_ratio = (secondPlanet.transform.position.y - firstPlanet.transform.position.y) / (secondPlanet.transform.position.x - firstPlanet.transform.position.x);
                //Debug.Log(dist_ratio);
                break;
            case 2:
                //Tutorial level 2
                StartCoroutine(waitToShowStuff(obstacle, 1f));
                break;
        }




	}
	
	// Update is called once per frame
	void Update () {

        //Debug.Log(dist_ratio + ", " + player_ratio);
        if ( Input.GetKeyDown(KeyCode.Space) || (Input.touches.Length > 0 && Input.touches[0].phase == TouchPhase.Ended)){
            if(stopped){
                if(tutorialNum == 0){
                    if(playerIntro.activeSelf){
                        playerIntro.SetActive(false);
                        resume();
                    }
                    if (0 < hintNum && hintNum < hints.transform.childCount){
                        //Actions for after each small step in the second step
                        stopped = true;
                        showHints();
                        return;
                    }

                    if (hintNum >= hints.transform.childCount){
                        //Actions for after the second step
                        //Debug.Log("Unlock hints" + hints.transform.childCount);
                        hints.transform.GetChild(hintNum - 1).gameObject.SetActive(false);
                        resume();

                        StartCoroutine(waitToEnableTap());
                    }


                }else if(tutorialNum == 1){
                    if (finger.activeSelf)
                    {
                        //Actions for the first step
                        finger.SetActive(false);
                        tangent.SetActive(false);
                        stopped = false;
                        resume();
                        player_sc.requiredStop = false;

                        checkedMark++;
                        //checkCursor++;
                    }
                    else if (dustIntro.activeSelf)
                    {
                        //Actions for the third step
                        resume();
                        dustIntro.SetActive(false);

                        checkedMark++;
                        //checkCursor++;
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
                        checkedMark++;
                        //checkCursor++;
                        StartCoroutine(waitToEnableTap());
                    }
                    //Debug.Log("Check " + checkCursor);

                }else if(tutorialNum == 2){
                    if (obstacle.activeSelf)
                        obstacle.SetActive(false);
                    resume();
                }
                stopped = false;
            }
        }

        if(player_sc.rotatingPlanet != null){
            if(tutorialNum == 0){
                if (player_sc.rotatingPlanet == firstPlanet && !finger.activeSelf && checkedMark < 2){
                    //Debug.Log("roatating " + player_sc.rotatingPlanet + " " + checkedList[0] + finger.activeSelf);
                    //printList();
                    checkedMark++;
                    StartCoroutine(waitToHints(0.5f));
                    GameStates.instance.SaveTutorialData(checkedMark, saveName);
                }
            }else if(tutorialNum == 1){
                if ((player_sc.rotatingPlanet != secondPlanet || player_sc.rotatingPlanet == null) && thumbup.activeSelf && checkedMark >= 2){
                    //After the player leaves the second planet
                    thumbup.SetActive(false);
                }else if (player_sc.rotatingPlanet == firstPlanet && !finger.activeSelf && checkedMark < 1){
                    //Detect when the player on the first planet is on a good position to shoot onto the second planet
                    float player_ratio = player.GetComponent<Rigidbody2D>().velocity.y / player.GetComponent<Rigidbody2D>().velocity.x;
                    //Debug.Log(player_ratio + "," + dist_ratio);
                    if (Mathf.Abs((player_ratio - dist_ratio)) < 0.05f)
                    {
                        finger.SetActive(true);
                        tangent.SetActive(true);
                        //spacecraft.GetComponent<Rigidbody2D>().freezeRotation;
                        pause();
                        stopped = true;
                        //Debug.Log("Show hint");
                        checkedMark++;
                        GameStates.instance.SaveTutorialData(checkedMark, saveName);
                    }
                }else if (player_sc.rotatingPlanet == thirdPlanet && !dustIntro.activeSelf && checkedMark < 3){
                    pause();
                    stopped = true;
                    dustIntro.SetActive(true);
                    player_sc.requiredStop = true;
                    checkedMark++;
                    GameStates.instance.SaveTutorialData(checkedMark, saveName);
                    //checkCursor++;
                }
            } 
            if (player_sc.rotatingPlanet == secondPlanet && !thumbup.activeSelf && checkedMark < 2){
                if (!thumbup.activeSelf){
                    thumbup.SetActive(true);
                    //Detect when the player lands on the second planet
                    //checkCursor++;
                    checkedMark++;
                    StartCoroutine(waitToHints(0.5f));
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
        player_sc.requiredStop = true;
        pause();
        showHints();
    }

    IEnumerator waitToShowStuff(GameObject obj, float time)
    {
        /*
        Todo: this coroutine is used wait to show warning for obstacles
        */
        yield return new WaitForSeconds(time);
        pause();
        obj.SetActive(true);
        stopped = true;
        checkedMark++;
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

    public static bool TouchRelease(){
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

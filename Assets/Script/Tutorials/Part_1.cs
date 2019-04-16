using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class Part_1 {
    public spacecraft player_sc;
    public GameObject firstPlanet;
    public GameObject secondPlanet;
    public int checkedMark;
    public Text textfield;
    Dictionary<string, hint> inst;
    public MonoBehaviour monoBehaviour;

    public Part_1(spacecraft player_sc, GameObject firstPlanet, GameObject secondPlanet,Text textfield, MonoBehaviour monoBehaviour)
    {
        this.player_sc = player_sc;
        this.firstPlanet = firstPlanet;
        this.secondPlanet = secondPlanet;
        this.textfield = textfield;

        this.monoBehaviour = monoBehaviour;
        Dictionary<string, hint> instr = new Dictionary<string, hint>();
        instr["release"] = new hint("Relase to launch yourself");
        instr["hold"] = new hint("First, press and hold your screen.");
        instr["compliment"] = new hint("Good job!", false, 300, 150);
        instr["goal_1"] = new hint("Get here!", false, 200, 200);
        instr["goal_2"] = new hint("Now get here!", false, 400, 300);
        instr["congrat"] = new hint("Well done, let's get into the game!", false, 0, 200);

        this.inst = instr;
    }

    public void runtimeRoutine(){
        if (player_sc.dead)
        {

            if (checkedMark < 3)
            {
                if (!inst["goal_1"].showed)
                {
                    secondPlanet.transform.Find("arrow").gameObject.SetActive(true);
                }
                showText(inst["goal_1"]);
            }
            else if (checkedMark >= 3 && checkedMark < 4)
            {

                if (!inst["goal_2"].showed)
                {
                    player_sc.end.transform.Find("arrow").gameObject.SetActive(true);
                }
                showText(inst["goal_2"]);
            }

        }
        if(player_sc.won){
            Debug.Log("Won!");
            monoBehaviour.StartCoroutine(showEnd(0, inst["congrat"], null));
        }

    }

    public void onPlanetRoutine(){
        if (player_sc.rotatingPlanet == firstPlanet)
        {
            if (player_sc.touchHold() && player_sc.playerModel.pressTime > 1f)
            {
                showText(inst["release"], 0);

            }else
            {
                showText(inst["hold"], 0);
            }
            //Debug.Log("roatating " + player_sc.rotatingPlanet + " " + checkedList[0] + finger.activeSelf);
            //printList();

            if (player_sc.end.transform.Find("arrow").gameObject.activeSelf)
            {
                player_sc.end.transform.Find("arrow").gameObject.SetActive(false);
                showText(null, 0);
            }

            if (checkedMark < 2)
            {
                checkedMark++;
            }
        }
        else if (player_sc.rotatingPlanet == secondPlanet && checkedMark < 3)
        {
            //if(textfield.text != "")
            //showText(null, 0);
            showText(inst["compliment"]);

            monoBehaviour.StartCoroutine(showEnd(1, inst["goal_2"],player_sc.end.transform.Find("arrow").gameObject));
            //showText(inst["goal_2"]);

            if (secondPlanet.transform.Find("arrow").gameObject.activeSelf)
            {
                secondPlanet.transform.Find("arrow").gameObject.SetActive(false);
                //showText(null, 0);
            }

            checkedMark++;
            Debug.Log("checked: " + checkedMark);
        }
    }

    private void showText(hint text, float time_interval = 0.01f)
    {
        if (textfield == null)
            textfield = GameObject.FindGameObjectWithTag("tutorialText").GetComponent<Text>();
        if (text == null)
        {
            textfield.text = "";
            return;
        }
        if (text.showed)
            return;
        textfield.rectTransform.localPosition = new Vector2(text.x, text.y);
        textfield.text = "";
        monoBehaviour.StartCoroutine(loadText(text.content, 0, time_interval));
        text.showed = true;
    }

    IEnumerator loadText(string text, int i, float time_interval)
    {
        yield return new WaitForSeconds(time_interval);
        textfield.text = textfield.text + text[i];
        if (i < text.Length - 1)
            monoBehaviour.StartCoroutine(loadText(text, i + 1, time_interval));
    }

    IEnumerator showEnd(float time_interval, hint h, GameObject obj)
    {
        yield return new WaitForSeconds(time_interval);
        showText(h);
        if (obj != null && !obj.activeSelf)
            obj.SetActive(true);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelSelection : MonoBehaviour {


    float screenW;
    float screenH;
    float selfW;
    float selfH;
    Vector2 startPos;

    public bool shaking;
    public bool startSelecting;
    public bool startDeselecting;
    public float showingSpeed;
    public float shakingSpeed;
    public float shakingRange;
    float T;

    

	// Use this for initialization
	void Start () {
        screenH = Screen.height;
        screenW = Screen.width;

        Rect currentRect = GetComponent<RectTransform>().rect;
        selfW = currentRect.width;
        selfH = currentRect.height;



        Rect newRect = new Rect();
        newRect.height = selfH;
        newRect.width = selfW;
        startPos = new Vector2(screenW / 2 + selfW / 2, 0f);
        newRect.position = startPos;

        Debug.Log(startPos);

        GetComponent<RectTransform>().localPosition = startPos;




    }
	
	// Update is called once per frame
	void Update () {
        if (startSelecting) {
            Select();
        }
        else if(startDeselecting){
            Deselect();
        }


    }

    void Select() {
        //GetComponent<RectTransform>().rect.Set(startPos.x, startPos.y, selfW, selfH);
        Vector2 currentPos = GetComponent<RectTransform>().localPosition;

        if(!shaking)
        {
            Vector2 newPos = new Vector2(currentPos.x - Time.deltaTime * showingSpeed * 1000f, currentPos.y);
            GetComponent<RectTransform>().localPosition = newPos;
            if (currentPos.x < 80f)
                shaking = true;
        }
        //shaking now
        else {
            T += Time.deltaTime * shakingSpeed;
            Vector2 newPos = new Vector2(currentPos.x - Mathf.Sin(T) * shakingRange, currentPos.y);
            GetComponent<RectTransform>().localPosition = newPos;

            //move left, 
            if (Mathf.Sin(T) > 0 && newPos.x < 0 && Mathf.Abs(newPos.x) - 0f < 5f) {
                //GetComponent<RectTransform>().localPosition = startPos;
                GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                shaking = false;
                startSelecting = false;
            }

        }

        //if (shaking) {
        //    StartCoroutine(wait());
        //}



        Debug.Log("!!!!");
    }

    void Deselect() {
        Vector2 currentPos = GetComponent<RectTransform>().localPosition;
        Vector2 newPos = new Vector2(currentPos.x + Time.deltaTime * showingSpeed * 1000f, currentPos.y);
        GetComponent<RectTransform>().localPosition = newPos;
        if (currentPos.x > startPos.x)
        {
            GetComponent<RectTransform>().localPosition = startPos;
            startDeselecting = false;
        }

    }

    IEnumerator wait() {
        yield return new WaitForSeconds(0.2f);
        shaking = false;
        startSelecting = false;
        //GetComponent<RectTransform>().localPosition = startPos;

    }
}

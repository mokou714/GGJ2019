using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class levelPlanet : MonoBehaviour {



    public int level;

    public bool showingSelection;
    public bool hidingSelection;

    public GameObject levelLinePrefab;
    public float minZoomInFOV;
    private GameObject levelLine;
    private IEnumerator showLevel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (showingSelection) {
            //store a copy of showlevels coroutine
            showLevel = showLevels();
            StartCoroutine(showLevel);
            showingSelection = false;
        }
        else if (hidingSelection) {
            for (int i = 0; i < transform.GetChild(2).childCount; ++i)
            {
                transform.GetChild(2).GetChild(i).gameObject.SetActive(false);
            }
            //stop showing level
            StopCoroutine(showLevel);
            Destroy(levelLine);
            hidingSelection = false;
        }





        //playe enters
        //if(GetComponent<dustPlanet>().thePlayerOnPlanet != null && showingSelection) {

        //    canvas.transform.GetChild(0).GetComponent<levelSelection>().startSelecting = true;
        //    showingSelection = false;
        //}
        //player leaves
        //else if(GetComponent<dustPlanet>().thePlayerOnPlanet == null && !showingSelection) {
        //    canvas.transform.GetChild(0).GetComponent<levelSelection>().startSelecting = false;
        //    showingSelection = true;
        //    canvas.transform.GetChild(0).GetComponent<levelSelection>().startDeselecting = true;

        //}

    }

    IEnumerator showLevels() {
        levelLine = Instantiate(levelLinePrefab);
        Transform levels = transform.GetChild(2);
        levelLine.GetComponent<LineRenderer>().positionCount += 1;
        levelLine.GetComponent<LineRenderer>().SetPosition(0, levels.GetChild(0).position);
        levels.GetChild(0).gameObject.SetActive(true);

        levelLine.SetActive(true);


        for (int i=1; i<levels.childCount; ++i) {

            //check if there is another des for line to go
            //if (i + 1 < levels.childCount) {
            levels.GetChild(i).gameObject.SetActive(true);
            levelLine.GetComponent<LineRenderer>().positionCount += 1;
            levelLine.GetComponent<LineRenderer>().SetPosition(i, levelLine.GetComponent<LineRenderer>().GetPosition(i-1));


            while (Vector3.Distance(levelLine.GetComponent<LineRenderer>().GetPosition(i),
             levels.GetChild(i).position) > 0.1f
            )
            {
                Vector3 midPos = Vector3.Lerp(levelLine.GetComponent<LineRenderer>().GetPosition(i), levels.GetChild(i).position
                        , 0.1f);

                levelLine.GetComponent<LineRenderer>().SetPosition(i, midPos);
                yield return new WaitForSeconds(0.001f);

            }


            levelLine.GetComponent<LineRenderer>().SetPosition(i , levels.GetChild(i).position);
            
            //}
        }


        //showingSelection = false;
    }


}

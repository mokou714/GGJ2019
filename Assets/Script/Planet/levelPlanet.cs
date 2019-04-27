using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class levelPlanet : Planet {

    //public float catchRadius;
    public int level;

    public bool showingSelection;
    public bool hidingSelection;

    public GameObject levelLinePrefab;
    public float minZoomInFOV;
    private GameObject levelLine;
    private IEnumerator showLevel;

    public int progress;
    private Transform planet2;
    private Transform particle;

    private Vector3 size;
    private Vector3 orig_particle_size;

    public GameObject title;

    private int bigNumber;

    private bool landed = false;
    private Transform planetName; 
	// Use this for initialization
	void Start () {
        setup();
        bigNumber = int.Parse(name);
        planetName = transform.Find("title");
        planet2 = transform.Find("Planet2");

	}
	
	// Update is called once per frame
	void Update () {
        checkCatching();
        if(thePlayerOnPlanet == null && landed){
            title.SetActive(true);
            for (int i = 0; i < transform.GetChild(2).childCount; ++i)
            {
                transform.GetChild(2).GetChild(i).gameObject.SetActive(false);
            }
            //stop showing leve
            StopCoroutine(showLevel);
            Destroy(levelLine);
            planetName.gameObject.SetActive(false);
            landed = false;
        }
    }

    public override void catchedAction(spacecraft sc)
    {
        showLevel = showLevels();
        StartCoroutine(showLevel);
        title.SetActive(false);
        landed = true;

        planetName.gameObject.SetActive(true);
        return;
    }
    public override void playerLeave()
	{
        base.playerLeave();
	}


	IEnumerator showLevels() {
        Debug.Log("Show levels:" + progress);
        levelLine = Instantiate(levelLinePrefab);
        Transform levels = transform.GetChild(2);
        levelLine.GetComponent<LineRenderer>().positionCount += 1;
        levelLine.GetComponent<LineRenderer>().SetPosition(0, levels.GetChild(0).position);
        levels.GetChild(0).gameObject.SetActive(true);

        levelLine.SetActive(true);


        int to_load = progress / 10 >= bigNumber ? 10 : progress % 10;

        for (int i=1; i < to_load; ++i) {

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

    public void showUp(){
        if(planet2 == null){
            planet2 = transform.Find("Planet2");
            size = planet2.localScale;
            particle = planet2.GetChild(0);
            orig_particle_size = particle.localScale;
        }
            
        //planet2.localScale = Vector3.zero;
        print("Pop out");
        StartCoroutine(popOut(0));
    }

    IEnumerator popOut(float ratio)
    {
        planet2.localScale = size * ratio;
        particle.localScale = orig_particle_size * ratio;
        yield return new WaitForSeconds(0.01f);
        if (ratio < 1)
        {
            StartCoroutine(popOut(ratio + 0.01f));
        }else{
            catchRadius = planet2.localScale.x;
        }
    }
  
}

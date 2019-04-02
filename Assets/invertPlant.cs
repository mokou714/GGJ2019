using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class invertPlant : Planet {


    public bool invBack;
    public GameObject next;
    private spacecraft sc_player;

    private bool move = false;
    private bool playerReached = false;

    public Canvas canvas;

    void Start(){
        setup();

        catchRadius = catchRadius * 3 + 0.3f;
    }

	void Update(){
        if(move && sc_player.camera.transform.position != next.transform.position){
            float step = 10 * Time.deltaTime; // calculate distance to move
            sc_player.camera.transform.position = Vector3.MoveTowards(sc_player.camera.transform.position, next.transform.position, step);
        }
	}

	public override void catchedAction(spacecraft sc)
    {
        base.catchedAction(sc);
        if(!playerReached && SceneManager.GetActiveScene().name == "2-start"){
            playerReached = true;
            Debug.Log("Player reached");
            sc.spawnPoint = transform.position - new Vector3(5, 1, 0);
            sc.playerModel.init();
            GameStates.instance.SaveLevel(curID: SceneManager.GetActiveScene().buildIndex - 3);
        }
        if(!invBack)
        {
            sc.camera.transform.GetChild(0).GetComponent<colorInverter>().inverting = true;
            StartCoroutine(invertActions(sc));
        }
        else{
            sc.camera.transform.GetChild(0).GetComponent<colorInverter>().invertingBack = true;
        }
    }

    IEnumerator invertActions(spacecraft sc)
    {
        sc_player = sc;
        yield return new WaitUntil(()=>!sc.camera.transform.GetChild(0).GetComponent<colorInverter>().inverting);
        yield return new WaitForSeconds(1f);
        sc.camera.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

        if (canvas != null)
            canvas.GetComponent<UIManager>().menuButtonInvert();

        for (int i = 0; i < transform.parent.childCount; i++)
        {
            
            GameObject ob = transform.parent.GetChild(i).gameObject;

            if (ob != gameObject && ob.tag != "Player" && ob.tag != "end" && ob.tag != "MainCamera" && ob.tag != "background")
            {
                Debug.Log(ob.tag);
                ob.SetActive(false);
            }
        }
        yield return new WaitForSeconds(0.1f);

            
        sc.camera.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        //StartCoroutine(moveCamera());
        move = true;
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainedAnimation : MonoBehaviour {

    public Transform player;
    private Vector3 origPos;
    private Vector3 origScale;
    public Transform origTrans;

    public bool player_dead = false;
    private Transform player_trans;

    public bool growing = false;

    public spacecraft player_sc;

	// Use this for initialization
	void Start () {
        origTrans = transform.parent;
        origPos = transform.position;
        origScale = transform.localScale;
        //Debug.Log("SC player:" + transform.parent.parent.parent.gameObject.GetComponent<BadgeManager>().sc_player);
        player_sc = transform.parent.parent.parent.gameObject.GetComponent<BadgeManager>().sc_player;
	}
	
	// Update is called once per frame
	void Update () {  
        if(transform.parent.name == "Planet2" && !growing && player_sc.wonAward.Length < 1){
            moveBack();
        }
	}

    IEnumerator zoom(float time, float curTime){            
        transform.localScale *= 1.01f;
        yield return new WaitForSeconds(0.01f);
        if (curTime <= time && !player_dead)
            StartCoroutine(zoom(time, curTime + 0.05f));
        yield return 0;
    }

    public IEnumerator moveStart(Transform to, int dir){
        if (player_trans == null)
            player_trans = to;
        float maxTime = 3f;
        float currentTime = 0f;
        if (dir == -1){
            transform.SetParent(origTrans);
            transform.parent.parent.gameObject.GetComponent<GoldenPlanet>().awardAvailable = true;
        }else{
            if (player_dead)
                player_dead = false;
        }
        while (currentTime < maxTime){
            //print(numAlive);
            if (currentTime == 0 && dir == 1){
                growing = true;
                Vector3 pVel = to.position - transform.position;
                StartCoroutine(zoom(2f, 0));
                yield return new WaitForSeconds(3f);
                currentTime += Time.deltaTime;

                growing = false;
                if (transform.parent.parent.GetComponent<Planet>().thePlayerOnPlanet != null)
                {
                    transform.SetParent(transform.parent.parent.GetComponent<Planet>().thePlayerOnPlanet.transform);

                }
            }else{
                if (dir == 1)
                {
                    Debug.Log(transform.parent);

                    if(player_sc.dead || player_sc.wonAward.Length < 1){
                        moveBack();
                        break;
                    }
                    transform.localScale -= dir * transform.localScale * (currentTime / maxTime);
                    transform.position = Vector3.Lerp(transform.position, to.position, (currentTime / maxTime));

                }else{
                    transform.localScale = origScale * (currentTime / maxTime);
                    transform.position = Vector3.Lerp(transform.position, origTrans.position, (currentTime / maxTime));
                }
                currentTime += Time.deltaTime;

            }
            yield return 0;
        }

    }


    public void moveBack(){
        float maxTime = 3f;
        float currentTime = 0f;
        transform.SetParent(origTrans);

        while (currentTime < maxTime){ 
            transform.localScale = origScale * (currentTime / maxTime);
            transform.position = Vector3.Lerp(transform.position, origTrans.position, (currentTime / maxTime));
            currentTime += Time.deltaTime;
        }
        transform.parent.parent.gameObject.GetComponent<GoldenPlanet>().awardAvailable = true;
    }
}

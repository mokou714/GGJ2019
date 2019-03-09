using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Collision : MonoBehaviour
{

    public float collide_strengh;
    public bool collided;
    public float asteroidDamage;
    public UIEffect UIeffect;

    public ParticleSystem energyLossOnCollide;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }

    private void LateUpdate()
    {
        if (collided)
        {
            gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
            collided = false;

            if (energyLossOnCollide)
                energyLossOnCollide.Play();
        }
    }

    void OnCollisionEnter2D(Collision2D col){
        float damage = 0;
        string hit_obj = col.gameObject.tag;
        switch (hit_obj)
        {
            case "asteroid":
                damage = col.gameObject.GetComponent<Asteroid>().damage;
                StartCoroutine(playerBlink());
                //UIeffect.blink = true;
                break;

            case "orbaerolite":
                damage = col.gameObject.GetComponent<orbitAsteroid>().damage;
                StartCoroutine(playerBlink());
                //UIeffect.blink = true;
                break;

            default:
                return;

        }
        col.gameObject.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity * collide_strengh;
        transform.GetChild(0).GetComponent<spacecraft>().energy -= damage;
        transform.GetChild(0).GetChild(0).GetComponent<TrailRenderer>().time = transform.GetChild(0).GetComponent<spacecraft>().energy / 100f;
        collided = true;
        AudioManager.instance.PlaySFX("being hit");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Finish"){
            if (transform.GetChild(0).GetComponent<spacecraft>().dead)
                return;
            AudioManager.instance.PlaySFX("Next Level");
            transform.GetChild(0).GetComponent<spacecraft>().requiredSleep = true;
            transform.GetChild(0).GetChild(0).GetComponent<TrailRenderer>().time = 0.5f;
            GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            StartCoroutine(waitToNext(1f));
        }
        return;
    }

    IEnumerator waitToNext(float time)
    {
        /*
        Todo: this coroutine to show instruction after the player lands on the second planet
        */
        yield return new WaitForSeconds(time);
        startNewLevel();
    }

    private void startNewLevel(){
        int cur_scene = SceneManager.GetActiveScene().buildIndex;
        if (cur_scene == Constants.maxNumOfLevel)
            SceneManager.LoadScene("end stage");
        else
        {
            int nextLevelID = SceneManager.GetActiveScene().buildIndex + 1;
            // load next level
            SceneManager.LoadScene(nextLevelID);

            // update level records
            GameStates.curLevelID = nextLevelID;
            GameStates.unlockedLevelID = nextLevelID;
        }
    }

    IEnumerator playerBlink() {
        transform.GetChild(0).GetChild(0).GetComponent<TrailRenderer>().enabled = false;
        yield return new WaitForSeconds(0.05f);
        transform.GetChild(0).GetChild(0).GetComponent<TrailRenderer>().enabled = true;
    }
}

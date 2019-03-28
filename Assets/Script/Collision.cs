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
    public ParticleSystem burstEnergy;


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

    void OnCollisionEnter2D(Collision2D col)
    {
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
        float left_health = transform.GetChild(0).GetComponent<spacecraft>().energy - damage;
        if (left_health <= 0)
        {
            burstEnergy.Play();
        }
        transform.GetChild(0).GetComponent<spacecraft>().energy = left_health;
        transform.GetChild(0).GetChild(0).GetComponent<TrailRenderer>().time = transform.GetChild(0).GetComponent<spacecraft>().energy / 100f;
        collided = true;
        AudioManager.instance.PlaySFX("being hit");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Tag: " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Finish" || collision.gameObject.tag == "preTutorial" || collision.gameObject.tag == "compaign")
        {
            if (transform.GetChild(0).GetComponent<spacecraft>().dead)
                return;
            transform.GetChild(0).GetComponent<spacecraft>().won = true;
            AudioManager.instance.PlaySFX("Next Level");
            transform.GetChild(0).GetComponent<spacecraft>().requiredSleep = true;
            transform.GetChild(0).GetChild(0).GetComponent<TrailRenderer>().time = 0.5f;
            GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);


            if (collision.gameObject.tag == "Finish")
            {
                if (SceneManager.GetActiveScene().buildIndex == 4)
                {
                    GameStates.instance.setAchievement(Achievements.achievement_passed_tutorial);
                    GameStates.instance.setAchievement(Achievements.unlock_whirlpool);
                }else if (SceneManager.GetActiveScene().buildIndex == 14)
                {
                    GameStates.instance.setAchievement(Achievements.unlock_whirlpool);
                }
                StartCoroutine(waitToNext(1f, false));
            }else if (collision.gameObject.tag == "preTutorial"){
                StartCoroutine(waitToNext(4f, false));
                GameStates.instance.SaveTutorialData(1,"pre");
            }else if (collision.gameObject.tag == "compaign"){
                Debug.Log("load");
                StartCoroutine(waitToNext(1f, true));
            }
                
                
        }
        return;
    }

    IEnumerator waitToNext(float time, bool load)
    {
        /*
        Todo: this coroutine to show instruction after the player lands on the second planet
        */
        yield return new WaitForSeconds(time);
        if (!load)
            startNewLevel(SceneManager.GetActiveScene().buildIndex + 1);
        else
            GameStates.instance.LoadLevel();
    }

    private void startNewLevel(int nextLevelID){
        int cur_scene = SceneManager.GetActiveScene().buildIndex;
        if (cur_scene == SceneManager.sceneCount - 2)
            SceneManager.LoadScene("end stage");
        else
        {
            //int nextLevelID = SceneManager.GetActiveScene().buildIndex + 1;
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

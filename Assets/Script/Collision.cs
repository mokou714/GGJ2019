using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Collision : MonoBehaviour
{

    public float collide_strengh;
    public bool collided;
    public float asteroidDamage;

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
    // Debug.Log("Collision:" + col.gameObject.tag);
        switch (hit_obj)
        {
            case "asteroid":
                damage = col.gameObject.GetComponent<Asteroid>().damage;
                break;

            case "orbaerolite":
                damage = col.gameObject.GetComponent<orbitAsteroid>().damage;
                break;
            case "Finish":
                AudioManager.instance.PlaySFX("Next Level");
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
                    
                return;
        }
        col.gameObject.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity * collide_strengh;
        transform.GetChild(0).GetComponent<spacecraft>().energy -= damage;
        transform.GetChild(0).GetChild(0).GetComponent<TrailRenderer>().time = transform.GetChild(0).GetComponent<spacecraft>().energy / 100f;
        collided = true;
        AudioManager.instance.PlaySFX("being hit");
    }
}

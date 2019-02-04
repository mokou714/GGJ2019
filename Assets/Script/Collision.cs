using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Collision : MonoBehaviour {

    public float collide_strengh;
    public bool collided;
    public float asteroidDamage;

    public ParticleSystem energyLossOnCollide;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	

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
        // Debug.Log("Collision:" + col.gameObject.tag);
        if (col.gameObject.tag == "asteroid")
        {
            col.gameObject.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity * collide_strengh;
            transform.GetChild(0).GetComponent<spacecraft>().energy -= col.gameObject.GetComponent<Asteroid>().damage;
            transform.GetChild(0).GetChild(0).GetComponent<TrailRenderer>().time = transform.GetChild(0).GetComponent<spacecraft>().energy / 100f;
            collided = true;
            AudioManager.instance.PlaySFX("being hit");
            //print("hit");
        }
        else if (col.gameObject.tag == "orbasteroidEc")
        {
            col.gameObject.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity * collide_strengh;
            transform.GetChild(0).GetComponent<spacecraft>().energy -= col.gameObject.GetComponent<orbitAsteroidEclipse>().damage;
            transform.GetChild(0).GetChild(0).GetComponent<TrailRenderer>().time = transform.GetChild(0).GetComponent<spacecraft>().energy / 100f;
            collided = true;
            AudioManager.instance.PlaySFX("being hit");
            //print("hit");


        }
        else if (col.gameObject.tag == "asteroid")
        {
            col.gameObject.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity * collide_strengh;
            transform.GetChild(0).GetComponent<spacecraft>().energy -= asteroidDamage;
            transform.GetChild(0).GetChild(0).GetComponent<TrailRenderer>().time = transform.GetChild(0).GetComponent<spacecraft>().energy / 100f;
            collided = true;
            //print("hit");

        }
        else if (col.gameObject.tag == "orbasteroid")
        {
            col.gameObject.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity * collide_strengh;
            transform.GetChild(0).GetComponent<spacecraft>().energy -= col.gameObject.GetComponent<orbitAsteroid>().damage;
            transform.GetChild(0).GetChild(0).GetComponent<TrailRenderer>().time = transform.GetChild(0).GetComponent<spacecraft>().energy / 100f;
            collided = true;
            AudioManager.instance.PlaySFX("being hit");
            //print("hit");

        }
        else if (col.gameObject.tag == "Finish"){

            AudioManager.instance.PlaySFX("Next Level");

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);


        }




    }

}

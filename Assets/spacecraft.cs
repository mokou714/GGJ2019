using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spacecraft : MonoBehaviour {


    public float direction;
    public int rotation_sensitivity;
    public float remaining_enegy;
    public float boost_power;

	// Use this for initialization
	void Start () {

        Vector3 dir = new Vector3(0, 0, direction);

	}
	
	// Update is called once per frame
	void Update () {
		
        if(Input.GetKey(KeyCode.LeftArrow)){
            transform.Rotate(0,0, Time.deltaTime * rotation_sensitivity);
            direction = transform.eulerAngles.z;
        }
        else if (Input.GetKey(KeyCode.RightArrow)){
            transform.Rotate(0,0, -Time.deltaTime * rotation_sensitivity);
            direction = transform.eulerAngles.z;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            Boost();
        }

    }

    void Boost()
    {
        if (gameObject.tag == "spacecraft")
        {

            transform.parent.GetComponent<Rigidbody2D>().AddForce(transform.up * boost_power);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "planet")
        {
            transform.parent.gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
        }
    }



}

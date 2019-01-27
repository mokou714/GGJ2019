using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spacecraft : MonoBehaviour {


    public float direction;
    public int rotation_sensitivity;
    public float remaining_enegy;
    public float boost_power;
    public float max_speed;
    public float launch_speed;


    public float rotating_speed;
    public bool rotate_on;
    public Vector3 rotation_center;

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!rotate_on)
                Boost();
            else
                Launch();
        }


        if(rotate_on){
            Rotate();
        }


    }

    void Boost()
    {
        if (gameObject.tag == "spacecraft")
        {
            float x_speed = Mathf.Abs(transform.parent.GetComponent<Rigidbody2D>().velocity.x);
            float y_speed = Mathf.Abs(transform.parent.GetComponent<Rigidbody2D>().velocity.y);
            float origin_speed = Mathf.Sqrt(x_speed * x_speed + y_speed * y_speed);

            if (origin_speed < 0.1f)
                origin_speed = 1f;
            transform.parent.GetComponent<Rigidbody2D>().velocity = transform.up * origin_speed * boost_power;

            //transform.parent.GetComponent<Rigidbody2D>().AddForce(transform.up * boost_power);
        }
    }

    void Rotate(){
        Vector2 pos1 = new Vector2(transform.position.x, transform.position.y);
        Vector2 pos2 = new Vector2(rotation_center.x, rotation_center.y);
        float dis = Vector2.Distance(pos1, pos2);

        float sin = (pos1.y - pos2.y) / dis;
        float cos = (pos1.x - pos2.x) / dis;

        //Vector2 x_y_dir = new Vector2(pos1.x - pos2.x, pos1.y - pos2.y);
        //x_y_dir.Normalize();

        Debug.Log(sin);
        Debug.Log(cos);
        Vector2 new_v = new Vector2(sin, -cos) * rotating_speed;

        transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity = new_v;


    }

    void Launch(){
        rotate_on = false;

        float x_speed = Mathf.Abs(transform.parent.GetComponent<Rigidbody2D>().velocity.x);
        float y_speed = Mathf.Abs(transform.parent.GetComponent<Rigidbody2D>().velocity.y);
        float origin_speed = Mathf.Sqrt(x_speed * x_speed + y_speed * y_speed);

        if (origin_speed < 0.1f)
            origin_speed = 1f;
        transform.parent.GetComponent<Rigidbody2D>().velocity = transform.up * origin_speed * launch_speed;

    }





}

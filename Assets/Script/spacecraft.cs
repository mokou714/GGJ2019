using UnityEngine;
using UnityEngine.SceneManagement;

public class spacecraft : MonoBehaviour {


    public float direction;
    public int rotation_sensitivity;
    public float launch_speed;

    public float energy;

    public bool moving;

    public float start_velocity;
    public GameObject end;


    public float rotating_speed;
    public bool rotate_on;
    public Vector3 rotation_center;
    public int rotating_dir;

    public float egdecSpeed;

    public GameObject rotating_planet;

    private float movingTime = 0;
    public bool movingStart = false;

    public float inwardVel;

    private float originalWidth;

    private ParticleSystem energyLoss;
	// Use this for initialization
	void Start () {
        energy = 100f;

        Vector3 dir = new Vector3(0, 0, direction);
        transform.parent.GetComponent<Rigidbody2D>().velocity = transform.right * start_velocity;

        originalWidth = transform.GetChild(0).gameObject.GetComponent<TrailRenderer>().widthMultiplier;
        energyLoss = transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		
        //if(Input.GetKey(KeyCode.LeftArrow)){
        //    transform.Rotate(0,0, Time.deltaTime * rotation_sensitivity);
        //    direction = transform.eulerAngles.z;
        //}
        //else if (Input.GetKey(KeyCode.RightArrow)){
        //    transform.Rotate(0,0, -Time.deltaTime * rotation_sensitivity);
        //    direction = transform.eulerAngles.z;
        //} 
        transform.GetChild(0).gameObject.GetComponent<TrailRenderer>().widthMultiplier = originalWidth * energy / 100f;

        if (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount == 1))
        {
            if (rotate_on)
                Launch();
        }else if(Input.GetKeyDown(KeyCode.R)|| (Input.touchCount == 2)){
            Application.LoadLevel(Application.loadedLevel);
        }
        else if(rotate_on){
            Rotate();
        }
        if(moving){
            float curTime = Time.time;
            if (!movingStart)
            {
                //Debug.Log("Energy starts Losing");
                movingStart = true;
                movingTime = curTime;
                energyLoss.Play();
            }else{
                float offset = ((curTime - movingTime) * transform.parent.GetComponent<Rigidbody2D>().velocity.magnitude) * egdecSpeed;
                //Debug.Log("Distance:" + offset);
                //Debug.Log("Energy keeps Losing offset:" + offset);
                energy -= offset;
                transform.GetChild(0).gameObject.GetComponent<TrailRenderer>().time = energy / 100f;

                movingTime = curTime;
                //Debug.Log("Energy:" + enegy);
            }

        }else{
            //if(movingStart){
            //    //Debug.Log("Moved:" + (Time.time - movingTime) * transform.parent.GetComponent<Rigidbody2D>().velocity.magnitude);
            //}
            if (energyLoss.isEmitting)
                energyLoss.Stop();
        }

        if (energy <= 0 ||
            transform.position.x < -17f ||
            transform.position.x > 17 ||
            transform.position.y < -10f ||
            transform.position.y > 12f
           )
        {
            Application.LoadLevel(Application.loadedLevel);
        }


    }

    //void Boost()
    //{
    //    if (gameObject.tag == "spacecraft")
    //    {
    //        float x_speed = Mathf.Abs(transform.parent.GetComponent<Rigidbody2D>().velocity.x);
    //        float y_speed = Mathf.Abs(transform.parent.GetComponent<Rigidbody2D>().velocity.y);
    //        float origin_speed = Mathf.Sqrt(x_speed * x_speed + y_speed * y_speed);

    //        if (origin_speed < 0.1f)
    //            origin_speed = 1f;
    //        transform.parent.GetComponent<Rigidbody2D>().velocity = transform.up * origin_speed * boost_power;

    //        //transform.parent.GetComponent<Rigidbody2D>().AddForce(transform.up * boost_power);
    //    }
    //}

    void Rotate(){
        Vector2 pos1 = new Vector2(transform.position.x, transform.position.y);
        Vector2 pos2 = new Vector2(rotation_center.x, rotation_center.y);
        float dis = Vector2.Distance(pos1, pos2);

        float sin = (pos1.y - pos2.y) / dis;
        float cos = (pos1.x - pos2.x) / dis;

        //Vector2 x_y_dir = new Vector2(pos1.x - pos2.x, pos1.y - pos2.y);
        //x_y_dir.Normalize();

        //Debug.Log(sin);
        //Debug.Log(cos);
        Vector2 new_v = new Vector2(sin, -cos) * rotating_dir* rotating_speed;

        Vector2 offset = pos2 - pos1;
        offset.Normalize();

        transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity = new_v + offset * inwardVel;
        //transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity =;


        Vector2 v_dir = transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity;
        v_dir.Normalize();

        transform.up = new Vector3(v_dir.x, v_dir.y, 0);


    }

    void Launch(){
        rotate_on = false;

        float x_speed = Mathf.Abs(transform.parent.GetComponent<Rigidbody2D>().velocity.x);
        float y_speed = Mathf.Abs(transform.parent.GetComponent<Rigidbody2D>().velocity.y);
        float origin_speed = Mathf.Sqrt(x_speed * x_speed + y_speed * y_speed);

        if (origin_speed < 0.1f)
            origin_speed = 1f;
        transform.parent.GetComponent<Rigidbody2D>().velocity = transform.up * origin_speed * launch_speed;
        moving = true;

    }





}

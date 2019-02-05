using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class spacecraft : MonoBehaviour {
    /*
    This is the script executed in the player, responsible for player control and basic behaviors
    */

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
    public bool movingStart;

    public float inwardVel;

    private float originalWidth;

    private ParticleSystem energyLoss;

    private Vector2 spawnPoint;

    public GameObject Player;

	// Use this for initialization
	void Start () {
        //Initialize();
        //Debug.Log("New player: " + transform.position);
        energy = 100f;
        spawnPoint = transform.position;
        Vector3 dir = new Vector3(1, 0, 0);
        transform.parent.GetComponent<Rigidbody2D>().velocity = dir * start_velocity;
        originalWidth = transform.GetChild(0).gameObject.GetComponent<TrailRenderer>().widthMultiplier;
        energyLoss = transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
    }

    private void Reinitialize(){
        //transform.gameObject.SetActive(true);

    }

    private void ReinitPlayer(){
        //transform.parent.transform.position = spawnPoint;
        //Debug.Log("Reinitialize player");
        energy = 100f;
        transform.GetChild(0).gameObject.GetComponent<TrailRenderer>().time = energy / 100f;
        transform.GetChild(0).gameObject.GetComponent<TrailRenderer>().widthMultiplier = originalWidth;
        movingStart = false;
        Vector3 dir = new Vector3(1, 0, 0);
        transform.parent.GetComponent<Rigidbody2D>().velocity = dir * start_velocity;
        moving = false;
        movingStart = false;
        movingTime = 0;
        energyLoss = transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
        rotating_planet = null;
    }
	
    private void ReinitScene(){
        object[] scene_obj = FindObjectsOfType(typeof(GameObject));
        foreach (object obj in scene_obj)
        {
            GameObject single_obj = (GameObject)obj;
            if(single_obj.tag == "begin"){
                //Debug.Log("Begin point reinitialize " + single_obj);
                single_obj.gameObject.GetComponent<StopParticles>().ParticleReStart();
            }else if(single_obj.tag == "asteroid"){
                single_obj.gameObject.GetComponent<Asteroid>().movingBack = true;
            }


        }
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
            //Derail when the input is detected and player is in an orbit
            if (rotate_on)
                Launch();
        }else if(Input.GetKeyDown(KeyCode.R)|| (Input.touchCount == 2)){
            Application.LoadLevel(Application.loadedLevel);
        }else if(rotate_on){
            //If no valid input, keep rotating around the current planet
            Rotate();
        }
        //Indicating wether the player is in orbit or flying straight
        if(moving){
            float curTime = Time.time;
            //Detecting if the previous moment is orbiting
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
            //If the player is in orbit, stop lossing energy
            if (energyLoss.isEmitting)
                energyLoss.Stop();
        }

        // Death detection
        if (energy <= 0 ||
            transform.position.x < -Constants.maxX - 10 ||
            transform.position.x > Constants.maxX ||
            transform.position.y < -Constants.maxY - 10 ||
            transform.position.y > Constants.maxY
           ){
            //Application.LoadLevel(Application.loadedLevel);
            RespawnPlayer();
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

        //Debug.Log(sin);
        //Debug.Log(cos);
        Vector2 new_v = new Vector2(sin, -cos) * rotating_dir * rotating_speed;

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


    void RespawnPlayer(){
        
        energyLoss.Stop();
        transform.parent.transform.position = spawnPoint;
        ReinitScene();
        StartCoroutine(waitStartReinit());

    }

    private IEnumerator waitStartReinit()
    {
        float currTime = 0;
        float startInSeconds = 2;

        while (currTime < startInSeconds)
        {
            currTime += Time.deltaTime;
            yield return 0;
        }
        ReinitPlayer();
    }

}

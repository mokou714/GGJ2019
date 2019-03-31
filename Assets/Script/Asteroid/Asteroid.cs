using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {
    public float EscapeRange;
    private int[] dirs = {1, 0, -1};
    public int damage;
    public float maxStrength;

    public float maxX, maxY;
    private Vector3 origPosition;
    public bool movingBack = false;
    private float speed = 50;
    private int offset = 10;
    private CapsuleCollider2D endCollider;
	// Use this for initialization
	void Start () {
        //maxX/maxY unassigned value(==0)
        if(Mathf.Abs(maxX) <= Mathf.Epsilon)
            maxX = 30f;
        if(Mathf.Abs(maxY) <= Mathf.Epsilon)
            maxY = 20f;

        origPosition = transform.position;

        //Cancel the collision between asteroid and end in case it blocks the end
        GameObject endObj = GameObject.FindWithTag("end");
        if (endObj != null)
            endCollider = endObj.transform.GetChild(2).GetComponent<CapsuleCollider2D>();
        if (endCollider != null)
            Physics2D.IgnoreCollision(transform.GetComponent<BoxCollider2D>(), endCollider);
	}

	void Update()
	{
        //When the player dies, asteroid is supposed to move back to where it originally was
        if(movingBack){
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, origPosition, step);

            //Disable the collider when moveing in case of hitting other asteroids
            //if(transform.GetComponent<Collider2D>().enabled)
                //transform.GetComponent<Collider2D>().enabled = false;
            if(Vector3.Distance(transform.position,origPosition) < 0.1f){
                movingBack = false;
                transform.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
                transform.GetComponent<Rigidbody2D>().freezeRotation = true;
                transform.GetComponent<Collider2D>().enabled = true;
            }
        }else{
            //If the asteroids are out of the bound, then freeze it and cancel the collider
            if(Mathf.Abs(transform.position.x) > maxX + offset|| Mathf.Abs(transform.position.y) > maxY + offset){
                transform.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
                transform.GetComponent<Collider2D>().enabled = false;
            }
        }

	}

    public void Recover(){
        movingBack = true;
    }

	// Update is called once per frame
	void FixedUpdate()
    {
        if(maxStrength > 0){
            Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
            Collider2D[] aroundColliders = Physics2D.OverlapCircleAll(myPos, EscapeRange);
            List<Vector2> escapeList = new List<Vector2>();
            if (myPos.x >= maxX)
            {
                escapeList.Add(new Vector2(myPos.x - maxX, 1));
                escapeList.Add(new Vector2(myPos.x - maxX, -1));
            }
            if (myPos.x <= -maxX)
            {
                escapeList.Add(new Vector2(myPos.x + maxX, 1));
                escapeList.Add(new Vector2(myPos.x + maxX, -1));
            }
            if (myPos.y >= maxY)
            {
                escapeList.Add(new Vector2(1, myPos.y - maxY));
                escapeList.Add(new Vector2(-1, myPos.y - maxY));
            }
            if (myPos.y <= - maxY)
            {
                escapeList.Add(new Vector2(1, myPos.y + maxY));
                escapeList.Add(new Vector2(-1, myPos.y + maxY));
            }

            foreach (Collider2D objAround in aroundColliders)
            {
                GameObject obj = objAround.gameObject;
                if (obj.tag == "planet")
                {
                    Vector2 pltPos = new Vector2(obj.transform.position.x, obj.transform.position.y);
                    Vector2 posDiff = pltPos - myPos;
                    posDiff.Normalize();
                    escapeList.Add(posDiff);
                }
            }


            Vector2 rand_dir = randDirGen();

            while (escapeList.Contains(rand_dir))
            {
                rand_dir = randDirGen();
            }
            //Debug.Log("Random Dir:" + rand_dir);

            float FloatStrenght = Random.Range(-maxStrength, maxStrength);

            GetComponent<Rigidbody2D>().AddForce(rand_dir * FloatStrenght);
            transform.Rotate(0, 0, 1);

        }

    }

    private Vector2 randDirGen(){
        int rand_x = Random.Range(-1, 1);
        int rand_y = Random.Range(-1, 1);
        return new Vector2(rand_x, rand_y);
    }


}

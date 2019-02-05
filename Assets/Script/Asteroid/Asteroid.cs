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
	// Use this for initialization
	void Start () {
        maxX = Constants.maxX;
        maxY = Constants.maxY;
        origPosition = transform.position;

	}

	void Update()
	{
        if(movingBack){
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, origPosition, step);
            if(transform.position == origPosition){
                movingBack = false;
                transform.GetComponent<Rigidbody2D>().freezeRotation = true;
                transform.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            }
        }

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

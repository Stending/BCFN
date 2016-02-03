using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CaracterScript : MonoBehaviour
{

    public GameManager gameManager;

    public float speed = 10.0f;
    public float horizontalMotion = 0.0f;
    public float gravityMax = 1.0f;
    public float gravitySpeed = 1.0f;
    public float force = -1;
    public float rebond = 10.0f;
    public PlanetScript planet;

    public float doubleTap = 0.0f;
    public float doubleTapDuration = 0.3f;
    public bool dashing = false;

    public bool active = false;

    public List<ButtonScript> buttonsHit = new List<ButtonScript>();
    // Use this for initialization
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (doubleTap > 0)
                doubleTap -= Time.deltaTime;
            else
                doubleTap = 0;

            if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Horizontal2"))
            {
                if (doubleTap > 0)
                    dashing = true;
                doubleTap = doubleTapDuration;
            }
            else if (Input.GetButtonUp("Horizontal") || Input.GetButtonUp("Horizontal2"))
            {
                if (dashing)
                {
                    dashing = false;
                    doubleTap = 0;
                }
            }
            if (dashing)
            {
                if (HorizontalAxis > 0)
                    horizontalMotion = 2 * speed;
                else
                    horizontalMotion = -2 * speed;
            }
            else
                horizontalMotion = HorizontalAxis * speed * ((dashing) ? 2 : 1);
        }
    }
    
    float HorizontalAxis
    {
        get {
            if (Mathf.Abs(Input.GetAxis("Horizontal2")) > Mathf.Abs(Input.GetAxis("Horizontal")))
                return Input.GetAxis("Horizontal2");
        else
                return Input.GetAxis("Horizontal");
        }
        set {; }
    }

    void FixedUpdate()
    {
        if (active)
        {
            if (force < 1)
                force += gravitySpeed / 100;
            float radius = Vector2.Distance(this.transform.position, planet.transform.position);
            float angle;
            if (this.transform.position.y > 0)
                angle = Mathf.Acos(this.transform.position.x / radius);
            else
                angle = -Mathf.Acos(this.transform.position.x / radius);
            Vector2 gravDir = -(new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));
            Vector2 gravVect = gravDir * gravityMax * force;
            //Vector2 forceVect = -gravDir * rebond * force;
            Vector2 direction = new Vector2(-gravDir.y, gravDir.x);


            GetComponent<Rigidbody2D>().velocity = direction * horizontalMotion + gravVect;

        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }

    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Button")
        {
            ButtonScript bs = coll.gameObject.GetComponent<ButtonScript>();
            if (bs.active)
            {
                buttonsHit.Insert(0,bs);
            }
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Button")
        {
            ButtonScript bs = coll.gameObject.GetComponent<ButtonScript>();
            if (buttonsHit.Contains(bs))
                buttonsHit.Remove(bs);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (active)
        {
            if (coll.gameObject.tag == "Planet")
            {
                if (buttonsHit.Count>0)
                {
                    force = -1f;
                    buttonsHit[0].destroyItself();
                    buttonsHit.RemoveAt(0);
                    gameManager.ButtonDestroyed();
                }
                else
                {
                    force = -1.3f;
                    gameManager.planetHit();
                }
            }
            /*else if (coll.gameObject.tag == "Button")
            {
                ButtonScript bs = coll.gameObject.GetComponent<ButtonScript>();
                if (bs.active)
                {
                    force = -1f;
                    bs.destroyItself();
                    gameManager.ButtonDestroyed();
                }
            }*/
        }
        else
        {
            force = 0;
        }
    }

    public float AngleFromPlanet
    {
        get
        {
            float radius = Vector2.Distance(this.transform.position, planet.transform.position);
            float angle;
            if (this.transform.position.y > 0)
                angle = Mathf.Acos(this.transform.position.x / radius);
            else
                angle = -Mathf.Acos(this.transform.position.x / radius);
            return angle;
        }
        set
        {
            ;
        }

    }

    public void Jump()
    {
        force = -1.5f;
    }

    public void Disable()
    {
        active = false;
        force = 0;
        horizontalMotion = 0;
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        dashing = false;
        doubleTap = 0;

    }

    public IEnumerator MoveTo(Vector2 pos, float t)
    {
        Vector3 pos3 = new Vector3(pos.x, pos.y, this.transform.position.z);
        Vector3 posPrec = this.transform.position;
        float c = 0.0f;
        for (c = 0.0f; c <= 1; c += (Time.deltaTime / t))
        {
            this.transform.position = Vector3.Lerp(posPrec, pos3, c);
            yield return null;
        }
    }
}

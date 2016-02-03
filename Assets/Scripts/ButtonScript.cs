using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {

    public GameManager gameManager;
    public Animator buttonAnim;
    public bool active = true;
	// Use this for initialization
	void Start () {
        this.transform.localScale = new Vector3(1, 1, 1);
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void destroyItself()
    {
        active = false;
        buttonAnim.SetBool("Active", false);
        Destroy(this.gameObject, 0.8f);
    }
}

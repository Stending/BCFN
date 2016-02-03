using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreCanvasScript : MonoBehaviour {


    public Text scoreText;
    public PlanetScript planet;
    public CaracterScript caracter;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        /*if (planet.ActualScale > 4.0f)
        {
            Angle = (Mathf.Rad2Deg * caracter.AngleFromPlanet) - 90;
            this.transform.position = new Vector3(Mathf.Cos(caracter.AngleFromPlanet), Mathf.Sin(caracter.AngleFromPlanet), 0)*(planet.ActualScale - 4)*2;
        }*/
	}

    public void updateText(int score)
    {
        scoreText.text = score.ToString("00");

    }

    public float Angle
    {
        get
        {
            return this.transform.eulerAngles.z;
        }
        set
        {
            this.transform.eulerAngles = new Vector3(0, 0, value);
        }

    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

    public PlanetScript planet;
    public CaracterScript caracter;
    public bool InGame = false;
    public bool gameEnded = false;
    public CameraScript cameraScript;

    public ScoreCanvasScript scoreCanvas;
    public Text bestScoreText;
    public Animator tuto1Anim;
    public Animator tuto2Anim;
    public Animator bestScoreAnim;

    public int score = 0;
    public int bestScore = 0;
    public int vie = 2;

    public AudioSource music;
    public int audioLevel = 3;

	// Use this for initialization
	void Start () {
        Screen.SetResolution(600,  600, false);
        bestScore = PlayerPrefs.GetInt("HighScore");
        bestScoreText.text = bestScore.ToString("00");
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Start") || Input.GetMouseButtonDown(0) || Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Horizontal2"))
        {
            if (!InGame && !gameEnded)
            {
                StartGame();
            }    
        }

        if (Input.GetKeyDown("m"))
        {
            ChangeMusicVolume();
        }
	}

    public void StartGame()
    {
        vie = 1;
        score = 0; scoreCanvas.updateText(0);
        caracter.active = true;
        caracter.Jump();
        planet.CreateButton(0.0f, Mathf.PI);
        InGame = true;
        tuto1Anim.SetBool("Active", false); tuto2Anim.SetBool("Active", false); bestScoreAnim.SetBool("Active", false);

    }
    public void ButtonDestroyed()
    {
        score++;
        scoreCanvas.updateText(score);
        planet.nextScale = planet.ActualScale + 0.15f;
        float angleCoef;
        if (planet.ActualScale < 4.0f) { 
            angleCoef = 1;
        }else if (planet.ActualScale < 7.0f) {
            angleCoef = 0.8f;
        }
        else if (planet.ActualScale < 10.0f) { 
            angleCoef = 0.6f;
        }
        else { 
            angleCoef = 0.4f;
        }

        planet.CreateButton(caracter.AngleFromPlanet, Mathf.PI *angleCoef);
        if(score == 15 || score == 30)
            planet.CreateButton(caracter.AngleFromPlanet, Mathf.PI * angleCoef);
        else if (score == 50)
        {
            planet.CreateButton(caracter.AngleFromPlanet, Mathf.PI * angleCoef);
            planet.CreateButton(caracter.AngleFromPlanet, Mathf.PI * angleCoef);
        }else if(score == 80)
        {
            planet.CreateButton(caracter.AngleFromPlanet, Mathf.PI * angleCoef);
            planet.CreateButton(caracter.AngleFromPlanet, Mathf.PI * angleCoef);
            planet.CreateButton(caracter.AngleFromPlanet, Mathf.PI * angleCoef);
        }
    }

    public void planetHit()
    {
        if (InGame)
        {

            planet.getHit();
            vie--;
            if(vie <= 0){
                EndGame();
            }
        }
    }

    public void EndGame()
    {
        gameEnded = true;
        Invoke("endEnded", 1.0f);
        planet.nextScale = 1.0f;
        StartCoroutine(cameraScript.GoToInitialPos());
        StartCoroutine(caracter.MoveTo(new Vector2(0, 1.2f), 1.0f));
        StartCoroutine(planet.GoToScale(1.0f, 1.0f));
        if (score > bestScore){
            bestScore = score;
            bestScoreText.text = bestScore.ToString("00");
            PlayerPrefs.SetInt("HighScore", bestScore);
        }
        tuto1Anim.SetBool("Active", true); tuto2Anim.SetBool("Active", true); bestScoreAnim.SetBool("Active", true);
        DestroyAllButtons();
        InGame = false;
        caracter.Disable();
    }

    public void endEnded()
    {
        gameEnded = false;
    }
    public void DestroyAllButtons()
    {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Button");
        foreach(GameObject b in buttons)
        {
			if(b!=null)
            b.GetComponent<ButtonScript>().destroyItself();
        }
    }

    private void ChangeMusicVolume()
    {
        audioLevel++;
        if (audioLevel > 3) audioLevel = 0;
        switch (audioLevel)
        {
            case 0:
                music.volume = 0.0f;
                break;
            case 1:
                music.volume = 0.1f;
                break;
            case 2:
                music.volume = 0.5f;
                break;
            case 3:
                music.volume = 1.0f;
                break;
        }
    }
}

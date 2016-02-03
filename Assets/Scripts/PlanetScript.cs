using UnityEngine;
using System.Collections;

public class PlanetScript : MonoBehaviour
{


    public float nextScale;
    public Object buttonPrefab;
    public Animator planetAnim;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (ActualScale < nextScale)
        {
            ActualScale = ActualScale + ((nextScale - ActualScale) * 0.1f + 0.01f);

        }
    }

    public void CreateButton(float angle,  float margin)
    {

        GameObject button = Instantiate(buttonPrefab) as GameObject;
        float radius;
        if (ActualScale <= 1)
            radius = 0.90f;
        else
            radius = ActualScale;
        float newAngle = Random.Range(-margin, margin);
        
        print("Angle de base : " + angle + " Angle random : " + newAngle);
        button.transform.localPosition = new Vector3(Mathf.Cos(newAngle) * radius, Mathf.Sin(newAngle) * radius, 1);
        button.transform.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * (newAngle - Mathf.PI / 2));

        button.transform.localScale = new Vector3(1, Random.Range(0.5f, 2.5f), 1);
    }
    public float ActualScale
    {
        get
        {
            return this.transform.localScale.x;
        }
        set
        {
            this.transform.localScale = new Vector2(value, value);
        }

    }

    public void getHit()
    {
        planetAnim.Play("Hit");
    }

    public IEnumerator GoToScale(float scale, float t)
    {
        Vector3 nextScale = new Vector3(scale, scale, this.transform.localScale.z);
        Vector3 precScale = this.transform.localScale;
        float c = 0.0f;
        for (c = 0.0f; c <= 1; c += (Time.deltaTime / t))
        {
            this.transform.localScale = Vector3.Lerp(precScale, nextScale, c);
            yield return null;
        }
        ActualScale = 1.0f;
    }
}

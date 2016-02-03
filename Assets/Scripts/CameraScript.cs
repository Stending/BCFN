using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{

    public CaracterScript caracter;
    public PlanetScript planet;
    public Camera cam;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (caracter.active)
        {

            if (planet.ActualScale > 4.0f && planet.ActualScale < 7.0f)
            {
                float dif = (planet.ActualScale - 4.0f) * 3.0f;
                cam.transform.localPosition = new Vector3(0, dif, 0);


            }
            if(planet.ActualScale > 7.0f)
            {
                print("taille : " + (8 + (planet.ActualScale - 7.0f) * 2));
                cam.orthographicSize = 8 + (planet.ActualScale - 7.0f)*2;
            }
            Angle = (Mathf.Rad2Deg * caracter.AngleFromPlanet) - 90;
        }
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

    public IEnumerator GoToInitialPos()
    {
        Vector3 initialPos = this.transform.localPosition;
        float initialAngle = Angle;
        float initialOrthoSize = cam.orthographicSize;
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            cam.transform.localPosition = Vector3.Lerp(initialPos, new Vector3(0, 0, -10), t);
            cam.orthographicSize = initialOrthoSize - t*initialOrthoSize + t*8.0f;
            if (initialAngle > 0)
                Angle = initialAngle - t * initialAngle;
            else
                Angle = initialAngle + t * initialAngle;

            yield return null;
        }

        cam.orthographicSize = 8.0f;
        cam.transform.localPosition = new Vector3(0, 0, -10);
        Angle = 0;

    }
}
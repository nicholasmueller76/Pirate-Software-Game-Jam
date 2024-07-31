using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycleManager : MonoBehaviour
{
    public float fullDaySeconds;
    public Vector3 dawnRot;
    public float dawnIntensity;
    public Vector3 midDayRot;
    public float midDayIntensity;
    public Vector3 midnightRot;
    public float midnightIntensity;

    public float currentTime;

    public GameObject sunObj;
    public Light sunLight;


    private void OnEnable()
    {
        PlayerActionController.OnClickBed += EndDay;
    }

    private void OnDisable()
    {
        PlayerActionController.OnClickBed -= EndDay;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if(currentTime < fullDaySeconds/2)
        {
            sunObj.transform.eulerAngles = Vector3.Lerp(dawnRot, midDayRot, currentTime / (fullDaySeconds / 2));
            sunLight.intensity = Mathf.Lerp(dawnIntensity, midDayIntensity, currentTime / (fullDaySeconds / 2));
        }
        else
        {
            sunObj.transform.eulerAngles = Vector3.Lerp(midDayRot, midnightRot, (currentTime - (fullDaySeconds / 2)) / (fullDaySeconds / 2));
            sunLight.intensity = Mathf.Lerp(midDayIntensity, midnightIntensity, (currentTime - (fullDaySeconds / 2)) / (fullDaySeconds / 2));
        }
    }

    public void EndDay()
    {
        //More stuff here for ending a day
        currentTime = 0;
    }
}

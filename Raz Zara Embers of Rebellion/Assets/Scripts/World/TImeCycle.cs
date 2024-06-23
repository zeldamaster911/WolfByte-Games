using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public GameObject sun;
    public GameObject moon;
    public int hours;
    public int minutes;
    public int SecondsPerMinute;
    private float startTime;
    private float currentTime;
    public float timeScale = 1f;
    public float timeMultiplier = 1f;  // Multiplier to control the speed of time

    private float updateInterval = 1f;  // Interval in seconds to update the celestial positions
    private float nextUpdate;

    void Start()
    {
        hours = PlayerPrefs.GetInt("hours", 12);  // Default to noon if not set
        minutes = PlayerPrefs.GetInt("minutes", 0);  // Default to 0 if not set
        startTime = Time.realtimeSinceStartup;
        nextUpdate = Time.time + updateInterval;
        sun = GameObject.Find("Sun");
        moon = GameObject.Find("Moon");
    }

    void Update()
    {
        Time.timeScale = timeScale;
        currentTime = Time.realtimeSinceStartup;

        // Calculate elapsed time, adjusted by the time multiplier
        if ((currentTime - startTime) * timeMultiplier >= SecondsPerMinute)
        {
            minutes++;
            startTime = currentTime;
            if (minutes >= 60)
            {
                minutes = 0;
                hours++;
                if (hours >= 24)
                {
                    hours = 0;
                }
            }

            // Save time to PlayerPrefs less frequently
            if (minutes % 5 == 0)
            {
                PlayerPrefs.SetInt("hours", hours);
                PlayerPrefs.SetInt("minutes", minutes);
            }
        }

        if (Time.time >= nextUpdate)
        {
            UpdateCelestialPositions();
            nextUpdate = Time.time + updateInterval;
        }
    }

    void UpdateCelestialPositions()
    {
        float dayFraction = (hours / 24f);
        if ((hours > 18) || (hours < 6))
        {
            day(false);
        }
        else if (hours >= 6 && hours <= 18)
        {
            day(true);
        }

        float sunAngle = dayFraction * 360f + 180f;  // Ensures that 12 PM is at 90 degrees (zenith)
        float moonAngle = sunAngle + 180f;  // Moon is opposite the sun

        Quaternion sunTargetRotation = Quaternion.Euler(90f - sunAngle, -90f, 0);  // Correct the sun's rotation
        Quaternion moonTargetRotation = Quaternion.Euler(90f - moonAngle, -90f, 0);  // Correct the moon's rotation

        // Smoothly interpolate the rotation
        sun.transform.rotation = Quaternion.Lerp(sun.transform.rotation, sunTargetRotation, Time.deltaTime * 0.5f);
        moon.transform.rotation = Quaternion.Lerp(moon.transform.rotation, moonTargetRotation, Time.deltaTime * 0.5f);
    }

    public void day(bool isDay)
    {
        sun.SetActive(isDay);
        moon.SetActive(!isDay);
    }
}

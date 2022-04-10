using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private Transform secondHand;
    [SerializeField] private Transform minuteHand;
    [SerializeField] private Transform hourHand;

    private System.DateTime currentTime;

    void Update()
    {
        currentTime = System.DateTime.Now;

        // second hand location
        float second = currentTime.Second;
        secondHand.transform.rotation = Quaternion.Euler(0, -90, -90 + (second * 6));

        // minute hand location
        float minute = currentTime.Minute;
        minuteHand.transform.rotation = Quaternion.Euler(0, -90, -90 + (minute * 6) + (second * 0.1f));

        // hour hand location
        float hour = currentTime.Hour;
        if (hour >= 12)
        {
            hour = hour - 12;
        }
        hourHand.transform.rotation = Quaternion.Euler(0, -90, -270 + (hour * 30) + (minute * 0.5f));
    }
}

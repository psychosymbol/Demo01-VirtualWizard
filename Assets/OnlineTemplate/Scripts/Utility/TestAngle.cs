using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAngle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float angle = Angle(new Vector2(transform.position.x, transform.position.y));

        string colorsch = "<color=red>";
        if (angle >= 315 || angle <= 45)
        {
            colorsch = "<color=red>";
        }
        else if (angle > 45 && angle <= 135)
        {
            colorsch = "<color=blue>";
        }
        else if (angle > 135 && angle <= 225)
        {
            colorsch = "<color=green>";
        }
        else if (angle > 225 && angle <= 315)
        {
            colorsch = "<color=yellow>";
        }
        Debug.Log(colorsch + "<b>" + angle + "</b></color>");
    }
    public static float Angle(Vector2 vector2)
    {
        if (vector2.x < 0)
        {
            return 360 - (Mathf.Atan2(vector2.x, vector2.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(vector2.x, vector2.y) * Mathf.Rad2Deg;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static float offset;
    
    void Update()
    {
        if (BirdScripts.instance != null)
            if (BirdScripts.instance.isAlive)
                Move();
    }

    void Move()
    {
        Vector3 temp = transform.position;
        temp.x = BirdScripts.instance.GetXPosition() + offset;
        transform.position = temp;
    }

}

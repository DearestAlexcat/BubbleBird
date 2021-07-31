using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeCollector : MonoBehaviour
{
    GameObject[] pipes = null;
    const float MIN = 0.0f;
    const float MAX = 3.6f;
    const float SHIFTX = 3.5f;
    float lastPipe;

    private void Awake()
    {
        pipes = GameObject.FindGameObjectsWithTag("PipeHolder");
        Vector3 temp;

        for (int i = 0; i < pipes.Length; i++)
        {
            temp = pipes[i].transform.position;
            temp.y = Random.Range(MIN, MAX);
            pipes[i].transform.position = temp;
        }

        lastPipe = pipes[0].transform.position.x;
        for (int i = 1; i < pipes.Length; i++)
        {
            if(lastPipe < pipes[i].transform.position.x)
            {
                lastPipe = pipes[i].transform.position.x;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PipeHolder"))
        {
            Vector3 temp = collision.transform.position;
            temp.x = lastPipe + SHIFTX;
            temp.y = Random.Range(MIN, MAX);
            collision.transform.position = temp;
            lastPipe = temp.x;
        }
    }


}

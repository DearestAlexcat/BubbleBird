using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGCollector : MonoBehaviour
{
    int numberBg;

    public void Awake()
    {     
        numberBg = GameObject.FindGameObjectsWithTag("Background").Length; // Определяем кол-во Background`ов
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Background"))
        {
            Vector3 temp = collision.transform.position;            
            temp.x += ((BoxCollider2D)collision).size.x * collision.transform.localScale.x * numberBg;
            collision.transform.position = temp;
        }
    }
}

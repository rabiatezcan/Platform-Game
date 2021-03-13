using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletControl : MonoBehaviour
{
    enemyControl enemy;
    Rigidbody2D physic; 
    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("mace").GetComponent<enemyControl>();
        physic = GetComponent<Rigidbody2D>();
        physic.AddForce(enemy.getDirection()*500);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maceControl : MonoBehaviour
{
    public Sprite front;
    public Sprite back;
    SpriteRenderer spriteRenderer;
    public GameObject bullet;    
    float fireTime;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void lookFront()
    {
        spriteRenderer.sprite = front; 
    }
    public void lookBack()
    {
        spriteRenderer.sprite = back;
    }

    public void fire()
    {
        fireTime += Time.deltaTime;
        if (fireTime > Random.Range(0.2f, 1f))
        {
            fireTime = 0;
            Instantiate(bullet, transform.position, Quaternion.identity);
        }
    }
}

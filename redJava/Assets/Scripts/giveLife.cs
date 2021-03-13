using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giveLife : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false; 
    }

    // Update is called once per frame
    public void openChest()
    {
        animator.enabled = true;
        animator.Play("chestAnimation");
    }
}

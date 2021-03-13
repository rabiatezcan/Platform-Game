using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
public class backMain : MonoBehaviour
{
    void Start()
    {
        Invoke("backToMainMenu", 3);
    }

    void backToMainMenu()
    {
        SceneManager.LoadScene("mainMenu");
    }
}

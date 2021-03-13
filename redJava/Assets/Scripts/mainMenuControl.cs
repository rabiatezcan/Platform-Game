using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class mainMenuControl : MonoBehaviour
{
    GameObject locks, levels; 
    void Start()
    { 
        locks = GameObject.FindGameObjectWithTag("locks");
        levels = GameObject.FindGameObjectWithTag("levels");
        for (int i = 0; i < levels.transform.childCount; i++)
        {
            levels.transform.GetChild(i).gameObject.SetActive(false);
            locks.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < PlayerPrefs.GetInt("levelNo"); i++)
        {
            levels.transform.GetChild(i).GetComponent<Button>().interactable = true;
            locks.transform.GetChild(i).GetComponent<Image>().enabled = false;

        }
    }

    public void selectButton(int buttonNo)
    {
        if (buttonNo == 1) {
            if(PlayerPrefs.GetInt("levelNo") > 0)
            {
                SceneManager.LoadScene(PlayerPrefs.GetInt("levelNo"));
            }
            else
            {
                SceneManager.LoadScene(1);
            }
            
        }
        else if (buttonNo == 2)
        {
            for (int i = 0; i < levels.transform.childCount; i++)
            {
                levels.transform.GetChild(i).gameObject.SetActive(true);
                locks.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        else if (buttonNo == 3)
        {
            Application.Quit();
        }
    }

    public void selectLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

}

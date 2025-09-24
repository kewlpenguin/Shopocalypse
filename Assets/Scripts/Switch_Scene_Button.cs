using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class Switch_Scene_Button : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Scene_Swap()
    {
        Scene Active_Scene = SceneManager.GetActiveScene();
        if (Active_Scene.buildIndex == 0)
        {
            SceneManager.LoadScene(1);
        }
        else if (Active_Scene.buildIndex == 1)
        {
            SceneManager.LoadScene(2);
        }
        else if (Active_Scene.buildIndex == 2)
        {
            SceneManager.LoadScene(3);
        }
        else if (Active_Scene.buildIndex == 3)
        {
            SceneManager.LoadScene(1);
        }

    }

}

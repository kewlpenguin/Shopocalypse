using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class Help_Button : MonoBehaviour
{
    private Button Help_TitleScreen_Button;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Help_TitleScreen_Button =  gameObject.GetComponent<Button>();

        Help_TitleScreen_Button.onClick.AddListener(Swap_Title_Help_Screen);
    }

    // Update is called once per frame
    void Update()
    {
        
    }







    void Swap_Title_Help_Screen()
    {
        Scene Active_Scene = SceneManager.GetActiveScene();
      
        if (Active_Scene.buildIndex == 4) {
            SceneManager.LoadScene(0);
        }
       
        else if(Active_Scene.buildIndex == 0)
        {
            SceneManager.LoadScene(4);
        }

    }
}

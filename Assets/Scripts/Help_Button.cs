using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class Help_Button : MonoBehaviour
{
    private Button Help_TitleScreen_Button;
    public GameObject Help_Screen;
    public AudioClip Button_Click_SFX;
    private Scene Current_Scene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Help_TitleScreen_Button =  gameObject.GetComponent<Button>();

        Help_TitleScreen_Button.onClick.AddListener(Swap_Title_Help_Screen);





    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && Help_Screen.gameObject.activeInHierarchy) 
        {
            Swap_Title_Help_Screen();
        }
    }







    void Swap_Title_Help_Screen() // now just shows and hides ui elements
    {
       
            if (Help_Screen.activeInHierarchy)
            {
                Audio_Manager_Script.instance.Play_Selected_Audio(Button_Click_SFX, gameObject.transform.position, .7f, 1);
                Help_Screen.SetActive(false);
            }

            else if (!Help_Screen.activeInHierarchy)
            {
                Audio_Manager_Script.instance.Play_Selected_Audio(Button_Click_SFX, gameObject.transform.position, .7f, 1);
                Help_Screen.SetActive(true);
            }
        }

    
}

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
    public GameObject Title_Screen_UI;
    public AudioClip Button_Click_SFX;


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







    void Swap_Title_Help_Screen() // now just shows and hides ui elements
    {
      
        if (Help_Screen.activeInHierarchy) {
            Audio_Manager_Script.instance.Play_Selected_Audio(Button_Click_SFX, gameObject.transform.position, .7f, 1);
            Help_Screen.SetActive(false);
            Title_Screen_UI.SetActive(true);
        }
       
        else if(!Help_Screen.activeInHierarchy)
        {
            Audio_Manager_Script.instance.Play_Selected_Audio(Button_Click_SFX, gameObject.transform.position, .7f, 1);
            Help_Screen.SetActive(true);
            Title_Screen_UI.SetActive(false);
        }

    }
}

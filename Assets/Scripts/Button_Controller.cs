using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;
public class Button_Controller : MonoBehaviour
{

    public Button Start_Button;


    public Toggle Normal_Mode_Button;
    public Toggle Easy_Mode_Button;
    public ToggleGroup Difficulty_Selector;

    public AudioClip Button_Click_SFX;
    public AudioClip Title_Screen_Music; // also applies to help menu


    //  public AudioClip Bat_Quick_Attack;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Assign_Buttons();
        Music_Controller.Music_instance.Play_Selected_Audio(Title_Screen_Music, gameObject.transform.position, .2f, 1,true,true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Assign_Buttons()// and toggles
    {

        Difficulty_Selector.allowSwitchOff = true;

        Normal_Mode_Button.group = Difficulty_Selector;
        Easy_Mode_Button.group = Difficulty_Selector;

        Normal_Mode_Button.onValueChanged.AddListener(Activate_Start_Button);
       
        Easy_Mode_Button.onValueChanged.AddListener(Activate_Start_Button);

        Start_Button.onClick.AddListener(Call_Swap_Scene_1_With_Message);
    



        Start_Button.gameObject.SetActive(false);



    }

    void Activate_Start_Button(bool Difficulty_Has_Been_Selected) //if we have selected a difficulty, activate and display the start button
    {
        Audio_Manager_Script.instance.Play_Selected_Audio(Button_Click_SFX, gameObject.transform.position, .7f, 1);

        if (Difficulty_Has_Been_Selected)
        {
            Start_Button.gameObject.SetActive(true);
        }
        else if (!Difficulty_Has_Been_Selected)
        {
            Start_Button.gameObject.SetActive(false);
        }
    }


    void Call_Swap_Scene_1_With_Message()
    {
        Audio_Manager_Script.instance.Play_Selected_Audio(Button_Click_SFX, gameObject.transform.position, .75f, 1);
        GameObject.Find("Persistent_Data_Store").SendMessage("Swap_To_Scene_1", SendMessageOptions.RequireReceiver);
    }

}

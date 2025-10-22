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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Assign_Buttons();
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
        GameObject.Find("Persistent_Data_Store").SendMessage("Swap_To_Scene_1", SendMessageOptions.RequireReceiver);
    }

}

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;
public class Button_Controller : MonoBehaviour
{

    public Button Start_Button; //all win screen bullshit is in enemy manager


    public Toggle Normal_Mode_Button;
    public Toggle Easy_Mode_Button;
    public ToggleGroup Difficulty_Selector;


    public Toggle Mute_Music;
    public Toggle Mute_Sound_Effects;

    public Toggle Enable_Free_Samples; // gives some of each ammo at the beginning, can help if struggling with hard mode, acts as a small reward for beating easy mode

    public Toggle Enable_Infinite_Ammo;// should be locked behind hard mode 12 day win
    public Toggle Enable_Custom_Difficulty_Increment; // added to normal increment
    public TMP_InputField Select_Difficulty_Increment;
    public Toggle Enable_Barrel_Launcher;
    public TextMeshProUGUI Warning_Message;







    // potential events mode after complete easy mode for later build if i feel like it

    public AudioClip Button_Click_SFX;
    public AudioClip Title_Screen_Music; // also applies to help menu


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Assign_Buttons();
        Music_Controller.Music_instance.Play_Selected_Audio(Title_Screen_Music, gameObject.transform.position, .2f, 1, true, true, false, 99999999);
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

        
        Mute_Music.onValueChanged.AddListener(Disable_Music);

        Mute_Sound_Effects.onValueChanged.AddListener(Disable_SFX);


        if(Persistent_Data_Store.Easy_Highscore_Persistent > 12) // hide or show ui options based on max wave reached
        {
            Enable_Free_Samples.gameObject.SetActive(true);
        }

        if (Persistent_Data_Store.Hard_Highscore_Persistent > 12)
        {
            Enable_Barrel_Launcher.gameObject.SetActive(true);
            Enable_Custom_Difficulty_Increment.gameObject.SetActive(true);
            Enable_Infinite_Ammo.gameObject.SetActive(true);
        }

        Enable_Free_Samples.onValueChanged.AddListener(Free_Samples);
        Enable_Infinite_Ammo.onValueChanged.AddListener(Infinite_Ammo);
        Enable_Custom_Difficulty_Increment.onValueChanged.AddListener(Enable_Custom_Difficulty_Field);
        Select_Difficulty_Increment.onValueChanged.AddListener(Change_Difficulty_Increment);
        Enable_Barrel_Launcher.onValueChanged.AddListener(Barrel_Launcher);

        


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
    
    void Disable_Music(bool Box_Checked)
    {
        Audio_Manager_Script.instance.Play_Selected_Audio(Button_Click_SFX, gameObject.transform.position, .75f, 1);
        if (Box_Checked)
        {
            Persistent_Data_Store.Music_Disabled = true;
            AudioSource[] Audio_Playing_In_Scene = GameObject.FindObjectsByType<AudioSource>(FindObjectsSortMode.None); // destroy all other audio sources like music etc

            for (int i = Audio_Playing_In_Scene.Length; i > 0; i--)
            {
                Destroy(Audio_Playing_In_Scene[i - 1]);
            }
        }

        else if (!Box_Checked)
        {
            Persistent_Data_Store.Music_Disabled = false;
            Music_Controller.Music_instance.Play_Selected_Audio(Title_Screen_Music, gameObject.transform.position, .2f, 1, true, true, false, 99999999); // restart title music
        }
    }

    void Disable_SFX(bool Box_Checked)
    {
        Audio_Manager_Script.instance.Play_Selected_Audio(Button_Click_SFX, gameObject.transform.position, .75f, 1);
        if (Box_Checked)
        {
            Persistent_Data_Store.SFX_Disabled = true;
        }
        else if (!Box_Checked)
        {
            Persistent_Data_Store.SFX_Disabled = false;
        }
    }

    void Infinite_Ammo(bool Box_Checked)
    {
        Audio_Manager_Script.instance.Play_Selected_Audio(Button_Click_SFX, gameObject.transform.position, .75f, 1);
        if (Box_Checked)
        {
            Persistent_Data_Store.Infinite_Ammo_Enabled = true; // basically just due to conflict with free samples we need these and also they might help with debugging later

            Persistent_Data_Store.Sniper_Ammo = 999999;
            Persistent_Data_Store.Saw_Ammo = 999999;
            Persistent_Data_Store.Slow_Wave_Ammo = 999999;
            Persistent_Data_Store.Vines_Ammo = 999999;
            Persistent_Data_Store.Pierce_Lazer_Ammo = 999999;
            Persistent_Data_Store.Burst_Module_Ammo = 999999;
        }
        else if (!Box_Checked)
        {
            Persistent_Data_Store.Infinite_Ammo_Enabled = false;
            if (!Persistent_Data_Store.Free_Samples_Enabled)  // if free samples not active reset to default values
            {
                Persistent_Data_Store.Sniper_Ammo = 0;
                Persistent_Data_Store.Saw_Ammo = 0;
                Persistent_Data_Store.Slow_Wave_Ammo = 0;
                Persistent_Data_Store.Vines_Ammo = 0;
                Persistent_Data_Store.Pierce_Lazer_Ammo = 0;
                Persistent_Data_Store.Burst_Module_Ammo = 0;
            }
            else if (Persistent_Data_Store.Free_Samples_Enabled)// dont overwrite free sample ammo counts, instead reset to sample ammounts, literally a fucking genius because this also covers the case where you try to select free samples 
                                                                // after infinite ammo, of course nothing happens but free samples is still true. so when we turn off infinite we reset to the free sample values absalute cinema
            {
                Persistent_Data_Store.Sniper_Ammo = 3;
                Persistent_Data_Store.Saw_Ammo = 3;
                Persistent_Data_Store.Slow_Wave_Ammo = 3;
                Persistent_Data_Store.Vines_Ammo = 3;
                Persistent_Data_Store.Pierce_Lazer_Ammo = 3;
                Persistent_Data_Store.Burst_Module_Ammo = 1;
            }
        }
    }

  

    void Free_Samples(bool Box_Checked) // adds 3 starting ammo for each weapon and 1 burst module for fun
    {
        Audio_Manager_Script.instance.Play_Selected_Audio(Button_Click_SFX, gameObject.transform.position, .75f, 1);
        if (Box_Checked)
        {
            Persistent_Data_Store.Free_Samples_Enabled = true;

            if(!Persistent_Data_Store.Infinite_Ammo_Enabled)
            {
                Persistent_Data_Store.Sniper_Ammo = 3;
                Persistent_Data_Store.Saw_Ammo = 3;
                Persistent_Data_Store.Slow_Wave_Ammo = 3;
                Persistent_Data_Store.Vines_Ammo = 3;
                Persistent_Data_Store.Pierce_Lazer_Ammo = 3;
                Persistent_Data_Store.Burst_Module_Ammo = 1;
            }

        }
        else if (!Box_Checked)
        {
            Persistent_Data_Store.Free_Samples_Enabled = false;

            if (!Persistent_Data_Store.Infinite_Ammo_Enabled)
            {
                Persistent_Data_Store.Sniper_Ammo = 0;
                Persistent_Data_Store.Saw_Ammo = 0;
                Persistent_Data_Store.Slow_Wave_Ammo = 0;
                Persistent_Data_Store.Vines_Ammo = 0;
                Persistent_Data_Store.Pierce_Lazer_Ammo = 0;
                Persistent_Data_Store.Burst_Module_Ammo = 0;
            }
        }
    }

    void Enable_Custom_Difficulty_Field(bool Box_Checked)
    {
        Audio_Manager_Script.instance.Play_Selected_Audio(Button_Click_SFX, gameObject.transform.position, .75f, 1);
        if (Box_Checked)
        {
            Select_Difficulty_Increment.gameObject.SetActive(true);
        }
        else if (!Box_Checked)
        {
            Select_Difficulty_Increment.gameObject.SetActive(false);
        }
    }

    void Change_Difficulty_Increment(string value)
    {
        Audio_Manager_Script.instance.Play_Selected_Audio(Button_Click_SFX, gameObject.transform.position, .75f, 1);

        if (value != "" && value != "0")
        {
            int intValue = int.Parse(value); // Convert string to int
            Persistent_Data_Store.Custom_Difficulty_Additive = intValue;
        }
        else
        {
            Persistent_Data_Store.Custom_Difficulty_Additive = 0;
        }

        if (Persistent_Data_Store.Custom_Difficulty_Additive >= 999)
        {
            Warning_Message.gameObject.SetActive(true);
        }
        else if(Persistent_Data_Store.Custom_Difficulty_Additive < 999)
        {
            Warning_Message.gameObject.SetActive(false);
        }
    }




    void Barrel_Launcher(bool Box_Checked)
    {
        Audio_Manager_Script.instance.Play_Selected_Audio(Button_Click_SFX, gameObject.transform.position, .75f, 1);
        if (Box_Checked)
        {
            Persistent_Data_Store.Barrel_Launcher_Enabled = true;
        }
        else if (!Box_Checked)
        {
            Persistent_Data_Store.Barrel_Launcher_Enabled = false;

        }
    }
    

}

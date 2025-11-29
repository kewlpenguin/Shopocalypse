using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;
public class Pause_Button : MonoBehaviour
{
    Button My_Button; //ts is the pause not return to menu
    bool Game_Is_Paused;
    Transform Paused_Indicator;
    Button Return_To_Main_Menu_Button;
    CursorLockMode Lock_Cursor;
    CursorLockMode UnLock_Cursor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Game_Is_Paused = false;

        My_Button = gameObject.GetComponent<Button>();
        Paused_Indicator = gameObject.transform.Find("Paused_Indicator");
        Return_To_Main_Menu_Button = Paused_Indicator.transform.Find("Return_To_Main_Menu").GetComponent<Button>();

        My_Button.onClick.AddListener(Pause_Game);

        Return_To_Main_Menu_Button.onClick.AddListener(Return_To_Main_Menu);

        Lock_Cursor = CursorLockMode.Locked;
        UnLock_Cursor = CursorLockMode.None;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // holy fuck dis is clean, so basically this will only apply to scenes with the pause button prefab in it so no need to worry abt unintended behavior. it reuses the pause function 
                                                //so the code stays clean. it works in the shopping scene because it bypasses the button entirely to execute the pause function so we can just hit escape again and it works jus fine
        {
            Pause_Game();
        }
    }


    void Pause_Game()
    {
        if (!Game_Is_Paused)
        {
            Cursor.lockState = UnLock_Cursor; // for the shop phase so we can go back to main menu if we want
            Time.timeScale = 0f;
            Game_Is_Paused = true;
            Paused_Indicator.gameObject.SetActive(true);
        }
        else if (Game_Is_Paused)
        {
            if (Persistent_Data_Store.Current_Scene.buildIndex == 3)
            {
                Cursor.lockState = Lock_Cursor;
            }
            Time.timeScale = 1f;
            Game_Is_Paused = false;
            Paused_Indicator.gameObject.SetActive(false);
         
        }

    }

    void Return_To_Main_Menu()
    {
        SceneManager.LoadScene(0);
    }





}

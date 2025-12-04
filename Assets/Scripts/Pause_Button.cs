using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;
public class Pause_Button : MonoBehaviour
{ // all private cus we want to have changes be auto from changingprefab
    public GameObject Help_Menu_Button;
   private Button My_Button; //ts is the pause not return to menu
    private bool Game_Is_Paused;
    private Transform Paused_Indicator;
    private Transform Are_You_Sure_Button_Set;
    private Button Yes_Button;
    private Button No_Button;
    public AudioClip Button_Click_SFX;
    private Button Return_To_Main_Menu_Button;
    private CursorLockMode Lock_Cursor;
    private CursorLockMode UnLock_Cursor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Game_Is_Paused = false;

        My_Button = gameObject.GetComponent<Button>(); //pause button
        Paused_Indicator = gameObject.transform.Find("Paused_Indicator"); // this is the paused icon in the middle
        
        Are_You_Sure_Button_Set = gameObject.transform.Find("Are_You_Sure_Confirm");
        Yes_Button = Are_You_Sure_Button_Set.transform.Find("Yes").GetComponent<Button>();
        No_Button = Are_You_Sure_Button_Set.transform.Find("No").GetComponent<Button>();

        Yes_Button.onClick.AddListener(Return_To_Main_Menu_Actual);
        No_Button.onClick.AddListener(Hide_Confirmation_Set);

        Return_To_Main_Menu_Button = Paused_Indicator.transform.Find("Return_To_Main_Menu").GetComponent<Button>();

        My_Button.onClick.AddListener(Pause_Game);

        Return_To_Main_Menu_Button.onClick.AddListener(Return_To_Main_Menu); // gives its child the main menu button this feature

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
            Audio_Manager_Script.instance.Play_Selected_Audio(Button_Click_SFX, gameObject.transform.position, .7f, 1);
            Cursor.lockState = UnLock_Cursor; // for the shop phase so we can go back to main menu if we want
            Time.timeScale = 0f;
            Game_Is_Paused = true;
            Paused_Indicator.gameObject.SetActive(true);
            Help_Menu_Button.gameObject.SetActive(true);
        }
        else if (Game_Is_Paused)
        {
            Audio_Manager_Script.instance.Play_Selected_Audio(Button_Click_SFX, gameObject.transform.position, .7f, 1);
            if (Persistent_Data_Store.Current_Scene.buildIndex == 3)
            {
                Cursor.lockState = Lock_Cursor;
            }
            Time.timeScale = 1f;
            Game_Is_Paused = false;
            Paused_Indicator.gameObject.SetActive(false);
            Help_Menu_Button.gameObject.SetActive(false);
            Are_You_Sure_Button_Set.gameObject.SetActive(false);
        }

    }

    void Return_To_Main_Menu()
    {
        Audio_Manager_Script.instance.Play_Selected_Audio(Button_Click_SFX, gameObject.transform.position, .7f, 1);
        Are_You_Sure_Button_Set.gameObject.SetActive(true);
    }


    void Return_To_Main_Menu_Actual()
    {
        Persistent_Data_Store.Instance.Save_All_Information();
        Audio_Manager_Script.instance.Play_Selected_Audio(Button_Click_SFX, gameObject.transform.position, .7f, 1);
        SceneManager.LoadScene(0);
    }

    void Hide_Confirmation_Set()
    {
        Audio_Manager_Script.instance.Play_Selected_Audio(Button_Click_SFX, gameObject.transform.position, .7f, 1);
        Are_You_Sure_Button_Set.gameObject.SetActive(false);
    }
}

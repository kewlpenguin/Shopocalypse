using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;



public class Main_Gun_Controller : MonoBehaviour
{
    private float Max_Hight = 7.59f;
    private float Min_Hight = -2.87f;
    Rigidbody Main_Gun_Rigidbody;
    public float Move_Speed = 10;
    CursorLockMode Battle_Cursor_Mode;
    public string Selected_Ammo = "None";
    private int Weapon_Selected_Font_Size = 32;
    private int Default_Font_Size = 20;

    public GameObject Main;
    public GameObject Slow_Wave;
    public GameObject Sniper;
    public GameObject Pierce_Lazer;
    public GameObject Saw;
    public GameObject Vines;
    public GameObject Vine_Spawn_Reference;

    public Material Skybox_Day;
    public Material Skybox_Dusk;

    bool One_Time_Death_Explosion_Triggered = false;

    public float Selected_Bullet_Cooldown;
    public bool Secondary_On_Cooldown;
    public bool Main_On_Cooldown;
    public bool Charging = false;

    public bool Slow_Wave_On_Cooldown = false;
    public bool Sniper_On_Cooldown = false;
    public bool Saw_On_Cooldown = false;
    public bool Vines_On_Cooldown = false;
    public bool Pierce_Lazer_On_Cooldown = false;
    public bool Burst_Module_On_Cooldown = true;

    public bool Has_Fired_Ammo_In_Last_30_Secs = false;
    public float Countdown_30_Seconds = 30;

    public bool Select_Weapon_Text_Flashing = false;

    public float Sniper_Cooldown;
    public float Pierce_Lazer_Cooldown;
    public float Saw_Cooldown;
    public float Vines_Cooldown;
    public float Burst_Module_Cooldown;

    public float Sniper_Countdown; // for dissplaying cooldowns (:
    public float Pierce_Lazer_Countdown;
    public float Saw_Countdown;
    public float Vines_Countdown;
    public float Burst_Module_Countdown;

    bool Slow_Wave_Active;
    bool Sniper_Active;
    bool Pierce_Lazer_Active;
    bool Saw_Active;
    bool Vines_Active;

    public AudioClip Sniper_Fire;
    public AudioClip Basic_Fire;
    public AudioClip Water_Fire;
    public AudioClip Lazer_Fire;
    public AudioClip Fish_Fire;
    public AudioClip Vines_Fire;
    public AudioClip Burst_Wind_Up;
    public AudioClip Bomb_Activate;
   
    public AudioClip Death_Explosion;

    public AudioClip Day_Fade_In_Sound;
    public AudioClip Normal_Music;
    public AudioClip Final_Level_Music; //Day 12

    public TextMeshProUGUI Slow_Wave2;
    public TextMeshProUGUI Saw2; // for cooldowns
    public TextMeshProUGUI Vines2;
    public TextMeshProUGUI Lazer2;
    public TextMeshProUGUI Sniper2;
    public TextMeshProUGUI Burst_Module1;

    public TextMeshProUGUI Saw1; // for ammo counts
    public TextMeshProUGUI Vines1;
    public TextMeshProUGUI Lazer1;
    public TextMeshProUGUI Sniper1;
    public TextMeshProUGUI Burst_Module;

    public RawImage Slow_Image;
    public RawImage Saw_Image; // for ammo counts
    public RawImage Vines_Image;
    public RawImage Lazer_Image;
    public RawImage Sniper_Image;
    public RawImage Burst_Module_Image;

    public RawImage Slow_Image_Selected_Ammo;
    public RawImage Saw_Image_Selected_Ammo; // for selected ammo 
    public RawImage Vines_Image_Selected_Ammo;
    public RawImage Lazer_Image_Selected_Ammo;
    public RawImage Sniper_Image_Selected_Ammo;
    public RawImage Burst_Module_Image_Selected_Ammo;
    public RawImage Heart_Break_Left;
    public RawImage Heart_Break_Right;
    public RawImage White_Out;


    public TextMeshProUGUI Select_Weapon_Text;
    public TextMeshProUGUI Right_Click_Reminder;
    public TextMeshProUGUI Out_Of_Ammo_Text;
    public TextMeshProUGUI Not_Enough_For_Burst;

    public TextMeshProUGUI Day_Counter;

    public TextMeshProUGUI House_Health;

    public Image Up_Arrow;
    public Image Down_Arrow;
   
    public GameObject Game_Over_Ui_Stuff;
    public Button Back_To_Title_Screen;
    public TextMeshProUGUI Days_Survived;

    public GameObject Health_Bomb_1; // use gameobject to access the child images and the tick on the healthbar
    public GameObject Health_Bomb_2;
    public GameObject Health_Bomb_3;

    public Slider House_Health_Bar;

    public bool Are_Dead = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        Day_Counter.text = "Day " + Persistent_Data_Store.Day;
        StartCoroutine(Day_Count_Fade());

        Are_Dead = false;

        House_Health_Bar.maxValue = 200;
        House_Health_Bar.minValue = 0;
        
        Health_Bomb_Setup();

        Back_To_Title_Screen.onClick.AddListener(Return_To_Title_Screen);

        Game_Over_Ui_Stuff.SetActive(false);
      
        Not_Enough_For_Burst.gameObject.SetActive(false);
        Out_Of_Ammo_Text.gameObject.SetActive(false);
        Right_Click_Reminder.gameObject.SetActive(false);
       
        StartCoroutine(Display_Right_Click_Reminder());
      
        Selected_Ammo = "None";
        Select_Weapon_Text.gameObject.SetActive(false);
       
        Main_Gun_Rigidbody = gameObject.GetComponent<Rigidbody>();
     
        Battle_Cursor_Mode = CursorLockMode.None;
       Cursor.lockState = Battle_Cursor_Mode;

    }



    // Update is called once per frame
    void Update()
    {
        Game_Over_Check();

        if (!Are_Dead)
        {
            Health_Bomb_Handler();
            Fire_Main_Gun();
            Switch_Selected_Ammo();
            Fire_Selected_Ammo(); // if right mouse button is pressed
            Fire_Selected_Ammo_Burst(); // if space is held for some seconds
            Update_Cooldowns_And_Ammo_Counts();
        }


    }



    private void FixedUpdate()
    {
        if (!Are_Dead)
        {
            Move_Main_Gun();
        }
    }



    void Move_Main_Gun()
    {
        float Vertical_Input = Input.GetAxis("Vertical") * Move_Speed * Time.deltaTime;
       
        bool Holding_W = Input.GetKey(KeyCode.W); // for glowing arrows
        bool Holding_S = Input.GetKey(KeyCode.S);

        if (Holding_S) // down   // for ui arrow elements
        {
            Up_Arrow.color = Color.white;
            Down_Arrow.GetComponentInParent<RectTransform>().localScale = new Vector3(2f, 2f, 2f);
          
            Down_Arrow.color = Color.green;
            Up_Arrow.GetComponentInParent<RectTransform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
     
        if (Holding_W) // up
        {
            Up_Arrow.color = Color.green;
            Down_Arrow.GetComponentInParent<RectTransform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);

            Down_Arrow.color = Color.white;
            Up_Arrow.GetComponentInParent<RectTransform>().localScale = new Vector3(2f, 2f, 2f);
        }

        if (!Holding_S)
        {
            Down_Arrow.color = Color.white;
            Down_Arrow.GetComponentInParent<RectTransform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
       
        if (!Holding_W)
        {
            Up_Arrow.color = Color.white;
            Up_Arrow.GetComponentInParent<RectTransform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }


        float Current_Pos = gameObject.transform.position.y;
        float change_In_Pos = Mathf.Clamp(Current_Pos + Vertical_Input, Min_Hight, Max_Hight);

        Main_Gun_Rigidbody.Move(new Vector3(gameObject.transform.position.x, change_In_Pos, gameObject.transform.position.z), gameObject.transform.rotation);
       
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = Main_Gun_Rigidbody.transform.position.z;

        // Calculate direction FROM mouse TO object (reverse of your original)
        Vector3 direction = mouseWorldPos - Main_Gun_Rigidbody.transform.position;

        // Use the direction for rotation
        Main_Gun_Rigidbody.rotation = Quaternion.LookRotation(direction, Vector3.up);


    }

    


    void Switch_Selected_Ammo() // very inneficient way of checking what the active weapon is
    {
       

        Slow_Wave_Active = Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Alpha1);
        Sniper_Active = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Alpha2); ;
        Pierce_Lazer_Active = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Alpha4); ;
        Saw_Active = Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Alpha3); ;
        Vines_Active = Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Alpha5); ;
      

        if (Selected_Ammo == "None" && Persistent_Data_Store.Total_Ammo > 0) // if we have no ammo do not pester player
        {

            Select_Weapon_Text.gameObject.SetActive(true);
           
            if (Select_Weapon_Text_Flashing == false) // so it is only started once
            {
                Select_Weapon_Text_Flashing = true;
                StartCoroutine(Flash_Text());
            }
            
          
        }

        // i hate how i had to do this but basically it just sets all other font sizes to default and the selected one to 30 also changes the active ammo selected image depending on ammo selected

        // first line changes all font sizes depending on selected ammo, second layer turns the selected ammo image on and off depending on weapon selected

        if (Slow_Wave_Active && Persistent_Data_Store.Slow_Wave_Ammo > 0)
        {
            Selected_Ammo = "Slow_Wave"; Slow_Wave2.fontSize = Weapon_Selected_Font_Size; Vines1.fontSize = Default_Font_Size; Saw1.fontSize = Default_Font_Size; Lazer1.fontSize = Default_Font_Size; Sniper1.fontSize = Default_Font_Size; Select_Weapon_Text_Flashing = false;
            Slow_Image_Selected_Ammo.gameObject.SetActive(true); Vines_Image_Selected_Ammo.gameObject.SetActive(false); Saw_Image_Selected_Ammo.gameObject.SetActive(false); Lazer_Image_Selected_Ammo.gameObject.SetActive(false); Sniper_Image_Selected_Ammo.gameObject.SetActive(false);        }

        else if (Sniper_Active && Persistent_Data_Store.Sniper_Ammo > 0)
        {
            Selected_Ammo = "Sniper"; Slow_Wave2.fontSize = Default_Font_Size; Vines1.fontSize = Default_Font_Size; Saw1.fontSize = Default_Font_Size; Lazer1.fontSize = Default_Font_Size; Sniper1.fontSize = Weapon_Selected_Font_Size; Select_Weapon_Text_Flashing = false;   
            Slow_Image_Selected_Ammo.gameObject.SetActive(false); Vines_Image_Selected_Ammo.gameObject.SetActive(false); Saw_Image_Selected_Ammo.gameObject.SetActive(false); Lazer_Image_Selected_Ammo.gameObject.SetActive(false); Sniper_Image_Selected_Ammo.gameObject.SetActive(true);        }

        else if (Pierce_Lazer_Active && Persistent_Data_Store.Pierce_Lazer_Ammo > 0)
        {
            Selected_Ammo = "Pierce_Lazer"; Slow_Wave2.fontSize = Default_Font_Size; Vines1.fontSize = Default_Font_Size; Saw1.fontSize = Default_Font_Size; Lazer1.fontSize = Weapon_Selected_Font_Size; Sniper1.fontSize = Default_Font_Size; Select_Weapon_Text_Flashing = false;
                Slow_Image_Selected_Ammo.gameObject.SetActive(false); Vines_Image_Selected_Ammo.gameObject.SetActive(false); Saw_Image_Selected_Ammo.gameObject.SetActive(false); Lazer_Image_Selected_Ammo.gameObject.SetActive(true); Sniper_Image_Selected_Ammo.gameObject.SetActive(false);
        }

        else if (Saw_Active && Persistent_Data_Store.Saw_Ammo > 0)
        {
            Selected_Ammo = "Saw"; Slow_Wave2.fontSize = Default_Font_Size; Vines1.fontSize = Default_Font_Size; Saw1.fontSize = Weapon_Selected_Font_Size; Lazer1.fontSize = Default_Font_Size; Sniper1.fontSize = Default_Font_Size; Select_Weapon_Text_Flashing = false;
                Slow_Image_Selected_Ammo.gameObject.SetActive(false); Vines_Image_Selected_Ammo.gameObject.SetActive(false); Saw_Image_Selected_Ammo.gameObject.SetActive(true); Lazer_Image_Selected_Ammo.gameObject.SetActive(false); Sniper_Image_Selected_Ammo.gameObject.SetActive(false);
        }

        else if (Vines_Active && Persistent_Data_Store.Vines_Ammo > 0)
        {
            Selected_Ammo = "Vines"; Slow_Wave2.fontSize = Default_Font_Size; Vines1.fontSize = Weapon_Selected_Font_Size; Saw1.fontSize = Default_Font_Size; Lazer1.fontSize = Default_Font_Size; Sniper1.fontSize = Default_Font_Size; Select_Weapon_Text_Flashing = false;
                Slow_Image_Selected_Ammo.gameObject.SetActive(false); Vines_Image_Selected_Ammo.gameObject.SetActive(true); Saw_Image_Selected_Ammo.gameObject.SetActive(false); Lazer_Image_Selected_Ammo.gameObject.SetActive(false); Sniper_Image_Selected_Ammo.gameObject.SetActive(false);
        }

   

}

    IEnumerator Flash_Text()
    {
        for (int i = 999999; i > 0; i--)
        {
            if (Select_Weapon_Text_Flashing)
            {
                Select_Weapon_Text.color = Color.white;

                yield return new WaitForSeconds(.5f);

                Select_Weapon_Text.color = Color.red;

                yield return new WaitForSeconds(.5f);

                if(Select_Weapon_Text_Flashing == false)
                {
                    Debug.Log("deactivate text");
                    Select_Weapon_Text.gameObject.SetActive(false);
                    break;
                   
                }
            }
        }
    }




    IEnumerator Weapon_Cooldown_Sniper() // the worst possible way to have variable cooldowns looooool
    {
        Sniper_On_Cooldown = true;
        Persistent_Data_Store.Sniper_Ammo -= 1;
        for (float i = Selected_Bullet_Cooldown; i > 0; i -= .1f)
        {
            Sniper_Countdown = i;
            yield return new WaitForSeconds(.1f);
        }

        Sniper_On_Cooldown = false;
    }

    IEnumerator Weapon_Cooldown_Slow_Wave()
    {
        Slow_Wave_On_Cooldown = true;
      
        Persistent_Data_Store.Slow_Wave_Ammo -= 1;
        yield return new WaitForSeconds(Selected_Bullet_Cooldown);
        Slow_Wave_On_Cooldown = false;

    }


    IEnumerator Weapon_Cooldown_Pierce_Lazer()
    {
        Pierce_Lazer_On_Cooldown = true;
        Persistent_Data_Store.Pierce_Lazer_Ammo -= 1;
        for (float i = Selected_Bullet_Cooldown; i > 0; i -= .1f)
        {
            Pierce_Lazer_Countdown = i;
            yield return new WaitForSeconds(.1f);
        }

        Pierce_Lazer_On_Cooldown = false;

    }


    IEnumerator Weapon_Cooldown_Saw()
    {
        Saw_On_Cooldown = true;

        for (int i = 0; i < 3; i++) // to make a 3 round burst
        {
            if (Persistent_Data_Store.Saw_Ammo > 0)
            {
                Persistent_Data_Store.Saw_Ammo -= 1;
                Audio_Manager_Script.instance.Play_Selected_Audio(Vines_Fire, gameObject.transform.position, .25f, .75f);
                Instantiate(Saw, gameObject.transform.position + gameObject.transform.forward * 2, gameObject.transform.rotation);
                yield return new WaitForSeconds(.11f);
            }
            else if(Persistent_Data_Store.Saw_Ammo <= 0) { break; }

        }
        
       
        for (float i = Selected_Bullet_Cooldown; i > 0; i -= .1f)
        {
            Saw_Countdown = i;
            yield return new WaitForSeconds(.1f);
        }

        Saw_On_Cooldown = false;

    }

    IEnumerator Weapon_Cooldown_Vines()
    {
        Vines_On_Cooldown = true;
        Persistent_Data_Store.Vines_Ammo -= 1;
        for (float i = Selected_Bullet_Cooldown; i > 0; i -= .1f)
        {
            Vines_Countdown = i;
            yield return new WaitForSeconds(.1f);
        }

        Vines_On_Cooldown = false;

    }



    IEnumerator Main_Weapon_Cooldown()
    {
        Main_On_Cooldown = true;
        for(int i = 0; i < 4; i++)
        {
            Audio_Manager_Script.instance.Play_Selected_Audio(Basic_Fire, gameObject.transform.position, .075f, 1);

            Instantiate(Main, gameObject.transform.position + gameObject.transform.up * .75f + gameObject.transform.forward * .75f, gameObject.transform.rotation);
            yield return new WaitForSeconds(.1f);
        }
      
        yield return new WaitForSeconds(1f);
        Main_On_Cooldown = false;
    }



    IEnumerator Burst_Module_Cooldown_Timer()
    {
       Debug.Log("Running cooldown timer");

        Burst_Module_On_Cooldown = true;
        Persistent_Data_Store.Burst_Module_Ammo--;
       
        for (float i = Burst_Module_Cooldown; i > 0; i -= .1f)
        {
            Burst_Module_Countdown = i;
            yield return new WaitForSeconds(.1f);
        }
      

        Burst_Module_On_Cooldown = false;
    }



    void Fire_Selected_Ammo()// instantiates bullets and such while the bullets have their own script that controlls their behavior
    {
        bool Mouse_Down = Input.GetMouseButton(1);
        if (Mouse_Down && !Charging)
        {
            Countdown_30_Seconds = 30; // reset reminder
            Right_Click_Reminder.gameObject.SetActive(false); // hide reminder text object


            switch (Selected_Ammo) // fires ammo based on the selected ammo string
            {
                case "Slow_Wave":
                  
                    Selected_Bullet_Cooldown = .1f; // we dont need to show this cooldown

                    if (!Slow_Wave_On_Cooldown && Persistent_Data_Store.Slow_Wave_Ammo > 0)
                    {
                        StartCoroutine(Weapon_Cooldown_Slow_Wave());
                        Audio_Manager_Script.instance.Play_Selected_Audio(Water_Fire, gameObject.transform.position, 10, .75f);
                        Instantiate(Slow_Wave, gameObject.transform.position + gameObject.transform.forward * 2, gameObject.transform.rotation);
                    }
                    
                    else if(Persistent_Data_Store.Slow_Wave_Ammo <= 0) // just another safety check
                    {
                        Selected_Ammo = "None";
                        Slow_Image_Selected_Ammo.gameObject.SetActive(false);
                    }

                    break;



                case "Sniper":
                 
                    Selected_Bullet_Cooldown = Sniper_Cooldown;

                    if (!Sniper_On_Cooldown && Persistent_Data_Store.Sniper_Ammo > 0)
                    {
                        StartCoroutine(Weapon_Cooldown_Sniper());
                        Audio_Manager_Script.instance.Play_Selected_Audio(Sniper_Fire, gameObject.transform.position, .15f, 1);
                        Instantiate(Sniper, gameObject.transform.position + gameObject.transform.forward * 4, gameObject.transform.rotation);

                    }

                    else if (Persistent_Data_Store.Sniper_Ammo <= 0)
                    {
                        Selected_Ammo = "None";
                        Sniper_Image_Selected_Ammo.gameObject.SetActive(false);
                    }

                    break;



                case "Pierce_Lazer":

                    Selected_Bullet_Cooldown = Pierce_Lazer_Cooldown;

                    if (!Pierce_Lazer_On_Cooldown && Persistent_Data_Store.Pierce_Lazer_Ammo > 0)
                    {
                        StartCoroutine(Weapon_Cooldown_Pierce_Lazer());
                        Audio_Manager_Script.instance.Play_Selected_Audio(Lazer_Fire, gameObject.transform.position, .045f, 1);
                        Instantiate(Pierce_Lazer, gameObject.transform.position + gameObject.transform.forward * 4, gameObject.transform.rotation);

                    }

                    else if (Persistent_Data_Store.Pierce_Lazer_Ammo <= 0)
                    {
                        Selected_Ammo = "None";
                        Lazer_Image_Selected_Ammo.gameObject.SetActive(false);
                    }
                    break;



                case "Saw":

                    Selected_Bullet_Cooldown = Saw_Cooldown;

                    if (!Saw_On_Cooldown && Persistent_Data_Store.Saw_Ammo > 0)
                    {
                        StartCoroutine(Weapon_Cooldown_Saw());
                    }

                    else if (Persistent_Data_Store.Saw_Ammo <= 0)
                    {
                        Selected_Ammo = "None";
                        Saw_Image_Selected_Ammo.gameObject.SetActive(false);
                    }
                    break;
              


                case "Vines":

                    Selected_Bullet_Cooldown = Vines_Cooldown;

                    if (!Vines_On_Cooldown && Persistent_Data_Store.Vines_Ammo > 0)
                    {
                        StartCoroutine(Weapon_Cooldown_Vines());
                        Audio_Manager_Script.instance.Play_Selected_Audio(Vines_Fire, gameObject.transform.position, .2f, 1);
                        GameObject Vine_Shot = Instantiate(Vines, gameObject.transform.position + gameObject.transform.forward * 2, gameObject.transform.rotation); // needs changed a bit here
                        Vine_Shot.GetComponent<Bullet_Control>().Vine_Spawn = Vine_Spawn_Reference;
                    }

                    else if (Persistent_Data_Store.Vines_Ammo <= 0)
                    {
                        Selected_Ammo = "None";
                        Vines_Image_Selected_Ammo.gameObject.SetActive(false);
                    }
                    break;



                case "None":
                    //do nothing lol

                    break;
            }
        }
     
    }

    IEnumerator Display_Right_Click_Reminder()
    {
        for(int i = 99999999; i > 0; i--) // runs until scene swap
        {
            Countdown_30_Seconds--;
          
            if(Countdown_30_Seconds <=  0 && Persistent_Data_Store.Total_Ammo > 0)
            {
                Right_Click_Reminder.gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(1);
        }

    }




    void Fire_Selected_Ammo_Burst()// initiates the hold for seconds check which in turn initiates the bullet burst from initiate bullet burst through setting the charged bool to true
    {
        bool Space_down = Input.GetKey(KeyCode.Space) || Input.GetKeyDown(KeyCode.Alpha6); ;
        if (Space_down && !Charging && Persistent_Data_Store.Burst_Module_Ammo >= 1 && !Burst_Module_On_Cooldown) { StartCoroutine(Held_Space_For_Seconds()); }

        else if (!Space_down || Burst_Module_On_Cooldown) { Charging = false; }   // charging is used to stop the other weapons from firing till the burst is done firing
    }




    IEnumerator Held_Space_For_Seconds()
    {
        Debug.Log("Charging");

        Audio_Manager_Script.instance.Play_Selected_Audio(Burst_Wind_Up, gameObject.transform.position, .075f, 1);
        for (float i = 0; i < 3f; i += .1f)
        {
            bool Space_Held = Input.GetKey(KeyCode.Space) || Input.GetKeyDown(KeyCode.Alpha6); ;
          
            
            if (Space_Held)
            {
                Charging = true;
                Burst_Module1.fontSize = Weapon_Selected_Font_Size;
     
                yield return new WaitForSeconds(.1f);
                if (i >= 1f)
                {
                    Debug.Log("fire coroutine");
                    StartCoroutine(Initiate_Bullet_Burst());
                    Charging = false;
                    Burst_Module1.fontSize = Default_Font_Size;
                    break;
                }
            }

            else if (!Space_Held)
            {
                Charging = false;
                Burst_Module1.fontSize = Default_Font_Size;
                break;
            }


        }

    }



    IEnumerator Initiate_Bullet_Burst()
    {
        switch (Selected_Ammo) // fires ammo based on the selected ammo string
        {
            case "Slow_Wave":

                if (Persistent_Data_Store.Slow_Wave_Ammo >= 100 && !Burst_Module_On_Cooldown)
                {
             
                    StartCoroutine(Burst_Module_Cooldown_Timer()); // put burst module on cooldown if firing is successful also ammo is subtracted from burst module in the cooldown coroutine
                   
                    for (float i = 0; i < 100; i ++)
                    {
                        Audio_Manager_Script.instance.Play_Selected_Audio(Water_Fire, gameObject.transform.position, 5, .75f);
                        Instantiate(Slow_Wave, gameObject.transform.position + gameObject.transform.forward * 2, gameObject.transform.rotation * Quaternion.Euler(Random.Range(-10f,10f),1,1)); // need to randomize this
                        Persistent_Data_Store.Slow_Wave_Ammo--;
                        yield return new WaitForSeconds(.03f);
                    }

                }

                else if(Persistent_Data_Store.Slow_Wave_Ammo < 100) { StartCoroutine(Show_Not_Enough_Burst_Text()); }

                break;


            case "Sniper":
                if (Persistent_Data_Store.Sniper_Ammo >= 30 && !Burst_Module_On_Cooldown)
                {

                    StartCoroutine(Burst_Module_Cooldown_Timer()); // put burst module on cooldown if firing is successful also ammo is subtracted from burst module in the cooldown coroutine

                    for (float i = 0; i < 30; i++)
                    {
                        Audio_Manager_Script.instance.Play_Selected_Audio(Sniper_Fire, gameObject.transform.position, .075f, 1);
                        Instantiate(Sniper, gameObject.transform.position + gameObject.transform.forward * 4, gameObject.transform.rotation * Quaternion.Euler(Random.Range(-10f, 10f), 1, 1)); // need to randomize this
                        Persistent_Data_Store.Sniper_Ammo--;
                        yield return new WaitForSeconds(.03f);
                    }

                }

                else if (Persistent_Data_Store.Sniper_Ammo < 30) { StartCoroutine(Show_Not_Enough_Burst_Text()); }

                break;

            case "Pierce_Lazer":

                if (Persistent_Data_Store.Pierce_Lazer_Ammo >= 15 && !Burst_Module_On_Cooldown)
                {

                    StartCoroutine(Burst_Module_Cooldown_Timer()); // put burst module on cooldown if firing is successful also ammo is subtracted from burst module in the cooldown coroutine

                    for (float i = 0; i < 15; i++)
                    {
                        Audio_Manager_Script.instance.Play_Selected_Audio(Lazer_Fire, gameObject.transform.position, .0225f, 1);
                        Instantiate(Pierce_Lazer, gameObject.transform.position + gameObject.transform.forward * 4, gameObject.transform.rotation * Quaternion.Euler(Random.Range(-10f, 10f), 1, 1)); // need to randomize this
                        Persistent_Data_Store.Pierce_Lazer_Ammo--;
                        yield return new WaitForSeconds(.03f);
                    }

                }

                else if (Persistent_Data_Store.Pierce_Lazer_Ammo < 15) { StartCoroutine(Show_Not_Enough_Burst_Text()); }

                break;


            case "Saw":

                if (Persistent_Data_Store.Saw_Ammo >= 40 && !Burst_Module_On_Cooldown)
                {

                    StartCoroutine(Burst_Module_Cooldown_Timer()); // put burst module on cooldown if firing is successful also ammo is subtracted from burst module in the cooldown coroutine

                    for (float i = 0; i < 40; i++)
                    {
                        Audio_Manager_Script.instance.Play_Selected_Audio(Vines_Fire, gameObject.transform.position, .125f, .75f);
                        Instantiate(Saw, gameObject.transform.position + gameObject.transform.forward * 2, gameObject.transform.rotation * Quaternion.Euler(Random.Range(-10f, 10f), 1, 1)); // need to randomize this
                        Persistent_Data_Store.Saw_Ammo--;
                        yield return new WaitForSeconds(.03f);
                    }

                }

                else if (Persistent_Data_Store.Saw_Ammo < 40) { StartCoroutine(Show_Not_Enough_Burst_Text()); }

                break;


            case "Vines":

                if (Persistent_Data_Store.Vines_Ammo >= 5 && !Burst_Module_On_Cooldown)
                {

                    StartCoroutine(Burst_Module_Cooldown_Timer()); // put burst module on cooldown if firing is successful also ammo is subtracted from burst module in the cooldown coroutine

                    for (float i = 0; i < 5; i++)
                    {
                        Audio_Manager_Script.instance.Play_Selected_Audio(Vines_Fire, gameObject.transform.position, .25f, 1);
                        Instantiate(Vines, gameObject.transform.position + gameObject.transform.forward * 2, gameObject.transform.rotation * Quaternion.Euler(Random.Range(-10f, 10f), 1, 1)); // need to randomize this
                        Persistent_Data_Store.Vines_Ammo--;
                        yield return new WaitForSeconds(.03f);
                    }

                }

                else if (Persistent_Data_Store.Vines_Ammo < 5) { StartCoroutine(Show_Not_Enough_Burst_Text()); }

                break;

        }




    }

    IEnumerator Show_Not_Enough_Burst_Text()
    {
        Not_Enough_For_Burst.fontSize = 35;
       
        for (int i = 100; i > 0; i--)
        {

            if (!Not_Enough_For_Burst.isActiveAndEnabled)
            {
                Not_Enough_For_Burst.gameObject.SetActive(true);
            }

            yield return new WaitForSeconds(.01f);

        }
        
        Not_Enough_For_Burst.gameObject.SetActive(false);

    }



    void Fire_Main_Gun()
    {
        bool Mouse_Down = Input.GetMouseButton(0);
        if (!Main_On_Cooldown && Mouse_Down)
        {
            StartCoroutine(Main_Weapon_Cooldown());
            
        }

    }
    

    void Update_Cooldowns_And_Ammo_Counts() // this whole system is fucking terrible and not able to be scaled up easily
        //basically just enables and disables ui elements based on ammo counts, also updates ammo counts every frame and helps de select ammo after it runs out of ammo
    {

        if (Persistent_Data_Store.Total_Ammo <= 0) { Out_Of_Ammo_Text.gameObject.SetActive(true); } else if(Persistent_Data_Store.Total_Ammo > 0) { Out_Of_Ammo_Text.gameObject.SetActive(false); }

        if (Persistent_Data_Store.Sniper_Ammo > 0) { Sniper2.enabled = true; Sniper_Image.enabled = true; } else if(Persistent_Data_Store.Sniper_Ammo <= 0) { Sniper1.enabled = false; Sniper_Active = false; Sniper_Image.enabled = false; }


        Sniper1.text = "D: " + Persistent_Data_Store.Sniper_Ammo;

        if (Sniper_On_Cooldown) // if its on cooldown display cooldown timer
        {
            Sniper2.text = Sniper_Countdown.ToString("F1");

        }
        else if (!Sniper_On_Cooldown) // otherwise disable the text object
        {
            Sniper2.enabled = false;
        }




        if (Persistent_Data_Store.Saw_Ammo > 0) { Saw2.enabled = true; Saw_Image.enabled = true; } else if (Persistent_Data_Store.Saw_Ammo <= 0) { Saw1.enabled = false; Saw_Image.enabled = false; }
        Saw1.text = "E: " + Persistent_Data_Store.Saw_Ammo;

        if (Saw_On_Cooldown)
        {
            Saw2.text = Saw_Countdown.ToString("F1");

        }
        else if (!Saw_On_Cooldown)
        {
            Saw2.enabled = false;
        }




        if (Persistent_Data_Store.Vines_Ammo > 0) { Vines2.enabled = true; Vines_Image.enabled = true; } else if (Persistent_Data_Store.Vines_Ammo <= 0) { Vines1.enabled = false; Vines_Image.enabled = false; }

        Vines1.text = "Q: " + Persistent_Data_Store.Vines_Ammo;

        if (Vines_On_Cooldown)
        {
            Vines2.text = Vines_Countdown.ToString("F1");

        }
        else if (!Vines_On_Cooldown)
        {
            Vines2.enabled = false;
        }




        if (Persistent_Data_Store.Pierce_Lazer_Ammo > 0) { Lazer2.enabled = true; Lazer_Image.enabled = true; } else if (Persistent_Data_Store.Pierce_Lazer_Ammo <= 0) { Lazer1.enabled = false; Lazer_Image.enabled = false; }

        Lazer1.text = "A: " + Persistent_Data_Store.Pierce_Lazer_Ammo;

        if (Pierce_Lazer_On_Cooldown)
        {
            Lazer2.text = Pierce_Lazer_Countdown.ToString("F1");

        }
        else if (!Pierce_Lazer_On_Cooldown)
        {
            Lazer2.enabled = false;
        }




        if (Persistent_Data_Store.Slow_Wave_Ammo > 0) { Slow_Wave2.enabled = true; Slow_Image.enabled = true; } else if (Persistent_Data_Store.Slow_Wave_Ammo <= 0) { Slow_Wave2.enabled = false; Slow_Image.enabled = false; }

        Slow_Wave2.text = "Shift: " + Persistent_Data_Store.Slow_Wave_Ammo;

        
        House_Health.text = Persistent_Data_Store.House_Health.ToString("f1");
        House_Health_Bar.value = Persistent_Data_Store.House_Health;




        if (Persistent_Data_Store.Burst_Module_Ammo > 0) { Burst_Module.enabled = true; Burst_Module_Image.enabled = true; } else if (Persistent_Data_Store.Burst_Module_Ammo <= 0) { Burst_Module1.enabled = false; Burst_Module_Image.enabled = false; }

        Burst_Module1.text = "SPACE: " + Persistent_Data_Store.Burst_Module_Ammo;

        if (Burst_Module_On_Cooldown)
        {
            Burst_Module.text = Burst_Module_Countdown.ToString("F1");

        }
        else if (!Burst_Module_On_Cooldown)
        {
            Burst_Module.enabled = false;
        }



    }


    void Return_To_Title_Screen()
    {
        SceneManager.LoadScene(0);
    }

    void Game_Over_Check()
    {

        if(Persistent_Data_Store.House_Health <= 0)
        {
            Are_Dead = true;
            AudioSource[] Audio_Playing_In_Scene = GameObject.FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

            if (!One_Time_Death_Explosion_Triggered)
            {
                for (int i = Audio_Playing_In_Scene.Length; i > 0; i--)
                {
                    Destroy(Audio_Playing_In_Scene[i - 1]);
                }
                Audio_Manager_Script.instance.Play_Selected_Audio(Death_Explosion, gameObject.transform.position, .2f, 1);
                
                One_Time_Death_Explosion_Triggered = true;
            }

            Days_Survived.text = "Days Survived: " + Persistent_Data_Store.Day;
            Game_Over_Ui_Stuff.SetActive(true);
        }
    }

    



    void Health_Bomb_Setup() // if easy mode then add bomb at 75, 50, and 25 percent health, if normal mode add bomb at only 25%. also goes based on which bombs have already been used
    {
        if(Persistent_Data_Store.Normal_Mode_Active == true)
        {
            if (!Persistent_Data_Store.Health_Bomb_1_Used)
            {
                Health_Bomb_1.SetActive(true);
            }
        
            else if (Persistent_Data_Store.Health_Bomb_1_Used)
            {
                Health_Bomb_1.SetActive(false);
            }

            Health_Bomb_2.SetActive(false);
            Health_Bomb_3.SetActive(false);
        }

        else if(Persistent_Data_Store.Normal_Mode_Active == false)
        {
            if (!Persistent_Data_Store.Health_Bomb_1_Used)
            {
                Health_Bomb_1.SetActive(true);
            }

            else if (Persistent_Data_Store.Health_Bomb_1_Used)
            {
                Health_Bomb_1.SetActive(false);
            }

            if (!Persistent_Data_Store.Health_Bomb_2_Used)
            {
                Health_Bomb_2.SetActive(true);
            }

            else if (Persistent_Data_Store.Health_Bomb_2_Used)
            {
                Health_Bomb_2.SetActive(false);
            }

            if (!Persistent_Data_Store.Health_Bomb_3_Used)
            {
                Health_Bomb_3.SetActive(true);
            }

            else if (Persistent_Data_Store.Health_Bomb_3_Used)
            {
                Health_Bomb_3.SetActive(false);
            }
        }

    }




    void Health_Bomb_Handler()
    {
        if (!Persistent_Data_Store.Normal_Mode_Active)
        {

            if(Persistent_Data_Store.House_Health < 150 && !Persistent_Data_Store.Health_Bomb_3_Used)
             {
                Persistent_Data_Store.Health_Bomb_3_Used = true;

                Enemy_Behavior[] All_Active_Enemies = FindObjectsByType<Enemy_Behavior>(FindObjectsSortMode.None);
                
                Audio_Manager_Script.instance.Play_Selected_Audio(Bomb_Activate, gameObject.transform.position, .075f, 1);

                StartCoroutine(Heart_Break_Animation());

                for (int i = 0; i < All_Active_Enemies.Length; i++)
                {
                    All_Active_Enemies[i].SendMessage("Get_Bombed", SendMessageOptions.DontRequireReceiver);
                }

                Health_Bomb_3.SetActive(false);
                
            }

            if (Persistent_Data_Store.House_Health < 100 && !Persistent_Data_Store.Health_Bomb_2_Used)
            {
                Persistent_Data_Store.Health_Bomb_2_Used = true;

                Enemy_Behavior[] All_Active_Enemies = FindObjectsByType<Enemy_Behavior>(FindObjectsSortMode.None);

                Audio_Manager_Script.instance.Play_Selected_Audio(Bomb_Activate, gameObject.transform.position, .075f, 1);

                StartCoroutine(Heart_Break_Animation());

                for (int i = 0; i < All_Active_Enemies.Length; i++)
                {
                    All_Active_Enemies[i].SendMessage("Get_Bombed", SendMessageOptions.DontRequireReceiver);
                }

                Health_Bomb_2.SetActive(false);
            }

            if (Persistent_Data_Store.House_Health < 50 && !Persistent_Data_Store.Health_Bomb_1_Used)
            {
                Persistent_Data_Store.Health_Bomb_1_Used = true;

                Enemy_Behavior[] All_Active_Enemies = FindObjectsByType<Enemy_Behavior>(FindObjectsSortMode.None);

                Audio_Manager_Script.instance.Play_Selected_Audio(Bomb_Activate, gameObject.transform.position, .075f, 1);

                StartCoroutine(Heart_Break_Animation());

                for (int i = 0; i < All_Active_Enemies.Length; i++)
                {
                    All_Active_Enemies[i].SendMessage("Get_Bombed", SendMessageOptions.DontRequireReceiver);
                }

                Health_Bomb_1.SetActive(false);
            }
        }


        else if (Persistent_Data_Store.Normal_Mode_Active)
        {
          if(Persistent_Data_Store.House_Health < 50 && !Persistent_Data_Store.Health_Bomb_1_Used)
            {
                Persistent_Data_Store.Health_Bomb_1_Used = true;

                Enemy_Behavior[] All_Active_Enemies = FindObjectsByType<Enemy_Behavior>(FindObjectsSortMode.None);

                Audio_Manager_Script.instance.Play_Selected_Audio(Bomb_Activate, gameObject.transform.position, .075f, 1);

                StartCoroutine(Heart_Break_Animation());

                for (int i = 0; i < All_Active_Enemies.Length; i++)
                {
                    All_Active_Enemies[i].SendMessage("Get_Bombed", SendMessageOptions.DontRequireReceiver);
                }

                Health_Bomb_1.SetActive(false);
            }
        }

    }


    IEnumerator Heart_Break_Animation() //for heart bombs, whites out screen, fades heart pieces while seperating them, then fades the white
    {
        // save heart pieces og positions
        Vector3 temp_Right = Heart_Break_Right.transform.position;
        Vector3 temp_Left = Heart_Break_Left.transform.position;

        //show white flash and heart break
        Heart_Break_Right.gameObject.SetActive(true);
        Heart_Break_Left.gameObject.SetActive(true);
        White_Out.gameObject.SetActive(true);

        //reset all alpha values (this system is retarded btw, tmprogui is 100X more efficient)
        Heart_Break_Right.color = new Color(Heart_Break_Right.color.r, Heart_Break_Right.color.g, Heart_Break_Right.color.b, 1f);
        Heart_Break_Left.color = new Color(Heart_Break_Right.color.r, Heart_Break_Right.color.g, Heart_Break_Right.color.b, 1f);
        White_Out.color = new Color(Heart_Break_Right.color.r, Heart_Break_Right.color.g, Heart_Break_Right.color.b, 1f);

        for (int i = 100; i > 0; i--)
        {
            Heart_Break_Right.color = new Color(Heart_Break_Right.color.r, Heart_Break_Right.color.g, Heart_Break_Right.color.b, (Heart_Break_Right.color.a - .01f));
            Heart_Break_Left.color = new Color(Heart_Break_Right.color.r, Heart_Break_Right.color.g, Heart_Break_Right.color.b, (Heart_Break_Left.color.a - .01f));
           
            Heart_Break_Right.transform.Translate(Vector2.right * .5f); //split the heart pieces
            Heart_Break_Left.transform.Translate(Vector2.left * .5f);

            yield return new WaitForSeconds(.01f);
        }

        for (int i = 100; i > 0; i--)
        {
            White_Out.color = new Color(Heart_Break_Right.color.r, Heart_Break_Right.color.g, Heart_Break_Right.color.b, (White_Out.color.a - .01f));
            yield return new WaitForSeconds(.01f);
        }

        Heart_Break_Right.gameObject.SetActive(false);
        Heart_Break_Left.gameObject.SetActive(false);
        White_Out.gameObject.SetActive(false);

        Heart_Break_Right.transform.position = temp_Right;
        Heart_Break_Left.transform.position = temp_Left;

    }











    IEnumerator Day_Count_Fade()
    {
        if (Persistent_Data_Store.Day % 12 == 0 && Persistent_Data_Store.Day != 0) // set day count color and skybox
        {
            RenderSettings.skybox = Skybox_Dusk;
            Day_Counter.color = Color.red;
        }
        else if (Persistent_Data_Store.Day % 12 != 0 || Persistent_Data_Store.Day == 0)
        {
            RenderSettings.skybox = Skybox_Day;
            Day_Counter.color = Color.white;
        }

            Day_Counter.alpha = 0; // make it dissapear then slowly reappear then dissapear again

        Audio_Manager_Script.instance.Play_Selected_Audio(Day_Fade_In_Sound, gameObject.transform.position, .5f, 1);

        for (int i = 100; i > 0; i--) // fade in
        {
            Day_Counter.alpha += .01f;
            yield return new WaitForSeconds(.01f);
        }

        yield return new WaitForSeconds(2);

        for (int i = 100; i > 0; i--) //fade out
        {
            Day_Counter.alpha -= .01f;
            yield return new WaitForSeconds(.01f);
        }

        if ((Persistent_Data_Store.Day % 12) != 0 || Persistent_Data_Store.Day == 0) //play normal level audio, also the second condition is for when the day is zero in which we get a false positive
        {
            Music_Controller.Music_instance.Play_Selected_Audio(Normal_Music, GameObject.Find("Main Camera").transform.position, .15f, 1, true, true,true, 999999999);
        }

        else if ((Persistent_Data_Store.Day % 12) == 0 && Persistent_Data_Store.Day != 0) // play special level audio aka level final and every increment of 12
        {
            Music_Controller.Music_instance.Play_Selected_Audio(Final_Level_Music, GameObject.Find("Main Camera").transform.position, .6f, 1, true, true, true, 99999999);

        }


    }


  






















}

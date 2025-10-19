using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;





public class Persistent_Data_Store : MonoBehaviour
{
    public static Persistent_Data_Store Instance;
    
    static public float House_Health = 100; 
    static public float Slow_Wave_Ammo = 0; 
    static public float Sniper_Ammo = 0;
    static public float Saw_Ammo = 0;
    static public float Vines_Ammo = 0;
    static public float Pierce_Lazer_Ammo = 0;
    static public float Burst_Module_Ammo = 0;

    static public int Difficulty = 0;   // increment on every shop scene swap, because we add 1 to difficulty when building enemy rosters this value technically starts at 1 even in normal mode
    static public int Difficulty_Increment = 0;



    static public int Points_To_Allocate;

    static public int Scene_Swaps = 0;

    static public int Pre_Shopping_Time = 99999999; // time to wander about and plan the shopping

    static public int Shopping_Time; // time the shopping spree actually lasts, does not start until pre shop time runs out or we pick up an object

    static public int Shopping_Countdown; // the number that gets decrem,ented and keeps track of time left to shop



    public bool Normal_Mode_Active = false;

    static public bool Game_Has_Started = false;

    static public Scene Current_Scene;


    public int Test_Show_Var;
    static public List<int> Choosen_Enemy_Numbers = new List<int>(); // for picking enemies from the different lists
    static public List<int> Choosen_Spawn_List_Numbers = new List<int>(); //  for picking what lists of enemies we want to pull enemir=es from

   public TextMeshProUGUI Shopping_Timer;

    private Button Start_Button;


    private Toggle Normal_Mode_Button;
   private Toggle Easy_Mode_Button;
   private ToggleGroup Difficulty_Selector;

  
   
 






    void Start()
    {
        Assign_Buttons();

        Current_Scene = SceneManager.GetActiveScene();
       
    }


 
    void Update()
    {
        Check_For_Scene_Swap();
        Test_Show_Var = Difficulty;
       
        if (Current_Scene.buildIndex == 3)
        {
            Game_Has_Started = FindFirstObjectByType<Player_Controller>().Started_Real_Countdown;
           
            Check_For_Object_Pickup();
         
        }

        if (Current_Scene.buildIndex == 0) // if we are on the title screen
        {
            Check_Difficulty_Toggle();
        }

    }



    private void Awake()
    {


        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Don't destroy this object
        }
       
        else
        {
            Destroy(gameObject); // Destroy any duplicates
        }

      


    }


    void Assign_Buttons()// and toggles
    {
        
        Start_Button = GameObject.Find("Start").GetComponent<Button>();

        Difficulty_Selector = GameObject.Find("Toggle_Group").GetComponent<ToggleGroup>();
        Normal_Mode_Button = GameObject.Find("Select_Difficulty_Option2").GetComponent<Toggle>();
        Easy_Mode_Button = GameObject.Find("Select_Difficulty_Option1").GetComponent<Toggle>();
        
       


        Difficulty_Selector.allowSwitchOff = true;

        Normal_Mode_Button.group = Difficulty_Selector;
        Easy_Mode_Button.group = Difficulty_Selector;
       
        Normal_Mode_Button.onValueChanged.AddListener(Activate_Start_Button);
        Easy_Mode_Button.onValueChanged.AddListener(Activate_Start_Button);
        
        Start_Button.onClick.AddListener(Swap_To_Scene_1);
       


        Start_Button.gameObject.SetActive(false);

    

    }


    void Check_Difficulty_Toggle()
    {
     
        if (Normal_Mode_Button.isOn)
        {
            Normal_Mode_True();
        }
       
        else if (!Normal_Mode_Button.isOn)
        {
            Normal_Mode_false();
        }

      

    }


    void Normal_Mode_True()
    {
        Normal_Mode_Active = true;
    }

    void Normal_Mode_false()
    {
        Normal_Mode_Active = false;
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



    void Swap_To_Scene_1() // to defend the house scene once from the title screen to the first defend the house instance
    {
        if (Normal_Mode_Active)
        {
            Difficulty += 1;
            Difficulty_Increment = 3;
        }
      
        else if (!Normal_Mode_Active) // difficulty starts lower and increments slower on easy mode
        {
            Difficulty_Increment = 2;
        }
       
        Build_Next_Enemy_Roster();

        SceneManager.LoadScene(1);
    }

  

    void Check_For_Scene_Swap() // scene 2 is transition 3 is shopp 0 is title and 1 is defend
    {
       
        Scene Temp = SceneManager.GetActiveScene();

        if (Temp != Current_Scene) // current and temp should be the same at the beginning but as soon as the scene changes temp will change first and the if will run
        {
            Shopping_Timer.gameObject.SetActive(false);
            Scene_Swaps++;
         
            if(Temp.buildIndex == 2) // increment difficulty during every show next enemies phase immediately after the enemies have been decided for the upcoming level
            {
                Difficulty += Difficulty_Increment;
               
                Build_Next_Enemy_Roster(); // after difficulty is incremented and while we are in the shopping scene so it is readyt for later, this will not be applyed until after the scene transition scene
                Shopping_Time = 20 + (2 * Difficulty);
           


                if (Shopping_Time > 110) // maximum shop time for later waves (slightly less than pre shop time)
                {
                    Shopping_Time = 110;
                }

            }
           

            if (Temp.buildIndex == 3) // if the scene is Shopping_Time then start shoping timer
            {

                Shopping_Time = 10 + (5 * Difficulty);
               
                Shopping_Timer.gameObject.SetActive(true);
              
                Shopping_Countdown = Pre_Shopping_Time;
               
                Shopping_Timer.text = "Time Until Shopping Spree Start: " + Shopping_Countdown;
              
                StartCoroutine(Shopping_Timer_Countdown());
            }
        }
        



        Current_Scene = SceneManager.GetActiveScene();

    }





    IEnumerator Shopping_Timer_Countdown() // decrements global timer that swaps to house defend scene when reaches 0
    {
        for (int i = Pre_Shopping_Time; i > 0; i--)
        {

            yield return new WaitForSeconds(1);
            Shopping_Countdown--;

        }
    }



    void Check_For_Object_Pickup() // if the game has started (due to an object being picked up or the pre shop timer running out) then increase the size of the countdown text
    {
        if (Game_Has_Started) // if we pickup an object before the pre shopping time is over set the countdown to be the shopping time because the game has started
        {
            Shopping_Timer.fontSize = 40;

            if (Shopping_Countdown > Shopping_Time)
            {
                Shopping_Countdown = Shopping_Time;
            }

        }


        if (Shopping_Countdown < Shopping_Time)
        {
            Game_Has_Started = true;
        }

        if (!Game_Has_Started) 
        { 
        Shopping_Timer.fontSize = 25;
         }


        if (Shopping_Countdown > Shopping_Time) // if we are in the pre shopping time make the text say untill spree starts instead of until next vwave arrives
        {

            Shopping_Timer.text = "WARNING: TAKING ANY ITEM WILL ALERT THE NEXT WAVE ";
            Shopping_Timer.color = Color.yellow;
        }
       
        else if (Shopping_Countdown <= Shopping_Time) // iff the spree time has begun
        {
            Shopping_Timer.fontSize = 40;
            Shopping_Timer.color = Color.red;
            Shopping_Timer.text = "Time Until Next Wave Arrives: " + Shopping_Countdown;
        }

    }







    void Build_Next_Enemy_Roster()
    {
        // Choosen_Enemy_Numbers : depends on choosen spawn list
        // Choosen_Spawn_List_Numbers :1 normal, 2 normal clumped and disruptor, 3  disruptor clumped, 5 hell, 6 hell clumped
        // List_Of_Above_Lists // 

        //Index Of Enemy Lists:
        // 
        // Spawn_List_List: 0: Normal_Spawns  1: Clumped Normal_Spawns  2: Disrupter_Spawns  3: Clumped Disrupter_Spawns  4: Hell_Spawns  5: Clumped Hell_Spawns
        //
        //
        // Normal_Enemies: 0: fast  1: Basic  2: Flyer
        //
        // Disrupter_Enemies: 0: Heavy  1: Lava_Mini  2: Roller
        //
        // Hell_Enemies: 0: Super_Heavy  1: Lava_Hound  2: Charger
        //
        //

        Choosen_Spawn_List_Numbers.Clear();// empty list first

       
        Points_To_Allocate = (Difficulty + 1); // each enemy spawn group will cost a different point value, we reset this each time 

        int Number_Choice;

        Debug.Log("Current Difficulty: " + Difficulty);
        for (int i = 999999; i > 0 && !(Points_To_Allocate <= 0); i--)
        {
           

            switch (Points_To_Allocate)
            {
                case >= 6:            // if we have more than six points then randomly choose between these enemy lists to pick from
                 //   Debug.Log("Points Left: " + Points_To_Allocate);
                    Number_Choice = Random.Range(0, 6);
                    if (Number_Choice == 0) { Choosen_Spawn_List_Numbers.Add(5); Points_To_Allocate -= 6; } // add hell clumped
                    else if (Number_Choice == 1) { Choosen_Spawn_List_Numbers.Add(4); Points_To_Allocate -= 5; } //add hell
                    else if (Number_Choice == 2) { Choosen_Spawn_List_Numbers.Add(3); Points_To_Allocate -= 3; } //add disrupter clumped    
                    else if (Number_Choice == 3) { Choosen_Spawn_List_Numbers.Add(2); Points_To_Allocate -= 2; } //add disrupter
                    else if (Number_Choice == 4) { Choosen_Spawn_List_Numbers.Add(1); Points_To_Allocate -= 2; } //add normal clumped
                    else if (Number_Choice == 5) { Choosen_Spawn_List_Numbers.Add(0); Points_To_Allocate--; }    //add normal

                    break;


                case 5:
                 //   Debug.Log("Points Left: " + Points_To_Allocate);
                    Number_Choice = Random.Range(0, 5);
                    if (Number_Choice == 0) { Choosen_Spawn_List_Numbers.Add(4); Points_To_Allocate -= 5; } //add hell
                    else if (Number_Choice == 1) { Choosen_Spawn_List_Numbers.Add(3); Points_To_Allocate -= 3; } //add disrupter clumped    
                    else if (Number_Choice == 2) { Choosen_Spawn_List_Numbers.Add(2); Points_To_Allocate -= 2; } //add disrupter
                    else if (Number_Choice == 3) { Choosen_Spawn_List_Numbers.Add(1); Points_To_Allocate -= 2; } //add normal clumped
                    else if (Number_Choice == 4) { Choosen_Spawn_List_Numbers.Add(0); Points_To_Allocate--; }    //add normal
                    break;


                case 4:
                 //   Debug.Log("Points Left: " + Points_To_Allocate);
                    Number_Choice = Random.Range(0, 4);
                    if (Number_Choice == 0) { Choosen_Spawn_List_Numbers.Add(3); Points_To_Allocate -= 3; } //add disrupter clumped    
                    else if (Number_Choice == 1) { Choosen_Spawn_List_Numbers.Add(2); Points_To_Allocate -= 2; } //add disrupter
                    else if (Number_Choice == 2) { Choosen_Spawn_List_Numbers.Add(1); Points_To_Allocate -= 2; } //add normal clumped
                    else if (Number_Choice == 3) { Choosen_Spawn_List_Numbers.Add(0); Points_To_Allocate--; }    //add normal
                    break;

                case 3:  // same as 4
                 //   Debug.Log("Points Left: " + Points_To_Allocate);
                    Number_Choice = Random.Range(0, 4);
                    if (Number_Choice == 0) { Choosen_Spawn_List_Numbers.Add(3); Points_To_Allocate -= 3; } //add disrupter clumped    
                    else if (Number_Choice == 1) { Choosen_Spawn_List_Numbers.Add(2); Points_To_Allocate -= 2; } //add disrupter
                    else if (Number_Choice == 2) { Choosen_Spawn_List_Numbers.Add(1); Points_To_Allocate -= 2; } //add normal clumped
                    else if (Number_Choice == 3) { Choosen_Spawn_List_Numbers.Add(0); Points_To_Allocate--; }    //add normal
                    break;


                case 2:
                 //   Debug.Log("Points Left: " + Points_To_Allocate);
                    Number_Choice = Random.Range(0, 3);
                    if (Number_Choice == 0) { Choosen_Spawn_List_Numbers.Add(2); Points_To_Allocate -= 2; } //add disrupter
                    else if (Number_Choice == 1) { Choosen_Spawn_List_Numbers.Add(1); Points_To_Allocate -= 2; } //add normal clumped
                    else if (Number_Choice == 2) { Choosen_Spawn_List_Numbers.Add(0); Points_To_Allocate--; }    //add normal
                    break;



                case 1:
                 //   Debug.Log("Points Left: " + Points_To_Allocate);
                    Choosen_Spawn_List_Numbers.Add(0); Points_To_Allocate--;  //add normal

                    break;

                case 0:
                    Debug.Log(Points_To_Allocate);

                    break;

            }
           
            
            if (Points_To_Allocate <= 0)
            {
                for (int j = Choosen_Spawn_List_Numbers.Count - 1; j >= 0; j--) // test list list
                {
                    Debug.Log(Choosen_Spawn_List_Numbers[j]);
                }
              
                for (int k = Choosen_Spawn_List_Numbers.Count; k > 0; k--) // choose what enemies to spawn from selected lists
                {
                    Choosen_Enemy_Numbers.Add(Random.Range(0, 3)); // each enemy list only has 3 options so we can use this for all of them
                }

                for (int l = Choosen_Enemy_Numbers.Count - 1; l >= 0; l--) //
                {
                    Debug.Log("Choosen enemy number " + l + ": " + Choosen_Enemy_Numbers[l]);
             
                }
            }
                
            

        }

       



    }












}

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;






public class Persistent_Data_Store : MonoBehaviour
{
    public static Persistent_Data_Store Instance;
    
    static public float House_Health = 100; 
    static public float Slow_Wave_Ammo = 999; 
    static public float Sniper_Ammo = 9999;
    static public float Saw_Ammo = 300;
    static public float Vines_Ammo = 99;
    static public float Pierce_Lazer_Ammo = 2990;
    static public float Burst_Module_Ammo = 3999;

    static public int Difficulty = 0;   // increment on every shop scene swap

    static public int Points_To_Allocate;

    static public int Scene_Swaps = 0;

    static public int Pre_Shopping_Time = 20;

    static public int Shopping_Time;
    
    bool Hard_Mode_Active = false;


    static public Scene Current_Scene;


    public int Test_Show_Var;
    static public List<int> Choosen_Enemy_Numbers = new List<int>(); // for picking enemies from the different lists
    static public List<int> Choosen_Spawn_List_Numbers = new List<int>(); //  for picking what lists of enemies we want to pull enemir=es from

   // List<List<int>> List_Of_Above_Lists = new List<List<int>>();






    void Start()
    {
        Current_Scene = SceneManager.GetActiveScene();
        Build_Next_Enemy_Roster();
    }


 
    void Update()
    {
        Check_For_Scene_Swap();
        Test_Show_Var = Difficulty;

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



   void Check_For_Scene_Swap()
    {
       
        Scene Temp = SceneManager.GetActiveScene();

        if (Temp != Current_Scene) // current and temp should be the same at the beginning but as soon as the scene changes temp will change first and the if will run
        {
            Scene_Swaps++;
         
            if(Temp.buildIndex == 2) // increment difficulty during every shop phase immediately after the enemies have been decided for the upcoming level
            {
                Difficulty += 3;
                Build_Next_Enemy_Roster(); // after difficulty is incremented and while we are in the shopping scene so it is readyt for later, this will not be applyed until after the scene transition scene
                Shopping_Time = 10 + (5 * Difficulty);
            }

        }
        



        Current_Scene = SceneManager.GetActiveScene();

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

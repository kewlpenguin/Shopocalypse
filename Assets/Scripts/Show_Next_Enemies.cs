using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;






public class Show_Next_Enemies : MonoBehaviour // an extremely butchered version of the real spawn routine
{
    
    public GameObject Basic_Enemy;
    public GameObject Roller_Enemy;
    public GameObject Fast_Enemy;
    public GameObject Heavy_Enemy;
    public GameObject Flyer_Enemy;
    public GameObject Lava_Hound_Mini_Enemy;
    public GameObject Lava_Hound_Enemy;
    public GameObject Super_Heavy;
    public GameObject Charger;

    public Vector3 General_Spawn_Pos = new Vector3(78, 4, 14);

    List<GameObject> Normal_Spawns = new List<GameObject>();
    List<GameObject> Disrupter_Spawns = new List<GameObject>();
    List<GameObject> Hell_Spawns = new List<GameObject>();

    List<List<GameObject>> Spawn_Lists = new List<List<GameObject>>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        General_Spawn_Pos = gameObject.transform.position;


        Fill_Out_Normal_Spawns_List();  // fill all lists
        Fill_Out_Disrupter_Spawns_List();
        Fill_Out_Hell_Spawns_List();
        Fill_Out_Spawns_List_List();

        StartCoroutine(Delay_Start());
      




    }


    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(0, .04f, 0);
    }

    IEnumerator Delay_Start() // wait for persistent data to generate enemies
    {
        yield return new WaitForSeconds(.2f);
        Instantiate_All_Enemies();
    }



    //-6, 14 ,30

    void Instantiate_Enemy_From_GameObject(GameObject Enemy, float Z_Offset) // z offset for clumped and normal differentiation, aka  More modifiable randomised spawning
    {
            Instantiate(Enemy, General_Spawn_Pos + new Vector3(0, 0, Z_Offset), Quaternion.Euler(0, 0, 0));
    }

   
    void Fill_Out_Normal_Spawns_List()
    {
        Normal_Spawns.Add(Fast_Enemy);
        Normal_Spawns.Add(Basic_Enemy);
        Normal_Spawns.Add(Flyer_Enemy);
    }

    void Fill_Out_Disrupter_Spawns_List()
    {
        Disrupter_Spawns.Add(Heavy_Enemy);
        Disrupter_Spawns.Add(Lava_Hound_Mini_Enemy);
        Disrupter_Spawns.Add(Roller_Enemy);
    }

    void Fill_Out_Hell_Spawns_List()
    {
        Hell_Spawns.Add(Super_Heavy);
        Hell_Spawns.Add(Lava_Hound_Enemy);
        Hell_Spawns.Add(Charger);
    }

    void Fill_Out_Spawns_List_List()
    {

        Spawn_Lists.Add(Normal_Spawns);
        Spawn_Lists.Add(Normal_Spawns); //clumped

        Spawn_Lists.Add(Disrupter_Spawns);
        Spawn_Lists.Add(Disrupter_Spawns);//clumped

        Spawn_Lists.Add(Hell_Spawns);
        Spawn_Lists.Add(Hell_Spawns);//clumped
    }


    // the way this spawning works is in x steps
    // 1: persistent data has the wanted enemy type numbers list and enemy lisnt numbers (normal, disruptor or hell)
    //
    // 2: we loop through the lists choosen in persistent data EX: Choosen_Lists(1, 0)  there are 2 lists so the loop runs twice
    //
    // 3: each loop calls a spawner that has its own for loops based on the type of enemy we want to spawn
    //
    // 4: every spawner loop calls the spawn delay coroutine which decides when said enemies will be spawned
    //






    void Instantiate_All_Enemies()
    {
        Debug.Log("To spawn this wave: ");
        for (int i = Persistent_Data_Store.Choosen_Spawn_List_Numbers.Count; i > 0; i--)
        {
            Modifiable_Enemy_Spawner((Spawn_Lists[Persistent_Data_Store.Choosen_Spawn_List_Numbers[i - 1]][Persistent_Data_Store.Choosen_Enemy_Numbers[i - 1]]), Persistent_Data_Store.Choosen_Spawn_List_Numbers[i - 1]);
            //gives the gameobject ref we want to spawn plus the spawn list num for clumped or not to our spawner function
        }

    }


    void Modifiable_Enemy_Spawner(GameObject Enemy_To_Spawn, int Spawn_List_Num)  // spawnlistnum for clumped or non clumped identification
    {
        Debug.Log(Enemy_To_Spawn.tag + ", ");
      
        //non clumped spawn calls
        if (Spawn_List_Num == 0 || Spawn_List_Num == 2 || Spawn_List_Num == 4)
        {
            Instantiate_Enemy_From_GameObject(Enemy_To_Spawn,Random.Range(-15f,15f));  
        }


        //clumped spawn calls
        if (Spawn_List_Num == 1 || Spawn_List_Num == 3 || Spawn_List_Num == 5)
        {

            StartCoroutine(Clumped_Enemy_Spawns(Enemy_To_Spawn, 3));
        }


    }


   IEnumerator Clumped_Enemy_Spawns(GameObject Enemy_To_Spawn, int Enemy_Count)
    {
        float Z_Offset_For_Clump = Random.Range(-15f, 15f);
               
        for (int k = Enemy_Count; k > 0; k--) 
                {
                    Instantiate_Enemy_From_GameObject(Enemy_To_Spawn, Z_Offset_For_Clump + Random.Range(-1f, 1f));
            yield return new WaitForSeconds(.01f);
                }

    }



}

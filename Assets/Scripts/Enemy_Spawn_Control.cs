using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;






public class Enemy_Spawn_Control : MonoBehaviour   // pulls straight from persistent data
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

    public Vector3 General_Spawn_Pos = new Vector3(37f, 1.56f, 4f);

    List<GameObject> Normal_Spawns = new List<GameObject>();
    List<GameObject> Disrupter_Spawns = new List<GameObject>();
    List<GameObject> Hell_Spawns = new List<GameObject>();

    List<List<GameObject>> Spawn_Lists = new List<List<GameObject>>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Fill_Out_Normal_Spawns_List();  // fill all lists
        Fill_Out_Disrupter_Spawns_List();
        Fill_Out_Hell_Spawns_List();
        Fill_Out_Spawns_List_List();


        Instantiate_All_Enemies();




    }


    // Update is called once per frame
    void Update()
    {
        
    }

    void Instantiate_Enemy_From_GameObject(GameObject Enemy)
    {
        if (Enemy.tag != "Roller" && Enemy.tag != "Lava_Hound")
        {
            Instantiate(Enemy, General_Spawn_Pos + new Vector3(Random.Range(1f, 2f), Random.Range(0f, 2f), Random.Range(-2f, 2f)), Quaternion.Euler(0, 0, 0));
        }
       
        else if(Enemy.tag == "Roller")
        {
            Instantiate(Roller_Enemy, General_Spawn_Pos + new Vector3(Random.Range(1f, 2f), Random.Range(0f, 2f), 0), Quaternion.Euler(0, 0, 0)); // make sure rollers have a consistently lined up z
                                                                                                                                                  // value because their collider must be smaller blah blah blah}
        }
       
        else if (Enemy.tag == "Lava_Hound")
        {
           GameObject My_Hound = Instantiate(Lava_Hound_Enemy, General_Spawn_Pos + new Vector3(Random.Range(1f, 2f), Random.Range(0f, 2f), Random.Range(-2f, 2f)), Quaternion.Euler(0, 0, 0)); // spawn lava hound
           StartCoroutine(Lavahound_Spawn_Routine(My_Hound)); // start children spawn coroutine

        }
    }

    IEnumerator Lavahound_Spawn_Routine(GameObject My_Hound) // spawn enemies forever while hound is alive
    {
        for (int i = 0; i < 999999; i++)
        {
            if (My_Hound.gameObject == true) // idk how this works lol
            {
                GameObject My_Spawn = Instantiate(Normal_Spawns[Random.Range(0, 3)], My_Hound.transform.position, Quaternion.Euler(0, 0, 0));
                My_Spawn.GetComponent<Rigidbody>().AddForce(new Vector3(20, 0, 0), ForceMode.Impulse);
                
                yield return new WaitForSeconds(5);
            }
        }

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
        if (Spawn_List_Num == 0) 
            {
                for (int i = 12; i > 0; i--) { StartCoroutine(Non_Clumped_Enemy_Spawn_Delay(Enemy_To_Spawn)); } // if normal spawn 20
            }



        else if (Spawn_List_Num == 2)
            {
                for (int J = 6; J > 0; J--) { StartCoroutine(Non_Clumped_Enemy_Spawn_Delay(Enemy_To_Spawn)); }  // if disrupter spawn 10
            }


        else if (Spawn_List_Num == 4)
            {

                if (Enemy_To_Spawn.CompareTag("Charger"))
                 {
                     for (int k = 5; k > 0; k--) { StartCoroutine(Non_Clumped_Enemy_Spawn_Delay(Enemy_To_Spawn)); }  // spawn 9 chargers
                 }


                 else if (!(Enemy_To_Spawn.tag == "Charger"))
                {
                     for (int l = 3; l > 0; l--) { StartCoroutine(Non_Clumped_Enemy_Spawn_Delay(Enemy_To_Spawn)); }  // if hell spawn 3
                }



        }



        //clumped spawn calls
        if (Spawn_List_Num == 1)
        {
            StartCoroutine(Clumped_Enemy_Spawn_Delay(Enemy_To_Spawn, 18));  // if normal spawn 20
        }


        else if (Spawn_List_Num == 3)
        {
            StartCoroutine(Clumped_Enemy_Spawn_Delay(Enemy_To_Spawn, 12));   // if disrupter spawn 10
        }

        else if (Spawn_List_Num == 5)
        {
            if (Enemy_To_Spawn.CompareTag("Charger"))  // spawn more chargers than opther hell enemies
            {
                StartCoroutine(Clumped_Enemy_Spawn_Delay(Enemy_To_Spawn, 9));
            }

            else if (!(Enemy_To_Spawn.tag == "Charger"))
            {
                StartCoroutine(Clumped_Enemy_Spawn_Delay(Enemy_To_Spawn, 3));   // if hell spawn 3
            }
        }




    }


    IEnumerator Clumped_Enemy_Spawn_Delay(GameObject Enemy_To_Spawn, int Enemy_Count)
    {


        if (Enemy_To_Spawn.tag != "Lava_Hound" && Enemy_To_Spawn.tag != "Super_Heavy")
        {
            for (int m = 3; m > 0; m--) // spawn cump 3 times
            {
                yield return new WaitForSeconds(16 + Random.Range(-5, 5)); //Spawn clumps at 3 random ish points and not at the beggining
              
                for (int k = (Enemy_Count / 3); k > 0; k--) // spawn 1/3 of the enemies 3 times if not normal hell enemies
                {
                    Instantiate_Enemy_From_GameObject(Enemy_To_Spawn);
                    yield return new WaitForSeconds(.1f + Random.Range(-.1f, .1f));  // time between enemies in clump spawn
                }

              
            }
          
        }


        else if(Enemy_To_Spawn.tag == "Lava_Hound" || Enemy_To_Spawn.tag == "Super_Heavy")
        {
            yield return new WaitForSeconds(25 + Random.Range(-20f, 20f)); // between 5 and 45 seconds
            for (int k = (Enemy_Count / 3); k > 0; k--)
            {
                Instantiate_Enemy_From_GameObject(Enemy_To_Spawn);
                yield return new WaitForSeconds(2 + Random.Range(-1f, 1f)); 
            }
        }

    }


    IEnumerator Non_Clumped_Enemy_Spawn_Delay(GameObject Enemy_To_Spawn)
    {
            yield return new WaitForSeconds(Random.Range(0f,50f));
            Instantiate_Enemy_From_GameObject(Enemy_To_Spawn);
    }

  







}

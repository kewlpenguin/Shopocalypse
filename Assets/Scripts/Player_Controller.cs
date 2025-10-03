using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
public class Player_Controller : MonoBehaviour
{
    public float Speed;
    public float Jump_Power;
    public float Gravity_Mult;
    CursorLockMode Lock_Cursor;
    Rigidbody Player_Rigidbody;
    GameObject Main_Camera;
  

    List<GameObject> Ammo_Pickups;
    List<string> Ammo_Spawn_Tags;
    public GameObject Slow_Wave_Pickup;
    public GameObject Sniper_Pickup;
    public GameObject Lazer_Pickup;
    public GameObject Saw_Pickup;
    public GameObject Vines_Pickup;
    public GameObject Burst_Module_Pickup;

    public GameObject Door_To_Enable;
    public GameObject Door_To_Disable;
    public GameObject Ticket;

    public GameObject Main_Door_Left;
    public GameObject Main_Door_Right;

    public GameObject Slow_Wave_Spawn;
    public GameObject Lazer_Spawn;

    public Vector3 Spawn_Pos;

    public TextMeshProUGUI Slow_Wave;
    public TextMeshProUGUI Saw; // for ammo counts
    public TextMeshProUGUI Vines;
    public TextMeshProUGUI Lazer;
    public TextMeshProUGUI Sniper;
    public TextMeshProUGUI Burst_Module;

    public GameObject Soda_1;
    public GameObject Soda_2;
    public GameObject Soda_3;
    public GameObject Soda_4;
    public GameObject Soda_5;

    private List<GameObject> Sodas = new List<GameObject>();


    public TextMeshProUGUI House_Health;

   public Slider Pickup_Progress_Bar;

    private bool In_Ticket_Booth = false;

    private bool Holding_Sniper_Large = false;
    private bool Holding_Health = false;
    private bool Holding_Sniper_Xtra = false;

    private bool Main_Doors_Open = false;
    private bool Main_Doors_Opening = false; // just means moving, not specifically opening
   
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.transform.position = Spawn_Pos;
       
        Lock_Cursor = CursorLockMode.Locked;
        Cursor.lockState = Lock_Cursor;
      
        Player_Rigidbody = GetComponent<Rigidbody>();
      
        Main_Camera = GameObject.Find("Main Camera");
      
        Instantiate_Ammo();

        StartCoroutine(Slow_Wave_Routine_Spawn());
        StartCoroutine(Lazer_Routine_Spawn());

        Sodas.Add(Soda_1);
        Sodas.Add(Soda_2);
        Sodas.Add(Soda_3);
        Sodas.Add(Soda_4);
        Sodas.Add(Soda_5);


    }


    // Update is called once per frame
    void Update()
    {
        Update_Ammo_Counts();
        Check_Object_Pickup();
    }



    private void FixedUpdate()
    {
        Move_Player();
        Match_Camera_Rotate();
    }



    private void Move_Player() // also includes the pickup objects function
    {
        Vector3 Horizontal = Input.GetAxis("Horizontal") * Speed * Time.deltaTime * Player_Rigidbody.transform.right;
        Vector3 Forward = Input.GetAxis("Vertical") * Speed * Time.deltaTime * Player_Rigidbody.transform.forward;
        Vector3 Vertical = Physics.gravity;


        Player_Rigidbody.linearVelocity = Horizontal + Vertical + Forward;

      


    }


    void Check_Object_Pickup()
    {
        bool Mouse_Down = Input.GetKeyDown(KeyCode.Mouse0);

        if (Mouse_Down)
        {
            Pickup_Object();
        }

    }






    private void Match_Camera_Rotate()
    {
        Quaternion turnRotation;
        float y = Main_Camera.transform.eulerAngles.y;


        turnRotation = Quaternion.Euler(0, y, 0);


        Player_Rigidbody.transform.rotation = (turnRotation);

        
    }


    private void Pickup_Object()
    {
        LayerMask Ammo_Layer = LayerMask.GetMask("ammo");

        RaycastHit Object_Info;
        bool Object_In_Range = Physics.Raycast(Main_Camera.transform.position, Main_Camera.transform.forward, out Object_Info, 7f, Ammo_Layer);
        if (Object_In_Range && Object_Info.collider.gameObject.tag != "Sniper_Ammo_Large" && Object_Info.collider.gameObject.tag != "Sniper_Ammo_Xtra_Large" && Object_Info.collider.gameObject.tag != "Health_Pickup" )   // if pickup is instant for this ammo type
        {
            if (Object_In_Range && Object_Info.collider.gameObject.tag != "ground") //make sure picked up is actually ammo also 13 is the ammo layer for all ammo types
            {


                Increment_Ammo_Counters(Object_Info.collider.tag);
                Destroy(Object_Info.collider.gameObject);

            }
        }
       
        else if(Object_In_Range && Object_Info.collider.gameObject.tag == "Sniper_Ammo_Large")   
        {
            if (!Holding_Sniper_Large) // so we are only able to start 1 coroutine at a time
            {
                StartCoroutine(Large_Ammo_Wait(Object_Info));
            }
        }
       

        else if (Object_In_Range && Object_Info.collider.gameObject.tag == "Sniper_Ammo_Xtra_Large")
        {
            StartCoroutine(Xtra_Large_Ammo_Wait(Object_Info));

        }
      

        else if (Object_In_Range && Object_Info.collider.gameObject.tag == "Health_Pickup")
        {
            StartCoroutine(Health_Pickup_Wait(Object_Info));

        }



    }


   

    IEnumerator Large_Ammo_Wait(RaycastHit Ammo_We_Looking_At) // all wait functions are checking if we are holding the pickup key over the course of X seconds
    {
        int Time_To_Wait = 10;
        Pickup_Progress_Bar.maxValue = Time_To_Wait;
        Pickup_Progress_Bar.gameObject.SetActive(true);
      
        for (int i = 0; i < 9999; i++)
        {
            bool temp = Input.GetKey(KeyCode.Mouse0);
            Holding_Sniper_Large = temp;

            Pickup_Progress_Bar.value = i;

            if (!Holding_Sniper_Large) // if we stop holding exit the loop
            {
                Pickup_Progress_Bar.gameObject.SetActive(false);
                break;
            }
         
            if (i > Time_To_Wait) // about 1 seconds
            {
                Increment_Ammo_Counters(Ammo_We_Looking_At.collider.tag);
                Destroy(Ammo_We_Looking_At.collider.gameObject);
                Holding_Sniper_Large = false;
                Pickup_Progress_Bar.gameObject.SetActive(false);
                break;
            }



            yield return new WaitForSeconds(.1f);

        }



    }


    IEnumerator Xtra_Large_Ammo_Wait(RaycastHit Ammo_We_Looking_At)
    {
        int Time_To_Wait = 30;
        Pickup_Progress_Bar.maxValue = Time_To_Wait;
        Pickup_Progress_Bar.gameObject.SetActive(true);
      
        for (int i = 0; i < 9999; i++)
        {
            bool temp = Input.GetKey(KeyCode.Mouse0);
            Holding_Sniper_Xtra = temp;

            Pickup_Progress_Bar.value = i;

            if (!Holding_Sniper_Xtra)
            {
                Pickup_Progress_Bar.gameObject.SetActive(false);
                break;
            }
            if (i > Time_To_Wait) // about 4 seconds
            {
                Increment_Ammo_Counters(Ammo_We_Looking_At.collider.tag);
                Destroy(Ammo_We_Looking_At.collider.gameObject);
                Holding_Sniper_Xtra = false;
                Pickup_Progress_Bar.gameObject.SetActive(false);
                break;
            }



            yield return new WaitForSeconds(.1f);

        }




    }


    IEnumerator Health_Pickup_Wait(RaycastHit Vending_Machine_Looking_At)
    {
        int Time_To_Wait = 10;
        Pickup_Progress_Bar.maxValue = Time_To_Wait;
        Pickup_Progress_Bar.gameObject.SetActive(true);

        for (int i = 0; i < 9999; i++)
        {
            bool temp = Input.GetKey(KeyCode.Mouse0);
            Holding_Health = temp;

            Pickup_Progress_Bar.value = i;


            if (!Holding_Health)
            {
                Pickup_Progress_Bar.gameObject.SetActive(false);
                break;
            }
            if (i > Time_To_Wait) //about 2 seconds
            {
                // do not destroy because we should be looking at a vending machine
                Holding_Health = false;
                Pickup_Progress_Bar.gameObject.SetActive(false);
                GameObject New_Soda = Instantiate(Sodas[Random.Range(0, 5)], Vending_Machine_Looking_At.transform.position + (Vending_Machine_Looking_At.transform.forward * 1.5f), Vending_Machine_Looking_At.transform.rotation);
                New_Soda.GetComponent<Rigidbody>().AddForce(Vending_Machine_Looking_At.transform.forward * 5, ForceMode.Impulse);
                break;
            }



            yield return new WaitForSeconds(.1f);

        }




    }



    void Increment_Ammo_Counters(string tag) //increments the global ammo counts based on the tag of the raycasted object
    {

        string My_Tag = tag;
       

        switch (My_Tag)
        {
            case "Slow_Wave_Ammo":
                Persistent_Data_Store.Slow_Wave_Ammo += 2;
                break;

            case "Sniper_Ammo":
                Persistent_Data_Store.Sniper_Ammo += 1;

                break;

            case "Sniper_Ammo_Large":
                Persistent_Data_Store.Sniper_Ammo += 10;

                break;


            case "Sniper_Ammo_Xtra_Large":
                Persistent_Data_Store.Sniper_Ammo += 50;

                break;

            case "Lazer_Ammo":
                Persistent_Data_Store.Pierce_Lazer_Ammo += 1;
                Debug.Log("increment lazer");
                break;


            case "Vines_Ammo":
                Persistent_Data_Store.Vines_Ammo += 1;

                break;


            case "Saw_Ammo":
                Persistent_Data_Store.Saw_Ammo += 10;

                break;

            case "Burst_Ammo":
                Persistent_Data_Store.Burst_Module_Ammo += 1;

                break;

            case null:
                break;

            case "Soda_Pickup":

                if (Persistent_Data_Store.House_Health < 100)
                {
                    Persistent_Data_Store.House_Health += 5;
                }
              
                break;

        }

    }



    void Instantiate_Ammo() // will be used later maybe, right now just used for sniper shelves
    {
   

        Ammo_Pickups = new List<GameObject>() ; // list for the ammo tags
        Ammo_Pickups.Add(Sniper_Pickup);
        Ammo_Pickups.Add(Vines_Pickup);

        Ammo_Spawn_Tags = new List<string>(); // the spawn empty game object objects tags
        Ammo_Spawn_Tags.Add("Sniper_Ammo_Spawn");
        Ammo_Spawn_Tags.Add("Vines_Spawn");


        for (int i = 0; i < Ammo_Spawn_Tags.Count; i++) // goes through each spawn tag instantiating ammo at each empty game object befor going to the next spawn tag
        {

            if (Ammo_Spawn_Tags[i] != "Vines_Spawn")
            {
                GameObject[] Current_Spawn = GameObject.FindGameObjectsWithTag(Ammo_Spawn_Tags[i]);

                for (int j = 0; j < Current_Spawn.Length; j++)
                {

                    Instantiate(Ammo_Pickups[i], Current_Spawn[j].transform.position, Current_Spawn[j].transform.rotation);

                }


            }
          
            
            else if(Ammo_Spawn_Tags[i] == "Vines_Spawn")   // if vines spawn make is chance based
            {
                GameObject[] Current_Spawn = GameObject.FindGameObjectsWithTag(Ammo_Spawn_Tags[i]);

                for (int j = 0; j < Current_Spawn.Length; j++)
                {
                    int Five_Is_True = Random.Range(0, 6);

                    if (Five_Is_True == 5)
                    {
                        Instantiate(Ammo_Pickups[i], Current_Spawn[j].transform.position, Current_Spawn[j].transform.rotation);
                    }
                }

            }
        }


    }



IEnumerator Slow_Wave_Routine_Spawn()
    {
        for (int i = 999999; i > 0; i--)
        {
            Instantiate(Slow_Wave_Pickup, Slow_Wave_Spawn.transform.position + new Vector3(Random.Range(-.1f,.1f), Random.Range(-.1f, .1f), Random.Range(-.1f, .1f)), Slow_Wave_Spawn.transform.rotation);

            yield return new WaitForSeconds(.33f);
        }
    }

    // Y  1.4 , -1.5, .3
    // Z -4 , 2,  -1.4
    // x 0
    IEnumerator Lazer_Routine_Spawn() // for the arcade machine
    {
        for (int i = 999999; i > 0; i--)
        {
            GameObject Last_Spawn = Instantiate(Lazer_Pickup, Lazer_Spawn.transform.position + new Vector3(0, Random.Range(-1f, 1f), Random.Range(-2f, 3f)), Slow_Wave_Spawn.transform.rotation); // rotation is just whatever
            StartCoroutine(Destroy_Lazer_Ammo(Last_Spawn));

            yield return new WaitForSeconds(.5f);
        }
    }




    IEnumerator Destroy_Lazer_Ammo(GameObject Last_Lazer_Pickup) 
    {
            yield return new WaitForSeconds(2f);
        Destroy(Last_Lazer_Pickup);
    }




    IEnumerator Ticket_Machine_Spawns() // perioically spawns tickets (Lazer Ammo with low gravity) while there are less than x tickets in the scene. differentiates lazer ammo based on whether they use gravity or not so be careful itf
    {
        for (int i = 99999; i > 0; i--)
        {
            int Tickets_In_Scene = 0;
           
            GameObject[] Lazer_Ammo_In_Scene = GameObject.FindGameObjectsWithTag("Lazer_Ammo");
          
            for(int j = Lazer_Ammo_In_Scene.Length - 1; j > 0; j--) // counts tickets in scene by differentiating lazer ammo whether or not the ammo uses gravity
            {
                if (Lazer_Ammo_In_Scene[j].GetComponent<Rigidbody>().useGravity)
                {
                    Tickets_In_Scene++;
                }
                

            }

            if(Tickets_In_Scene < 99999 && In_Ticket_Booth)   // limits maximum tickets after counting the tickets in the scene
            {
                GameObject Ticket_Spawn = GameObject.Find("Ticket_Blower_Center");
                Instantiate(Ticket, Ticket_Spawn.transform.position + new Vector3(Random.Range(-1.5f, 1.5f), 0, Random.Range(-1.5f, 1.5f)), Quaternion.Euler(Random.Range(-90f, 90f), Random.Range(-90f, 90f), Random.Range(-90f, 90f)));
            }

            if (!In_Ticket_Booth) // when we leave booth destroy tickets and de activate the coroutine with break
            {

                for (int k = Lazer_Ammo_In_Scene.Length - 1; k > 0; k--) // counts tickets in scene by differentiating lazer ammo whether or not the ammo uses gravity
                {
                    if (Lazer_Ammo_In_Scene[k].GetComponent<Rigidbody>().useGravity)
                    {
                        Destroy(Lazer_Ammo_In_Scene[k]);
                    }
                }

                break;
            }


            yield return new WaitForSeconds(.2f);
            

        }
    }


    IEnumerator Slide_Main_Doors(GameObject Left_Door, GameObject Right_Door) // actian depends on if the door is open or closed
    {
        if (!Main_Doors_Open)
        {
            for (int i = 20; i > 0; i--)
            {
                Main_Doors_Opening = true;
                Left_Door.transform.Translate(new Vector3(0, 0, .5f));
                Right_Door.transform.Translate(new Vector3(0, 0, -.5f));
                yield return new WaitForSeconds(.01f);
            }
           
            Main_Doors_Open = true;
            Main_Doors_Opening = false;
        }


       else if (Main_Doors_Open)
        {
            for (int j = 20; j > 0; j--)
            {
                Main_Doors_Opening = true;
                Left_Door.transform.Translate(new Vector3(0, 0, -.5f));
                Right_Door.transform.Translate(new Vector3(0, 0, .5f));
                yield return new WaitForSeconds(.01f);
            }
            Main_Doors_Open = false;
            Main_Doors_Opening = false;
          
        }
    }



    void Update_Ammo_Counts() // this whole system is fucking terrible and not able to be scaled up easily
    {
        if (Persistent_Data_Store.Sniper_Ammo > 0) { Sniper.enabled = true; } else if (Persistent_Data_Store.Sniper_Ammo <= 0) { Sniper.enabled = false; }
       
        Sniper.text = "F : " + Persistent_Data_Store.Sniper_Ammo;


        if (Persistent_Data_Store.Saw_Ammo > 0) { Saw.enabled = true; } else if (Persistent_Data_Store.Saw_Ammo <= 0) { Saw.enabled = false; }
       
        Saw.text = "E : " + Persistent_Data_Store.Saw_Ammo;


        if (Persistent_Data_Store.Vines_Ammo > 0) { Vines.enabled = true; } else if (Persistent_Data_Store.Vines_Ammo <= 0) { Vines.enabled = false; }

        Vines.text = "Q : " + Persistent_Data_Store.Vines_Ammo;



        if (Persistent_Data_Store.Pierce_Lazer_Ammo > 0) { Lazer.enabled = true; } else if (Persistent_Data_Store.Pierce_Lazer_Ammo <= 0) { Lazer.enabled = false; }

        Lazer.text = "A : " + Persistent_Data_Store.Pierce_Lazer_Ammo;


        if (Persistent_Data_Store.Slow_Wave_Ammo > 0) { Slow_Wave.enabled = true; } else if (Persistent_Data_Store.Slow_Wave_Ammo <= 0) { Slow_Wave.enabled = false; }

        Slow_Wave.text = "Shift : " + Persistent_Data_Store.Slow_Wave_Ammo;

        House_Health.text = "House " + Persistent_Data_Store.House_Health.ToString("f1");


        if (Persistent_Data_Store.Burst_Module_Ammo > 0) { Burst_Module.enabled = true; } else if (Persistent_Data_Store.Burst_Module_Ammo <= 0) { Burst_Module.enabled = false; }

        Burst_Module.text = "SPACE : " + Persistent_Data_Store.Burst_Module_Ammo;






    }

    private void OnTriggerEnter(Collider other)
    {


        if (other.name == "Ticket_Machine_Trigger")
        {
            Door_To_Enable.SetActive(true);
            Door_To_Disable.SetActive(false);

            In_Ticket_Booth = true;
            StartCoroutine(Ticket_Machine_Spawns());

        }
        else if (other.name == "Main_Door_Trigger" && !Main_Doors_Opening) // when we enter the main door trigger, if the doores are open, close em
        {
            if (!Main_Doors_Open) { 
            StartCoroutine(Slide_Main_Doors(Main_Door_Left,Main_Door_Right));
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if(other.name == "Main_Door_Trigger" && !Main_Doors_Opening)
       
        {
            StartCoroutine(Slide_Main_Doors(Main_Door_Left, Main_Door_Right));
        }
    }


    private void OnCollisionEnter(Collision collision)
    { 
        if (collision.gameObject.name == "Door_To_Enable") // when we walk into the newly enabled door, disable it and re enable the open door aka door to disable
        {
            In_Ticket_Booth = false;
            collision.gameObject.SetActive(false);
            Door_To_Disable.SetActive(true);
        }
    }

}




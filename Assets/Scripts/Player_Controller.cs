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
    //public GameObject Slow_Wave_Pickup;
    // public GameObject Slow_Wave_Pickup;
    // public GameObject Slow_Wave_Pickup;
    // public GameObject Slow_Wave_Pickup;
   
    
    public GameObject Slow_Wave_Spawn;

    public Vector3 Spawn_Pos;

    public TextMeshProUGUI Slow_Wave;
    public TextMeshProUGUI Saw; // for ammo counts
    public TextMeshProUGUI Vines;
    public TextMeshProUGUI Lazer;
    public TextMeshProUGUI Sniper;

    public TextMeshProUGUI House_Health;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.transform.position = Spawn_Pos;
       
        Lock_Cursor = CursorLockMode.Locked;
       // Cursor.lockState = Lock_Cursor;
      
        Player_Rigidbody = GetComponent<Rigidbody>();
      
        Main_Camera = GameObject.Find("Main Camera");
      
        Instantiate_Ammo();

        StartCoroutine(Slow_Wave_Routine_Spawn());
    }


    // Update is called once per frame
    void Update()
    {
        Update_Ammo_Counts();
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

        bool Mouse_Down = Input.GetKey(KeyCode.Mouse0);

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
      
        if (Object_In_Range && Object_Info.collider.gameObject.tag != "ground") //make sure picked up is actually ammo also 13 is the ammo layer for all ammo types
        {
           
         
            Increment_Ammo_Counters(Object_Info.collider.tag);
            Destroy(Object_Info.collider.gameObject);

        }

    }


   







    void Increment_Ammo_Counters(string tag) //increments the global ammo counts based on the tag of the raycasted object
    {

        string My_Tag = tag;
       

        switch (My_Tag)
        {
            case "Slow_Wave_Ammo":
                Persistent_Data_Store.Slow_Wave_Ammo += 10;
                break;

            case "Sniper_Ammo":
                Persistent_Data_Store.Sniper_Ammo += 1;

                break;

            case "Health_Pickup":
                if (Persistent_Data_Store.House_Health < 100)
                {
                    Persistent_Data_Store.House_Health += 10;
                }
                break;




            case null:
                break;





        }

    }



    void Instantiate_Ammo() // will be used later maybe, right now just used for sniper shelves
    {
   

        Ammo_Pickups = new List<GameObject>() ; // list for the ammo tags
        Ammo_Pickups.Add(Sniper_Pickup);


        Ammo_Spawn_Tags = new List<string>(); // the spawn empty game object objects tags
        Ammo_Spawn_Tags.Add("Sniper_Ammo_Spawn");



        for (int i = 0; i < Ammo_Spawn_Tags.Count; i++) // goes through each spawn tag instantiating ammo at each empty game object befor going to the next spawn tag
        {

            GameObject[] Current_Spawn = GameObject.FindGameObjectsWithTag(Ammo_Spawn_Tags[i]);
            
            for (int j = 0; j < Current_Spawn.Length; j++)
            {
                Instantiate(Ammo_Pickups[i], Current_Spawn[j].transform.position, Current_Spawn[j].transform.rotation);
               
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

     



    void Update_Ammo_Counts() // this whole system is fucking terrible and not able to be scaled up easily
    {
        if (Persistent_Data_Store.Sniper_Ammo > 0) { Sniper.enabled = true; } else if (Persistent_Data_Store.Sniper_Ammo <= 0) { Sniper.enabled = false; }
       
        Sniper.text = "F : " + Persistent_Data_Store.Sniper_Ammo;


        if (Persistent_Data_Store.Saw_Ammo > 0) { Saw.enabled = true; } else if (Persistent_Data_Store.Saw_Ammo <= 0) { Saw.enabled = false; }
       
        Saw.text = "E : " + Persistent_Data_Store.Saw_Ammo;


        if (Persistent_Data_Store.Vines_Ammo > 0) { Vines.enabled = true; } else if (Persistent_Data_Store.Vines_Ammo <= 0) { Vines.enabled = false; }

        Vines.text = "Q : " + Persistent_Data_Store.Vines_Ammo;



        if (Persistent_Data_Store.Pierce_Lazer_Ammo > 0) { Lazer.enabled = true; } else if (Persistent_Data_Store.Pierce_Lazer_Ammo <= 0) { Lazer.enabled = false; }

        Lazer.text = "D : " + Persistent_Data_Store.Pierce_Lazer_Ammo;


        if (Persistent_Data_Store.Slow_Wave_Ammo > 0) { Slow_Wave.enabled = true; } else if (Persistent_Data_Store.Slow_Wave_Ammo <= 0) { Slow_Wave.enabled = false; }

        Slow_Wave.text = "Shift : " + Persistent_Data_Store.Slow_Wave_Ammo;

        House_Health.text = "House " + Persistent_Data_Store.House_Health.ToString("f1");








    }




}




using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class Enemy_Behavior : MonoBehaviour
{
    string Enemy_Type;
    [SerializeField] private float Health = 1;
     public float Move_Speed = -1;
    private float Damage = 1;
    private float Max_Fly_Hight = 1;
    private float Min_Fly_Hight = 1;
    private float Max_Health = 1;


    private Rigidbody EnemyRigidbody;
    private Slider Health_Bar;
    private Transform Health_Bar_Canvas;

    private float Hit_Speed = 1;
    bool Touching_House = false;
    bool Slowed = false;
   
    //knockback values
    private float Sniper_Knockback = 30;
    private float Main_Knockback = 4;
    private float Pierce_Lazer_Knockback = 2;
   
    public bool On_Ground;
    
    public bool Hit_On_Cooldown;
  
    private bool Is_Rising; // for flying enemies

    public string spinTag = "Saw"; // for spinning the saw weapon once it becomes a child of the enemy


    private bool Half_Hp; // for super heavy

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        EnemyRigidbody = gameObject.GetComponent<Rigidbody>();
        Enemy_Type = gameObject.tag; // get enemy type from tag
        Enemy_Behavior_From_Type(Enemy_Type);
        //Physics.SetLayerCollisionMask(Enemy, Environment, false);
        Physics.gravity = Physics.gravity;
      
        Health_Bar = gameObject.GetComponentInChildren<Slider>();
       
        Health_Bar_Canvas = gameObject.transform.Find("Canvas");

        Health_Bar_Canvas.gameObject.SetActive(false);




    }


    
    // Update is called once per frame
    void Update()
    {
        Update_Enemy_Healthbar();

        if (Health < Max_Health)
        {
            Health_Bar_Canvas.gameObject.SetActive(true);
            if(gameObject.tag == "Roller")
            {
                Health_Bar_Canvas.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

        }



        // House_Health_Reference = Persistent_Data_Store.House_Health; // for debuging 
        if (Health <= 0)
        {
            Destroy(gameObject);
          
        }
        
        foreach (Transform child in transform) // if the enemy has a saw added as a child rotate it
        {

            // Spin all children that have the specified tag
            if (child.CompareTag("Saw"))
            {
                child.Rotate( new Vector3(-2500,0,0) * Time.deltaTime, Space.Self);
            }
        }

        if (gameObject.tag == "Super_Heavy")
        {
            Knockback_Super_Heavy();
        }

    }


    private void FixedUpdate()
    {
        Universal_Enemy_Movement();
    }



    void Enemy_Behavior_From_Type(string Enemy_To_Initialize) // reads the tag of the instantiated prefab to give it behavior
    {
        switch (Enemy_To_Initialize)
        {
            case "Basic":
                //callm function that will activate the wanted behavior and stats when enemy with certain tag is instantiated
                Inialize_Basic_Enemy();
                break;

            case "Roller":
                Inialize_Roller_Enemy();
                break;

            case "Fast":
                Inialize_Fast_Enemy();
                break;
           
            case "Heavy":
                Inialize_Heavy_Enemy();
                break;

            case "Flyer":
                Inialize_Flyer_Enemy();
                break;

            case "Lava_Hound_Mini":
                Inialize_Lava_Hound_Mini_Enemy();
                break;

            case "Lava_Hound":
                Inialize_Lava_Hound_Enemy();
                break;

            case "Super_Heavy":
                Inialize_Super_Heavy_Enemy();
                break;

            case "Charger":
                Inialize_Charger_Enemy();
                break;
        }





    }

    void Inialize_Basic_Enemy() // called when an enemies tag is Basic
    {
      Max_Health = 12;
      Health = 12;
      Move_Speed = -4f -Random.Range(-1f,1f); // so enemies stand out from eachother
      Damage = 2;
      Hit_Speed = 1;
     
      

    }

    void Inialize_Roller_Enemy() // called when an enemies tag is roller etc
    {
        Max_Health = 40;
        Health = 40;
        Move_Speed = 0;
        Damage = 5f;
        Hit_Speed = 2;
       

    }

    void Inialize_Fast_Enemy()
    {
        Max_Health = 9;
        Health = 9;
        Move_Speed = -9f - Random.Range(-2f, 2f);
        Damage = 1;
        Hit_Speed =.5f;


    }

    void Inialize_Heavy_Enemy() 
    {
        Max_Health = 100;
        Health = 100;
        Move_Speed = -1.5f - Random.Range(-.5f, .5f);
        Damage = 10;
        Hit_Speed = 3f;


    }

    void Inialize_Flyer_Enemy() 
    {
        Max_Health = 9;
        Health = 9;
        Move_Speed = -4f - Random.Range(-1f, 1f);
        Damage = 2;
        Hit_Speed = 2f;
        Max_Fly_Hight = Random.Range(3f, 3.5f);
        Min_Fly_Hight = 2;
        

    }
    void Inialize_Lava_Hound_Mini_Enemy() // called when an enemies tag is roller etc
    {
        Max_Health = 70;
        Health = 70;
        Move_Speed = -6f - Random.Range(-1f, 1f);
        Damage = 2f;
        Hit_Speed = 2f;
        Max_Fly_Hight = Random.Range(2.5f, 3f);
        Min_Fly_Hight = 2;

    }
    void Inialize_Lava_Hound_Enemy() // acts as like a blocker for the cannon bc obviosly there are more pressing matters then the areal turd
    {
        Max_Health = 200;
        Health = 200;
        Move_Speed = -1.25f;
        Max_Fly_Hight = 6f;
        Min_Fly_Hight = 5.9999f;

    }

    void Inialize_Super_Heavy_Enemy() // acts as like a blocker for the cannon bc obviosly there are more pressing matters then the areal turd
    {
        Max_Health = 1000;
        Health = 1000;
        Move_Speed = -.5f;
        Damage = 25f;
        Hit_Speed = 3f;
     

    }
    void Inialize_Charger_Enemy() // acts as like a blocker for the cannon bc obviosly there are more pressing matters then the areal turd
    {
        Max_Health = 70;
        Health = 70;
        Move_Speed = -.5f;
        Damage = 5;
        Hit_Speed = .5f;
        StartCoroutine(Charger_Speed_Scaling());

    }


    void Update_Enemy_Healthbar()
    {

        Health_Bar.value = (Health / Max_Health);
    }







    void Universal_Enemy_Movement() // with some special cases, moves the enemy at a constant rate towards the base
    {
        if (!(gameObject.tag == "Roller") && !(gameObject.tag == "Charger"))
        {
            EnemyRigidbody.rotation = Quaternion.Euler(0, -180, 0);
            Vector3 Move_Enemy = new Vector3(1, 0, 0) * Move_Speed * Time.deltaTime;
            if (EnemyRigidbody.linearVelocity.x > Move_Speed)
            {
                EnemyRigidbody.AddForce(Move_Enemy, ForceMode.VelocityChange);

            }
        }

        else if(gameObject.tag == "Roller")
        {
            EnemyRigidbody.AddTorque(0, 0, 60);// if its a roller enemy
        }
      
        else if (gameObject.tag == "Charger") // if its a charger
        {
            if (On_Ground)
            {
                Vector3 Move_Enemy = new Vector3(1, 0, 0) * Move_Speed * Time.deltaTime;
                if (EnemyRigidbody.linearVelocity.x > Move_Speed)
                {
                    EnemyRigidbody.AddForce(Move_Enemy, ForceMode.VelocityChange);

                }
            }


        }

        if (gameObject.tag == "Flyer" || gameObject.tag == "Lava_Hound_Mini" || gameObject.tag == "Lava_Hound") // if the enemy is a flyer also run this
        {
           
           if(gameObject.transform.position.y < Min_Fly_Hight || Is_Rising == true) // a mess to be sure but this should create a falling zone between the max and min flight hights where it will bob up and fall to the min hight and rtepeat
            {
                Vector3 Enemy_Fly = new Vector3(0, 1, 0) * 20 * Time.deltaTime;
                EnemyRigidbody.AddForce(Enemy_Fly, ForceMode.VelocityChange);
               
                if(gameObject.transform.position.y > Max_Fly_Hight)
                {
                    Is_Rising = false;
                }
                else if(gameObject.transform.position.y < Min_Fly_Hight)
                {
                    Is_Rising = true;
                }

            }
         
        }

        if (gameObject.tag == "Lava_Hound" && gameObject.transform.position.x < -11) // special lavahound code to stop the hound from endlessly flying into the base because it literally cant attack
        {
            EnemyRigidbody.AddForce(new Vector3(1f, 0, 0), ForceMode.Impulse); // lava hound will hover a short distance from the base and spawn enemies by launching them out its ass
        }
        

    }


    IEnumerator Charger_Speed_Scaling()
    {
        for (int i = 99999; i >= 0 && !(Move_Speed < -30); i--)
        {
            Move_Speed += -1f;
            yield return new WaitForSeconds(.5f);
        }
    }


    IEnumerator Attack_Interval_Delayer() // defines the attack interval based on the hitspeed. this is an infinite loop while enemy is touching the house
    {
        if (Touching_House)
        {
            for (int i = 0; i < 99999; i++) // i refuse to use a fucking while loop
            {
                Hit_On_Cooldown = true;                             // so coroutine only runs once before needing to wait on the cooldown
                Persistent_Data_Store.House_Health -= Damage;

                if (gameObject.CompareTag("Roller")) //launch up when hit base if its the roller
                {
                    EnemyRigidbody.AddForce(new Vector3(2, 5, 0), ForceMode.Impulse);
                }
                else if (gameObject.CompareTag("Charger"))
                {
                    EnemyRigidbody.AddForce(new Vector3(Move_Speed * -1.75f, 0, 0), ForceMode.Impulse);
                }
                yield return new WaitForSeconds(Hit_Speed);
                Hit_On_Cooldown = false;

                if (!Touching_House) { yield break; }
            }
        }
    }


    


    IEnumerator Slow_Timer() 
    {
        Slowed = true; // so the slow effect doesnt stack
        float Temp = Move_Speed; // stores move speed before the slowing for revertion
      
        Move_Speed = -.5f; // should effect large enemies less but still be useful
            yield return new WaitForSeconds(7);

        Move_Speed = Temp;
         Slowed = false;

    }



    IEnumerator Repeated_Saw_Damage()
    {
        for (int i = 0; i < 999999; i++) 
        { 
        yield return new WaitForSeconds(.33f);
        Health -= 1f;
        }
    }

    void Knockback_Super_Heavy()
    {  
        if(Health <= 500 && !Half_Hp)
        {
            Three_Q_Hp = true;
            EnemyRigidbody.AddForce(Vector3.right * 3000, ForceMode.Impulse);
        }
 

    }


    private void OnCollisionEnter(Collision collision) // start hit repeating coroutine if its hitting house
    { 
      
        if (collision.gameObject.CompareTag("House_Collider"))
        {
            Touching_House = true;
            if (!Hit_On_Cooldown)
            {
                StartCoroutine(Attack_Interval_Delayer());
            }
        }
       
        if (collision.gameObject.CompareTag("ground"))
        {
            On_Ground = true;
        }
      
        if (collision.gameObject.CompareTag("Vine_Spawn"))
        {
           
            EnemyRigidbody.AddForce(Vector3.up * 10, ForceMode.Impulse);

            Vector3 Velocity_Before_Stun = EnemyRigidbody.linearVelocity;

            EnemyRigidbody.linearVelocity = Velocity_Before_Stun - new Vector3(Velocity_Before_Stun.x, 0, 0);


        }

        if (collision.gameObject.CompareTag("Saw"))
        {
            Debug.Log("Saw hit enemy");

            if (collision.gameObject.GetComponent<Bullet_Control>().Enemies_Hit < 1)
            {
                StartCoroutine(Repeated_Saw_Damage());


                collision.gameObject.GetComponent<Bullet_Control>().Enemies_Hit++;
            }
        }
    }




    private void OnCollisionExit(Collision collision) // when not touching house stop dealing damage, also used for ground collision (:
    {
        if (collision.gameObject.CompareTag("House_Collider"))
        {
            Touching_House = false;
        }

        if (collision.gameObject.CompareTag("ground"))
        {
            On_Ground = false;
        }


    }


    private void OnTriggerEnter(Collider other) // for getting shot
    {
        if (other.tag == "Slow_Wave" && !Slowed)
        {
            StartCoroutine(Slow_Timer());
        }

        else if (other.gameObject.CompareTag("Sniper"))
        {
            Destroy(other.gameObject);
            // StartCoroutine(Local_Invincibility_Sniper());// because the stupid roller has multiple segments but also to prevent potential multi hits
            if (other.gameObject.GetComponent<Bullet_Control>().Enemies_Hit < 1)
            {
                Health -= 20;
                EnemyRigidbody.AddForce(Vector3.right * Sniper_Knockback + new Vector3(0, (Sniper_Knockback / 2), 0), ForceMode.Impulse);
                other.gameObject.GetComponent<Bullet_Control>().Enemies_Hit++;
            }



        }

        else if (other.gameObject.CompareTag("Main"))
        {

            Destroy(other.gameObject);
            if (other.gameObject.GetComponent<Bullet_Control>().Enemies_Hit < 1) { 
            Health -= 3;
            EnemyRigidbody.AddForce(Vector3.right * Main_Knockback + new Vector3(0, (Main_Knockback / 2), 0), ForceMode.Impulse);
                other.gameObject.GetComponent<Bullet_Control>().Enemies_Hit++;
             }
        }


        else if (other.gameObject.CompareTag("Pierce_Lazer"))
        {
            Health -= 1.9f; // does abt 20 to rollers
          
            if (gameObject.tag != "Roller") // roller has multiple segments so it normally gets one shotted llooooll

            {
                Health -= 7;
            }
          
            if(!On_Ground && gameObject.tag != "Roller") // double damage to Airborne enemies
            {
                Health -= 20;
                EnemyRigidbody.AddForce(Vector3.right * Pierce_Lazer_Knockback * 15, ForceMode.Impulse);
            }

            EnemyRigidbody.AddForce(Vector3.right * Pierce_Lazer_Knockback + new Vector3(0, (Pierce_Lazer_Knockback / 2), 0), ForceMode.Impulse);
        }


      



    }




}

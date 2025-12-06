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

    public AudioClip Sniper_Hit;
    public AudioClip Water_Hit;
    public AudioClip Lazer_Hit;
    public AudioClip Lazer_crit;
    public AudioClip Fish_Chomp;
    public AudioClip Vines_bounce;
    public AudioClip Basic_Hit;
    public AudioClip House_Take_Damage;
    public AudioClip Death_Sound_Small;
    public AudioClip Death_Sound_Large_1;
    public AudioClip Death_Sound_Large_2;

    public GameObject Sniper_Hit_VFX;
    public GameObject Basic_Hit_VFX;
    public GameObject Slow_Hit_VFX;
    public GameObject Lazer_Hit_VFX;
    public GameObject Lazer_Crit_VFX;



    private float Hit_Speed = 1;
    bool Touching_House = false;
    bool Slowed = false;
   
    //knockback values
    private float Sniper_Knockback = 10;
    private float Slow_Knockback = 2;
    private float Main_Knockback = 4;
    private float Pierce_Lazer_Knockback = 2;
   
    public bool On_Ground;
    
    public bool Hit_On_Cooldown;
  
    private bool Is_Rising; // for flying enemies

    public bool Is_Lava_Child = false; // if enemy is child of lavahound we do not count their kills


    private bool Half_Hp; // for super heavy
    private bool Game_Over;

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



        //  for different death sounds based on enemy type
        if (Health <= 0)
        {
            if (!Is_Lava_Child) 
            { 
            GameObject.Find("Enemy_Spawn_Manager").SendMessage("OnEnemyKilled", SendMessageOptions.RequireReceiver);
             }
           
            if(gameObject.tag == "Basic" || gameObject.tag == "Roller" || gameObject.tag == "Flyer" || gameObject.tag == "Fast")
            {
                Audio_Manager_Script.instance.Play_Selected_Audio(Death_Sound_Small, gameObject.transform.position, .5f, 1 + Random.Range(-.2f, .2f));
            }
          
            if(gameObject.tag == "Heavy" || gameObject.tag == "Lava_Hound_Mini" || gameObject.tag == "Charger")
            {
                Audio_Manager_Script.instance.Play_Selected_Audio(Death_Sound_Large_2, gameObject.transform.position, .3f, 1.5f + Random.Range(-.2f, .2f));
            }
           
            if (gameObject.tag == "Super_Heavy" || gameObject.tag == "Lava_Hound")
            {
                Audio_Manager_Script.instance.Play_Selected_Audio(Death_Sound_Large_1, gameObject.transform.position, .6f, .9f + Random.Range(-.2f, .2f));
            }


            Destroy(gameObject);
          
        }
        

        /* cool code from when the fish were saws
        foreach (Transform child in transform) // if the enemy has a saw added as a child rotate it
        {

            // Spin all children that have the specified tag
            if (child.CompareTag("Saw"))
            {
               child.Rotate( new Vector3(-2500,0,0) * Time.deltaTime, Space.Self);
            }
        }
        
        */

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
        //  Max_Health = 9;
        Max_Health = 9;
        Health = 9;
        Move_Speed = -9f - Random.Range(-2f, 2f);
        Damage = 1;
        Hit_Speed =.5f;


    }

    void Inialize_Heavy_Enemy() 
    {
        Max_Health = 120;
        Health = 120;
        Move_Speed = -1.5f - Random.Range(-.5f, .5f);
        Damage = 10;
        Hit_Speed = 3f;


    }

    void Inialize_Flyer_Enemy() 
    {
        Max_Health = 9;
        Health = 9;
        Move_Speed = -3f - Random.Range(-1f, 1f);
        Damage = 2;
        Hit_Speed = 2f;
        Max_Fly_Hight = Random.Range(3f, 3.5f);
        Min_Fly_Hight = 2;
        

    }
    void Inialize_Lava_Hound_Mini_Enemy() // called when an enemies tag is roller etc
    {
        Max_Health = 50;
        Health = 50;
        Move_Speed = -5f - Random.Range(-1f, 1f);
        Damage = 2f;
        Hit_Speed = 2f;
        Max_Fly_Hight = Random.Range(2.5f, 3f);
        Min_Fly_Hight = 2;

    }
    void Inialize_Lava_Hound_Enemy() //
    {
        Max_Health = 200;
        Health = 200;
        Move_Speed = -1.25f;
        Max_Fly_Hight = Random.Range(6f, 7f);
        Min_Fly_Hight = 5.9999f;

    }

    void Inialize_Super_Heavy_Enemy() // 
    {
        Max_Health = 500;
        Health = 500;
        Move_Speed = -1f;
        Damage = 20f;
        Hit_Speed = 3f;
     

    }
    void Inialize_Charger_Enemy() // 
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
            if (On_Ground && !Slowed) // if slowed do not move it at all
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
        for (int i = 99999; i >= 0 && !(Move_Speed < -20); i--)
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

                if (Persistent_Data_Store.House_Health > 0) //stop playing sounds once game is over
                {
                    Audio_Manager_Script.instance.Play_Selected_Audio(House_Take_Damage, gameObject.transform.position, .075f, .6f + Random.Range(-.1f, .1f));
                }

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
            yield return new WaitForSeconds(3);

        Move_Speed = Temp;
         Slowed = false;

    }



    IEnumerator Repeated_Saw_Damage()
    {
        for (int i = 0; i < 999999; i++) 
        { 
        yield return new WaitForSeconds(.33f);
            Audio_Manager_Script.instance.Play_Selected_Audio(Fish_Chomp, gameObject.transform.position, .1f, 1 + Random.Range(-.1f, .1f));

            Health -= 1f;
        }
    }

    void Knockback_Super_Heavy()
    {  
        if(Health <= 250 && !Half_Hp)
        {
            Half_Hp = true;
            Audio_Manager_Script.instance.Play_Selected_Audio(Vines_bounce, gameObject.transform.position, .5f, 1 + Random.Range(-.1f, .1f));
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
       
     
        if (collision.gameObject.CompareTag("Vine_Spawn"))
        {
            if (gameObject.tag != "Flyer" && gameObject.tag != "Lava_Hound_Mini" && gameObject.tag != "Lava_Hound" && gameObject.tag != "Roller") // if enemy does not fly
            {
                Audio_Manager_Script.instance.Play_Selected_Audio(Vines_bounce, gameObject.transform.position, .4f, 1 + Random.Range(-.1f, .1f));

                EnemyRigidbody.AddForce(Vector3.up * 5, ForceMode.Impulse);

                Vector3 Velocity_Before_Stun = EnemyRigidbody.linearVelocity;

                EnemyRigidbody.linearVelocity = Velocity_Before_Stun - new Vector3(Velocity_Before_Stun.x, 0, 0);
            }

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


    }


    private void OnTriggerEnter(Collider other) // slow does not reapply but the knockback does
    {
        if (other.tag == "Slow_Wave")
        {
            Audio_Manager_Script.instance.Play_Selected_Audio(Water_Hit, gameObject.transform.position, .1f, 1 + Random.Range(-.1f, .1f));
            GameObject Hit_Impact = Instantiate(Slow_Hit_VFX, other.transform.position, gameObject.transform.rotation * Quaternion.Euler(Random.Range(-90f,90f), Random.Range(-90f, 90f), Random.Range(-90f, 90f)));
            Destroy(Hit_Impact, 2);

            if (!Slowed)
            {
                StartCoroutine(Slow_Timer());
            }
           
            if (gameObject.tag != "Roller")
            {
                EnemyRigidbody.AddForce(Vector3.right * Slow_Knockback + new Vector3(0, (Slow_Knockback / 2), 0), ForceMode.Impulse); 
            }
           
            else if (gameObject.tag == "Roller") // aply less knockback because they have 9 segments that the knockback applies to
            {
                EnemyRigidbody.AddForce(Vector3.right * (Slow_Knockback / 8) + new Vector3(0, ((Slow_Knockback / 8) / 2), 0), ForceMode.Impulse); 
            }

        }
        
        if (other.gameObject.CompareTag("ground"))
        {
            On_Ground = true;
        }

        else if (other.gameObject.CompareTag("Sniper"))
        {
            Audio_Manager_Script.instance.Play_Selected_Audio(Sniper_Hit, gameObject.transform.position, .25f, .9f + Random.Range(-.1f, .1f));
            GameObject Hit_Impact = Instantiate(Sniper_Hit_VFX, other.transform.position, gameObject.transform.rotation);
            Destroy(Hit_Impact, 2);

            Destroy(other.gameObject);

            if (other.gameObject.GetComponent<Bullet_Control>().Enemies_Hit < 1) // so we dont get collaterals
            {
                Health -= 15;
                EnemyRigidbody.AddForce(Vector3.right * Sniper_Knockback + new Vector3(0, (Sniper_Knockback / 2), 0), ForceMode.Impulse);
                other.gameObject.GetComponent<Bullet_Control>().Enemies_Hit++;
            }



        }

        else if (other.gameObject.CompareTag("Main"))
        {
            Audio_Manager_Script.instance.Play_Selected_Audio(Basic_Hit, gameObject.transform.position, .15f, 1.2f + Random.Range(-.1f, .1f));
            GameObject Hit_Impact = Instantiate(Basic_Hit_VFX, other.transform.position, gameObject.transform.rotation);
            Destroy(Hit_Impact, 2);

            Destroy(other.gameObject);
            if (other.gameObject.GetComponent<Bullet_Control>().Enemies_Hit < 1) { 
            Health -= 3;
            EnemyRigidbody.AddForce(Vector3.right * Main_Knockback + new Vector3(0, (Main_Knockback / 2), 0), ForceMode.Impulse);
                other.gameObject.GetComponent<Bullet_Control>().Enemies_Hit++;
             }
        }


        else if (other.gameObject.CompareTag("Pierce_Lazer"))
        {
            Audio_Manager_Script.instance.Play_Selected_Audio(Lazer_Hit, gameObject.transform.position, .075f, 1 + Random.Range(-.2f, .2f));
            GameObject Hit_Impact = Instantiate(Lazer_Hit_VFX, other.transform.position, gameObject.transform.rotation);
            Destroy(Hit_Impact, 2);

            Health -= 2.1f; // does abt 24 to rollers
            EnemyRigidbody.AddForce(Vector3.right * Pierce_Lazer_Knockback + new Vector3(0, (Pierce_Lazer_Knockback / 2), 0), ForceMode.Impulse);

            if (gameObject.tag != "Roller") // roller has multiple segments so it normally gets one shotted llooooll

            {
                Health -= 6;
            }
          
            if(!On_Ground && gameObject.tag != "Roller") // quadrouple damage to Airborne enemies
            {
                Audio_Manager_Script.instance.Play_Selected_Audio(Lazer_crit, gameObject.transform.position, .15f, 1 + Random.Range(-.1f, .1f));
                GameObject Hit_Impact1 = Instantiate(Lazer_Crit_VFX, other.transform.position, gameObject.transform.rotation);
                Destroy(Hit_Impact1, 2);

                Health -= 30;
                EnemyRigidbody.AddForce(Vector3.right * Pierce_Lazer_Knockback * 15, ForceMode.Impulse);
            }
           
            if (gameObject.tag == "Roller")
            {
                Audio_Manager_Script.instance.Play_Selected_Audio(Lazer_crit, gameObject.transform.position, .15f, 1 + Random.Range(-.1f, .1f));
                GameObject Hit_Impact2 = Instantiate(Lazer_Crit_VFX, other.transform.position, gameObject.transform.rotation);
                Destroy(Hit_Impact2, 2);

            }

        }


      



    }
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("ground"))
        {
            On_Ground = false;
        }

    }




    void Get_Bombed()
    {
        EnemyRigidbody.linearVelocity = new Vector3(40, 0, 0);
        Health -= 60;
    }

}

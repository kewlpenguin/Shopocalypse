using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;



public class Main_Gun_Controller : MonoBehaviour
{
    private float Max_Hight = 7.59f;
    private float Min_Hight = -2.87f;
    Rigidbody Main_Gun_Rigidbody;
    public float Move_Speed = 10;
    CursorLockMode Battle_Cursor_Mode;
    public string Selected_Ammo = "none";
    
    public GameObject Main;
    public GameObject Slow_Wave;
    public GameObject Sniper;
    public GameObject Pierce_Lazer;
    public GameObject Saw;
    public GameObject Vines;
    public GameObject Vine_Spawn_Reference;

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


    public TextMeshProUGUI House_Health;










    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Main_Gun_Rigidbody = gameObject.GetComponent<Rigidbody>();
       Battle_Cursor_Mode = CursorLockMode.None;
       Cursor.lockState = Battle_Cursor_Mode;

    }

    // Update is called once per frame
    void Update()
    {
        Fire_Main_Gun();
        Switch_Selected_Ammo();
        Fire_Selected_Ammo(); // if right mouse button is pressed
        Fire_Selected_Ammo_Burst(); // if space is held for some seconds
        Update_Cooldowns_And_Ammo_Counts();
    }



    private void FixedUpdate()
    {
        Move_Main_Gun();
    }

    void Move_Main_Gun()
    {
        float Vertical_Input = Input.GetAxis("Vertical") * Move_Speed * Time.deltaTime;
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
        bool Slow_Wave_Active = Input.GetKeyDown(KeyCode.LeftShift);
        bool Sniper_Active = Input.GetKeyDown(KeyCode.D);
        bool Pierce_Lazer_Active = Input.GetKeyDown(KeyCode.A);
        bool Saw_Active = Input.GetKeyDown(KeyCode.E);
        bool Vines_Active = Input.GetKeyDown(KeyCode.Q);




        if (Slow_Wave_Active) { Selected_Ammo = "Slow_Wave"; }
            
        else if (Sniper_Active) {Selected_Ammo = "Sniper";}

        else if (Pierce_Lazer_Active) { Selected_Ammo = "Pierce_Lazer"; }

        else if (Saw_Active) { Selected_Ammo = "Saw"; }
       
        else if (Vines_Active) { Selected_Ammo = "Vines"; }

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

        for (int i = 0; i < 3; i++)
        {
            Persistent_Data_Store.Saw_Ammo -= 1;
            Instantiate(Saw, gameObject.transform.position, gameObject.transform.rotation);
            yield return new WaitForSeconds(.11f);
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
            Instantiate(Main, gameObject.transform.position, gameObject.transform.rotation);
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
            switch (Selected_Ammo) // fires ammo based on the selected ammo string
            {
                case "Slow_Wave":
                  
                    Selected_Bullet_Cooldown = .1f; // we dont need to show this cooldown

                    if (!Slow_Wave_On_Cooldown && Persistent_Data_Store.Slow_Wave_Ammo > 0)
                    {
                        StartCoroutine(Weapon_Cooldown_Slow_Wave());
                        Instantiate(Slow_Wave, gameObject.transform.position, gameObject.transform.rotation);
                    }
                    break;


                case "Sniper":
                 
                    Selected_Bullet_Cooldown = Sniper_Cooldown;

                    if (!Sniper_On_Cooldown && Persistent_Data_Store.Sniper_Ammo > 0)
                    {
                        StartCoroutine(Weapon_Cooldown_Sniper());
                        Instantiate(Sniper, gameObject.transform.position, gameObject.transform.rotation);

                    }
                    break;

                case "Pierce_Lazer":

                    Selected_Bullet_Cooldown = Pierce_Lazer_Cooldown;

                    if (!Pierce_Lazer_On_Cooldown && Persistent_Data_Store.Pierce_Lazer_Ammo > 0)
                    {
                        StartCoroutine(Weapon_Cooldown_Pierce_Lazer());
                        Instantiate(Pierce_Lazer, gameObject.transform.position, gameObject.transform.rotation);

                    }
                    break;

                case "Saw":

                    Selected_Bullet_Cooldown = Saw_Cooldown;

                    if (!Saw_On_Cooldown && Persistent_Data_Store.Saw_Ammo > 0)
                    {
                        StartCoroutine(Weapon_Cooldown_Saw());
                    }
                    break;
              
                case "Vines":

                    Selected_Bullet_Cooldown = Vines_Cooldown;

                    if (!Vines_On_Cooldown && Persistent_Data_Store.Vines_Ammo > 0)
                    {
                        StartCoroutine(Weapon_Cooldown_Vines());
                        GameObject Vine_Shot = Instantiate(Vines, gameObject.transform.position, gameObject.transform.rotation); // needs changed a bit here
                        Vine_Shot.GetComponent<Bullet_Control>().Vine_Spawn = Vine_Spawn_Reference;
                    }
                    
                    break;

            }
        }
     
    }






    void Fire_Selected_Ammo_Burst()// initiates the hold for seconds check which in turn initiates the bullet burst from initiate bullet burst through setting the charged bool to true
    {
        bool Space_down = Input.GetKey(KeyCode.Space);
        if (Space_down && !Charging && Persistent_Data_Store.Burst_Module_Ammo >= 1 && !Burst_Module_On_Cooldown) { StartCoroutine(Held_Space_For_Seconds()); }

        else if (!Space_down || Burst_Module_On_Cooldown) { Charging = false; }   // charging is used to stop the other weapons from firing till the burst is done firing
    }




    IEnumerator Held_Space_For_Seconds()
    {
        Debug.Log("Charging");

        for (float i = 0; i < 3f; i += .1f)
        {
            bool Space_Held = Input.GetKey(KeyCode.Space);
          
            
            if (Space_Held)
            {
                Charging = true;
                yield return new WaitForSeconds(.1f);
                if (i >= 1f)
                {
                    Debug.Log("fire coroutine");
                    StartCoroutine(Initiate_Bullet_Burst());
                    Charging = false;
                    break;
                }
            }

            else if (!Space_Held)
            {
                Charging = false;
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
                        Instantiate(Slow_Wave, gameObject.transform.position, gameObject.transform.rotation * Quaternion.Euler(Random.Range(-10f,10f),1,1)); // need to randomize this
                        Persistent_Data_Store.Slow_Wave_Ammo--;
                        yield return new WaitForSeconds(.03f);
                    }

                }
                break;


            case "Sniper":
                if (Persistent_Data_Store.Sniper_Ammo >= 30 && !Burst_Module_On_Cooldown)
                {

                    StartCoroutine(Burst_Module_Cooldown_Timer()); // put burst module on cooldown if firing is successful also ammo is subtracted from burst module in the cooldown coroutine

                    for (float i = 0; i < 30; i++)
                    {
                        Instantiate(Sniper, gameObject.transform.position, gameObject.transform.rotation * Quaternion.Euler(Random.Range(-10f, 10f), 1, 1)); // need to randomize this
                        Persistent_Data_Store.Sniper_Ammo--;
                        yield return new WaitForSeconds(.03f);
                    }

                }
                break;

            case "Pierce_Lazer":

                if (Persistent_Data_Store.Pierce_Lazer_Ammo >= 15 && !Burst_Module_On_Cooldown)
                {

                    StartCoroutine(Burst_Module_Cooldown_Timer()); // put burst module on cooldown if firing is successful also ammo is subtracted from burst module in the cooldown coroutine

                    for (float i = 0; i < 15; i++)
                    {
                        Instantiate(Pierce_Lazer, gameObject.transform.position, gameObject.transform.rotation * Quaternion.Euler(Random.Range(-10f, 10f), 1, 1)); // need to randomize this
                        Persistent_Data_Store.Pierce_Lazer_Ammo--;
                        yield return new WaitForSeconds(.03f);
                    }

                }
                break;


            case "Saw":

                if (Persistent_Data_Store.Saw_Ammo >= 40 && !Burst_Module_On_Cooldown)
                {

                    StartCoroutine(Burst_Module_Cooldown_Timer()); // put burst module on cooldown if firing is successful also ammo is subtracted from burst module in the cooldown coroutine

                    for (float i = 0; i < 40; i++)
                    {
                        Instantiate(Saw, gameObject.transform.position, gameObject.transform.rotation * Quaternion.Euler(Random.Range(-10f, 10f), 1, 1)); // need to randomize this
                        Persistent_Data_Store.Saw_Ammo--;
                        yield return new WaitForSeconds(.03f);
                    }

                }
                break;


            case "Vines":

                if (Persistent_Data_Store.Vines_Ammo >= 15 && !Burst_Module_On_Cooldown)
                {

                    StartCoroutine(Burst_Module_Cooldown_Timer()); // put burst module on cooldown if firing is successful also ammo is subtracted from burst module in the cooldown coroutine

                    for (float i = 0; i < 15; i++)
                    {
                        Instantiate(Vines, gameObject.transform.position, gameObject.transform.rotation * Quaternion.Euler(Random.Range(-10f, 10f), 1, 1)); // need to randomize this
                        Persistent_Data_Store.Vines_Ammo--;
                        yield return new WaitForSeconds(.03f);
                    }

                }
                break;

        }




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
    {
        if (Persistent_Data_Store.Sniper_Ammo > 0) { Sniper2.enabled = true; } else if(Persistent_Data_Store.Sniper_Ammo <= 0) { Sniper1.enabled = false; }


        Sniper1.text = "D : " + Persistent_Data_Store.Sniper_Ammo;

        if (Sniper_On_Cooldown)
        {
            Sniper2.text = Sniper_Countdown.ToString("F1");

        }
        else if (!Sniper_On_Cooldown)
        {
            Sniper2.enabled = false;
        }




        if (Persistent_Data_Store.Saw_Ammo > 0) { Saw2.enabled = true; } else if (Persistent_Data_Store.Saw_Ammo <= 0) { Saw1.enabled = false; }
        Saw1.text = "E : " + Persistent_Data_Store.Saw_Ammo;

        if (Saw_On_Cooldown)
        {
            Saw2.text = Saw_Countdown.ToString("F1");

        }
        else if (!Saw_On_Cooldown)
        {
            Saw2.enabled = false;
        }




        if (Persistent_Data_Store.Vines_Ammo > 0) { Vines2.enabled = true; } else if (Persistent_Data_Store.Vines_Ammo <= 0) { Vines1.enabled = false; }

        Vines1.text = "Q : " + Persistent_Data_Store.Vines_Ammo;

        if (Vines_On_Cooldown)
        {
            Vines2.text = Vines_Countdown.ToString("F1");

        }
        else if (!Vines_On_Cooldown)
        {
            Vines2.enabled = false;
        }




        if (Persistent_Data_Store.Pierce_Lazer_Ammo > 0) { Lazer2.enabled = true; } else if (Persistent_Data_Store.Pierce_Lazer_Ammo <= 0) { Lazer1.enabled = false; }

        Lazer1.text = "A : " + Persistent_Data_Store.Pierce_Lazer_Ammo;

        if (Pierce_Lazer_On_Cooldown)
        {
            Lazer2.text = Pierce_Lazer_Countdown.ToString("F1");

        }
        else if (!Pierce_Lazer_On_Cooldown)
        {
            Lazer2.enabled = false;
        }




        if (Persistent_Data_Store.Slow_Wave_Ammo > 0) { Slow_Wave2.enabled = true; } else if (Persistent_Data_Store.Slow_Wave_Ammo <= 0) { Slow_Wave2.enabled = false; }

        Slow_Wave2.text = "Shift : " + Persistent_Data_Store.Slow_Wave_Ammo;

        House_Health.text = "House " + Persistent_Data_Store.House_Health.ToString("f1");




        if (Persistent_Data_Store.Burst_Module_Ammo > 0) { Burst_Module.enabled = true; } else if (Persistent_Data_Store.Burst_Module_Ammo <= 0) { Burst_Module1.enabled = false; }

        Burst_Module1.text = "SPACE : " + Persistent_Data_Store.Burst_Module_Ammo;

        if (Burst_Module_On_Cooldown)
        {
            Burst_Module.text = Burst_Module_Countdown.ToString("F1");

        }
        else if (!Burst_Module_On_Cooldown)
        {
            Burst_Module.enabled = false;
        }



    }

























}

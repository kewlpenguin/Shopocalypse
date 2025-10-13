using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
public class Ammo_Behavior : MonoBehaviour 
{

    public bool In_Water_Moving = false;
    public bool In_Water = false;

    public bool On_Conveyer_1 = false;
    public bool On_Conveyer_2 = false;


    Rigidbody My_Rigidbody;
    private float Buyancy_Force = 10;
    private float Water_Speed = 5;
    private float Conveyer_Belt_Speed = 1;
    public float Tech_Pickup_Time;
    private TextMeshProUGUI My_Text;



    private GameObject Lazer_Game_Center;
  




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       

        Lazer_Game_Center = GameObject.Find("Lazer_Game_Middle");


        My_Rigidbody = gameObject.GetComponent<Rigidbody>();
        
        if(gameObject.tag == "Lazer_Ammo" && My_Rigidbody.useGravity == false)
        {
            My_Rigidbody.AddForce(((Lazer_Game_Center.transform.position - gameObject.transform.position) + new Vector3(0, Random.Range(-.3f, .3f), Random.Range(-.3f, .3f))) * 2, ForceMode.Impulse);
            My_Rigidbody.AddTorque(new Vector3(0, 0, 10));
          
        }


        if (gameObject.tag == "fish" || gameObject.tag == "Saw_Ammo_Pickup_1")
        {
          
            StartCoroutine(Fish_Float());

        }




    }


    private void Awake()
    {
        if (gameObject.tag == "Tech_Small" || gameObject.tag == "Tech_Medium" || gameObject.tag == "Tech_Large")
        {
            My_Text = gameObject.GetComponentInChildren<TextMeshProUGUI>();
       
            Tech_Pickup_Time_Assignment();
        }

        if (gameObject.tag == "Slow_Wave_Ammo")
        {
            StartCoroutine(Destroy_Slow_Wave());
        }

    }



    void Update()
    {
      

    }

    
    IEnumerator Destroy_Slow_Wave()// destroys slow wave ammo after 30 secs so it does not build up in fountain base
    { 
           yield return new WaitForSeconds(30);
            Destroy(gameObject);
    }




    


    private void FixedUpdate()
    {
        if (In_Water)
        {
            My_Rigidbody.AddForce(Vector3.up * Buyancy_Force, ForceMode.Acceleration);


        }
        if (In_Water_Moving)
        {
            My_Rigidbody.AddForce(Vector3.up * Buyancy_Force, ForceMode.Acceleration);

            if (!(My_Rigidbody.linearVelocity.z < -3))
            {
                My_Rigidbody.AddForce(Vector3.back * Water_Speed, ForceMode.Acceleration); // this is only used in 1 spot so idc that its not flexible
            }

        }
        
        if (On_Conveyer_1)   // different movement methods for each belt because velocity change was giving me shit when i used it for both
        {

            if (!(My_Rigidbody.linearVelocity.x < -Conveyer_Belt_Speed))
            {

                My_Rigidbody.linearVelocity = new Vector3(-Conveyer_Belt_Speed, 0, 0);
            }

        }

        if (On_Conveyer_2)
        {
            if (!(My_Rigidbody.linearVelocity.z < -Conveyer_Belt_Speed))
            {
                My_Rigidbody.AddForce(Vector3.back * Conveyer_Belt_Speed, ForceMode.VelocityChange);
            }
        }



        if (gameObject.tag == "Lazer_Ammo" && My_Rigidbody.useGravity == true)
        {
            My_Rigidbody.AddForce(Vector3.up * 9f, ForceMode.Acceleration);

        }







    }


    void Tech_Pickup_Time_Assignment() // random time for each, bigger objects take longer on average to pick up, value is used in player controller
    {
        if(gameObject.tag == "Tech_Small")
        {
            Tech_Pickup_Time = Random.Range(.5f, 5f);
            My_Text.text = Tech_Pickup_Time.ToString("f1");

        }
     
        else if(gameObject.tag == "Tech_Medium")
        {
            Tech_Pickup_Time = Random.Range(4f, 13f);
            My_Text.text = Tech_Pickup_Time.ToString("f1");
        }
      
        else if (gameObject.tag == "Tech_Large")
        {
            Tech_Pickup_Time = Random.Range(8f, 30f);
            My_Text.text = Tech_Pickup_Time.ToString("f1");
        }

    }













    IEnumerator Fish_Float()
    { 
        yield return new WaitForSeconds(Random.Range(0f,5f)); // create random fish offsets so they dont look uniform
       
        for (int i = 0; i < 9999999; i++) {
         
            for (int j = 0; j < 50; j++) // float fish up
            {
       
                gameObject.transform.Translate(Vector3.up * .01f);
                yield return new WaitForSeconds(Random.Range(.02f,.04f));
               
            }
        

            for (int k = 0; k < 50; k++)  // float fish down
            {
        
                gameObject.transform.Translate(Vector3.down * .01f);
                yield return new WaitForSeconds(Random.Range(.02f, .04f));
            }

        }
    }






    private void OnTriggerEnter(Collider other)    // on entering watermake it float in the dirtection of the current
    {
        if (other.gameObject.tag == "Water")
        {
            In_Water = true;
        }
        else if (other.gameObject.tag == "Water (Moving)")
        {
            In_Water_Moving = true;
        }



    }


    private void OnTriggerExit(Collider other)  // stop floating and accelerating object
    {
        if (other.gameObject.tag == "Water")
        {
            In_Water = false;
        }

        else if (other.gameObject.tag == "Water (Moving)")
        {
            In_Water_Moving = false;
        }
    }


    private void OnCollisionEnter(Collision collision)   // when the object touches the ground bounce it (only used for lazer ammo in ticket machine right now)
    {
        if(collision.gameObject.tag == "ground")
        {
            My_Rigidbody.AddForce(Vector3.up * 1, ForceMode.Impulse);


        }
        
        if (collision.gameObject.name == "Conveyer_Bottom_Collider")
        {
            On_Conveyer_1 = true;     
 
        }

        if (collision.gameObject.name == "Conveyer_Bottom_Collider (1)")
        {
            On_Conveyer_2 = true;

        }

        if (collision.gameObject.name == "Food_Destroyer")
        {
            Destroy(gameObject);

        }




    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Conveyer_Bottom_Collider")
        {
            On_Conveyer_1 = false;

        }

        if (collision.gameObject.name == "Conveyer_Bottom_Collider (1)")
        {
            On_Conveyer_2 = false;

        }
    }












}

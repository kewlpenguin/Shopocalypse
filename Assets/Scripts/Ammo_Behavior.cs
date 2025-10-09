using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Ammo_Behavior : MonoBehaviour 
{

    public bool In_Water_Moving = false;
    public bool In_Water = false;

    public bool On_Conveyer_1 = false;
    public bool On_Conveyer_2 = false;


    Rigidbody My_Rigidbody;
    private float Buyancy_Force = 10;
    private float Water_Speed = 5;
    private float Conveyer_Belt_Speed = 20;



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





    }


    private void Awake()
    {
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
        
        if (On_Conveyer_1)
        {

            if (!(My_Rigidbody.linearVelocity.x < Conveyer_Belt_Speed))
            {
                My_Rigidbody.AddForce(Vector3.left * Conveyer_Belt_Speed, ForceMode.VelocityChange);
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

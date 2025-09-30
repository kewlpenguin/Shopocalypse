using UnityEngine;

public class Ammo_Behavior : MonoBehaviour
{

    public bool In_Water_Moving = false;
    public bool In_Water = false;
    Rigidbody My_Rigidbody;
    private float Buyancy_Force = 10;
    private float Water_Speed = 5;
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


    void Update()
    {
      

    }




    private void FixedUpdate()
    {
        if (In_Water)
        {
            My_Rigidbody.AddForce(Vector3.up * Buyancy_Force, ForceMode.Acceleration);


        }
        else if (In_Water_Moving)
        {
            My_Rigidbody.AddForce(Vector3.up * Buyancy_Force, ForceMode.Acceleration);
           
            if (!(My_Rigidbody.linearVelocity.z < -3))
            {
                My_Rigidbody.AddForce(Vector3.back * Water_Speed, ForceMode.Acceleration); // this is only used in 1 spot so idc that its not flexible
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
    }



}

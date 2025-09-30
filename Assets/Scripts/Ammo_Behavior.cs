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
        
        if(gameObject.tag == "Lazer_Ammo")
        {
            My_Rigidbody.AddForce((Lazer_Game_Center.transform.position - gameObject.transform.position) * 2, ForceMode.Impulse);
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
    }



    private void OnTriggerEnter(Collider other)
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


    private void OnTriggerExit(Collider other)
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
}

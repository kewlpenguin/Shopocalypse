using UnityEngine;

public class Ammo_Behavior : MonoBehaviour
{

    public bool In_Water_Moving = false;
    public bool In_Water = false;
    Rigidbody My_Rigidbody;
    private float Buyancy_Force = 10;
    private float Water_Speed = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        My_Rigidbody = gameObject.GetComponent<Rigidbody>();



    }

    // Update is called once per frame
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

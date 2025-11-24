using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Bullet_Control : MonoBehaviour
{
    public AudioClip vine_Grow;

    string My_Tag;
   

    public GameObject Vine_Spawn;



    private Rigidbody Bullet_Rb;

    public float Bullet_Lifetime;

    public float Bullet_Move_Speed;

    public int Enemies_Hit;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
      
    }



    private void Awake()
    {
        Bullet_Rb = GetComponent<Rigidbody>();
       
        My_Tag = gameObject.tag;
        
        Choose_Behavior(My_Tag); // also activates behavior selected



    }






    // Update is called once per frame
    void Update()
    {
        
    }


    void Choose_Behavior(string my_Tag) // these all assume the bullet is spawned on the main gun and pointed the right direction
    {
       

        switch (my_Tag)
        {
            case "Main":
                StartCoroutine(Bullet_Movement());
                break;

            case "Slow_Wave":
                StartCoroutine(Bullet_Movement());
                break;

            case "Sniper":
                StartCoroutine(Bullet_Movement());
                break;



            case "Saw":
                Saw_Behavior();
                break;

            case "Pierce_Lazer":
                StartCoroutine(Bullet_Movement());
                break;

            case "Vines":
                StartCoroutine(Bullet_Movement());
                break;

            case "Vine_Spawn":
                Vine_Spawn_Behavior();
                break;

            case null:

                break;
        }
    }


    IEnumerator Bullet_Movement() // bullet movespeed and lifetime are assigned in the inspector per bullet type
    {
        if(gameObject.tag == "Main")
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            gameObject.transform.LookAt(mouseWorldPos);
        }

        Bullet_Rb.AddForce(transform.forward * Bullet_Move_Speed, ForceMode.Impulse);
      
        yield return new WaitForSeconds(Bullet_Lifetime);
      
        Destroy(gameObject);
      
    }

    IEnumerator Destroy_Spawned_Vines() // how long spawned vines last
    {
        yield return new WaitForSeconds(6f);
        Destroy(gameObject);
    }

    IEnumerator Grow_Vines() // how long spawned vines last
    {
        for (int i = 0; i < 40; i++)
        {
            yield return new WaitForSeconds(.01f);
            gameObject.transform.Translate(new Vector3(0,.1f,0));
        }
    }


    void Vine_Spawn_Behavior()
    {
        StartCoroutine(Destroy_Spawned_Vines());
        StartCoroutine(Grow_Vines());

    }


    void Saw_Behavior() // while bullet is active move it forward
    {
        StartCoroutine(Bullet_Movement());
      

    }


    private void OnCollisionEnter(Collision collision) // for vines and saw
    {
        if(gameObject.tag == "Vines" && collision.gameObject.CompareTag("ground"))
        {
            Audio_Manager_Script.instance.Play_Selected_Audio(vine_Grow, gameObject.transform.position, .075f, 1);
            Instantiate(Vine_Spawn, new Vector3(gameObject.transform.position.x, -6, 7), Quaternion.Euler(0,0,0));
           
            Destroy(gameObject);

        }
        
        if (gameObject.tag == "Saw" && collision.gameObject.CompareTag("ground"))
        {
            Destroy(gameObject);
        }
        
        if(gameObject.tag == "Saw" && collision.gameObject.layer == 9) // if is an enemy
        {
           Saw_Stick_To_Enemy(collision.gameObject);
        }
         


    }


    void Saw_Stick_To_Enemy(GameObject Enemy) // im really fricking smart
    {
       
      
        Collider col = gameObject.GetComponent<Collider>();
        col.enabled = false;
        Bullet_Rb.isKinematic = true;

        float y = transform.eulerAngles.y;

        gameObject.transform.LookAt(Enemy.transform);

        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, y, transform.eulerAngles.z);

        StartCoroutine(Fish_Bite());

        transform.SetParent(Enemy.transform);

    }



    IEnumerator Fish_Bite()
    {
        Debug.Log("Fish Bite");

        Transform Open_Fish = gameObject.transform.Find("Fish");
        Transform Closed_Fish = gameObject.transform.Find("Fish - Copy");

        Open_Fish.GetComponent<MeshRenderer>().enabled = true;
        Closed_Fish.GetComponent<MeshRenderer>().enabled = false;

        for (int i = 99999; i > 0; i--)
        {
            yield return new WaitForSeconds(.2f);

                Open_Fish.GetComponent<MeshRenderer>().enabled = false;
            Closed_Fish.GetComponent<MeshRenderer>().enabled = true;

            yield return new WaitForSeconds(.2f);

            Open_Fish.GetComponent<MeshRenderer>().enabled = true;
            Closed_Fish.GetComponent<MeshRenderer>().enabled = false;

        }


    }





}

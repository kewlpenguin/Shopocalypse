using UnityEngine;

public class Vending_Behavior : MonoBehaviour
{
    Transform Child;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      




    }

    private void Awake()
    {
        Child = gameObject.GetComponent<Transform>().GetChild(0);
       
    }


    // Update is called once per frame
    void Update()
    {
   





    }
    private void FixedUpdate()
    {
        Child.transform.Rotate(new Vector3(0, 1, 0));
    }




}

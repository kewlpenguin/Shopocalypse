using UnityEngine;

public class Vending_Behavior : MonoBehaviour
{
    Transform Children;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Children = gameObject.GetComponent<Transform>().GetChild(0);




    }

    // Update is called once per frame
    void Update()
    {
   





    }
    private void FixedUpdate()
    {
        Children.transform.Rotate(new Vector3(0, 1, 0));
    }




}

using UnityEngine;

public class Follow_Main_Gun : MonoBehaviour
{

    public GameObject Main_Gun;





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, Main_Gun.transform.position.y, gameObject.transform.position.z);
    }


}

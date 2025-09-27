using UnityEngine;

public class Enemy_Health_Bar : MonoBehaviour
{
  
    public Transform fillTransform;

    public void SetHealth(float healthPercent)
    {
        fillTransform.localScale = new Vector3(healthPercent, 1, 1);
    }

}

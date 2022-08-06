using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTheGame : MonoBehaviour
{
    public GameObject car;
    void OnCollisionEnter(Collision collision)
    {
        string ObjectName = collision.collider.transform.name;
        Debug.Log("撞到的物体名字为"+ ObjectName);
        if (string.Equals(ObjectName, "Car")) {
            car.GetComponent<SuperSportsCar>().isWin = true;
        }
    }
}

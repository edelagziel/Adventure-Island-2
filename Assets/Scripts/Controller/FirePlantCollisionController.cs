using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePlantCollisionController : MonoBehaviour
{
    public FirePlantController _firePlantController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject + " Start");
        if(collision.gameObject.transform.tag == "Player")
        {
            _firePlantController.SetMeeleStategy();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject + " Exit"); 
        if (collision.gameObject.transform.tag == "Player")
        {
            _firePlantController.SetRangeStategy();
        }
    }
}

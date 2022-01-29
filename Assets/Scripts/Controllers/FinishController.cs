using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishController : MonoBehaviour
{

    private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {

        gameController = GameController.instance;

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameController)
            gameController = GameController.instance;

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {

            gameController.ArriveDestination();

        }
    }
}

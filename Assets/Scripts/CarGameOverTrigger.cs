using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class CarGameOverTrigger : MonoBehaviour
{
    GameObject managera;
    GameManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
        // managera
    }

    // Update is called once per frame
    void Update()
    {
        toptouch();
    }
    // void OnTriggerEnter(Collider other){
    //     if(other.gameObject.name == "World piece")
	// 		manager.GameOver();
    // }
    // void OnCollisionEnter(Collision other){
    //     Debug.Log(other.gameObject.name);
    //     if(other.gameObject.name == "World piece")
	// 		manager.GameOver();
    // }
    void toptouch(){
        if(Physics.Raycast(this.transform.position,transform.up,0.5f)){
            // Debug.Log("touch");
            manager.GameOver();
        }
    }

}

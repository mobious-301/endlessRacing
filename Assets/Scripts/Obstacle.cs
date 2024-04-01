using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
	//game manager reference
	GameManager manager;
	public AudioSource gameover;	
	void Start(){
		//find the game manager
		manager = GameObject.FindObjectOfType<GameManager>();
	}
	
	void OnCollisionEnter(Collision other){
        Debug.Log(other.gameObject.name);
		//if the player hits this obstacle, end the game
		if(other.gameObject.transform.root.CompareTag("Player"))
			manager.GameOver();
        if(other.gameObject.name=="Car"||other.gameObject.name=="ColliderBottun")
			manager.GameOver();
	}
}

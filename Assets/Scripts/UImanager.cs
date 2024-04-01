using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UImanager : MonoBehaviour
{
    public GameObject GameOver;
    public GameObject Gameing;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void toGameOver(){
        GameOver.SetActive(true);
         Gameing.SetActive(false);
    }
    public void toNewOver(){
        GameOver.SetActive(false);
        Gameing.SetActive(true);
    }
    public void toGameing(){
        GameOver.SetActive(true);
    }
    // public void toNewOver(){
    //     GameOver.SetActive(false);
    // }
    

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public AudioSource Backgroundloop;
    // public AudioSource Score;
    // public AudioSource gameOverAudio;
    // Start is called before the first frame update
    void Awake(){
        		//check instance and if there is one already, destroy this object
        if(!instance){
            instance = this;
		}
		else{
            Destroy(gameObject);
		}
		
		//make sure this object won't be destroyed (so background music keeps playing)
        DontDestroyOnLoad(this.gameObject);
        // Backgroundplay();
    }
    void Start()
    {
         Backgroundplay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void  Backgroundplay(){
        Backgroundloop.Play();
    }
}

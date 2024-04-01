using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class startGame : MonoBehaviour
{
    AudioManager AudioManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Return) ||
		//     (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()))
		// {
		// 	if (!(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began &&
		// 	     EventSystem.current.IsPointerOverGameObject((Input.GetTouch(0).fingerId))))
		// 	{
		// 		StartGame();
		// 	}
		// }
        if(Input.anyKeyDown){
            StartGame();
        }

        
    }
    	public void StartGame()
	{
		// UIAnimator.SetTrigger("Start");
        // AudioManager.Backgroundplay();
        Debug.Log("start");
		StartCoroutine(LoadScene("Game"));
	}

	IEnumerator LoadScene(string scene)
	{
		yield return new WaitForSeconds(0.6f);

		SceneManager.LoadScene(scene);
	}

}

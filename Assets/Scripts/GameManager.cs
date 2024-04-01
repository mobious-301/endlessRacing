using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    	//visible in the inspector
	public Text scoreLabel;
	public Text timeLabel;
	public Text gameOverScoreLabel;
	public Text gameOverBestLabel;
	public Animator scoreEffect;
	public Animator UIAnimator;
	public Animator gameOverAnimator;
	public AudioSource gameOverAudio;
	public Car car;

    	//not visible in the inspector
	float time;
	int score;
	
	bool gameOver;
	

    public UImanager UImanager;
    public AudioManager AudioManager;

    
    // Start is called before the first frame update
    void Start()
    {
        score=0;
        time=0;
        AudioManager=GameManager.FindAnyObjectByType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        updateTime();

        //点击开始游戏 restart the game if we're game over and pressed enter or left mouse button
		if(gameOver && (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))){
			// UIAnimator.SetTrigger("Start");


			StartCoroutine(LoadScene(SceneManager.GetActiveScene().name));
		}
        
    }
    void updateTime(){
        time+=Time.deltaTime;
        int timer=(int)time;
        int seconds=timer%60;
        int minutes=timer/60;
        // string timestr=

        timeLabel.text=minutes.ToString()+":"+seconds.ToString();

    }
    // public void GameOver(){
    //     if(gameOver){
    //         return;
    //     }
    //     gameOver=true;
    //     // car.
    //     foreach(BasicMovement Movement in GameObject.FindObjectsOfType<BasicMovement>()){
    //         Movement.speed=0;
    //         Movement.rotateSpeed=0;
    //     }
    // }

    public void GameOver(){
		//the game cannot be over multiple times so we need to return if the game was over already
		if(gameOver)
			return;
		
		//update the score and highscore
		SetScore();
        Debug.Log("GAME OVER");
		
		//show the game over animation and play the audio
		// gameOverAnimator.SetTrigger("Game over");
        // UImanager=new UImanager();
        
        UImanager.toGameOver();
		gameOverAudio.Play();
        // AudioManager.gameOverplay();
		
		//the game is over
		gameOver = true;
		
		//break the car
		car.FallApart();
		
		//stop the world from moving or rotating
		foreach(BasicMovement basicMovement in GameObject.FindObjectsOfType<BasicMovement>()){
			basicMovement.stop();
		}
	}
    public void UpdateScore(int i){
        score+=i;
        scoreLabel.text=score.ToString();
    }

    // 分数 计算,设置
    void SetScore(){
		//update the highscore if our score is higher then the previous best score
		if(score > PlayerPrefs.GetInt("best"))
			PlayerPrefs.SetInt("best", score);
		
		//show the score and the high score
		gameOverScoreLabel.text = "score: " + score;
		gameOverBestLabel.text = "best: " + PlayerPrefs.GetInt("best");
	}
	public void StartGame()
	{
		// UIAnimator.SetTrigger("Start");
		StartCoroutine(LoadScene("Game"));
	}

    IEnumerator LoadScene(string scene){
		yield return new WaitForSeconds(0.6f);
		
		SceneManager.LoadScene(scene);
	}
}

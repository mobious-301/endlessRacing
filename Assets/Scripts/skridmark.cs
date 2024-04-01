using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class skridmark : MonoBehaviour
{
    public Transform[] GrassEffects;
    public Transform[] SkidMarkPoit;
    public GameObject skidMark;
    public float SkidMarkDelay;
    public WorldGenerater generater;
    public float skidsize;

    public int skidMarki;
    public int skidMarknum;
    public GameObject[] skidMarks;
    public GameObject skidMarkParent;
    // Start is called before the first frame update
    void Start()
    {

        creatSkidMarki(skidMarknum);
        StartCoroutine(SkidMark());
        
    }
    void creatSkidMarki(int num){
        skidMarks=new GameObject[num];
        for(int i=0;i<num;i++){
            skidMarks[i]=Instantiate(skidMark,new Vector3(0,-1000,0),SkidMarkPoit[(i)%2].rotation);
            skidMarks[i].transform.parent = skidMarkParent.transform;
            skidMarks[i].transform.localScale=new Vector3(1,0.6f,1)*skidsize;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEffect();
    }

    IEnumerator SkidMark(){
        while(true){
            yield return new WaitForSeconds(SkidMarkDelay);

            for(int i=0;i<SkidMarkPoit.Length;i++){
                if(isCreatSkidMesk(i)){
                // //定时生成刹车痕迹
                // GameObject newSkidMesk=Instantiate(skidMark,SkidMarkPoit[i].position,SkidMarkPoit[i].rotation);
                // newSkidMesk.transform.parent = generater.GetWorldPiece();
                // newSkidMesk.transform.localScale=new Vector3(1,0.6f,1)*skidsize;}
                //更改为定时更新刹车痕迹
                skidMarks[skidMarki].transform.position=SkidMarkPoit[i].position;
                skidMarks[skidMarki].transform.parent = generater.GetWorldPiece();
                skidMarki=(skidMarki+1)%skidMarknum;
            }
        }
        // yield return 0;
    }
    }
    bool isCreatSkidMesk(int i){
        Car car=this.GetComponent<Car>();
        
            if(Mathf.Abs(Input.GetAxis("Horizontal")) >=0.9){
                if(car.onground[i]==true){
                    // Debug.Log("true");
                            return true;
                }
            }
        return false;
    }
    void UpdateEffect(){
        Car car=this.GetComponent<Car>();
        for(int i=0;i<GrassEffects.Length;i++){
            if(car.onground[i]==true){
                if(!GrassEffects[i].gameObject.active==true){
                    GrassEffects[i].gameObject.SetActive(true);
                    
                }
            }else{
                GrassEffects[i].gameObject.SetActive(false);
            }
        }
        
    }
    // void Horizontal(){
    //     if(Input.GetAxis("Horizontal")==1){

    //     }
    // }
    
}

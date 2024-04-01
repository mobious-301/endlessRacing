using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public Transform[] whellMesh;
    public WheelCollider[] whellColliders;

    public int rotationSpeed;
    public int rotationAngle;
    public int whellRotateSpeed;
     int TargetRotation;
    // public float TargetRotationV;
    public int rotationvertical;

//粒子射线判断的长度
    public float GrassEffectOffset;
    public bool[] onground=new bool[4];

    public GameObject car;
    public GameObject ragdoll;
	public AudioSource scoreAudio;    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        // Debug.Log("FixedUpdate");
        for(int i=0;i<whellColliders.Length;i++){
            Quaternion quat;
            Vector3 pos;
            whellColliders[i].GetWorldPose(out pos,out quat);
            whellMesh[i].position =pos;
            whellMesh[i].Rotate(Vector3.right*Time.deltaTime*whellRotateSpeed);
        }
        if(Input.GetMouseButton(0)||Input.GetAxis("Horizontal")!=0||Input.GetAxis("Vertical")!=0){
            UpdateTargetRotation();
            // Debug.Log("GetMouseButton");
        }
        else{
            TargetRotation=0;
        }
        

        Vector3 rotaion=new Vector3(transform.localEulerAngles.x,TargetRotation,transform.localEulerAngles.z);
        // Vector3 rotaion=new Vector3(TargetRotationV,TargetRotation,transform.localEulerAngles.z);
        transform.rotation= Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(rotaion),rotationSpeed*Time.deltaTime);
        // transform.rotation=Quaternion.Euler(new Vector3(TargetRotationV,transform.localEulerAngles.y,transform.localEulerAngles.z));
        // transform.Rotate(new Vector3(Input.GetAxis("Vertical")*360*Time.deltaTime,0,0));
        
        // car.gameObject.transform.Rotate(new Vector3(Input.GetAxis("Vertical")*360*Time.deltaTime,0,0));
UpdateEffect();
    }
    //更新车的角度
    void UpdateTargetRotation(){
        //旋转
        if(Input.GetAxis("Horizontal")!=0){
            if(Input.mousePosition.x>Screen.width*0.5f){
                TargetRotation=rotationAngle;
            }else{
                TargetRotation=-rotationAngle;
            }
        }

        {//ad方向键旋转
            TargetRotation= (int)(rotationAngle*Input.GetAxis("Horizontal"));
            // Debug.Log(rotationAngle);
        }
    }

    // void FixedUpdate(){
    //     //更新车轮痕迹和粒子特效
    //     UpdateEffect();
    // }
    void UpdateEffect(){
        for(int i=0;i<2;i++){
            Transform wheelMesh=whellMesh[i+2];
            if(Physics.Raycast(wheelMesh.position,Vector3.down,GrassEffectOffset)){
                onground[i]=true;
            }
            else{
                onground[i]=false;
            }
        }
    }

    //碰撞代理
    public void FallApart(){
		//destroy the car
		Instantiate(ragdoll, transform.position, transform.rotation);
		gameObject.SetActive(false);
	}
}

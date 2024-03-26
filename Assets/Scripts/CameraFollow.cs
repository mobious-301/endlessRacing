using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraFollow : MonoBehaviour
{
    public Transform camTarget;

    //Damping 美datetime移动的量
    public float rotationDamping;
    public float heightDamping;

    public float height=5f;

    //相机向后偏移
    public float Distance=5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void LateUpdate(){
        if(camTarget==null){
            return;
        }
        float wantRotationAngle=camTarget.transform.eulerAngles.y;
        float wantHeight=camTarget.transform.position.y+height;

        float currentRotationAngle=transform.eulerAngles.y;
        float currentHeight=transform.position.y;

        currentRotationAngle=Mathf.LerpAngle(currentHeight,wantRotationAngle,rotationDamping*Time.deltaTime);
        currentHeight=Mathf.Lerp(currentHeight,wantHeight,heightDamping*Time.deltaTime);


        //1.移动相机到观察者位置
        Quaternion currentRotation=Quaternion.Euler(0,currentRotationAngle,0);
        //2.观察者的基础上 向后偏移
        transform.position = camTarget.position;
        transform.position-= currentRotation*Vector3.forward*Distance;
        //3.相机高度
        transform.position=new Vector3(transform.position.x,currentHeight,transform.position.z);

        //摄像机看向目标
        transform.LookAt(camTarget);

    }
}

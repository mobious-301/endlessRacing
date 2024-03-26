using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBerlin : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    public bool UsePerlin;
    public Vector4 PerlinNoiseST=new Vector4(0.04f,0.04f,0,0);
    public float _a=0.04f;
    public float timei=0;

    private Vector3[] posArr;
    void Awake(){

    }
    // Start is called before the first frame update
    void Start()
    {
        posArr=new Vector3[100];
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }
    void FixedUpdate(){
        if(timei%1<0.5){
            RenderLine(posArr);
            if(timei>100){
            timei=0;
        }
            
        }
        
        timei+=Time.deltaTime;
        

    }
    void RenderLine(Vector3[] posArr){
        _lineRenderer=GetComponent<LineRenderer>();
        // Vector3[] posArr=new Vector3[100];
        for(int i=0;i<posArr.Length;i++){
            if(UsePerlin==true){
                // posArr[i]=new Vector3(i*0.1f,Mathf.PerlinNoise(i*_a,i*_a),0);
                //单位x y值 的输入在 3左右可以得到平滑曲线
                posArr[i]=new Vector3(i*0.1f,Mathf.PerlinNoise(i*PerlinNoiseST.x+PerlinNoiseST.z,i*PerlinNoiseST.y+PerlinNoiseST.w),0);
            }else{
                posArr[i]=new Vector3(i*0.1f,Random.value,0);
            }

            //应用柏林噪声

        }
        _lineRenderer.SetPositions(posArr);
    }
}

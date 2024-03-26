using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class WorldGenerater : MonoBehaviour
{
    public Material material;
    public Vector2 dimensions;

    // public float scale;
	//取消scale使其默认值为一.保证z的重复性
    public float perlinScale;
    
    public float offset;
    public float waveHeight;
	GameObject[] pieces=new GameObject[2];
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i<2;i++){
            GeneratWorldPice(i);
        }
    }
		void GeneratWorldPice(int i){
		//初始化位置
		pieces[i]= CreateCyLinder();
		pieces[i].transform.Translate(Vector3.forward*(dimensions.y-1)*Mathf.PI*i);

		//函数标记尾部位置 z=z*scale*Mathf.PI
		//z太靠后 会被刷新到最前，物体0+z
		}


    // Update is called once per frame
    void Update()
    {
        
    }
    GameObject CreateCyLinder(){
        GameObject newCyLinder=new GameObject();
        newCyLinder.name="World piece";

		//添加移动脚本
		BasicMovement BasicMovement=newCyLinder.AddComponent<BasicMovement>();


        MeshFilter meshFilter=newCyLinder.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer=newCyLinder.AddComponent<MeshRenderer>();

        //材质
        meshRenderer.material=material;
        meshFilter.mesh=Generate();

        //碰撞
        // newCyLinder.AddComponent<MeshCollider>();


        //三角型
		return newCyLinder;





    }

    Mesh Generate(){
        Mesh mesh=new Mesh();
        mesh.name="mesh";
        //uv 顶点三角形
        Vector3[] vertices=null;
        Vector2[] uvs=null;
        int[] triangles=null;

        //创建形状
        creatShape(ref vertices,ref uvs,ref triangles);
        // CreateShapea(ref vertices,ref uvs,ref triangles);

        //赋值
        mesh.vertices=vertices;
        mesh.uv = uvs;
		mesh.triangles = triangles;

        mesh.RecalculateNormals();

        return mesh;
    }

    void creatShape(ref Vector3[] vertices,ref Vector2[] uvs,ref int[] triangle){
        //z轴延伸
        int xCount= (int)dimensions.x;
        int zCount= (int)dimensions.y;

        //初始化顶点和 uv  
        vertices=new Vector3[(xCount+1)*(zCount)];
        uvs=new Vector2[(xCount+1)*(zCount)];

        // 半径
        float radius=xCount*0.5f;

        int index=0;
		StringBuilder loga=new StringBuilder();
        // 变形
		
        for(int z=0;z<zCount;z++){
        	for(int x=0;x<xCount;x++){
                
                //圆柱体角度
				//sin的x变化周期为2pi
                // float angle = x * Mathf.PI * 2f/xCount;
				
				// xCount-1 保证每一圈最后一个顶点 和第一个顶点位置相同
                float angle=(float)x/(float)(xCount-1)*Mathf.PI * 2f;
				
                vertices[index]=new Vector3(Mathf.Cos(angle)*radius,Mathf.Sin(angle)*radius,z*Mathf.PI);
				//平面模式检查模型错误
				// vertices[index]=new Vector3(angle,angle,z*scale*Mathf.PI);
				
				loga.AppendLine(vertices[index].ToString());


                uvs[index]=new Vector2(x,z);

                //柏林噪声偏移
                float px=vertices[index].x*perlinScale+offset;
				px= vertices[index].x/(xCount-1);
				// Debug.Log(vertices[index].x/Mathf.PI);
                float py=vertices[index].z*perlinScale+offset;
				py=vertices[index].z*0.06f+100;
				py=vertices[index].z/Mathf.PI;
				Debug.Log(vertices[index].z/Mathf.PI);
                //中心线 顶点延归一化法线做偏移
                Vector3 center=new Vector3(0,0,vertices[index].z);
                vertices[index]+=(center-vertices[index]).normalized*Mathf.PerlinNoise(px,py)*waveHeight;
                index++;
            }
        }
		// Debug.Log(loga);
        //三角形数组  一个矩形2个三角形 每个三角形三个顶点
        triangle=new int[xCount*(zCount-1)*6];
        //创建二位数组存储顶点方便调用
        int[] boxBase=new int[6];
		int current=0;
        for(int x=0;x<zCount-1;x++){
			//串联三角型时避免串连最后一排 避免最后一排循环到错误的第一排
			for(int z=0;z<xCount-1;z++){
				boxBase=new int[]
				{
					x*xCount+z,
					(x+1)*xCount+z,
					x*xCount+1+z,

					
					(x+1)*xCount+z,
					(x+1)*xCount+1+z,
					x*xCount+1+z
					//顺序串联三角形时还要考虑 左手螺旋定哲
					//避免两三角型法线不一致

				};
			
				for(int j=0;j<6;j++){
					// Debug.Log(current+j);
					triangle[current+j]=boxBase[j];
				}
				current+=6;

			}
        }
		for(int i=0;i<triangle.Length;i++){
			// Debug.Log(triangle[i]);
		}
    }
    

	
}

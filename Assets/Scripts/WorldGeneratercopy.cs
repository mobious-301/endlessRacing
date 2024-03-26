using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneratercopy : MonoBehaviour
{
    public Material material;
    public Vector2 dimensions;

    public float scale;
    public float perlinScale;
    
    public float offset;
    public float waveHeight;


	public float globalSpeed;

	public float positionz; //圆柱体0点刷新的偏移值
	GameObject[] pieces=new GameObject[2];
    // private object pz;

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
		pieces[i].transform.Translate(Vector3.forward*dimensions.y*scale*Mathf.PI*i);

		//函数标记尾部位置 z=z*scale*Mathf.PI
		//z太靠后 会被刷新到最前，物体0+z

    }
	void UpdateSinglePiece(GameObject pieces){
		BasicMovement movement=pieces.AddComponent<BasicMovement>();
		movement.speed=-globalSpeed;

	}

    // Update is called once per frame
    void Update()
    {
		//位置重制使用相对位置，避免时间精度导致位置偏移
		for(int i=0;i<2;i++){
			if(pieces[0].transform.position.z<-(int)dimensions.y*scale*Mathf.PI+positionz){
				pieces[0].transform.position =pieces[1].transform.position+Vector3.forward*dimensions.y*scale*Mathf.PI;
			}
			if(pieces[1].transform.position.z<-(int)dimensions.y*scale*Mathf.PI+positionz){
				pieces[1].transform.position =pieces[0].transform.position+Vector3.forward*dimensions.y*scale*Mathf.PI;
			}
		}
        
    }
    GameObject CreateCyLinder(){
        GameObject newCyLinder=new GameObject();
        newCyLinder.name="World piece";

		//添加移动脚本
		BasicMovement BasicMovement=newCyLinder.AddComponent<BasicMovement>();
		BasicMovement.speed=-globalSpeed;

        MeshFilter meshFilter=newCyLinder.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer=newCyLinder.AddComponent<MeshRenderer>();

        //材质
        meshRenderer.material=material;
        meshFilter.mesh=Generate();

        //碰撞
        newCyLinder.AddComponent<MeshCollider>();


        //三角型
		return newCyLinder;





    }

    private Mesh Generate(){
        Mesh mesh=new Mesh();
        mesh.name="mesh";
        //uv 顶点三角形
        Vector3[] vertices=null;
        Vector2[] uvs=null;
        int[] triangles=null;

        //创建形状
        // creatShape(ref vertices,ref uvs,ref triangles);
        CreateShapea(ref vertices,ref uvs,ref triangles);

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
        vertices=new Vector3[(xCount+1)*(zCount+1)];
        uvs=new Vector2[(xCount+1)*(zCount+1)];

        // 半径
        float radius=xCount*scale*0.5f;

        int index=0;
        // 变形
        for(int x=0;x<=xCount;x++){
            for(int z=0;z<=zCount;z++){
                
                //圆柱体角度
				//sin的x变化周期为2pi
                // float angle = x * Mathf.PI * 2f/xCount;
                float angle=(float)x/(float)xCount*Mathf.PI * 2f;
				// angle=(float)x/(float)xCount;
				Debug.Log(angle);
                vertices[index]=new Vector3(Mathf.Cos(angle),Mathf.Sin(angle),z*scale*Mathf.PI);

                uvs[index]=new Vector2(x*scale,z*scale);

                //柏林噪声偏移 柏林噪声还要考虑连续性 以uv_st 的概念为基础
                float px=vertices[index].x*perlinScale+offset;
                float py=vertices[index].z*perlinScale+offset;
                //中心线 顶点延归一化法线做偏移
                Vector3 center=new Vector3(0,0,vertices[index].z);
                vertices[index]+=(center-vertices[index]).normalized*Mathf.PerlinNoise(px,py)*waveHeight;
                index++;
            }
        }
        //三角形数组  一个矩形2个三角形 每个三角形三个顶点
        triangle=new int[xCount*zCount*6];
        //创建二位数组存储顶点方便调用
        int[] boxBase=new int[6];
        for(int x=0;x<xCount;x++){
            boxBase=new int[]
            {
                x * (zCount + 1), 
				x * (zCount + 1) + 1,
				(x + 1) * (zCount + 1),
				x * (zCount + 1) + 1,
				(x + 1) * (zCount + 1) + 1,
				(x + 1) * (zCount + 1),

            };
        }

        int current=0;
        for(int z=0;z<zCount;z++){
            for(int i=0;i<6;i++){
                boxBase[i]=boxBase[i]+1;
            }
            //
            for(int j=0;j<6;j++){
                triangle[current+j]=boxBase[j]-1;
            }
            current+=6;

        }

    }
    

	void CreateShapea(ref Vector3[] vertices, ref Vector2[] uvs, ref int[] triangles){
		
		//get the size for this piece on the x and z axis
		int xCount = (int)dimensions.x;
		int zCount = (int)dimensions.y;
		
		//initialize the vertices and uv arrays using the desired dimensions
		vertices = new Vector3[(xCount + 1) * (zCount + 1)];
		uvs = new Vector2[(xCount + 1) * (zCount + 1)];
		
		int index = 0;
		
		//get the cylinder radius
		float radius = xCount * scale * 0.5f;
		
		//nest two loops to go through all vertices on the x and z axis
		for(int x = 0; x <= xCount; x++){
			for(int z = 0; z <= zCount; z++){
								//get the angle in the cylinder to position this vertice correctly
				float angle = x * Mathf.PI * 2f/xCount;
				
				//use cosinus and sinus of the angle to set this vertice
				vertices[index] = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, z * scale * Mathf.PI);
				
				//also update the uvs so we can texture the terrain
				uvs[index] = new Vector2(x * scale, z * scale);
				
				//now use our perlin scale and offset to create x and z values for the perlin noise
				float pX = (vertices[index].x * perlinScale) + offset;
				// pX= vertices[index].x/(xCount-1);
				float pZ = (vertices[index].z * perlinScale) + offset;
				// pZ=vertices[index].z*0.06f+100;
				// pZ=vertices[index].z/vertices[vertices.Length-xCount-zCount].z*8%1;
				// Debug.Log(pZ);
				
				//get the center of the cylinder but keep the z of this vertice so we can point inwards to the center
				Vector3 center = new Vector3(0, 0, vertices[index].z);
				//now move this vertice inwards towards the center using perlin noise and the desired wave height
				vertices[index] += (center - vertices[index]).normalized * Mathf.PerlinNoise(pX, pZ) * waveHeight;
				
				//this part handles smooth transition between world pieces:
				
				//check if there are begin points and if we're at the start of the mesh (z means the forward direction, so through the cylinder)
				// if(z < startTransitionLength && beginPoints[0] != Vector3.zero){
				// 	//if so, we must combine the perlin noise value with the begin points
				// 	//we need to increase the percentage of the vertice that comes from the perlin noise 
				// 	//and decrease the percentage from the begin point
				// 	//this way it will transition from the last world piece to the new perlin noise values
					
				// 	//the percentage of perlin noise in the vertices will increase while we're moving further into the cylinder
				// 	float perlinPercentage = z * (1f/startTransitionLength);
				// 	//don't use the z begin point since it will not have the correct position and we only care about the noise on x and y axis
				// 	Vector3 beginPoint = new Vector3(beginPoints[x].x, beginPoints[x].y, vertices[index].z);
					
				// 	//combine the begin point(which are the last vertices from the previous world piece) and original vertice to smoothly transition to the new world piece
				// 	vertices[index] = (perlinPercentage * vertices[index]) + ((1f - perlinPercentage) * beginPoint);
				// }
				// else if(z == zCount){
				// 	//it these are the last vertices, update the begin points so the next piece will transition smoothly as well
				// 	beginPoints[x] = vertices[index];
				// }
				
				// //spawn items at random positions using the mesh vertices
				// if(Random.Range(0, startObstacleChance) == 0 && !(gate == null && obstacles.Length == 0))
				// 	CreateItem(vertices[index], x);
				
				//increase the current vertice index
				index++;
			}
		}
		
		//initialize the array of triangles (x * z is the number of squares, and each square has two triangles so 6 vertices)
		triangles = new int[xCount * zCount * 6];
		
		//create the base for our squares (which makes the generation algorithm easier)
		int[] boxBase = new int[6];
		
		int current = 0;
		
		//for all x positions
		for(int x = 0; x < xCount; x++){
			//create a new base that we can use to populate a new row of squares on the z axis
			boxBase = new int[]{ 
				x * (zCount + 1), 
				x * (zCount + 1) + 1,
				(x + 1) * (zCount + 1),
				x * (zCount + 1) + 1,
				(x + 1) * (zCount + 1) + 1,
				(x + 1) * (zCount + 1),
			};
			
			//this was used to close the mesh (it would connect the last triangles to the first triangles)
			//it messes up the texture so I decided not to use it
			//if(x == xCount - 1){
			//	boxBase[2] = 0;
			//	boxBase[4] = 1;
			//	boxBase[5] = 0;
			//}
			
			//for all z positions
			for(int z = 0; z < zCount; z++){
				//increase all vertice indexes in the box by one to go to the next square on this z row
				for(int i = 0; i < 6; i++){
					boxBase[i] = boxBase[i] + 1;
				}
				
				//assign 2 new triangles based upon 6 vertices to fill in one new square
				for(int j = 0; j < 6; j++){					
					triangles[current + j] = boxBase[j] - 1;
				}
				
				//now increase current by 6 to go to the next square
				current += 6;
			}
		}
	}

}

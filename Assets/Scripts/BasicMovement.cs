using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    public float speed=0;
    public float rotateSpeed=15f;
    public bool lamp;
    public Car car;
    Transform carTransform;
    public WorldGenerater generator;
    // Start is called before the first frame update
    void Start()
    {
        car=GameObject.FindAnyObjectByType<Car>();
		generator = GameObject.FindObjectOfType<WorldGenerater>().GetComponent<WorldGenerater>();
		
		if(car != null)
			carTransform = car.gameObject.transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward*speed*Time.deltaTime);
        if(car != null)
			CheckRotate();
        
    }
    void CheckRotate(){
		//the directional light rotates over an other axis than the world objects
		Vector3 direction = (lamp) ? Vector3.right : Vector3.forward;
		//get the car rotation
		float carRotation = carTransform.localEulerAngles.y;
		
		//get the left rotation (eulerAngles always returned positive rotations)
		if(carRotation > car.rotationAngle * 2f)
			carRotation = (360 - carRotation) * -1f;
		
		//rotate this object based on the direction value, speed value, car rotation and world dimensions
		transform.Rotate(direction * -rotateSpeed * (carRotation/(float)car.rotationAngle) * (36f/(float)generator.dimensions.x) * Time.deltaTime);
        // Debug.Log(direction * -rotateSpeed * (carRotation/(float)car.rotationAngle) * (36f/generator.dimensions.x) * Time.deltaTime);
	}
    public void stop(){
        speed = 0;
		rotateSpeed = 0;
    }
    
}

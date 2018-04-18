using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPlacer : MonoBehaviour {

	public GameObject target;
	public Camera myCamera;
	public RawImage myRawImage;
	public RenderTexture myTexture;

	void Start ()
	{
		myCamera = transform.GetComponentInChildren<Camera>();
		myRawImage = transform.GetComponentInChildren<RawImage>();
		myTexture = new RenderTexture(200, 200, 0);
		myCamera.targetTexture = myTexture;
		myRawImage.texture = myTexture;
		//myCamera.transform.position = target.transform.position + (Vector3.back * 10);
	}
	
	void Update ()
	{
		
		myCamera.transform.position = target.transform.GetComponent<MeshRenderer>().bounds.center + (Vector3.back * 10);
		myCamera.transform.eulerAngles = Vector3.zero;
		target.transform.Rotate(Vector3.up);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPlacer : MonoBehaviour {

	public GameObject target;
	public Camera myCamera;
	public RawImage myRawImage;
	public RenderTexture myTexture;

	private Vector3 targetPos;

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

		if (target.transform.GetComponent<MeshRenderer>())
			myCamera.transform.position = target.transform.GetComponent<MeshRenderer>().bounds.center + (Vector3.back * 10);
		else
		{
			if (Vector3.Distance(targetPos,target.transform.position) > 20f)
			{
				Debug.Log("poooss");
				Transform[] pos = target.transform.GetComponentsInChildren<Transform>();
				Vector3 max = pos[0].position;
				Vector3 min = pos[0].position;
				int leng = pos.Length;
				for (int i = 0; i < leng; ++i)
				{
					Vector3 tmp = pos[i].position;
					if (max.x < tmp.x)
						max.x = tmp.x;
					if (max.y < tmp.y)
						max.y = tmp.y;
					if (max.z < tmp.z)
						max.z = tmp.z;
					if (min.x > tmp.x)
						min.x = tmp.x;
					if (min.y > tmp.y)
						min.y = tmp.y;
					if (min.z > tmp.z)
						min.z = tmp.z;
				}
				targetPos = (max + min) / 2;
				Debug.Log(targetPos);
			}
			myCamera.transform.position = targetPos + (Vector3.back * 10);
		}
		myCamera.transform.eulerAngles = Vector3.zero;
		//target.transform.Rotate(Vector3.up);
		//target.transform.Rotate(Vector3.back * 0.7f);
		//target.transform.Rotate(Vector3.right * 1.2f);
		target.transform.Rotate(Vector3.up * Mathf.Sin(Time.time));
		target.transform.Rotate(Vector3.forward * Mathf.Sin(Time.time + 2));
		target.transform.Rotate(Vector3.right * Mathf.Sin(Time.time + 4));
	}
}

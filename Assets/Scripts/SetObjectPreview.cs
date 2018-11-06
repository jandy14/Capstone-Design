using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetObjectPreview : MonoBehaviour {
	public GameObject target;

	void Start ()
	{
		target = Instantiate(target);
		target.transform.parent = transform;
		target.transform.localPosition = new Vector3(-230,0,0);

	}
	void Update ()
	{
		target.transform.Rotate(Vector3.up);
	}
}

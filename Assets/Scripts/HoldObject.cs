using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldObject : MonoBehaviour {
	
	[SerializeField] private int isIn;
	public bool isHolding;

	void Start ()
	{
		isIn = 0;
		isHolding = false;
	}
	
	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("In trigger");
		if(other.tag == "Glass")
		{
			++isIn;
			if (isIn == 1)
			{
				isHolding = true;
			}
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Glass")
		{
			--isIn;
			if (isIn == 0)
			{
				isHolding = false;
			}
		}
	}
}

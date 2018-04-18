using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalLoadListScript : MonoBehaviour {

	static public LocalLoadListScript instance;
	public GameObject button;
	public GameObject[] stlList;

	private void Awake()
	{
		if(instance == null)
			instance = this;
	}
	// Use this for initialization
	void Start ()
	{
			
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetSTLList(GameObject[] list, string[] path)
	{
		Transform contents = transform.GetChild(0);
		int count = contents.childCount;
		Debug.Log(count);
		for(int i = count - 1; i >= 0; --i)
		{
			Destroy(contents.GetChild(i).GetComponent<ObjectPlacer>().target);
			Destroy(contents.GetChild(i).gameObject);
		}
		Debug.Log("list Length" + list.Length);
		stlList = list;
		int leng = list.Length;
		for(int i = 0; i < leng; ++i)
		{
			Debug.Log(stlList[i].transform.GetComponent<MeshRenderer>().bounds.size);
			Debug.Log(stlList[i].transform.GetComponent<MeshRenderer>().bounds.max);
			Debug.Log(stlList[i].transform.GetComponent<MeshRenderer>().bounds.min);
			stlList[i].transform.position = new Vector3(i * 100, 4000, 0);
			Vector3 size = stlList[i].transform.GetComponent<MeshRenderer>().bounds.size;
			float max = size.x > size.y ? (size.x > size.z ? size.x : size.z) : (size.y > size.z ? size.y : size.z);
			stlList[i].transform.localScale = Vector3.one / (max * 0.8f);
			GameObject g = Instantiate(button, transform.GetChild(0));
			string tmp = path[i];
			g.GetComponent<Button>().onClick.AddListener(() => { CreativeZoneReadAndWrite.instance.LoadSTLToPath(tmp); Debug.Log(tmp); } );
			g.transform.GetComponentInChildren<ObjectPlacer>().target = stlList[i];
		}
	}
}

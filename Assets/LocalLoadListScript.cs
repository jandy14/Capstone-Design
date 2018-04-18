using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalLoadListScript : MonoBehaviour {

	static public LocalLoadListScript instance;
	public GameObject button;
	public GameObject[] itemList;

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
	public void SetCZList(GameObject[] list, string[] path)
	{
		//기존 리스트 제거
		Transform contents = transform.GetChild(0);
		int count = contents.childCount;
		Debug.Log(count);
		for (int i = count - 1; i >= 0; --i)
		{
			Destroy(contents.GetChild(i).GetComponent<ObjectPlacer>().target);
			Destroy(contents.GetChild(i).gameObject);
		}
		//리스트 아이템 추가
		Debug.Log("list Length" + list.Length);
		itemList = list;
		int leng = list.Length;
		for (int i = 0; i < leng; ++i)
		{
			//Debug.Log(itemList[i].transform.GetComponent<MeshRenderer>().bounds.size);
			//Debug.Log(itemList[i].transform.GetComponent<MeshRenderer>().bounds.max);
			//Debug.Log(itemList[i].transform.GetComponent<MeshRenderer>().bounds.min);
			itemList[i].transform.position = Vector3.zero;
			MeshRenderer[] render = itemList[i].GetComponentsInChildren<MeshRenderer>();
			Bounds combine = new Bounds();
			foreach (MeshRenderer r in render)
			{
				combine.Encapsulate(r.bounds);
			}
			Vector3 size = combine.size;
			Debug.Log(size);
			float max = size.x > size.y ? (size.x > size.z ? size.x : size.z) : (size.y > size.z ? size.y : size.z);
			itemList[i].transform.localScale = Vector3.one / (max);
			GameObject g = Instantiate(button, transform.GetChild(0));
			string tmp = path[i];
			g.GetComponent<Button>().onClick.AddListener(() => { CreativeZoneReadAndWrite.instance.LoadCZToPath(false, tmp); Debug.Log(tmp); });
			g.transform.GetComponentInChildren<ObjectPlacer>().target = itemList[i];
			itemList[i].transform.position = new Vector3(i * 100, 4000, 0);
		}
	}

	public void SetCZListInCreative(GameObject[] list, string[] path)
	{
		//기존 리스트 제거
		Transform contents = transform.GetChild(0);
		int count = contents.childCount;
		Debug.Log(count);
		for (int i = count - 1; i >= 0; --i)
		{
			Destroy(contents.GetChild(i).GetComponent<ObjectPlacer>().target);
			Destroy(contents.GetChild(i).gameObject);
		}
		//리스트 아이템 추가
		Debug.Log("list Length" + list.Length);
		itemList = list;
		int leng = list.Length;
		for (int i = 0; i < leng; ++i)
		{
			//Debug.Log(itemList[i].transform.GetComponent<MeshRenderer>().bounds.size);
			//Debug.Log(itemList[i].transform.GetComponent<MeshRenderer>().bounds.max);
			//Debug.Log(itemList[i].transform.GetComponent<MeshRenderer>().bounds.min);
			itemList[i].transform.position = Vector3.zero;
			MeshRenderer[] render = itemList[i].GetComponentsInChildren<MeshRenderer>();
			Bounds combine = new Bounds();
			foreach (MeshRenderer r in render)
			{
				combine.Encapsulate(r.bounds);
			}
			Vector3 size = combine.size;
			Debug.Log(size);
			float max = size.x > size.y ? (size.x > size.z ? size.x : size.z) : (size.y > size.z ? size.y : size.z);
			itemList[i].transform.localScale = Vector3.one / (max);
			GameObject g = Instantiate(button, transform.GetChild(0));
			string tmp = path[i];
			g.GetComponent<Button>().onClick.AddListener(() => { CreativeZoneReadAndWrite.instance.LoadCZToPath(true, tmp); Debug.Log(tmp); });
			g.transform.GetComponentInChildren<ObjectPlacer>().target = itemList[i];
			itemList[i].transform.position = new Vector3(i * 100, 4000, 0);
		}
	}
	public void SetitemList(GameObject[] list, string[] path)
	{
		//기존 리스트 제거
		Transform contents = transform.GetChild(0);
		int count = contents.childCount;
		Debug.Log(count);
		for(int i = count - 1; i >= 0; --i)
		{
			Destroy(contents.GetChild(i).GetComponent<ObjectPlacer>().target);
			Destroy(contents.GetChild(i).gameObject);
		}

		//리스트 아이템 추가
		Debug.Log("list Length" + list.Length);
		itemList = list;
		int leng = list.Length;
		for(int i = 0; i < leng; ++i)
		{
			//Debug.Log(itemList[i].transform.GetComponent<MeshRenderer>().bounds.size);
			//Debug.Log(itemList[i].transform.GetComponent<MeshRenderer>().bounds.max);
			//Debug.Log(itemList[i].transform.GetComponent<MeshRenderer>().bounds.min);
			itemList[i].transform.position = new Vector3(i * 100, 4000, 0);
			Vector3 size = itemList[i].transform.GetComponent<MeshRenderer>().bounds.size;
			float max = size.x > size.y ? (size.x > size.z ? size.x : size.z) : (size.y > size.z ? size.y : size.z);
			itemList[i].transform.localScale = Vector3.one / (max * 0.8f);
			GameObject g = Instantiate(button, transform.GetChild(0));
			string tmp = path[i];
			g.GetComponent<Button>().onClick.AddListener(() => { CreativeZoneReadAndWrite.instance.LoadSTLToPath(tmp); Debug.Log(tmp); } );
			g.transform.GetComponentInChildren<ObjectPlacer>().target = itemList[i];
		}
	}
}

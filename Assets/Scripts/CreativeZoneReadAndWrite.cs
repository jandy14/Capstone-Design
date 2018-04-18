using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//string[] file = Directory.GetFiles("./data/", "*.stl");
//foreach(string item in file)
//{
//	Debug.Log(item);
//}

public class CreativeZoneReadAndWrite : MonoBehaviour {

	static public CreativeZoneReadAndWrite instance;
	[SerializeField] private Transform v;
	[SerializeField] private Material m;
	public GameObject target;
	public GameObject madeOf;

	private void Awake()
	{
		if (instance == null)
			instance = this;
	}
	// Use this for initialization
	void Start ()
	{
		Debug.Log(GameObject.FindWithTag("MainCamera"));
		v = GameObject.FindWithTag("MainCamera").transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
		//if (Input.GetKeyDown(KeyCode.Q))
		//{
		//	WriteCZ(target, "./data/data.cz");
		//}
		//if (Input.GetKeyDown(KeyCode.E))
		//{
		//	ReadCZ("./data/data.cz", madeOf);
		//}
		//if (Input.GetKeyDown(KeyCode.T))
		//{
		//	CZtoSTL("./data/data.cz", "./data/cz2stl.stl", madeOf);
		//}
	}

	public void SaveToCZ()
	{
		WriteCZ(target, "./data/data.cz");

		//int childs = target.transform.childCount;
		//for (int i = childs - 1; i > 1; --i)
		//{
		//	Destroy(transform.GetChild(i).gameObject);
		//}
	}
	public void SaveToSTL()
	{
		CZtoSTL(target, "./data/cz2stl.stl");

		//int childs = target.transform.childCount;
		//for (int i = childs - 1; i > 1; --i)
		//{
		//	Destroy(transform.GetChild(i).gameObject);
		//}
	}

	public void LoadCZInGame()
	{
		string[] file = Directory.GetFiles("./data/", "*.cz");
		GameObject[] result = new GameObject[file.Length];

		int index = 0;
		foreach (string item in file)
		{
			result[index] = ReadCZ(item, madeOf);
			Destroy(result[index].GetComponent<Rigidbody>());
			//result[index].GetComponent<MeshRenderer>().material = m;
			++index;
		}

		LocalLoadListScript.instance.SetCZList(result, file);
	}

	public void LoadCZInCreative()
	{
		string[] file = Directory.GetFiles("./data/", "*.cz");
		GameObject[] result = new GameObject[file.Length];

		int index = 0;
		foreach (string item in file)
		{
			result[index] = ReadCZ(item, madeOf);
			//Destroy(result[index].GetComponent<Rigidbody>());
			//result[index].GetComponent<MeshRenderer>().material = m;
			++index;
		}

		LocalLoadListScript.instance.SetCZListInCreative(result, file);
	}

	public void LoadCZToPath(bool isFreeze, string path)
	{
		GameObject g = ReadCZ(path, madeOf);

		if (target)
		{
			g.transform.position = target.transform.position;
			g.transform.rotation = target.transform.rotation;
			g.transform.localScale = target.transform.localScale;
			Destroy(target);
			target = g;
		}
		else
		{
			g.AddComponent<HoldObject>();
			g.transform.position = v.position + (v.rotation * Vector3.forward * 0.5f);
			MeshRenderer[] render = g.GetComponentsInChildren<MeshRenderer>();
			Bounds combine = new Bounds();
			foreach (MeshRenderer r in render)
			{
				combine.Encapsulate(r.bounds);
			}
			Vector3 size = combine.size;
			float max = size.x > size.y ? (size.x > size.z ? size.x : size.z) : (size.y > size.z ? size.y : size.z);
			g.transform.localScale = Vector3.one / (max * 2);
			g.transform.position = v.position + (v.rotation * Vector3.forward * 0.5f);
			
		}
		if(!isFreeze)
		{
			g.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		}
	}
	public void LoadSTL()
	{
		string[] file = Directory.GetFiles("./data/", "*.stl");
		GameObject[] result = new GameObject[file.Length];

		int index = 0;
		foreach (string item in file)
		{
			result[index] = STLReadAndWrite.ReadSTL(item);
			result[index].GetComponent<MeshRenderer>().material = m;
			++index;
		}

		LocalLoadListScript.instance.SetitemList(result, file);
	}
	public void LoadSTLToPath(string path)
	{
		GameObject g = STLReadAndWrite.ReadSTL(path);

		g.AddComponent<HoldObject>();
		g.AddComponent<Rigidbody>();
		g.AddComponent<MeshCollider>().convex = true;
		g.GetComponent<MeshRenderer>().material = m;
		Vector3 size = g.GetComponent<MeshRenderer>().bounds.size;
		float max = size.x > size.y ? (size.x > size.z ? size.x : size.z) : (size.y > size.z ? size.y : size.z);
		g.transform.localScale = Vector3.one / (max * 2);
		//위치 조정
		g.transform.position = v.position;// + (v.rotation * Vector3.forward * 0.5f);
		g.transform.position += (v.position - g.GetComponent<MeshRenderer>().bounds.center);
		g.transform.position += (v.rotation * Vector3.forward * 0.5f);
	}
	public static void WriteCZ (GameObject target, string path)
	{
		FileStream fs;
		int index = 1;
		if (File.Exists(path))
		{
			string name = path.Substring(0, path.Length - 3);
			while (File.Exists(name + "(" + index + ")" + ".cz"))
			{
				++index;
			}
			fs = new FileStream(name + "(" + index + ")" + ".cz", FileMode.CreateNew, FileAccess.Write);
		}
		else
		{
			fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
		}
		BinaryWriter bw = new BinaryWriter(fs);

		string header = "This is CZFILE made by meeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee";
		
		
		bw.Write(header.Substring(0, 80).ToCharArray());

		//Count
		uint childCount = (uint)target.transform.childCount;
		bw.Write(childCount);
		Debug.Log(childCount);

		for (uint i = 0; i < childCount; ++i)
		{
			bw.Write(target.transform.GetChild((int)i).localPosition.x);
			bw.Write(target.transform.GetChild((int)i).localPosition.y);
			bw.Write(target.transform.GetChild((int)i).localPosition.z);
		}

		bw.Close();
		fs.Close();
	}
	public static GameObject ReadCZ(string path, GameObject madeOf)
	{
		GameObject result = new GameObject();
		result.AddComponent<Rigidbody>();
		result.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
		FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
		BinaryReader br = new BinaryReader(fs);

		//header
		br.ReadChars(80);
		//tcount
		uint count = br.ReadUInt32();

		for(uint i = 0; i < count; ++i)
		{
			float x = br.ReadSingle();
			float y = br.ReadSingle();
			float z = br.ReadSingle();

			GameObject tmp = Instantiate(madeOf, result.transform);
			tmp.transform.localPosition = new Vector3(x, y, z);
		}

		return result;
	}

	public static void CZtoSTL(string czPath, string stlPath, GameObject madeOf)
	{
		GameObject target = ReadCZ(czPath, madeOf);
		CZtoSTL(target, stlPath);
		Destroy(target);
	}
	public static void CZtoSTL(GameObject target, string stlPath)
	{
		MeshFilter[] meshFilters = target.GetComponentsInChildren<MeshFilter>();
		CombineInstance[] combine = new CombineInstance[meshFilters.Length];

		for (int i = 0; i <meshFilters.Length; ++i)
		{
			combine[i].mesh = meshFilters[i].sharedMesh;
			combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
		}

		GameObject tmp = new GameObject();
		tmp.AddComponent<MeshFilter>();
		tmp.GetComponent<MeshFilter>().mesh = new Mesh();
		tmp.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);

		STLReadAndWrite.WriteSTL(tmp, stlPath);
		Destroy(tmp);
	}
}

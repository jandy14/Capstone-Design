﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CreativeZoneReadAndWrite : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	public static void WriteCZ (GameObject target, string path)
	{
		FileStream fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
		BinaryWriter bw = new BinaryWriter(fs);

		string header = "This is CZFILE made by meeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee";
		bw.Write(header.ToCharArray());

		//Count
		uint childCount = (uint)target.transform.childCount;
		bw.Write(childCount);
		
		for(uint i = 0; i < childCount; ++i)
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
}

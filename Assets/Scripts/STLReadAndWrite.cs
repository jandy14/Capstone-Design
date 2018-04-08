using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class STLReadAndWrite : MonoBehaviour {

	public GameObject target;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.W))
		{
			WriteSTL(target, "./data.stl");
		}
		if(Input.GetKeyDown(KeyCode.R))
		{
			ReadSTL("./data.stl");
		}
	}

	public static void WriteSTL(GameObject pTarget, string pPath)
	{
		FileStream _fs = new FileStream(pPath, FileMode.CreateNew, FileAccess.Write);
		BinaryWriter _bw = new BinaryWriter(_fs);
		string _header = "This is mySTLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL";
		_bw.Write(_header.ToCharArray());
		//t count
		Mesh _m = pTarget.GetComponent<MeshFilter>().mesh;
		_bw.Write(_m.triangles.Length / 3);

		int[] _tindex = _m.triangles;
		Vector3[] _vindex = _m.vertices;

		int _loopCount = _tindex.Length / 3;
		for (int i = 0; i < _loopCount; ++i)
		{
			//normal
			_bw.Write(0f);
			_bw.Write(0f);
			_bw.Write(0f);
			//triangle
			_bw.Write(_vindex[_tindex[i * 3]].x);
			_bw.Write(_vindex[_tindex[i * 3]].y);
			_bw.Write(_vindex[_tindex[i * 3]].z);

			_bw.Write(_vindex[_tindex[i * 3 + 1]].x);
			_bw.Write(_vindex[_tindex[i * 3 + 1]].y);
			_bw.Write(_vindex[_tindex[i * 3 + 1]].z);

			_bw.Write(_vindex[_tindex[i * 3 + 2]].x);
			_bw.Write(_vindex[_tindex[i * 3 + 2]].y);
			_bw.Write(_vindex[_tindex[i * 3 + 2]].z);
			//Color
			//_bw.Write((ushort)Random.RandomRange(0,127));
			_bw.Write((ushort)0);

		}
		
		_bw.Close();
		_fs.Close();
	}
	public static GameObject ReadSTL(string pPath)
	{
		GameObject _result = new GameObject();
		Mesh _m = new Mesh();

		FileStream _fs = new FileStream(pPath, FileMode.Open, FileAccess.Read);
		BinaryReader _br = new BinaryReader(_fs);

		//header
		_br.ReadChars(80);
		//tcount
		uint _tCount = _br.ReadUInt32();
		
		Vector2[] _uvArray = new Vector2[_tCount * 3];
		Vector3[] _vArray = new Vector3[_tCount * 3];
		Color[] _cArray = new Color[_tCount * 3];
		int[] _tArray = new int[_tCount * 3];

		for (int i = 0; i < _tCount; ++i)
		{
			float _x = _br.ReadSingle();
			float _y = _br.ReadSingle();
			float _z = _br.ReadSingle();

			for (int j = 0; j < 3; ++j)
			{
				_x = _br.ReadSingle();
				_y = _br.ReadSingle();
				_z = _br.ReadSingle();
				_vArray[i * 3 + j] = new Vector3(_x, _y, _z);
				_tArray[i * 3 + j] = i * 3 + j;
			}
			//color
			ushort _color = _br.ReadUInt16();
			float _red = (float)(_color & 0x1f);
			_red /= 32.0f;
			float _green = (float)((_color & 0x3e0) >> 5);
			_green /= 32.0f;
			float _blue = (float)((_color & 0x7c00) >> 10);
			_blue /= 32.0f;
			for (int j = 0; j < 3; j++)
			{
				Color vtxColor = new Color(_red, _green, _blue);
				_cArray[i * 3 + j]=vtxColor;
			}
		}

		
		_m.vertices = _vArray;
		_m.triangles = _tArray;
		_m.colors = _cArray;
		_m.RecalculateNormals();

		_result.AddComponent<MeshFilter>();
		_result.GetComponent<MeshFilter>().mesh = _m;
		
		_result.AddComponent<MeshRenderer>();

		_br.Close();
		_fs.Close();
		return _result;
	}
}

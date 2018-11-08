using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {
	public AudioClip[] playList;
	public float[] soundSetting;
	public AudioSource[] speaker;

	private int index;
	// Use this for initialization
	void Start ()
	{
		index = Random.Range(0, playList.Length);
	}
	private void Update()
	{
		if(!speaker[0].isPlaying)
		{
			index = (index + 1) >= playList.Length? 0 : (index + 1);

			foreach (AudioSource a in speaker)
			{
				a.clip = playList[index];
				if (soundSetting.Length > index)
					a.volume = soundSetting[index];
				a.Play();
			}
		}
	}
}

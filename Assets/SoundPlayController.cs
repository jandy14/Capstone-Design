using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayController : MonoBehaviour {

	public AudioClip saveSound;
	[SerializeField] private AudioSource soundPlayer;

	public void PlaySound()
	{
		soundPlayer.clip = saveSound;
		soundPlayer.Play();
	}
}

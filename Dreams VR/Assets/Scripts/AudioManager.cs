using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {
	public Sound[] sounds;
	public Sound[] FootStepSounds;
	// Use this for initialization
	public static AudioManager instance;
	void Awake () {
		if (instance == null)
			instance = this;
		else {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);

		foreach(Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
		}

		foreach (Sound s in FootStepSounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
		}

	}

	private void Start()
	{
		//Play("Ambient");
		//Play ("HeartBeat");
	}

	public void Play(string name)
	{
		Sound s = Array.Find(sounds, sound => sound.name == name);
		if (s == null) {
			Debug.LogWarning ("Sound: " + name + " was not found!");
			return;
		}
		s.source.Play();
	}

	public void PlayFS(int idx)
	{
		Sound s = FootStepSounds[idx];
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " was not found!");
			return;
		}
		s.source.Play();
	}

	public void Pause(string name)
	{
		Sound s = Array.Find(sounds, sound => sound.name == name);
		s.source.Pause();
	}
}

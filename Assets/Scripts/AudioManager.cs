using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    private AudioSource source;
    public float Volum;
    public bool loop;
    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
    }
    public void SetVolumn()
    {
        source.volume = Volum;
    }
    public void Play()
    {
        source.Play();
    }
    public void Stop()
    {
        source.Stop();
    }
    public void SetLoop()
    {
        source.loop = true;
    }
    public void SetLoopCancel()
    {
        source.loop = false;
    }
}
public class AudioManager : MonoBehaviour
{
    [SerializeField]
    public Sound[] sounds;
    private void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject soundObj = new GameObject("사운드 파일 이름 : " + i + "=" + sounds[i].name);
            sounds[i].SetSource(soundObj.AddComponent<AudioSource>());
            soundObj.transform.SetParent(this.transform);
        }
    }
    public void Play(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].Play();
                return;

            }
        }
    }
    public void Stop(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].Stop();
                return;

            }
        }
    }
    public void SetLoop(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].SetLoop();
                return;

            }
        }
    }
    public void SetLoopCancel(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].SetLoopCancel();
                return;

            }
        }
    }
    public void SetVolumn(string _name, float _Volumn)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].Volum = _Volumn;
                sounds[i].SetVolumn();
                return;

            }
        }
    }



}

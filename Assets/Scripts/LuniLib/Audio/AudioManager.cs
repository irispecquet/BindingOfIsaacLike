using System.Collections.Generic;
using LuniLib.SingletonClassBase;
using UnityEngine;

namespace LuniLib.Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private List<Sound> _sounds = new List<Sound>();
        
        private Dictionary<string, Sound> _soundDictionary = new();

        protected override void InternalAwake()
        {
            foreach (Sound s in _sounds)
                AddSound(s);
        }

        public void AddSound(Sound sound)
        {
            _sounds.Add(sound);
            _soundDictionary.Add(sound.Name ?? sound.Source.name, sound);
            
            sound.Source = gameObject.AddComponent<AudioSource>();
            sound.Source.clip = sound.Clip;
            sound.Source.volume = sound.Volume;
            sound.Source.pitch = sound.Pitch;
            sound.Source.loop = sound.Loop;
        }

        public void PlaySound(string name)
        {
            Sound s = _soundDictionary[name];
            if (s == null)
            {
                Debug.LogWarning("Sound : " + name + " not found");
                return;
            }
            s.Source.Play();
        }

        public void StopSound(string name)
        {
            Sound s = _soundDictionary[name];
            if (s == null)
            {
                Debug.LogWarning("Sound : " + name + " not found in StopSound");
                return;
            }
            s.Source.Stop();
        }
    }
}
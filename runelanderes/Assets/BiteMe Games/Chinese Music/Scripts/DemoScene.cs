using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace bitemegames.chinesemusic
{
    public class DemoScene : MonoBehaviour
    {
        public Button[] songButtons; // Reference to your buttons
        public AudioClip[] songs; // Reference to your songs
        public AudioSource audioSource; // Reference to your audio source

        private void Start()
        {
            // Assign each button a method to handle its click event
            for (int i = 0; i < songButtons.Length; i++)
            {
                int index = i; // Capture the index variable
                songButtons[i].onClick.AddListener(() => PlaySong(index));
            }
        }

        private void PlaySong(int index)
        {
            // Stop any currently playing song
            audioSource.Stop();
        
            // Play the selected song
            if (index >= 0 && index < songs.Length)
            {
                audioSource.clip = songs[index];
                audioSource.Play();
            }
            else
            {
                Debug.LogError("Invalid song index: " + index);
            }
        }
    }

}

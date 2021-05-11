using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Music : MonoBehaviour
{
    //This is where the songs go
    public AudioClip Song1, Song2;
    
    //This script is for playing music
    IEnumerator Start()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = Song1;
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
        audio.clip = Song2;
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
        audio.clip = Song2;
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
        audio.clip = Song2;
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
        audio.clip = Song2;
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
        audio.clip = Song2;
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
        audio.clip = Song2;
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
        audio.clip = Song2;
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
        audio.clip = Song2;
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
        audio.clip = Song2;
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
        audio.clip = Song2;
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
    }
}

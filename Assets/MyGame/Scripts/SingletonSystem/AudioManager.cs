using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : SingletonMonoBehavior<AudioManager>
{

    public AudioSource AudioSeSource;
    public AudioSource AudioBGMSource;
    [SerializeField]  AudioClip[] _audioSeClips;
    [SerializeField]  AudioClip[] _audioBGMClips;
    public void PlaySe(SeType soundIndex)
        => AudioSeSource.PlayOneShot(_audioSeClips[(int)soundIndex]);
    public void PlaySe(int soundIndex)
        => AudioSeSource.PlayOneShot(_audioSeClips[soundIndex]);
    public void PlayBGM(BGMType soundIndex)
    {
        AudioBGMSource.clip = _audioBGMClips[(int)soundIndex];
        AudioBGMSource.Play();
    }
}
public enum SeType
{
    MouseOver = 0,
    ClickButton =  1,
    Opening = 2,
    Order = 3 ,
    SelectMaterial = 4 ,
    RotateCake = 5,
    BurnCake = 6,
    FinishCook = 7,
    Success = 8, 
    Failure = 9,
    TimeUp = 10,
    Result = 11,
}
public enum BGMType
{
    Title = 0, 
    InGame = 1, 
    Result = 2,
}

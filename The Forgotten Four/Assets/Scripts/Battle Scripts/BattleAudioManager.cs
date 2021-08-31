using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAudioManager : MonoBehaviour
{
    public AudioSource BattleEffectSource, BattleUISource;
    public AudioClip[] BattleUIClips;

    public void PlayUIMoveAudio()
    {
        BattleUISource = AudioManager.instance.ChangeAudioClip(BattleUISource, BattleUIClips[0]);
        if (!BattleUISource.isPlaying)
        {
            BattleUISource.Play();
        }

    }

    public void PlayUISelectAudio()
    {
        BattleUISource = AudioManager.instance.ChangeAudioClip(BattleUISource, BattleUIClips[1]);
        if (!BattleUISource.isPlaying)
        {
            BattleUISource.Play();
        }
    }

    public void PlayUICancelAudio()
    {
        BattleUISource = AudioManager.instance.ChangeAudioClip(BattleUISource, BattleUIClips[2]);
        if (!BattleUISource.isPlaying)
        {
            BattleUISource.Play();
        }
    }

    public void PlayMoveAudio(AudioClip effectClip)
    {
        BattleEffectSource = AudioManager.instance.ChangeAudioClip(BattleEffectSource, effectClip);
        if (!BattleEffectSource.isPlaying)
        {
            BattleEffectSource.Play();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityTemplateProjects.Sound;

public class FaderManager : MonoBehaviour {
    [SerializeField] private AudioMixerGroup mixer;

    [FormerlySerializedAs("ActiveMainLoop")] [SerializeField] private AudioSource ActiveMainLoopSource;
    [FormerlySerializedAs("ActiveTransition")] [SerializeField] private AudioSource ActiveTransitionSource;

    [SerializeField] private AudioClip menuLoop;
    [SerializeField] private AudioClip gameplayLoop;
    [SerializeField] private AudioClip endLoop;

    [SerializeField] private AudioClip menuToGameplayTransition;
    [SerializeField] private AudioClip gameplayToEndTransition;

    [SerializeField] private float transitionTime = 2f;

    private string _transitionParam = "volTransition";
    private string _mainLoopParam = "volMainLoop";

    private void Start() {
        if (menuLoop != null) {
            ActiveMainLoopSource.clip = menuLoop;
            ActiveMainLoopSource.Play();
            ActiveMainLoopSource.loop = true;
        }
    }

    public void GoToMainLoop() {
        Debug.Log("transition playing");
        StartCoroutine(
            FaderMixer.StartFade(
                mixer.audioMixer, 
                _mainLoopParam, 
                transitionTime, 
                0.0001f));
        ActiveTransitionSource.clip = menuToGameplayTransition;
        ActiveTransitionSource.Play();
        StartCoroutine(
            FaderMixer.StartFade(
                mixer.audioMixer,
                _transitionParam,
                transitionTime,
                1f));
        StartCoroutine(TimerBefore());

        
        // start coroutine pour faire jouer la main loop apres le transitiontime
    }
    
    IEnumerator TimerBefore() {
        yield return new WaitForSeconds(transitionTime);
        ActiveMainLoopSource.clip = gameplayLoop;
        ActiveMainLoopSource.Play();
    }
}

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
        FadeOutMainLoop();
        ActiveTransitionSource.clip = menuToGameplayTransition;
        ActiveTransitionSource.Play();
        FadeInTransition();
        StartCoroutine(TimerBefore());
        
        IEnumerator TimerBefore() {
        
            yield return new WaitForSeconds(transitionTime);
            FadeInMainLoop();
            FadeOutTransition();
            ActiveMainLoopSource.clip = gameplayLoop;
            ActiveMainLoopSource.Play();
        }
    }

    public void GoToEndLoop() {
        FadeOutMainLoop();
        ActiveTransitionSource.clip = gameplayToEndTransition;
        ActiveTransitionSource.Play();
        FadeInTransition();
        StartCoroutine(TimerBefore());

        IEnumerator TimerBefore() {
            yield return new WaitForSeconds(transitionTime);
            FadeInMainLoop();
            FadeOutTransition();
            ActiveMainLoopSource.clip = endLoop;
            ActiveMainLoopSource.Play();
        }
    }
    private void FadeInTransition() {
        StartCoroutine(
            FaderMixer.StartFade(
                mixer.audioMixer,
                _transitionParam,
                transitionTime,
                1f));
    }
    
    private void FadeOutTransition() {
        StartCoroutine(
            FaderMixer.StartFade(
                mixer.audioMixer,
                _transitionParam,
                transitionTime,
                0.0001f));
    }
    private void FadeOutMainLoop() {
        StartCoroutine(
            FaderMixer.StartFade(
                mixer.audioMixer,
                _mainLoopParam,
                transitionTime,
                0.0001f));
    }
    
    
    private void FadeInMainLoop() {
        StartCoroutine(
            FaderMixer.StartFade(
                mixer.audioMixer,
                _mainLoopParam,
                transitionTime,
                1f));
    }
}

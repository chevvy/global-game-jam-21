using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace UnityTemplateProjects.Sound {
    public static class FaderMixer {
        public static IEnumerator StartFade(AudioMixer audiomixer, string exposedParam, float duration, float targetVolume) {
            float currentTime = 0;
            float currentVol;
            audiomixer.GetFloat(exposedParam, out currentVol);
            currentVol = Mathf.Pow(10, currentVol / 20);
            float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
                audiomixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20);
                yield return null;
            }
            yield break;
        }
    }
}
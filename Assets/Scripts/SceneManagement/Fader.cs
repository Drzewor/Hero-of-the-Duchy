using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        private CanvasGroup canvas;
        Coroutine currentActiveFade = null;

        private void Awake() 
        {
            canvas = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate()
        {
            canvas.alpha = 1;
        }

        public IEnumerator FadeOut(float time)
        {
            yield return Fade(1, time);
        }

        public IEnumerator FadeIn(float time)
        {
            yield return Fade(0, time);
        }

        public IEnumerator Fade(float target, float time)
        {
            if(currentActiveFade != null)
            {
                StopCoroutine(currentActiveFade);
            }
            currentActiveFade = StartCoroutine(FadeRoutine(target,time));
            yield return currentActiveFade;
        }

        private IEnumerator FadeRoutine(float target,float time)
        {
            while(!Mathf.Approximately(canvas.alpha, target))
            {
                canvas.alpha = Mathf.MoveTowards(canvas.alpha, target, Time.unscaledDeltaTime / time);
                yield return null;
            }
        }
    }
}



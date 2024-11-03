using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D, E, F, G, H, I, J, K
        }
        [SerializeField] private int sceneToLoad = -1;
        [SerializeField] private Transform spawnPoint = null;
        [SerializeField] private DestinationIdentifier destination;
        [SerializeField] private float timeToFadeOut = 1f;
        [SerializeField] private float timeToWait = 0.5f;
        [SerializeField] private float timeToFadein = 2f;
        private bool isActive = false;
        private void OnTriggerEnter(Collider other) 
        {
            if(other.tag != "Player" || isActive) return;
            if(sceneToLoad == -1)
            {
                Debug.Log($"Scene is not Selected on {name}");
                return;
            }
            isActive = true;
            StartCoroutine(Transition());
        }

        private IEnumerator Transition()
        {
            Fader fader = FindObjectOfType<Fader>();
            SavingWraper wraper = FindObjectOfType<SavingWraper>();

            DontDestroyOnLoad(gameObject);

            yield return fader.FadeOut(timeToFadeOut);
            wraper.Save();
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            yield return new WaitForEndOfFrame();
            wraper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            yield return new WaitForSeconds(timeToWait);
            wraper.Save();
            
            yield return fader.FadeIn(timeToFadein);
            
            Destroy(gameObject);
        }

        private Portal GetOtherPortal()
        {
            foreach(Portal portal in FindObjectsOfType<Portal>())
            {
                if(portal == this) continue;
                if(portal.destination != destination) continue;

                return portal;
            }
            return null;
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = otherPortal.spawnPoint.position;
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            player.GetComponent<CharacterController>().enabled = true;
        }
    }
}


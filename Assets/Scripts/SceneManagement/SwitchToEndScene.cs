using System.Collections;
using System.Collections.Generic;
using RPG.SceneManagement;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SwitchToEndScene : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other) 
        {
            if(other.tag != "Player") return;
            SavingWraper savingWraper = FindObjectOfType<SavingWraper>();
            savingWraper.LoadEndScene();
        }
    }
}

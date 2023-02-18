using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

namespace Coots.Menus.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        #region Variables
        private Button playButton;
        #endregion

        #region Methods

        private void Awake()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;

            playButton = root.Q<Button>("play-button");

            playButton.clicked += PlayButtonPressed;
        }

        private void OnDestroy()
        {
            playButton.clicked -= PlayButtonPressed;
        }

        void PlayButtonPressed()
        {
            SceneManager.LoadScene("OpeningScene");
        }

        #endregion
    }
}
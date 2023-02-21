using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Coots.Code.Controllers.Cutsenes
{
    public class EndingCutscene : MonoBehaviour
    {
        #region Variables

        #endregion

        #region Unity Methods
        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Time.timeScale = 5;
            }
            else
            {
                Time.timeScale = 1;
            }
            if (Input.GetKeyDown(KeyCode.Space)){
                GoToGameScene();
            }
        }
        #endregion

        #region Methods
        public void GoToGameScene()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("GameWorld");
        }
        #endregion
    }
}
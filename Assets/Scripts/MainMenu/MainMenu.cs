using UnityEngine;
using UnityEngine.SceneManagement;

namespace SadBrains.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        public void EnterGame()
        {
            SceneManager.LoadScene("Main");
        }
    }
}
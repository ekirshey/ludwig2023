using UnityEngine;
using UnityEngine.SceneManagement;

namespace SadBrains
{
    public class GameEnd : MonoBehaviour
    {
        public void MainMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}
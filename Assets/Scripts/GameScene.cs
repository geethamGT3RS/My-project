using UnityEngine;
using UnityEngine.SceneManagement;  // <-- Important!

public class GameScene : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}

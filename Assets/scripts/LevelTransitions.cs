using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitions : MonoBehaviour
{
    public void LoadScene(int id)
    {
        SceneManager.LoadScene(id);
    }

    public void LoadScene(string Name)
    {
        SceneManager.LoadScene(Name);
    }
}

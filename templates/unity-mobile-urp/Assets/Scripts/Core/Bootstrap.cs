using UnityEngine;
using UnityEngine.SceneManagement;
public class Bootstrap : MonoBehaviour
{
    [SerializeField] private string firstScene = "Main";
    [SerializeField] private float delay = 0.05f;
    private void Start()
    {
        UnityEngine.Input.multiTouchEnabled = false;
        Invoke(nameof(LoadNext), delay);
    }
    private void LoadNext()
    {
        if (!string.IsNullOrEmpty(firstScene)) SceneManager.LoadScene(firstScene);
    }
}
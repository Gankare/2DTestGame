using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    PlayerInput input;
    InputAction changeSceneAction;

    private void Start()
    {
        input = GetComponent<PlayerInput>();
        changeSceneAction = input.actions.FindAction("OnSpacePressed");

        changeSceneAction.performed += StartGame;
    }

    private void OnDestroy()
    {
        changeSceneAction.performed -= StartGame; 
    }

    public void StartGame(InputAction.CallbackContext ctx)
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextSceneIndex);
    }
}

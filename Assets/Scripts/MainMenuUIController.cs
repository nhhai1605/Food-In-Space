using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuUIController : MonoBehaviour
{
    [SerializeField]
    private InputActionReference menuButtonAction;
    [SerializeField]
    private GameObject playerCamera;
    [SerializeField]
    private Vector3 menuOffset = new Vector3(0f, -0.1f, 0.5f);

    [SerializeField]
    private GameObject menuToggle;

    [SerializeField]
    private bool enableButton = true;

    // Start is called before the first frame update
    private void Awake()
    {
        menuButtonAction.action.started += onMenu;
    }

    private void onDestroy()
    {
        menuButtonAction.action.started -= onMenu;
    }

    private void onMenu(InputAction.CallbackContext context)
    {
        if (!enableButton)
            return;

        if (playerCamera == null)
            return;

        // Set Position
        Vector3 newPosition = playerCamera.transform.position + playerCamera.transform.forward + menuOffset;

        menuToggle.transform.position = newPosition;
        menuToggle.transform.LookAt(playerCamera.transform.position, playerCamera.transform.up);

        // Show / Hide
        menuToggle.SetActive(!menuToggle.activeSelf);
    }

    public void onReturnToMenuButton()
    {
        // If we're not already in the menu, load the scene
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            SceneManager.LoadScene(0);
        }
    }
}

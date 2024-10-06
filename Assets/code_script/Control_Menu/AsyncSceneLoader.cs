using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncSceneLoader : MonoBehaviour
{
    // for variable Progress Bar
    public Slider progressBar; // UI Slider used for showing loading progress

    // for UI elements that should be shown/hidden
    public GameObject mainUI;         // UI ??????????????
    public GameObject mainUI_2;       // UI ????????????????????
    public GameObject loadingUI;      // UI ?????????????
    public float initialDelay = 1f;   // Initial delay before showing loading UI
    public float loadingDelay = 1f;   // Delay in seconds before starting loading
    public float updateDelay = 0.5f;  // Delay for updating the progress bar (can be adjusted)

    void Start()
    {
        // Setting up the Slider
        if (progressBar != null)
        {
            progressBar.interactable = false; // Disable interaction to prevent user changes
            progressBar.minValue = 0f; // Minimum value of the slider
            progressBar.maxValue = 1f; // Maximum value of the slider
            progressBar.value = 0f; // Set initial value to 0
        }
    }

    // ??????????????????????? Scene ??? Asynchronous
    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    // Coroutine for asynchronous scene loading
    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        // Show loading UI after an initial delay
        yield return new WaitForSeconds(initialDelay); // Wait for the initial delay

        // Hide main UI
        if (mainUI != null)
        {
            mainUI.SetActive(false); // Hide main UI
        }
        if (mainUI_2 != null)
        {
            mainUI_2.SetActive(false); // Hide main UI 2
        }

        // Show loading UI
        if (loadingUI != null)
        {
            loadingUI.SetActive(true); // Show loading UI
        }

        // Delay before starting the loading process
        yield return new WaitForSeconds(loadingDelay); // Wait for the loading delay

        // Start loading the scene asynchronously
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        // Prevent the scene from activating immediately
        asyncOperation.allowSceneActivation = false;

        // Define the increments for updating the progress
        float[] progressSteps = { 0.2f, 0.4f, 0.6f, 0.8f, 1.0f }; // Steps for progress
        float currentStep = 0f; // Track the current progress step

        // Show loading progress
        while (!asyncOperation.isDone)
        {
            // Update the progress bar at defined steps
            for (int i = 0; i < progressSteps.Length; i++)
            {
                while (currentStep < progressSteps[i])
                {
                    progressBar.value += 0.2f; // Increase the progress bar value by 0.2
                    yield return new WaitForSeconds(updateDelay); // Wait for the update delay
                    currentStep += 0.2f; // Increment the current step
                }
            }

            // When loading is finished, allow scene activation
            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }

            yield return null; // Wait for the next frame
        }

        // Hide loading UI when done
        if (loadingUI != null)
        {
            loadingUI.SetActive(false); // Hide loading UI
        }
    }
}
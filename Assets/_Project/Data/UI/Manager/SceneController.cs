using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Xử lý khi chuyển scene
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Đóng các feature khi chuyển scene
        UIManager.instance.ResetDataWhenChangeScene();

        // Tạo EventSystem
        UIManager.instance.CreateEventSystemIfNeeded();

        // Mở feature chính của scene
        UIManager.instance.OpenFeatureMainScene();

        // mở các feature trong hàng đợi
        UIManager.instance.OpenFeatureInQueue();

        Time.timeScale = 1.0f;
    }

    public void ChangeScene(EnumBase.Scenes scene)
    {
        SceneManager.LoadScene(scene.ToString());
        UIManager.instance.SetCurrentScene(scene);
    }
}

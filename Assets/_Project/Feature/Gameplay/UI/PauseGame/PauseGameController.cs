using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseGameController : MonoBehaviour
{
    [SerializeField]
    protected Canvas canvas;

    [SerializeField]
    protected Button btnContinue;

    [SerializeField]
    protected Button btnExit;

    [SerializeField]
    protected EnumBase.Scenes scene;

    protected void Start()
    {
        canvas.sortingLayerName = "UI";
        canvas.sortingOrder = 201;

        btnContinue.onClick.AddListener(OnClickContinue);
        btnExit.onClick.AddListener(OnClickExit);
    }

    protected void OnClickContinue() 
    {
        LevelExpController.instance.TimeScale();
        UIManager.instance.CloseFeature(EnumBase.Feature.PauseGame);
    }

    protected void OnClickExit()
    {
        Time.timeScale = 1;
        SceneController.instance.ChangeScene(scene);
    }
}

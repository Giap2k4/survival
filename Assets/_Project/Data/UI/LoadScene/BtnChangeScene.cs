using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnChangeScene : MonoBehaviour
{
    [SerializeField]
    protected Button btn;

    [SerializeField]
    protected EnumBase.Scenes scene;

    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    protected void OnClick()
    {
        SceneController.instance.ChangeScene(scene);
    }
}

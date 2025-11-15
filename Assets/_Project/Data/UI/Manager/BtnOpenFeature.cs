using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnOpenFeature : MonoBehaviour
{
    [SerializeField]
    protected Button btn;

    [SerializeField]
    protected EnumBase.Feature feature;

    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    protected void OnClick()
    {
        UIManager.instance.OpenFeature(feature);
    }
}

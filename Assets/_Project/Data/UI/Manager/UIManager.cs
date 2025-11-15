using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    /// <summary>
    /// Các tính năng đang mở
    /// </summary>
    public Dictionary<EnumBase.Feature, GameObject> listFeatureOpen = new Dictionary<EnumBase.Feature, GameObject>();

    /// <summary>
    /// Các feature sẽ được mở khi chuyển đến scene này
    /// </summary>
    public Dictionary<EnumBase.Scenes, Queue<EnumBase.Feature>> featureSequence = new Dictionary<EnumBase.Scenes, Queue<EnumBase.Feature>>();

    public Dictionary<UIGroupName, GameObject> parentFeature = new Dictionary<UIGroupName, GameObject>();

    /// <summary>
    /// Các tính năng sẽ bị đóng khi chuyển scene
    /// </summary>
    public List<UIGroupName> deleteFeatureWhenChangeScene = new List<UIGroupName>() { UIGroupName.Main, UIGroupName.Modal};

    protected int startLayer;
    protected int currentLayer;
    protected EnumBase.Scenes currentScene;
    protected EnumBase.Feature mainFeature;

    public enum UIGroupName
    {
        Main,
        Modal
    }

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
        ResetDataWhenChangeScene();

        // Mở feature chính của scene
        OpenFeatureMainScene(mainFeature);

        // mở các feature trong hàng đợi
        OpenFeatureInQueue();
    }

    /// <summary>
    /// Đăng ký khi chuyển scene
    /// </summary>
    public void ResetDataWhenChangeScene()
    {
        currentLayer = startLayer;
        foreach (var item in deleteFeatureWhenChangeScene)
        {
            foreach (Transform trans in parentFeature[item].transform)
            {
                GameObject.Destroy(trans.gameObject);
            }
        }
    }

    public EnumBase.Scenes GetCurrentScene() => currentScene;
    public void SetCurrentScene(EnumBase.Scenes scene) { currentScene = scene; }

    public EnumBase.Feature GetMainFeature() => mainFeature;
    public void SetMainFeature(EnumBase.Feature feature) { mainFeature = feature; }


    protected override void Awake()
    {
        base.Awake();

        // Tạo các group UI
        foreach (UIGroupName obj in Enum.GetValues(typeof(UIGroupName)))
        {
            GameObject gameObject = new GameObject(obj.ToString());
            parentFeature.Add(obj, gameObject);
        }

        foreach (EnumBase.Scenes scene in Enum.GetValues(typeof(EnumBase.Scenes)))
        {
            featureSequence.Add(scene, new Queue<EnumBase.Feature>());
        }
    }

    private void Start()
    {
        startLayer = 10;
        currentLayer = startLayer;
    }

    /// <summary>
    /// Mở tính năng
    /// </summary>
    /// <param name="feature"></param>
    /// <param name="grp"></param>
    public void OpenFeature(EnumBase.Feature feature, UIGroupName grp = UIGroupName.Modal)
    {
        string namePrefab = "screen_" + feature.ToString().ToLower();
        GameObject prefab = Resources.Load<GameObject>(namePrefab);

        GameObject obj = null;

        switch (grp)
        {
            case UIGroupName.Main:
                obj = Instantiate(prefab, parentFeature[grp].transform);
                listFeatureOpen.Add(feature, obj);
                break;

            case UIGroupName.Modal:
                obj = Instantiate(prefab, parentFeature[grp].transform);
                listFeatureOpen.Add(feature, obj);
                break;
        }

        currentLayer++;
        Canvas canvas = obj.GetComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingOrder = currentLayer;
    }

    public void CloseFeature(EnumBase.Feature feature)
    {
        if (listFeatureOpen.TryGetValue(feature, out var gameObject))
        {
            Destroy(gameObject);
            listFeatureOpen.Remove(feature);
        }
    }

    /// <summary>
    /// Add các feature cần mở vào hàng đợi khi chuyển scene
    /// </summary>
    /// <param name="scene"></param>
    /// <param name=""></param>
    public void AddFeatureOpenLoadScene(EnumBase.Scenes scene, EnumBase.Feature feature)
    {
        featureSequence[scene].Enqueue(feature);
    }

    /// <summary>
    /// Mở các tính năng trong hàng đợi
    /// </summary>
    public void OpenFeatureInQueue()
    {
        var queue = featureSequence[currentScene];
        
        while (queue.Count > 0)
        {
            var feature = queue.Dequeue();
            OpenFeature(feature);
        }
    }

    /// <summary>
    /// Mở tính năng chính trong scene
    /// </summary>
    /// <param name="feature"></param>
    public void OpenFeatureMainScene(EnumBase.Feature feature)
    {
        OpenFeature(feature, UIGroupName.Main);
    }
}

using Project.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgressSceneLoader : Mediator {
    [Header("References Login")]
    [SerializeField]
    private UILabel _propressText;
    [SerializeField]
    private UISlider _slider;
    private AsyncOperation operation;
    private Canvas canvas;
    [Header("Networking")]
    private NetworkClient _networkClient;
    private void Start()
    {
        _propressText = transform.Find("Label").GetComponent<UILabel>();
        _slider = transform.Find("Slider").GetComponent<UISlider>();
        _networkClient = NetworkClient._instance;
        LoadScene(_networkClient._sceneName);
    }
    public void LoadScene(string sceneName)
    {
        UpdateProgressUI(0);
        StartCoroutine(BeginLoad(sceneName));
    }

    public override void onUpdate(string key, object para = null)
    {
        switch (key)
        {
            case "": break;
        }
    }

    private IEnumerator BeginLoad(string sceneName)
    {
        operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            UpdateProgressUI(operation.progress);
            yield return null;
        }

        UpdateProgressUI(operation.progress);
        PanelManager._instance.RecyclePanel(PanelName.LoadScene);
    }
    private void UpdateProgressUI(float progress)
    {
        _slider.value = progress;
        _propressText.text = (int)(progress * 100f) + "%";
    }

    private void OnDestroy()
    {
        operation = null;
        _propressText = null;
        _slider = null;
        _networkClient = null;
    }
}

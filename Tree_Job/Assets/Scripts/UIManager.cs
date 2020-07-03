using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class used to control UI behaviour
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField]
    Button removeChunkButton;
    [SerializeField]
    Button nextLevelButton;
    void Start()
    {
        EventBroker.OnTreeDestroyed += DisableRemoveChunkButton;
        EventBroker.OnAnimationEnded += EnableRemoveChunkButton;
        EventBroker.OnGameEnded += EnableNextLevelButton;
    }

    void DisableRemoveChunkButton()
    {
        removeChunkButton.interactable = false;
    }
    void EnableRemoveChunkButton()
    {
        removeChunkButton.interactable = true;
    }
    void EnableNextLevelButton()
    {
        EventBroker.OnTreeDestroyed -= DisableRemoveChunkButton;
        EventBroker.OnAnimationEnded -= EnableRemoveChunkButton;
        EventBroker.OnGameEnded -= EnableNextLevelButton;

        removeChunkButton.gameObject.SetActive(false);
        nextLevelButton.gameObject.SetActive(true);
    }
}

using UnityEngine;
using UnityEngine.UI;
using Core.EventChannels;

public class UIManager : MonoBehaviour
{
    [Header("Event Channels")]
    [SerializeField] private EventChannel _generateMapEventChannel;

    [Header("UI Elements")]
    [SerializeField] private Button _generateButton;


    private void Start()
    {
        if (_generateButton == null)
            return;

        _generateButton.onClick.AddListener(OnGenerateButtonClicked);
    }

    private void OnGenerateButtonClicked()
    {
        _generateMapEventChannel.Raise(null);
    }
}

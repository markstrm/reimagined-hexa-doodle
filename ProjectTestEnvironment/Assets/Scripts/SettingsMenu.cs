using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;


public class SettingsMenu : MonoBehaviour
{
    private const int _defaultQualityIndex = 3;

    [SerializeField] private AudioMixer _audioMixer;

    [SerializeField] private TMP_Dropdown _resolutionDropdown;

    [SerializeField] private TMP_Dropdown _qualityDropdown;

    public Texture2D CursorArrow; //reference to our new cursor

    Resolution[] _resolutions;

    void Start()
    {
        Cursor.SetCursor(CursorArrow, Vector2.zero, CursorMode.ForceSoftware);

        _resolutions = Screen.resolutions;

        _resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + " x " + _resolutions[i].height;
            options.Add(option);

            if(_resolutions[i].width == Screen.currentResolution.width && _resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }

        }

        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.value = currentResolutionIndex;
        _resolutionDropdown.RefreshShownValue();

        _qualityDropdown.value = _defaultQualityIndex;
        _qualityDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float volume)
    {

        _audioMixer.SetFloat("volume", volume);
        Debug.Log(volume);
    }

    public void SetQuality(int qualityIndex)
    {

        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

}

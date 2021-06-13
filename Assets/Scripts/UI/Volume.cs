using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class Volume : MonoBehaviour
{
    [Tooltip("This contains the value from the sliders and it converts to the volume.")]
    public static float SFXValue = 0.8f;
    //public static float voiceValue = 0.8f;

    [Tooltip("This contains the information for the Volume panel.")]
    public GameObject volumeMenu;

    public TextMeshProUGUI volumeText;

    // Start is called before the first frame update
    void Start()
    {

        if (!PlayerPrefs.HasKey("SFX Value"))
        {
            PlayerPrefs.SetFloat("SFX Value", 80f);

        }

        // This keeps the volume setting that the player had
        SFXValue = PlayerPrefs.GetFloat("SFX Value");

        // This invokes the SetSFX function.
        SetSFX(SFXValue);

    }



    /// <summary>
    /// This adjusts the SFX volume and slider so they will
    /// be consistant and stay the same after leaving the options panel.
    /// </summary>
    /// <param name="volume">The volume of the background music.</param>
    public void SetSFX(float volume)
    {
        // The game volume will now be the same value
        // as the volume that was just set.
        AudioListener.volume = volume;

        // Update volume.
        PlayerPrefs.SetFloat("SFX Value", volume);

        UpdateSFXLabel();
    }

    /// <summary>
    /// This function adjusts the text above the slider and makes it
    /// so the slider and its values will be the same
    /// even after changing the scene.
    /// </summary>
    void UpdateSFXLabel()
    {
        // This looks at options panel and finds the SFX text.
        Transform sfxObj = volumeMenu.transform.Find("Slider");

        // This gives volumeSlider the value of the slider in the 
        // options panel so it can be used.
        Slider sfxSlider = sfxObj.GetComponent<Slider>();

        // This sets the volumeSlider as the volume so it stays
        // the same after changing scenes.
        sfxSlider.value = AudioListener.volume;

        volumeText.text = "Volume: " + AudioListener.volume + "%";
    }
}

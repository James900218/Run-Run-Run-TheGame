using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    // audio used in the scene
    public AudioSource player_FootStep;
    public AudioSource enemy_Footstep;
    public AudioSource player_Damage;
    public AudioSource enemy_Attack;
    public AudioSource enemy_Chase;
    public AudioSource enemy_Travel;
    public AudioSource enemy_Shout;
    public AudioSource keysSFX;
    public AudioSource ambience;
    // lists for audio types
    private List<AudioSource> sfxList;
    private List<AudioSource> musicList;
    // ui sliders
    public Slider volSliderSFX;
    public Slider volSliderMusic;
    // is muted? bool
    public bool sfxMuted;
    public bool musicMuted;
    // setters
    public float sfxVol;
    public float musicVol;

    public void Awake()
    {
        sfxVol = 0.5f;
        musicVol = 0.5f;

        sfxList.Add(player_FootStep);
        sfxList.Add(enemy_Footstep);
        sfxList.Add(player_Damage);
        sfxList.Add(enemy_Attack);
        sfxList.Add(enemy_Chase);
        sfxList.Add(enemy_Travel);
        sfxList.Add(enemy_Shout);
        sfxList.Add(keysSFX);
        musicList.Add(ambience);

        foreach (AudioSource audio in sfxList)
        {
            audio.volume = sfxVol;
        }
        foreach (AudioSource audio in musicList)
        {
            audio.volume = musicVol;
        }

    }

    public void Update()
    {
        volSliderSFX.onValueChanged.AddListener((sliderValue1) => { sfxVol = sliderValue1; });
        volSliderMusic.onValueChanged.AddListener((sliderValue2) => { musicVol = sliderValue2; });

        foreach (AudioSource audio in sfxList)
        {
            audio.volume = sfxVol;
        }
        foreach (AudioSource audio in musicList)
        {
            audio.volume = musicVol;
        }
    }
}

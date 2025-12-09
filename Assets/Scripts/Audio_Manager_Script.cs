using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;
public class Audio_Manager_Script : MonoBehaviour
{
    public static Audio_Manager_Script instance;
    
    public AudioSource Base_Audio_Source;




    private void Awake()
    {
        
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject); // Destroy any duplicates
        }
    }





    public void Play_Selected_Audio(AudioClip Audio_Clip, Vector3 Audio_Source_Position, float Volume, float Pitch)
    {
        if (!Persistent_Data_Store.SFX_Disabled)
        {
            // instantiate audio source
            AudioSource My_Audio_Source = Instantiate(Base_Audio_Source, Audio_Source_Position, Quaternion.identity);

            My_Audio_Source.clip = Audio_Clip;

            //change volume
            My_Audio_Source.volume = Volume;

            //change pitch
            My_Audio_Source.pitch = Pitch;

            //play clip
            My_Audio_Source.Play();

            //Destroy after done playing
            float Clip_Length = My_Audio_Source.clip.length;
            StartCoroutine(Wait_For_Clip_Over(Clip_Length, My_Audio_Source));
        }
    }


    IEnumerator Wait_For_Clip_Over(float Clip_Length, AudioSource Source_To_Destroy)
    {
        yield return new WaitForSeconds(Clip_Length);
        if (Source_To_Destroy != null)
        {
            Destroy(Source_To_Destroy.gameObject);
        }
    }


}

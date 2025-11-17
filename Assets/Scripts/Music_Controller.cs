using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;
public class Music_Controller : MonoBehaviour
{

    public static Music_Controller Music_instance;

    public AudioSource Base_Audio_Source;




    private void Awake()
    {

        if (Music_instance == null)
        {
            Music_instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject); // Destroy any duplicates
        }
    }





    public void Play_Selected_Audio(AudioClip Audio_Clip, Vector3 Audio_Source_Position, float Volume, float Pitch, bool Loop, bool End_If_False, bool Fade_In) // end if false does nothing now but it is in case we want to use it later we wont have to go back and change every music play call
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
        if (Loop == false)
        {
            float Clip_Length = My_Audio_Source.clip.length;
            StartCoroutine(Wait_For_Clip_Over(Clip_Length, Volume, My_Audio_Source, Loop, Fade_In));
        }

        else if (Loop == true)
        {
            float Clip_Length = My_Audio_Source.clip.length;
            StartCoroutine(Wait_For_Clip_Over(Clip_Length, Volume, My_Audio_Source, Loop, Fade_In));
        }

    }



    IEnumerator Wait_For_Clip_Over(float Clip_Length, float Volume, AudioSource Source_To_Destroy, bool Loop, bool Fade_In)
    {
        if (Fade_In == true)
        {
            StartCoroutine(Fade_Music_In(Volume, Source_To_Destroy));
        }


        yield return new WaitForSeconds(Clip_Length);
        if (Source_To_Destroy != null && Loop == false)
        {
            Destroy(Source_To_Destroy.gameObject);
        }

        else if (Source_To_Destroy != null && Loop == true)  // wait for clip length then play again
        {
            for (int i = 99999; i > 0; i--)
            {
                if (Source_To_Destroy != null) // when we swap scenes audio source will be destroyed so stop this loop
                {
                    Source_To_Destroy.Play();
                    yield return new WaitForSeconds(Clip_Length);
                }
                else if (Source_To_Destroy == null)
                {
                    break;
                }
            }
        }
    }


    IEnumerator Fade_Music_In(float Volume, AudioSource Source_To_Destroy)
    {
        Source_To_Destroy.volume = 0; // reset volume to 0

        float Percent_Additive = Volume / 200;// calc how much to add every loop iteration

        float Running_Total = 0; //the ammount we will be adding to


        for (int i = 0; i < 200; i++)
        {
            Source_To_Destroy.volume = Running_Total += Percent_Additive; // i am so fucking smart gib me job plz
            yield return new WaitForSeconds(.01f);
        }

        Source_To_Destroy.volume = Volume;
    }











}

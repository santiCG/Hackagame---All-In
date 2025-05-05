using UnityEngine;

public class TriggerBoxSound : MonoBehaviour
{
    public AudioSource sound;
    public bool OneShotSound;

    private bool played;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(OneShotSound && !played)
            {
                sound.Play();
                played = true;
            }
            else if(!OneShotSound && !sound.isPlaying)
            {
                sound.Play();
            }
        }
    }
}

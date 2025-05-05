using UnityEngine;

public class TriggerBoxSound : MonoBehaviour
{
    public AudioSource sound;
    private bool played;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(!played)
            {
                sound.Play();
                played = true;
            }
        }
    }
}

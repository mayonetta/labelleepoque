using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffectControl : MonoBehaviour
{
    public ParticleSystem ExplosionVisualEffect;

    public AudioClip[] ExplosionSoundEffects;

    Rigidbody rigidbody;

    AudioSource ExplosionSoundControl;

    float Chronometre;
    float AirBombFallAcceleration;

    void Start()
    {
        ExplosionSoundControl = GetComponent<AudioSource>();

        TotalManagement.Instance.ChronoState += Chrono;

        switch (this.gameObject.name)
        {
            case "Aerial Explosion(Clone)":
                ExplosionSoundControl.PlayOneShot(ExplosionSoundEffects[0]);
                StartCoroutine(ExplosionEffectChrono());
                break;
            case "Air Bomb(Clone)":
                rigidbody = this.gameObject.GetComponent<Rigidbody>();
                ExplosionSoundControl.PlayOneShot(ExplosionSoundEffects[1]);
                StartCoroutine(AirBombChrono());
                break;
            case "Chain Explosion(Clone)":
                ExplosionSoundControl.PlayOneShot(ExplosionSoundEffects[2]);
                StartCoroutine(ExplosionEffectChrono());
                break;
        }
    }

    void Update()
    {
        if (this.gameObject.name == "Air Bomb(Clone)")
        {
            AirBombFallAcceleration += 0.04f;

            this.transform.Translate(Vector3.up * AirBombFallAcceleration * Time.deltaTime);
        }
    }

    IEnumerator AirBombChrono()
    {
        while (true)
        {
            switch (TotalManagement.Instance.PresentChronoState)
            {
                case O.One:
                    ExplosionSoundControl.UnPause();
                    break;
                case O.Nought:
                    ExplosionSoundControl.Pause();
                    break;
            }

            yield return null;
        }
    }

    IEnumerator ExplosionEffectChrono()
    {
        while (true)
        {
            switch (TotalManagement.Instance.PresentChronoState)
            {
                case O.One:
                    Chronometre += Time.deltaTime;
                    if (ExplosionVisualEffect.isPlaying != true)
                    {
                        ExplosionVisualEffect.Play();
                        ExplosionSoundControl.UnPause();
                    }
                    break;
                case O.Nought:
                    ExplosionVisualEffect.Pause();
                    ExplosionSoundControl.Pause();
                    break;
            }

            if (Chronometre >= 1.0f)
            {
                Destroy(this.gameObject);
            }

            yield return null;
        }
    }

    void OnDestroy()
    {
        TotalManagement.Instance.ChronoState -= Chrono;
    }

    void Chrono(O o)
    {
        enabled = o == O.One;
    }
}

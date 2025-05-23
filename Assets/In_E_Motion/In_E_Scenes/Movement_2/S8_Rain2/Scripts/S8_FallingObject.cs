using UnityEngine;


namespace NuitrackSDK.Tutorials.SegmentExample
{
    [AddComponentMenu("NuitrackSDK/Tutorials/Segment Example/Falling Object")]
    public class S8_FallingObject : MonoBehaviour
    {
        
        bool active = true;

        [Header("Effects Settings")]
        public ParticleSystem[] particleEffects;   // Array of different particle effects to play on collision
        public AudioClip[] soundEffects;           // Array of different sound effects to play on collision
        public GameObject explosionEffectPrefab;   // Optional prefab for an explosion or special effect
        public Color[] colorsToChange;             // Array of colors to randomly pick and apply

        private AudioSource audioSource;

        private void Start()
        {
            // Attach an audio source if not already attached
            audioSource = gameObject.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.tag == "UserPixel")
            {
             
                TriggerRandomEffect();
            }
            else if (collision.transform.tag == "BottomLine")
            {

            }

            Destroy(gameObject);
        }

        void TriggerRandomEffect()
        {
            int effectType = Random.Range(0, 4);  // We have 4 types of effects
            switch (effectType)
            {
                case 0:
                    PlayRandomParticleEffect();
                    break;
                case 1:
                    PlayRandomSoundEffect();
                    break;
                case 2:
                    ChangeColor();
                    break;
                case 3:
                    CreateExplosionEffect();
                    break;
            }
        }

        void PlayRandomParticleEffect()
        {
            if (particleEffects.Length > 0)
            {
                int index = Random.Range(0, particleEffects.Length);
                ParticleSystem effect = Instantiate(particleEffects[index], transform.position, Quaternion.identity);
                effect.Play();
                Destroy(effect.gameObject, effect.main.duration);
            }
        }

        void PlayRandomSoundEffect()
        {
            if (soundEffects.Length > 0)
            {
                int index = Random.Range(0, soundEffects.Length);
                audioSource.PlayOneShot(soundEffects[index]);
            }
        }

        void ChangeColor()
        {
            if (colorsToChange.Length > 0)
            {
                int index = Random.Range(0, colorsToChange.Length);
                Renderer renderer = GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = colorsToChange[index];
                }
            }
        }

        void CreateExplosionEffect()
        {
            if (explosionEffectPrefab != null)
            {
                Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}
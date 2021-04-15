using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class VisualEffectManager : MonoBehaviour
{

    [SerializeField] private TextMeshPro DamageTextPrefab;
    [SerializeField] private Vector3 damageParticleOffset;
    [SerializeField] private Vector3 damageParticleEndOffset;
    [SerializeField] private float damageParticleTime;
    [SerializeField] private float flashTime;
    [SerializeField] private Color dmgFlashColor;
    [SerializeField] private Canvas canvas;
    [SerializeField] private float particleTransformSmoothing;
    [SerializeField] private float particleColorSmoothing;
    [SerializeField] private Color dmgParticleColor;

    [Header ("Audio Effect Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip dmgSound;
    [SerializeField] private float dmgSoundVolume;
    [SerializeField] private AudioClip playerDmgSound;
    [SerializeField] private float playerDmgSoundVolume;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Update to death sprite with particle effect explosion?
    void PlayEnemyDeathEffect()
    {

    }

    void PlayDamgeSound()
    {
        audioSource.clip = dmgSound;
        audioSource.volume = dmgSoundVolume;
        audioSource.Play();
    }

    void PlayPlayerTakeDamageSound()
    {
        audioSource.clip = playerDmgSound;
        audioSource.volume = playerDmgSoundVolume;
        audioSource.Play();
    }

    IEnumerator PlayFlashEffect(SpriteRenderer sr, Color color)
    {
        //lerp the sprite color to hue


        yield return null;
    }

    //Damage Number Throw Out Effect
    public IEnumerator PlayEnemyTakeDamageEffect(Enemy enemy, float damage)
    {
        enemy.is_Idle = false;

        print("Starting Enemy Damage Effect.");

        //Create the particle at the enemy location with offset and set text
        TextMeshPro dmgTxt = Instantiate(DamageTextPrefab);
        dmgTxt.gameObject.transform.position = enemy.gameObject.transform.position + damageParticleOffset;
        dmgTxt.text = "" + damage.ToString();

        float currentAlpha = 1;

        StartCoroutine(PlayFlashEffect(enemy.gameObject.GetComponent<SpriteRenderer>(), dmgFlashColor));

        PlayDamgeSound();

        //lerp the position of the text particle while fading out
        while (Vector3.Distance(dmgTxt.gameObject.transform.position, enemy.gameObject.transform.position + damageParticleEndOffset) > 0.05f)
        {
            dmgTxt.gameObject.transform.position = 
                Vector3.Slerp(dmgTxt.gameObject.transform.position, enemy.gameObject.transform.position + damageParticleEndOffset, particleTransformSmoothing * Time.deltaTime);
            currentAlpha = Mathf.Lerp(currentAlpha, 0f, particleColorSmoothing * Time.deltaTime);
            dmgTxt.faceColor = new Color(dmgParticleColor.r, dmgParticleColor.g, dmgParticleColor.b, currentAlpha); //fade out text before deletion

            print("Alpha: " + currentAlpha);

            yield return null;
        }

        print("Particle Destroyed");
        Destroy(dmgTxt);

        enemy.is_Idle = true;

        yield return null;
    }

    //
    void PlayPlayerDeathEffect()
    {

    }

    public IEnumerator PlayPlayerTakeDamageEffect()
    {
        PlayPlayerTakeDamageSound();

        yield return null;
    }

    void UpdateCycleEffect()
    {

    }

    void PlayPlayerGainHealthEffect()
    {

    }

    void PlayPlayerGainArmorEffect()
    {

    }

    void PlayEnemyGainBuffEffect()
    {

    }
}

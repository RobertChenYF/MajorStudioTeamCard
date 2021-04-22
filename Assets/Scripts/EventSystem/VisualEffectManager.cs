using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class VisualEffectManager : MonoBehaviour
{
    [Header ("Enemy Damage Particle Effect")]
    [SerializeField] private TextMeshPro DamageTextPrefab;
    [SerializeField] private Vector3 damageParticleOffset;
    [SerializeField] private Vector3 damageParticleEndOffset;
    [SerializeField] private float damageParticleTime;
    [SerializeField] private float particleTransformSmoothing;
    [SerializeField] private float particleColorSmoothing;
    [SerializeField] private Color dmgParticleColor;
    [SerializeField] private Vector3 jitterOffset;
    [SerializeField] private float jitterSmoothing;

    [Header ("Flash Effect")]
    [SerializeField] private Color dmgFlashColor;
    [SerializeField] private float dmgFlashSmoothingIn;
    [SerializeField] private float dmgFlashSmoothingOut;

    [Header ("Audio Effect Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip enemyDmgSound;
    [SerializeField] private float dmgSoundVolume;
    [SerializeField] private AudioClip playerDmgSound;
    [SerializeField] private float playerDmgSoundVolume;
    [SerializeField] private AudioClip nextCycleSound;
    [SerializeField] private float nextCycleSoundVolume;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Update to death sprite with particle effect explosion?
    void PlayEnemyDeathEffect()
    {

    }

    void PlayEnemyDamageSound()
    {
        audioSource.clip = enemyDmgSound;
        audioSource.volume = dmgSoundVolume;
        audioSource.Play();
    }

    void PlayPlayerTakeDamageSound()
    {
        audioSource.clip = playerDmgSound;
        audioSource.volume = playerDmgSoundVolume;
        audioSource.Play();
    }

    IEnumerator PlayFlashEffect(SpriteRenderer sr, Color color, float smoothIn, float smoothOut)
    {
        Color temp = sr.color;

        //print("Beginning Enemy Damage Flash");

        //lerp the sprite color to hue
        while (Mathf.Abs(color.r - sr.color.r) > 0.05f || Mathf.Abs(color.g - sr.color.g) > 0.05f || Mathf.Abs(color.b - sr.color.b) > 0.05f) //use red value for tracking change
        {
            sr.color = Color.Lerp(sr.color, color, smoothIn * Time.deltaTime);
            //print("Current Color: " + sr.color.ToString());

            yield return null;
        }

        //lerp back
        while (Mathf.Abs(color.r - sr.color.r) > 0.05f || Mathf.Abs(color.g - sr.color.g) > 0.05f || Mathf.Abs(color.b - sr.color.b) > 0.05f) //use red value for tracking change
        {
            sr.color = Color.Lerp(sr.color, temp, smoothOut * Time.deltaTime);

            yield return null;
        }

        sr.color = temp;

        yield return null;
    }

    IEnumerator PlayEnemyHitJitter(Enemy enemy)
    {

        Vector3 tempPos = enemy.gameObject.transform.position;

        while (Vector3.Distance(enemy.gameObject.transform.position, tempPos + jitterOffset) > 0.05f)
        {
            enemy.gameObject.transform.position =
                Vector3.Slerp(enemy.gameObject.transform.position, tempPos + jitterOffset, jitterSmoothing * Time.deltaTime);

            //print("POS: " + enemy.gameObject.transform.position.ToString());

            yield return null;
        }

        enemy.gameObject.transform.position = tempPos;

            yield return null;
    }

    public void PlayEnemyTakeDamageEffect(Enemy enemy, float damage)
    {
        StartCoroutine(EnemyTakeDamageEffect(enemy, damage));
    }

    //Damage Number Throw Out Effect
    IEnumerator EnemyTakeDamageEffect(Enemy enemy, float damage)
    {
        enemy.is_Idle = false;

        //print("Starting Enemy Damage Effect.");

        //Create the particle at the enemy location with offset and set text
        TextMeshPro dmgTxt = Instantiate(DamageTextPrefab);
        dmgTxt.gameObject.transform.position = enemy.gameObject.transform.position + damageParticleOffset;
        dmgTxt.text = "" + damage.ToString();
        float currentAlpha = 1;

        //Play Flash, jitter, and Sound EFfect
        StartCoroutine(PlayFlashEffect(enemy.gameObject.GetComponent<SpriteRenderer>(), dmgFlashColor, dmgFlashSmoothingIn, dmgFlashSmoothingOut));
        StartCoroutine(PlayEnemyHitJitter(enemy));
        PlayEnemyDamageSound();

        //lerp the position of the text particle while fading out
        while (Vector3.Distance(dmgTxt.gameObject.transform.position, enemy.gameObject.transform.position + damageParticleEndOffset) > 0.05f)
        {
            dmgTxt.gameObject.transform.position = //Slerp position
                Vector3.Slerp(dmgTxt.gameObject.transform.position, enemy.gameObject.transform.position + damageParticleEndOffset, particleTransformSmoothing * Time.deltaTime);

            currentAlpha = Mathf.Lerp(currentAlpha, 0f, particleColorSmoothing * Time.deltaTime);
            dmgTxt.faceColor = new Color(dmgParticleColor.r, dmgParticleColor.g, dmgParticleColor.b, currentAlpha); //fade out text

            yield return null;
        }

        //print("Particle Destroyed");
        Destroy(dmgTxt.gameObject);

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

    void PlayNextCycleSound()
    {
        audioSource.clip = nextCycleSound;
        audioSource.volume = nextCycleSoundVolume;
        audioSource.Play();
    }

    public void PlayUpdateCycleEffect()
    {
        StartCoroutine(UpdateCycleEffect());
    }

    IEnumerator UpdateCycleEffect()
    {
        //screen flash & woosh ?

        PlayNextCycleSound();

        yield return null;
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

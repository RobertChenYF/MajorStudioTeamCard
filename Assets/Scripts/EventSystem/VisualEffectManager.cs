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
    [SerializeField] private AudioClip errorSound;
    [SerializeField] private float errorSoundVolume;

    [Header("Error Message Pop Up")]
    [SerializeField] private TextMeshPro ErrorMsgTextPrefab;
    [SerializeField] private float errorTxtColorSmoothing;
    [SerializeField] private float popUpTime;

    [Header("Screen Shake Effect")]
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakeMagnitude;
    [SerializeField] private float dampingSpeed;
    private Vector3 initCamPos;
    private Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        audioSource = this.gameObject.GetComponent<AudioSource>();
        initCamPos = mainCam.gameObject.transform.position;
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

    void PlayErrorSound()
    {
        audioSource.clip = errorSound;
        audioSource.volume = errorSoundVolume;
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
        foreach (Enemy e in Services.combatManager.AllMainEnemy)
        {
            e.is_Idle = false;
        }

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

        foreach (Enemy e in Services.combatManager.AllMainEnemy)
        {
            e.is_Idle = true;
        }

        yield return null;
    }

    //
    void PlayPlayerDeathEffect()
    {

    }

    public void PlayPlayerTakeDamageEffect(float dmg)
    {
        StartCoroutine(PlayerTakeDamageEffect(dmg));
    }

    IEnumerator PlayerTakeDamageEffect(float dmg)
    {
        PlayPlayerTakeDamageSound();

        //screen shake to a degree based off the damage number
        StartCoroutine(PlayCameraShake(Remap(dmg, 0, 10, 0, 0.75f), Remap(dmg, 0, 10, 0, 0.4f), dampingSpeed));

        yield return null;
    }

    public static float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    IEnumerator PlayCameraShake(float magnitude, float duration, float damping)
    {
        float currentTime = Time.time;
        float endTime = Time.time + duration;

        while (currentTime < endTime)
        {
            mainCam.transform.localPosition = initCamPos + Random.insideUnitSphere * magnitude;
            currentTime += Time.deltaTime * damping;

            yield return null;
        }

        mainCam.transform.position = initCamPos;

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

    public void PlayErrorPopUp(string error)
    {
        StartCoroutine(ErrorPopUp(error));
    }

    IEnumerator ErrorPopUp(string error)
    {
        PlayErrorSound();

        TextMeshPro errorTxt = Instantiate(ErrorMsgTextPrefab);
        errorTxt.text = error;
        float currentAlpha = 1;

        yield return new WaitForSeconds(popUpTime);

        while (errorTxt.color.a > 0.05f)
        {
            currentAlpha = Mathf.Lerp(currentAlpha, 0f, errorTxtColorSmoothing * Time.deltaTime);
            errorTxt.faceColor = new Color(errorTxt.color.r, errorTxt.color.g, errorTxt.color.b, currentAlpha);

            yield return null;
        }

        Destroy(errorTxt);

        yield return null;
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
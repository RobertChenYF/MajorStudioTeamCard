using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class VisualEffectManager : MonoBehaviour
{
    [HideInInspector] public bool debug;

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

    [Header ("Damage Flash Effect")]
    [SerializeField] private Color dmgFlashColor;
    [SerializeField] private float dmgFlashSmoothingIn;
    [SerializeField] private float dmgFlashSmoothingOut;

    [Header("Enemy Deal Damage Jump")]
    [SerializeField] private float jumpPeak;
    [SerializeField] private float jumpBottom;
    [SerializeField] private float jumpDist;
    [SerializeField] private Vector3 finalScale;
    [SerializeField] private float zRotation;
    [SerializeField] private float jumpSmoothingx;
    [SerializeField] private float jumpSmoothingUp;
    [SerializeField] private float jumpSmoothingDown;
    [SerializeField] private float jumpSmoothingS;
    [SerializeField] private float jumpSmoothingR;
    [SerializeField] private float jumpWaitTime;
    [SerializeField] private float jumpHoldWaitTime;
    [SerializeField] private float jumpReturnSmoothing;
    bool is_jumping;
    bool deal_dmgflag;

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
    [SerializeField] private AudioClip buffSound;
    [SerializeField] private float buffSoundVolume;
    [SerializeField] private AudioClip decompressSound;
    [SerializeField] private float decompressSoundVolume;
    [SerializeField] private AudioClip redrawSound;
    [SerializeField] private float redrawSoundVolume;
    [SerializeField] private AudioClip armorSound;
    [SerializeField] private float armorSoundVolume;

    [Header("Enemy Buff Effect")]
    [SerializeField] private ParticleSystem buffParticles;
    [SerializeField] private Color buffFlashColor;
    [SerializeField] private float buffFlashSmoothingIn;
    [SerializeField] private float buffFlashSmoothingOut;

    [Header("Gain Armor Effect")]
    [SerializeField] private ParticleSystem armorParticles;
    [SerializeField] private ParticleSystem armorParticlesSmall;
    [SerializeField] private Color armorFlashColor;
    [SerializeField] private float armorFlashSmoothingIn;
    [SerializeField] private float armorFlashSmoothingOut;
    [SerializeField] private Vector3 armorNumParticleOffset;
    [SerializeField] private Vector3 armorNumParticleEndOffset;
    [SerializeField] private Color armorNumParticleColor;

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

    [Header("Health Bar Flash Effect")]
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject healthBarFill;
    [SerializeField] private GameObject healthBarText;
    [SerializeField] private float hpFlashSmoothingIn;
    [SerializeField] private float hpFlashSmoothingOut;
    [SerializeField] private Vector3 hpParticleOffset;
    [SerializeField] private Vector3 hpParticleEndOffset;
    [SerializeField] private Color hpParticleColor;

    [Header ("Player Gain Health EFfect")]
    [SerializeField] private Color gainhpParticleColor;

    private bool colorIsSpinning;
    private bool animIsSpinning;
    private bool clipIsPlaying;

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
        if (Input.GetKey(KeyCode.LeftShift))
        {
            debug = true;
        }
        else
        {
            debug = false;
        }
    }

    //Update to death sprite with particle effect explosion?
    void PlayEnemyDeathEffect()
    {

    }

    void PlayEnemyDamageSound()
    {
        PlayClip(enemyDmgSound, dmgSoundVolume);
    }

    void PlayErrorSound()
    {
        PlayClip(errorSound, errorSoundVolume);
    }

    void PlayPlayerTakeDamageSound()
    {
        PlayClip(playerDmgSound, playerDmgSoundVolume);
    }

    IEnumerator PlayFlashEffect(SpriteRenderer sr, Color color, float smoothIn, float smoothOut)
    {
        while (colorIsSpinning)
        {
            yield return null;
        }

        colorIsSpinning = true;

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

        colorIsSpinning = false;
        yield return null;
    }

    IEnumerator PlayFlashEffect(Image image, Color color, float smoothIn, float smoothOut)
    {
        while (colorIsSpinning)
        {
            yield return null;
        }

        colorIsSpinning = true;

        Color temp = image.color;

        //print("Beginning Enemy Damage Flash");

        //lerp the sprite color to hue
        while (Mathf.Abs(color.r - image.color.r) > 0.05f || Mathf.Abs(color.g - image.color.g) > 0.05f || Mathf.Abs(color.b - image.color.b) > 0.05f) //use red value for tracking change
        {
            image.color = Color.Lerp(image.color, color, smoothIn * Time.deltaTime);
            //print("Current Color: " + sr.color.ToString());

            yield return null;
        }

        //lerp back
        while (Mathf.Abs(color.r - image.color.r) > 0.05f || Mathf.Abs(color.g - image.color.g) > 0.05f || Mathf.Abs(color.b - image.color.b) > 0.05f) //use red value for tracking change
        {
            image.color = Color.Lerp(image.color, temp, smoothOut * Time.deltaTime);

            yield return null;
        }

        image.color = temp;

        colorIsSpinning = false;
        yield return null;
    }

    IEnumerator PlayFlashEffect(Enemy enemy, Color color, float smoothIn, float smoothOut)
    {
        while (colorIsSpinning)
        {
            yield return null;
        }

        colorIsSpinning = true;

        SpriteRenderer sr = enemy.gameObject.GetComponent<SpriteRenderer>();
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
            sr.color = Color.Lerp(sr.color, enemy.savedColor, smoothOut * Time.deltaTime);

            yield return null;
        }

        sr.color = enemy.savedColor;

        colorIsSpinning = false;
        yield return null;
    }

    IEnumerator PlayEnemyHitJitter(Enemy enemy)
    {
        while (animIsSpinning)
        {
            yield return null;
        }

        animIsSpinning = true;

        foreach (Enemy e in Services.combatManager.AllMainEnemy)
        {
            e.is_Idle = false;
        }

        Vector3 tempPos = enemy.gameObject.transform.position;

        while (Vector3.Distance(enemy.gameObject.transform.position, tempPos + jitterOffset) > 0.05f)
        {
            enemy.gameObject.transform.position =
                Vector3.Slerp(enemy.gameObject.transform.position, tempPos + jitterOffset, jitterSmoothing * Time.deltaTime);

            yield return null;
        }

        enemy.gameObject.transform.position = tempPos;

        foreach (Enemy e in Services.combatManager.AllMainEnemy)
        {
            e.is_Idle = true;
        }

        animIsSpinning = false;

        yield return null;
    }

    IEnumerator EnemyDealDamageJump(Enemy enemy)
    {
        is_jumping = true;

        while (Mathf.Abs(enemy.gameObject.transform.position.y - (enemy.savedEnemyPos.y + jumpPeak)) > 0.05f)
        {
            enemy.gameObject.transform.position =
                new Vector3(enemy.gameObject.transform.position.x,
                Mathf.Lerp(enemy.gameObject.transform.position.y, enemy.savedEnemyPos.y + jumpPeak, jumpSmoothingUp * Time.deltaTime), 0);

            yield return null;
        }

        yield return new WaitForSeconds(jumpHoldWaitTime);

        while (Mathf.Abs(enemy.gameObject.transform.position.y - (enemy.savedEnemyPos.y + jumpBottom)) > 0.05f)
        {
            enemy.gameObject.transform.position =
                new Vector3(enemy.gameObject.transform.position.x,
                Mathf.Lerp(enemy.gameObject.transform.position.y, enemy.savedEnemyPos.y + jumpBottom, jumpSmoothingDown * Time.deltaTime), 0);

            yield return null;
        }

        deal_dmgflag = true;
        is_jumping = false;
        PlayEnemyDamageSound();
    }

    public void PlayEnemyDealDamageEffect(Enemy enemy)
    {
        StartCoroutine(EnemyDealDamageEffect(enemy));
    }

    IEnumerator EnemyDealDamageEffect(Enemy enemy)
    {

        while (animIsSpinning)
        {
            yield return null;
        }

        animIsSpinning = true;

        foreach (Enemy e in Services.combatManager.AllMainEnemy)
        {
            e.is_Idle = false;
        }

        Vector3 tempScale = enemy.gameObject.transform.localScale;
        Quaternion tempRotation = enemy.gameObject.transform.rotation;
        Vector3 savedPos = enemy.gameObject.transform.position;

        //Leap the y
        StartCoroutine(EnemyDealDamageJump(enemy));

        //lerp the x, scale, and rotation
        while (Mathf.Abs(enemy.gameObject.transform.position.x - (enemy.savedEnemyPos.x + jumpDist)) > 0.05f)
        {
            //print("" + Mathf.Abs(enemy.gameObject.transform.position.x - (enemy.savedEnemyPos.x + jumpDist)).ToString());
            enemy.gameObject.transform.position =
                new Vector3(Mathf.Lerp(enemy.gameObject.transform.position.x, enemy.savedEnemyPos.x + jumpDist, jumpSmoothingx * Time.deltaTime),
                enemy.gameObject.transform.position.y, 0);

            enemy.gameObject.transform.localScale = Vector3.Lerp(enemy.gameObject.transform.localScale, finalScale, jumpSmoothingS * Time.deltaTime);

            Quaternion newRotation = new Quaternion();
            newRotation.eulerAngles = new Vector3(enemy.gameObject.transform.rotation.eulerAngles.x, enemy.gameObject.transform.rotation.eulerAngles.y,
                Mathf.Lerp(enemy.gameObject.transform.rotation.eulerAngles.z, zRotation, jumpSmoothingR * Time.deltaTime));
            enemy.gameObject.transform.rotation = newRotation;

            yield return null;
        }

        //idle for a time

        while (is_jumping) //Wait for the other coroutine to finish
        {
            yield return null;
        }

        //leap back into place and lerp scale to normal
        while (Vector3.Distance(enemy.gameObject.transform.position, savedPos) > 0.05f)
        {
            enemy.gameObject.transform.position =
                Vector3.Slerp(enemy.gameObject.transform.position, savedPos, jumpReturnSmoothing * Time.deltaTime);

            enemy.gameObject.transform.localScale = Vector3.Lerp(enemy.gameObject.transform.localScale, tempScale, jumpReturnSmoothing * Time.deltaTime);

            enemy.gameObject.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, tempRotation, jumpReturnSmoothing * Time.deltaTime);

            yield return null;
        }
        enemy.gameObject.transform.position = savedPos;
        enemy.gameObject.transform.localScale = tempScale;
        enemy.gameObject.transform.rotation = tempRotation;

        foreach (Enemy e in Services.combatManager.AllMainEnemy)
        {
            e.is_Idle = true;
        }

        animIsSpinning = false;

        yield return null;
    }

    public static float EaseInCubic(float start, float end, float value)
    {
        end -= start;
        return end * value * value * value + start;
    }

    public static float EaseInCirc(float start, float end, float value)
    {
        end -= start;
        return -end * (Mathf.Sqrt(1 - value * value) - 1) + start;
    }

    public static float EaseInExpo(float start, float end, float value)
    {
        end -= start;
        return end * Mathf.Pow(2, 10 * (value - 1)) + start;
    }
    public static float EaseOutExpo(float start, float end, float value)
    {
        end -= start;
        return end * (-Mathf.Pow(2, -10 * value) + 1) + start;
    }

    public static Vector3 EaseInExpo(Vector3 start, Vector3 end, float value)
    {
        return new Vector3(EaseInExpo(start.x, end.x, value), EaseInExpo(start.y, end.y, value), EaseInExpo(start.z, end.z, value));
    }

    public void PlayEnemyTakeDamageEffect(Enemy enemy, float damage)
    {
        StartCoroutine(PlayFlashEffect(enemy, dmgFlashColor, dmgFlashSmoothingIn, dmgFlashSmoothingOut));
        StartCoroutine(PlayEnemyHitJitter(enemy));
        StartCoroutine(NumberFlyOut(enemy.gameObject.transform.position + damageParticleOffset, damageParticleEndOffset,
            dmgParticleColor, damage.ToString()));

        PlayEnemyDamageSound();
    }

    public void PlayDecompresSound()
    {
        PlayClip(decompressSound, decompressSoundVolume);
    }

    void PlayClip(AudioClip clip, float volume)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
    }

    public void PlayRedrawSound()
    {
        PlayClip(redrawSound, redrawSoundVolume);
    }

    public void PlayEnemyGainHealthEffect(Enemy enemy, float hp)
    {
        StartCoroutine(NumberFlyOut(enemy.gameObject.transform.position + damageParticleOffset, damageParticleEndOffset,
            gainhpParticleColor, "+" + hp.ToString()));
        PlayBuffSound();
    }

    IEnumerator NumberFlyOut(Vector3 startpos, Vector3 endOffset, Color color, string text)
    {

        TextMeshPro Txt = Instantiate(DamageTextPrefab);
        Txt.gameObject.transform.position = startpos;
        Txt.gameObject.transform.position += new Vector3(0, 0, 3f);
        Txt.text = text;
        float currentAlpha = 1;

        while (Vector3.Distance(Txt.gameObject.transform.position, startpos + endOffset) > 0.05f)
        {
            Txt.gameObject.transform.position = //Slerp position
                Vector3.Slerp(Txt.gameObject.transform.position, startpos + endOffset, particleTransformSmoothing * Time.deltaTime);

            currentAlpha = Mathf.Lerp(currentAlpha, 0f, particleColorSmoothing * Time.deltaTime);
            Txt.faceColor = new Color(color.r, color.g, color.b, currentAlpha); //fade out text

            yield return null;
        }

        Destroy(Txt.gameObject);

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
        while (!deal_dmgflag) //wait to play
        {
            yield return null;
        }

        deal_dmgflag = false;

        PlayPlayerTakeDamageSound();

        //screen shake to a degree based off the damage number
        StartCoroutine(PlayCameraShake(Remap(dmg, 0, 10, 0, 0.75f), Remap(dmg, 0, 10, 0, 0.4f), dampingSpeed));

        //flash health bar red
        StartCoroutine(PlayFlashEffect(healthBar.GetComponent<Image>(), dmgFlashColor, hpFlashSmoothingIn, hpFlashSmoothingOut));
        StartCoroutine(PlayFlashEffect(healthBarFill.GetComponent<Image>(), dmgFlashColor, hpFlashSmoothingIn, hpFlashSmoothingOut));

        //Number Fly Out
        StartCoroutine(NumberFlyOut(healthBar.transform.position + hpParticleOffset, hpParticleEndOffset, hpParticleColor, "-" + dmg.ToString()));

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
        PlayClip(nextCycleSound, nextCycleSoundVolume);
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

    public void PlayPlayerGainHealthEffect(float hp)
    {
        StartCoroutine(NumberFlyOut(healthBar.transform.position + hpParticleOffset, hpParticleEndOffset, gainhpParticleColor, "+" + hp.ToString()));
    }

    public void EnemyGainArmorEffect(GameObject gameObject)
    {
        StartCoroutine(PlayEnemyGainArmorEffect(gameObject));
    }

    public void PlayPlayerLoseArmorEffect(float value)
    {
        StartCoroutine(NumberFlyOut(healthBar.gameObject.transform.position + armorNumParticleOffset,
            healthBar.gameObject.transform.position + armorNumParticleEndOffset, armorNumParticleColor, "-" + value.ToString()));
    }

    void PlayArmorSound()
    {
        PlayClip(armorSound, armorSoundVolume);
    }

    IEnumerator PlayEnemyGainArmorEffect(GameObject gameObject)
    {
        ParticleSystem particles = Instantiate(armorParticles);
        particles.transform.position = gameObject.transform.position;
        StartCoroutine(PlayFlashEffect(gameObject.GetComponent<SpriteRenderer>(), armorFlashColor, armorFlashSmoothingIn, armorFlashSmoothingOut));
        PlayArmorSound();

        yield return new WaitForSeconds(armorParticles.main.duration);
        Destroy(particles);
        yield return null;
    }

    public void PlayBuffSound()
    {
        PlayClip(buffSound, buffSoundVolume);
    }

    public void PlayPlayerGainArmorEffect(float value)
    {
        StartCoroutine(PlayerGainArmorEffect(value));
    }

    IEnumerator PlayerGainArmorEffect(float value)
    {
        ParticleSystem particles = Instantiate(armorParticlesSmall);
        particles.transform.position = healthBar.gameObject.transform.position;
        StartCoroutine(NumberFlyOut(healthBar.gameObject.transform.position + armorNumParticleOffset, 
            healthBar.gameObject.transform.position + armorNumParticleEndOffset, armorNumParticleColor, "+" + value.ToString()));
        PlayArmorSound();

        yield return new WaitForSeconds(armorParticlesSmall.main.duration);
        Destroy(particles);
        yield return null;
    }

    public void EnemyGainBuffEffect(GameObject gameObject)
    {
        StartCoroutine(PlayEnemyGainBuffEffect(gameObject));
    }

    IEnumerator PlayEnemyGainBuffEffect(GameObject gameObject)
    {
        ParticleSystem particles = Instantiate(buffParticles);
        particles.transform.position = gameObject.transform.position;
        StartCoroutine(PlayFlashEffect(gameObject.GetComponent<SpriteRenderer>(), buffFlashColor, buffFlashSmoothingIn, buffFlashSmoothingOut));
        PlayBuffSound();
        
        yield return new WaitForSeconds(buffParticles.main.duration);
        Destroy(particles);
        yield return null;
    }
}

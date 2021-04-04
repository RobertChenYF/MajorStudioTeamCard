using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class VisualEffectManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI DamageTextPrefab;
    [SerializeField] private Vector3 damageParticleOffset;
    [SerializeField] private Vector3 damageParticleEndOffset;
    [SerializeField] private float damageParticleTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Update to death sprite with particle effect explosion?
    void PlayEnemyDeathEffect()
    {

    }

    //Damage Number Throw Out Effect
    public IEnumerator PlayEnemyTakeDamageEffect(Transform enemyTransform, float damage)
    {
        print("Starting Enemy Damage Effect.");
        TextMeshProUGUI dmgTxt = Instantiate(DamageTextPrefab);
        dmgTxt.text = "" + damage.ToString();
        float currentAlpha = 255;

        while (Vector3.Distance(dmgTxt.transform.position, damageParticleEndOffset) > 0.05f)
        {
            dmgTxt.transform.position = Vector3.Slerp(dmgTxt.transform.position, enemyTransform.position + damageParticleEndOffset, 1f * Time.deltaTime);
            dmgTxt.color = new Color32(255, 255, 255, (byte) Mathf.Lerp(currentAlpha, 0f, 1f * Time.deltaTime));
        }

        Destroy(dmgTxt);

        yield return null;
    }

    //
    void PlayPlayerDeathEffect()
    {

    }

    void PlayPlayerTakeDamageEffect()
    {

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandartEnemy : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected Collider mainCol;
    [SerializeField] protected ParticleSystem particle;
    [SerializeField] protected SkinnedMeshRenderer mesh;
    [SerializeField] protected UpgradeType enemyType;

    [Header("Properties")]
    [SerializeField] protected int enemyPower = 1;
    [SerializeField] protected int money = 100;

    [Header("Animation")]
    [SerializeField] protected SimpleAnimancer animancer;
    [SerializeField] protected AnimationClip idle;

    private Collider[] ragDollColliders;
    private Rigidbody[] limbsRigidbodies;

    private MaterialPropertyBlock matBlock;
    private ObjectPooler pooler;
    private GameManager gameManager;

    void Start()
    {
        pooler = ObjectPooler.Instance;
        gameManager = GameManager.Instance;
        matBlock = new MaterialPropertyBlock();
        mesh.SetPropertyBlock(matBlock);
        CloseRagdoll();
        animancer.PlayAnimation(idle);
    }
    public void GetRagdollBits()
    {
        ragDollColliders = gameObject.GetComponentsInChildren<Collider>();
        limbsRigidbodies = gameObject.GetComponentsInChildren<Rigidbody>();
    }

    public void OnHit(Transform target, out UpgradeType upgradeType, out int power)
    {
        upgradeType = enemyType;
        power = enemyPower;

        matBlock.SetColor("_Color", Color.gray);
        mesh.SetPropertyBlock(matBlock);

        particle.Play();
        animancer.enabled = false;
        OpenRagdoll(target);
        PlayTexts(target);
        gameManager.Money = money;
        GameManager.Haptic(0);
        StartCoroutine(CloseEnemyBody(4f));
    }

    private void PlayTexts(Transform pos)
    {
        var moneyText = pooler.GetPooledText();
        moneyText.gameObject.SetActive(true);
        moneyText.SetTheText(money, TextType.Money, Color.green, null, transform.position + Vector3.forward + Vector3.up);

        var upgradeText = pooler.GetPooledText();
        upgradeText.gameObject.SetActive(true);
        upgradeText.SetTheText(enemyPower, TextType.Size, Color.red, pos, pos.position);
    }

    public void CloseRagdoll()
    {
        if (ragDollColliders == null) GetRagdollBits();

        foreach (Collider col in ragDollColliders)
        {
            if (!col == mainCol) col.enabled = false;
        }
        foreach (Rigidbody rigid in limbsRigidbodies)
        {
            rigid.isKinematic = true;
        }
    }

    public void OpenRagdoll(Transform player)
    {
        if (ragDollColliders == null) GetRagdollBits();

        foreach (Collider col in ragDollColliders)
        {
            col.enabled = true;
        }
        foreach (Rigidbody rigid in limbsRigidbodies)
        {
            rigid.isKinematic = false;
            rigid.AddForce(player.forward * 10f, ForceMode.VelocityChange);
            rigid.AddForce(transform.up * 3.5f, ForceMode.VelocityChange);
            rigid.AddForce(transform.right * Random.Range(-13.7f, 13.7f), ForceMode.VelocityChange);
        }
    }

    private IEnumerator CloseEnemyBody(float duration)
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }
}

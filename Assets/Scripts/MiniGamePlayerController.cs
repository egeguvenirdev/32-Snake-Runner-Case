using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGamePlayerController : MonoBehaviour
{
    [Header("Fight Settings")]
    [SerializeField] private float hitCooldown = 2f;
    
    private Rigidbody rb;
    private Collider col;
    private bool didMiniGameStart;
    private bool canHit = true;
    private float characterPower;
    private float characterSize;

    private MiniGame miniGame;
    private BossBase boss;

    public void Init(float size, float power)
    {
        characterSize = size;
        characterPower = power;
        if(rb == null) gameObject.AddComponent<Rigidbody>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        rb.useGravity = false;
        miniGame = FindObjectOfType<MiniGame>();
        boss = miniGame.GetBoss;
        ActionManager.ManagerUpdate += OnManagerUpdate;
    }

    public void DeInit()
    {
        col.enabled = false;
        canHit = false;
        StopAllCoroutines();
        StopDragging();
        ActionManager.ManagerUpdate -= OnManagerUpdate;
    }

    private void OnManagerUpdate(float deltaTime)
    {
        if (Input.GetMouseButtonDown(0) && canHit)
        {
            StartCoroutine(HitCoroutine());
        }
    }

    private IEnumerator HitCoroutine()
    {
        col.enabled = true;
        canHit = false;
        rb.AddForce(Vector3.left * characterPower / 25, ForceMode.VelocityChange);
        yield return new WaitForSeconds(hitCooldown);
        StopDragging();
        canHit = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.TryGetComponent(out BossBase enemy))
        {
            GameManager.Haptic(0);
            float enemyPower = enemy.TakeHit(characterPower);
            TakeHit(enemyPower);
            
        }

        if (other.CompareTag("Cliff"))
        {
            rb.useGravity = true;
            ActionManager.GameEnd?.Invoke(false);
        }
    }

    public void TakeHit(float enemyPower)
    {
        col.enabled = false;
        StopDragging();
        col.enabled = true;
        if (characterSize < 1) characterSize = 1f;
        rb.AddForce(Vector3.right * enemyPower / characterSize / 50, ForceMode.VelocityChange);
    }

    private void StopDragging()
    {
        col.enabled = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}

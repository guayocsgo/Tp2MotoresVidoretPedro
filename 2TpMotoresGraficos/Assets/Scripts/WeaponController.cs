

using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public float FireRange = 200f;
    public LayerMask hittableLayers;
    public Transform cameraPlayerTransform;
    public GameObject bulletHolePrefab;
    public float recoilForce = 5f;
    public Transform weaponMuzzle;
    public GameObject flashEffect;
    public float fireRate = 0.6f;
    public int maxAmmo = 8;
    private float lastTimeShoot = Mathf.NegativeInfinity;
    public float reloadTime = 1.5f;
    public int currentAmmo { get; private set; }

    private void Awake()
    {
        currentAmmo = maxAmmo;
        EventManager.current.updateBulletsEvent.Invoke(currentAmmo, maxAmmo);
    }

    private void Start()
    {
        cameraPlayerTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryShoot();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * 5f);
    }

    private bool TryShoot()
    {
        if (lastTimeShoot + fireRate < Time.time)
        {
            if (currentAmmo >= 1)
            {
                HandleShoot();
                currentAmmo -= 1;
                EventManager.current.updateBulletsEvent.Invoke(currentAmmo, maxAmmo);
                return true;
            }
        }
        return false;
    }

    private void HandleShoot()
    {
        GameObject flashClone = Instantiate(flashEffect, weaponMuzzle.position, Quaternion.Euler(weaponMuzzle.forward), transform);
        Destroy(flashClone, 0.5f);

        AddRecoil();

        RaycastHit hit;
        if (Physics.Raycast(cameraPlayerTransform.position, cameraPlayerTransform.forward, out hit, FireRange, hittableLayers))
        {
            GameObject bulletHoleClone = Instantiate(bulletHolePrefab, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));
            Destroy(bulletHoleClone, 5f);

            
            EnemyLife enemy = hit.collider.GetComponent<EnemyLife>();
            if (enemy != null)
            {
                enemy.TakeDamage(10); 
            }
        }

        lastTimeShoot = Time.time;
    }

    private void AddRecoil()
    {
        transform.Rotate(-recoilForce, 0f, 0f);
        transform.position = transform.position - transform.forward * (recoilForce / 50f);
    }

    IEnumerator Reload()
    {
        Debug.Log("Recargando...");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        EventManager.current.updateBulletsEvent.Invoke(currentAmmo, maxAmmo);
        Debug.Log("Recargado");
    }
}

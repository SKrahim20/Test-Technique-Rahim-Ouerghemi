using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform pickUpPoint;
    public float bulletSpeed = 10f;
    public float bulletDestroyTime = 1.5f;
    public float fireCooldown = 2f; 

    private bool canFire = true;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && firePointIsChild() && canFire)
        {
            StartCoroutine(FireBulletWithCooldown());
        }
    }

    bool firePointIsChild()
    {
        return firePoint != null && pickUpPoint != null && firePoint.IsChildOf(pickUpPoint);
    }

    IEnumerator FireBulletWithCooldown()
    {
        canFire = false;

        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 shootDirection = (hit.point - firePoint.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().velocity = shootDirection * bulletSpeed;
            Destroy(bullet, bulletDestroyTime);
        }

        yield return new WaitForSeconds(fireCooldown);
        canFire = true;
    }
}

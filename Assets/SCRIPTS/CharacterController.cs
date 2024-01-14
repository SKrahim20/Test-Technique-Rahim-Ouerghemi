using Cinemachine;
using System.Collections;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public static CharacterController instance;
    public float moveSpeed = 5f;
    public float sprintSpeed = 10f;
    public float rotationSpeed = 10f;
    public float aimRotationSpeed = 5f;
    public Transform aimTransform;
    public LineRenderer lineRenderer;
    public Transform PickUpPoint;

    private Rigidbody rb;
    private Animator anim;
    private Camera mainCamera;
    private CinemachineVirtualCamera virtualCamera;

    private bool isAiming = false;
    private bool canShoot = true;

    public bool hit;
    public bool canHit;

    public GameObject coin;

    public int attackDamage = 50;

    public AudioSource swordHitSound;
    public AudioSource gunHitSound;
    public AudioSource walkSound; 

    void Start()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        anim = GetComponent<Animator>();
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Camera[] cameras = FindObjectsOfType<Camera>();
            if (cameras.Length > 0)
                mainCamera = cameras[0];
        }

        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (!PlayerHealth.instance.dead)
        {
            Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            inputDirection.Normalize();

            bool isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            float speed = isSprinting ? sprintSpeed : moveSpeed;

            float animationSpeed = 0;

            if (inputDirection != Vector3.zero)
            {
                animationSpeed = isSprinting ? 2 : 1;

                
                if (!isAiming && !walkSound.isPlaying)
                {
                    walkSound.Play();
                }

                Quaternion toRotation = Quaternion.LookRotation(inputDirection.normalized, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
                rb.velocity = inputDirection.normalized * speed;
            }
            else
            {
                rb.velocity = Vector3.zero;
                walkSound.Stop(); 
            }

            if (Input.GetMouseButton(1))
            {
                rb.velocity = Vector3.zero;
                isAiming = true;
                lineRenderer.enabled = true;

                if (virtualCamera != null && mainCamera != null)
                {
                    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        Vector3 cursorPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                        Vector3 lookDir = cursorPosition - transform.position;
                        lookDir.y = 0f;
                        Quaternion toRotation = Quaternion.LookRotation(lookDir, Vector3.up);
                        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, aimRotationSpeed * Time.deltaTime);

                        lineRenderer.SetPosition(1, cursorPosition);
                    }
                }

                if (Input.GetMouseButtonDown(0) && HasGunInHand() && canShoot)
                {
                    anim.SetBool("isShooting", true);
                    StartCoroutine(ShootCooldown());
                    if (gunHitSound != null)
                    {
                        gunHitSound.Play();
                    }
                }
                else
                {
                    anim.SetBool("isShooting", false);
                }
            }
            else
            {
                isAiming = false;
                lineRenderer.enabled = false;

                Vector3 targetDirection = inputDirection.normalized;
                Vector3 velocity = targetDirection * speed;

                if (inputDirection != Vector3.zero)
                {
                    animationSpeed = isSprinting ? 2 : 1;
                    Quaternion toRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
                    transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
                    rb.velocity = velocity;
                }
                else
                {
                    rb.velocity = Vector3.zero;
                }

                anim.SetBool("IsAttacking", false);

                if (!isAiming && Input.GetMouseButtonDown(0) && HasSwordInHand())
                {
                    anim.SetBool("IsAttacking", true);
                    if (swordHitSound != null)
                    {
                        swordHitSound.Play();
                    }
                }
            }

            anim.SetFloat("Magnitude", animationSpeed, 0.2f, Time.deltaTime);
        }
    }

    IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(2f);
        canShoot = true;
    }

    bool HasGunInHand()
    {
        if (PickUpPoint != null)
        {
            foreach (Transform child in PickUpPoint)
            {
                if (child.gameObject.layer == 7)
                {
                    return true;
                }
            }
        }
        return false;
    }

    bool HasSwordInHand()
    {
        if (PickUpPoint != null)
        {
            foreach (Transform child in PickUpPoint)
            {
                if (child.gameObject.layer == 6)
                {
                    return true;
                }
            }
        }
        return false;
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Rock"))
        {
            canHit = true;
            if (hit)
            {
                int rockHitCount = collision.gameObject.GetComponent<RockHitCounter>().hitCount;
                rockHitCount++;

                if (rockHitCount >= 3)
                {
                    SpawnCoins(collision.transform.position, 4);
                    Destroy(collision.gameObject);
                }

                collision.gameObject.GetComponent<RockHitCounter>().hitCount = rockHitCount;
                hit = false;
                canHit = false;
            }
        }
    }

    void Hit()
    {
        if (canHit)
        {
            hit = true;
            Debug.Log("Sword hit!");
        }
    }

    void SpawnCoins(Vector3 spawnPosition, int numberOfCoins)
    {
        for (int i = 0; i < numberOfCoins; i++)
        {
            Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), 0f, Random.Range(-0.5f, 0.5f));
            Instantiate(coin, spawnPosition + offset, Quaternion.identity);
        }
    }
}

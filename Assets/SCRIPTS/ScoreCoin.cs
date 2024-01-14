using UnityEngine;
using TMPro;

public class ScoreCoin : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public AudioClip coinCollectSound;
    private int score = 0;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.volume = 0.1f;  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("coin"))
        {
            Destroy(other.gameObject);
            score++;
            UpdateScoreText();
            PlayCoinCollectSound();

            ShopManager shopManager = GameObject.FindObjectOfType<ShopManager>();

            if (shopManager != null)
            {
                shopManager.CollectCoins(1);
            }
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }

    private void PlayCoinCollectSound()
    {
        if (coinCollectSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(coinCollectSound);
        }
    }
}

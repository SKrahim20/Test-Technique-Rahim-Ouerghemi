using UnityEngine;
using UnityEngine.UI;

public class ShopClick : MonoBehaviour
{
    public Canvas myCanvas;
    public CharacterController characterController;
    public Animator playerAnimator; 

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null && hit.collider.GetComponent<Collider>() != null)
                {
                    if (hit.collider.GetComponent<Collider>() == GetComponent<Collider>())
                    {
                        if (myCanvas != null)
                        {
                            myCanvas.gameObject.SetActive(true);
                            DisableMovement();
                        }
                    }
                }
            }
        }
    }

    public void CloseShop()
    {
        if (myCanvas != null)
        {
            myCanvas.gameObject.SetActive(false);
            EnableMovement();
        }
    }

    void DisableMovement()
    {
        if (characterController != null)
        {
            characterController.enabled = false;
        }

        if (playerAnimator != null)
        {
            
            playerAnimator.SetFloat("Magnitude", 0f);
        }
    }

    void EnableMovement()
    {
        if (characterController != null)
        {
            characterController.enabled = true;
        }
    }
}

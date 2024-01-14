using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectPickUp : MonoBehaviour
{
    public GameObject hand;
    public GameObject weapons;
    public GameObject backBone;

    private GameObject firstSlotWeapon;
    private GameObject secondSlotWeapon;

    private List<GameObject> equippedWeapons = new List<GameObject>();

    public TextMeshProUGUI firstSlot;
    public TextMeshProUGUI secondSlot;



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            DropObject();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            PickUpObject();
        }

        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwapWeapons();
        }

        
        if (hand.transform.childCount > 0)
        {
            GameObject equippedWeapon = hand.transform.GetChild(0).gameObject;

            if (equippedWeapon.layer == 7) 
            {
                
                equippedWeapon.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
            }
            else if (equippedWeapon.layer == 6) 
            {
                
                equippedWeapon.transform.localRotation = Quaternion.identity;
            }
        }


    }



    public void PickUpObject()
    {
        GameObject[] weaponsInRange = GameObject.FindGameObjectsWithTag("Weapon");

        foreach (GameObject weapon in weaponsInRange)
        {
            if (Vector3.Distance(transform.position, weapon.transform.position) < 2f && !equippedWeapons.Contains(weapon) && equippedWeapons.Count < 2)
            {
                equippedWeapons.Add(weapon);

                if (firstSlot != null && secondSlot != null)
                {
                    if (weapon.layer == 6)
                    {
                        firstSlot.text = "Sword";  
                    }

                    if (weapon.layer == 7)
                    {
                        secondSlot.text = "Gun";                    
                    }
                }

                if (hand.transform.childCount == 0)
                {
                    weapon.transform.SetParent(hand.transform);
                    firstSlotWeapon = weapon;
                }

                else
                {
                    weapon.transform.SetParent(backBone.transform);
                    secondSlotWeapon = weapon;
                }
                firstSlotWeapon.transform.localRotation = Quaternion.Euler(70f, 0f, 0f);
                weapon.transform.localScale = Vector3.one;
                weapon.transform.localPosition = Vector3.zero;
                weapon.transform.localRotation = Quaternion.identity;
                weapon.GetComponent<BoxCollider>().isTrigger = true;

                Rigidbody weaponRigidbody = weapon.GetComponent<Rigidbody>();
                if (weaponRigidbody != null)
                {
                    weaponRigidbody.isKinematic = true;
                }
            }
        }
    }

    public void DropObject()
    {
        if (equippedWeapons.Count > 0)
        {
            GameObject lastEquippedWeapon = equippedWeapons[equippedWeapons.Count - 1];

            lastEquippedWeapon.transform.SetParent(weapons.transform);
            lastEquippedWeapon.GetComponent<BoxCollider>().isTrigger = false;

            Rigidbody weaponRigidbody = lastEquippedWeapon.GetComponent<Rigidbody>();
            if (weaponRigidbody != null)
            {
                weaponRigidbody.isKinematic = false;
            }

            equippedWeapons.Remove(lastEquippedWeapon);

            if (firstSlot != null)
            {
                firstSlot.text = equippedWeapons.Exists(w => w.layer == 6) ? "Sword" : "None";
            }

            if (secondSlot != null)
            {
                secondSlot.text = equippedWeapons.Exists(w => w.layer == 7) ? "Gun" : "None";
            }
        }
    }

    private void SwapWeapons()
    {
        if (equippedWeapons.Count == 2)
        {
            secondSlotWeapon.transform.SetParent(hand.transform);
            secondSlotWeapon.transform.localPosition = Vector3.zero;
            secondSlotWeapon.transform.localRotation = Quaternion.identity;

            firstSlotWeapon.transform.SetParent(backBone.transform);
            firstSlotWeapon.transform.localPosition = Vector3.zero;


            if (firstSlotWeapon.layer == 6)
            {
                firstSlotWeapon.transform.localRotation = Quaternion.Euler(70f, 0f, 0f);
            }
            else
            {
                firstSlotWeapon.transform.localRotation = Quaternion.identity;
            }


            firstSlotWeapon.transform.localPosition = Vector3.zero;

            GameObject aux = secondSlotWeapon;
            secondSlotWeapon = firstSlotWeapon;
            firstSlotWeapon = aux;
        }
    }




}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class TakeDamage : MonoBehaviourPun
{
    public float startHealth = 100;
    private float health;
    public Image healthBar;
    Rigidbody rb;

    public GameObject PlayerGraphics;
    public GameObject PlayerUI;
    public GameObject PlayerWeaponHolder;
    public GameObject DeathPanelUIPrefab;
    private GameObject DeathPanelUIGameObejct;
    // Start is called before the first frame update
    void Start()
    {
        health = startHealth;
        healthBar.fillAmount = health / startHealth;
        rb = GetComponent<Rigidbody>();
    }

    [PunRPC]
    public void DoDamage(float _damage)
	{
        health -= _damage;
        Debug.Log(health);
        healthBar.fillAmount = health / startHealth;
        if (health <= 0f)
		{
            Die();
		}
	}

    public void Die()
	{
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        PlayerGraphics.SetActive(false);
        PlayerUI.SetActive(false);
        PlayerWeaponHolder.SetActive(false);

        if (photonView.IsMine)
		{
            // respawn
            StartCoroutine(Respawn());
		}
	}
    IEnumerator Respawn()
	{
        GameObject canvasGameObject = GameObject.Find("Canvas");

        if (DeathPanelUIGameObejct == null)
		{
            DeathPanelUIGameObejct = Instantiate(DeathPanelUIPrefab, canvasGameObject.transform);
        } else
		{
            DeathPanelUIGameObejct.SetActive(true);
		}
        Text respawnTimeText = DeathPanelUIGameObejct.transform.Find("RespawnTimeText").GetComponent<Text>();
        float respawnTime = 8.0f;
        respawnTimeText.text = respawnTime.ToString(".00"); 

        while (respawnTime > 0.0f)
		{
            yield return new WaitForSeconds(1.0f);
            respawnTime -= 1.0f;
            respawnTimeText.text = respawnTime.ToString(".00f");

            GetComponent<CarMoveMent>().enabled = false;
            GetComponent<Shooting>().enabled = false;
        }
        DeathPanelUIGameObejct.SetActive(false);

        GetComponent<CarMoveMent>().enabled = true;
        GetComponent<Shooting>().enabled = true;
        int randomPoint = Random.Range(-20, 20);
        transform.position = new Vector3(randomPoint, 0, randomPoint);

        photonView.RPC("Reborn", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void Reborn()
	{
        health = startHealth;
        healthBar.fillAmount = health / startHealth;

        PlayerGraphics.SetActive(true);
        PlayerUI.SetActive(true);
        PlayerWeaponHolder.SetActive(true);
	}
    // Update is called once per frame
    void Update()
    {
        
    }
}

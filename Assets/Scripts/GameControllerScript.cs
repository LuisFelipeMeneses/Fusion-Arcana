using NUnit;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject[] playersInventorySlots;
    [SerializeField] private GameObject[] playersKeysSlots;
    [SerializeField] private GameObject[] playersLifeSlots;
    private static GameObject[] players = new GameObject[2];
    void Start()
    {
        var p1 = PlayerInput.Instantiate(playerPrefab, controlScheme: "Player1", pairWithDevice: Keyboard.current);
        p1.transform.position = new Vector2(-7, -0.5f);
        p1.GetComponent<PlayerScript>().Instantiate(1);

        var p2 = PlayerInput.Instantiate(playerPrefab, controlScheme: "Player2", pairWithDevice: Keyboard.current);
        p2.transform.position = new Vector2(7, -0.5f);
        p2.GetComponent<PlayerScript>().Instantiate(2);

        PlayerScript.OnSpellCast += UpdateInventoryUI;
        PlayerScript.OnKeysPressed += UpdateKeysUI;
        PlayerScript.OnPlayerDamaged += UpdatePlayerLife; 
        players[0] = p1.gameObject;
        players[1] = p2.gameObject;
    }

    void Update()
    {
        
    }

    public static GameObject GetPlayerByOther(GameObject other)
    {
        if (other == players[0])
        {
            return players[1];
        } else if (other == players[1])
        {
            return players[0];
        }
        return null;
    }

    public void UpdateInventoryUI(SpellSO[] inv, int playerNumber)
    {
        int buffer = 4 * (playerNumber - 1);
        for (int i = 0; i < inv.Length; i++)
        {
            Image img = playersInventorySlots[i + buffer].GetComponent<Image>();

            if (inv[i] != null)
            {
                img.sprite = inv[i].ImageHUD;
                img.color = Color.white;
            }
            else
            {
                Color c = img.color;
                c.a = 0f;
                img.color = c;
            }
        }
    }

    public void UpdateKeysUI(Keys[] combo, int playerNumber)
    {
        int buffer = 4 * (playerNumber - 1);
        for (int i = 0; i < combo.Length; i++)
        {
            Image img = playersKeysSlots[i + buffer].GetComponent<Image>();

            if (combo[i] != Keys.Null)
            {
                img.color = Color.white;
                img.rectTransform.localRotation = Quaternion.Euler(0, 0, 90 * ((int)combo[i] - 1));
            }
            else
            {
                Color c = img.color;
                c.a = 0f;
                img.color = c;
            }
        }
    }

    public void UpdatePlayerLife(float lifePercent, int playerNumer)
    {
        Debug.Log("Porcentagem: " + lifePercent);
        playersLifeSlots[playerNumer - 1].GetComponent<Image>().fillAmount = lifePercent;
    }
}

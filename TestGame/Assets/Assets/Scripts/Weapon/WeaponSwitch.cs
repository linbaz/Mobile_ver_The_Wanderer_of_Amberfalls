using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitch : MonoBehaviour
{
    public Sword[] swords;
    public int selectedWeapon = 0;

    public Button button1;
    public Button button2;

    private void Start()
    {
        swords = GetComponentsInChildren<Sword>();
        SelectWeapon();

        // Додайте обробники подій для кнопок
        button1.onClick.AddListener(() => SelectWeaponByIndex(0));
        button2.onClick.AddListener(() => SelectWeaponByIndex(1));
    }

    public void Update()
    {
        // Залиште цей метод порожнім або видаліть його, якщо не потрібно обробляти клавіатуру
    }

    public void SelectWeaponByIndex(int index)
    {
        if (index < transform.childCount)
        {
            selectedWeapon = index;
            SelectWeapon();
        }
    }

    public void SelectWeapon()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == selectedWeapon)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(false);
                if (swords != null && i < swords.Length)
                {
                    swords[i].CancelAttack();
                }
            }
        }
    }
}

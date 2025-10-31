using UnityEngine;
using TMPro;

public class WeaponInfo : MonoBehaviour
{
    public TMP_Text currentBullets;
    public TMP_Text totalBullets;

    private void OnEnable()
    {
        EventManager.current.updateBulletsEvent.AddListener(UpdateBullets);
    }

    private void OnDisable()
    {
        
    }
    public void UpdateBullets(int newCurrentBullets, int newTotalBullets)
    {
        if(newCurrentBullets <= 0)
        {
            currentBullets.color = new Color(1, 0, 0);
         
        }
        else
        {
            currentBullets.color = Color.white;
        }
            currentBullets.text = newCurrentBullets.ToString();
        totalBullets.text = newTotalBullets.ToString();
    }

   
}

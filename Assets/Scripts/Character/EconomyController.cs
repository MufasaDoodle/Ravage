using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyController : MonoBehaviour
{
    public int Money { get; private set; }

    public delegate void MoneyChanged(int amount);
    public MoneyChanged moneyChangedCB;

    public void GiveMoney(int amount)
    {
        Money += amount;
        NotifyMoneyChanged();
    }

    public bool SpendMoney(int amount)
    {
        if(Money >= amount)
        {
            Money -= amount;
            NotifyMoneyChanged();
            return true;
        }

        return false;
    }

    private void NotifyMoneyChanged()
    {
        if (moneyChangedCB != null)
        {
            moneyChangedCB.Invoke(Money);
        }
    }
}

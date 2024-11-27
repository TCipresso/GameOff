using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerInventory is a <see cref="KeywordHandler"/> that stores the 
/// number of items the player has.
/// It's been quickly thrown together to allow certain interactions to happen.
/// </summary>
public class PlayerInventory : KeywordHandler
{
    public static PlayerInventory instance { private set; get; }
    [SerializeField] int appleCount = 0;
    [SerializeField] int smokeCount = 0;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    /// <summary>
    /// Show the player how many of each item they currently have.
    /// "Inventory" shows full inventory.
    /// "Inventory <item>" shows that specific item.
    /// </summary>
    /// <param name="tokens">Player input.</param>
    /// <returns>String of inventory.</returns>
    public override string ReadTokens(string[] tokens)
    {
        if(tokens.Length == 1)
        {
            return $"You look into your bag and find:\nApples: {appleCount}\nSmokes: {smokeCount}";
        }

        for(int i = 1; i < tokens.Length; i++)
        {
            switch(tokens[i])
            {
                case "apple":
                case "apples":
                    return $"You look into your bag and find:\nApples: {appleCount}";
                case "smoke":
                case "smokes":
                    return $"You look into your bag and find:\nSmokes: {smokeCount}";
            }
        }

        return "Unknown item.";
    }

    public int GetApples()
    {
        return appleCount;
    }

    public void AddApple()
    {
        appleCount++;
    }

    public void AddApple(int amount)
    {
        appleCount += amount;
    }

    public void RemoveApple()
    {
        if (appleCount <= 0) throw new InvalidOperationException("You do not have any apples.");
        appleCount--;
    }

    public void RemoveApple(int amount)
    {
        if (appleCount < amount) throw new InvalidOperationException("You do not have enough apples.");
        appleCount -= amount;
    }

    public int GetSmokes()
    {
        return smokeCount;
    }

    public void AddSmoke()
    {
        smokeCount++;
    }

    public void AddSmoke(int amount) 
    { 
        smokeCount += amount;
    }

    public void RemoveSmoke() 
    {
        if (smokeCount <= 0) throw new InvalidOperationException("You do not have any smokes.");
        smokeCount--;
    }

    public void RemoveSmoke(int amount)
    {
        if (smokeCount < amount) throw new InvalidOperationException("You do not have enough smokes.");
        smokeCount -= amount;
    }
}

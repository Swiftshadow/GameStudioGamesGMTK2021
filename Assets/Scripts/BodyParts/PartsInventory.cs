using System.Collections.Generic;

[System.Serializable]
public class PartsInventory
{
    private List<BodyPartBaseSO> inventory = new List<BodyPartBaseSO>();

    /// <summary>
    /// Public accessor for the inventory
    /// </summary>
    public List<BodyPartBaseSO> Inventory
    {
        get => inventory;
    }

    /// <summary>
    /// Add a new part to the inventory
    /// </summary>
    /// <param name="part">Part to add</param>
    public void AddToInventory(BodyPartBaseSO part)
    {
        inventory.Add(part);
    }

    /// <summary>
    /// Remove a part from the inventory
    /// </summary>
    /// <param name="part">Part to remove</param>
    /// <returns>True if removal was successful, false if it was not</returns>
    public bool RemoveFromInventory(BodyPartBaseSO part)
    {
        return inventory.Remove(part);
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents a slot for an item in the itemBar or in the inventory.
/// The generic class MySlot already implements all functionality, but generic classes
/// can't be attached or serialized, but a subclass can, which is why this class exists.
/// </summary>
public class ItemSlot : Slot<Item> { }

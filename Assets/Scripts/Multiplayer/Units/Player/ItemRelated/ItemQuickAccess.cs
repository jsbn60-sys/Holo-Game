using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents the quickAccessBar for items.
/// The generic class QuickAccess already implements all functionality, but generic classes
/// can't be attached or serialized, but a subclass can, which is why this class exists.
/// </summary>
public class ItemQuickAccess : QuickAccess<ItemSlot, Item> { }

using System.Collections.Generic;
using AuroraRgb.Nodes;

namespace AuroraRgb.Profiles.Payday_2.GSI.Nodes;

public class ItemsNode : Node
{
    private readonly List<ItemNode> _items = [];

    public int Count => _items.Count;

    internal ItemsNode(string json) : base(json)
    {
        foreach (var jt in _ParsedData.Children())
        {
            _items.Add(new ItemNode(jt.First.ToString()));
        }
    }

    /// <summary>
    /// Gets the weapon with index &lt;index&gt;
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public ItemNode this[int index]
    {
        get
        {
            if (index > _items.Count - 1)
            {
                return new ItemNode(string.Empty);
            }

            return _items[index];
        }
    }
}
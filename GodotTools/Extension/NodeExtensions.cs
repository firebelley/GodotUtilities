using System;
using System.Collections.Generic;
using System.Text;
using Godot;

namespace GodotTools.Extension
{
    public static class NodeExtensions
    {
        public static Node HAFUCKITWORKS(this Node n, int idx)
        {
            return n.GetParent().GetChild(idx);
        }
    }
}

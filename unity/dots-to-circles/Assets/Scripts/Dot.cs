using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTC.Characters {
    public enum ChantMarkType {
        None,
        Sun,
        Moon,
        Poison,
        Tree
    }

    public static class ChantMark
    {
        public static ChantMarkType NameToType(string name)
        {
            Dictionary<string, ChantMarkType> dict = new Dictionary<string, ChantMarkType>();

            dict.Add("sun", ChantMarkType.Sun);
            dict.Add("moon", ChantMarkType.Moon);
            dict.Add("poison", ChantMarkType.Poison);
            dict.Add("tree", ChantMarkType.Tree);

            if (dict.ContainsKey(name)) 
            {
                return dict[name.ToLower()];
            }
            else
            {
                return ChantMarkType.None;
            }

        }
    }
}
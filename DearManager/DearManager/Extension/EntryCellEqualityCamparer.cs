using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DearManager.Extension
{
    class EntryCellEqualityCamparer : IEqualityComparer<EntryCell>
    {
        public bool Equals(EntryCell x, EntryCell y)
        {
            return x.Label == y.Label;
        }

        public int GetHashCode(EntryCell obj)
        {
            return obj.Label.GetHashCode();
        }
    }
}

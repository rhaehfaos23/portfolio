using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace DearManager.Model
{
    public interface ILocalize
    {
        CultureInfo GetCurrentCultureInfo();
        void SetLocale(CultureInfo ci);
    }
}

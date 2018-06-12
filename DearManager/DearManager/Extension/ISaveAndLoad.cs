using System;
using System.Collections.Generic;
using System.Text;

namespace DearManager.Extension
{
    public interface ISaveAndLoad
    {
        void SaveImage(string fileName, byte[] imageByBase64);
        byte[] LoadImage(string fileName);
        void DeleteImage(string fileName);
    }
}

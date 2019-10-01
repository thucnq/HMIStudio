using System;
using System.Collections.Generic;
using System.Text;

namespace HMIStudio.Shared.Interfaces
{
    public interface IClipboard
    {
        void CopyToClipboard(string text);
        string BaseDirectory { get; }

        void OpenUrl(string url);
    }
}

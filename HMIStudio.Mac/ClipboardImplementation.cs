﻿using System;
using AppKit;
using Foundation;
using HMIStudio.Shared.Interfaces;

namespace HMIStudio.Mac
{
    public class ClipboardImplementation : IClipboard
    {
        static readonly string pasteboardType = NSPasteboard.NSPasteboardTypeString;
        static readonly string[] pasteboardTypes = { pasteboardType };
        static NSPasteboard Pasteboard => NSPasteboard.GeneralPasteboard;

        public string BaseDirectory
        {
            get
            {
                var test = 
                 NSSearchPath.GetDirectories(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User);

                return test[0];
            }
        }

        public void CopyToClipboard(string text)
        {
            Pasteboard.DeclareTypes(pasteboardTypes, null);
            Pasteboard.ClearContents();
            Pasteboard.SetStringForType(text, pasteboardType);
        }

        public void OpenUrl(string url)
        {
            NSWorkspace.SharedWorkspace.OpenUrl(new NSUrl(url));
        }
    }
}

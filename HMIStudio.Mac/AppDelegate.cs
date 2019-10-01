﻿using System;
using AppKit;
using CoreGraphics;
using Foundation;
using HMIStudio.Shared.Helpers;
using HMIStudio.Shared.Interfaces;
using HMIStudio.UI;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

namespace HMIStudio.Mac
{
    public class MyWindow : NSWindow
    {
        public MyWindow()
        {
        }

        public MyWindow(NSCoder coder) : base(coder)
        {
        }

        public MyWindow(CGRect contentRect, NSWindowStyle aStyle, NSBackingStore bufferingType, bool deferCreation) : base(contentRect, aStyle, bufferingType, deferCreation)
        {
        }

        public MyWindow(CGRect contentRect, NSWindowStyle aStyle, NSBackingStore bufferingType, bool deferCreation, NSScreen screen) : base(contentRect, aStyle, bufferingType, deferCreation, screen)
        {
        }

        protected MyWindow(NSObjectFlag t) : base(t)
        {
        }

        protected internal MyWindow(IntPtr handle) : base(handle)
        {
        }

        [Export("showHelp:")]
        private void HandleShowHelp(NSObject sender)
        {
            var clipboard = ServiceContainer.Resolve<IClipboard>();
            clipboard.OpenUrl("https://google.com/");
        }

        public override void PerformClose(NSObject sender)
        {
            NSApplication.SharedApplication.Terminate(sender);
        }

    }

    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        NSWindow window;
        public AppDelegate()
        {
            var style = NSWindowStyle.Closable | NSWindowStyle.Miniaturizable | NSWindowStyle.Resizable | NSWindowStyle.Titled;

            var rect = new CoreGraphics.CGRect(500, 300, 560, 300);
            window = new MyWindow(rect, style, NSBackingStore.Buffered, false);
            window.Title = "HMI Studio"; // choose your own Title here
            window.TitleVisibility = NSWindowTitleVisibility.Visible;
            
        }

        public override NSWindow MainWindow => window;

        public override void DidFinishLaunching(NSNotification notification)
        {
            Forms.Init();

            ServiceContainer.Register<IClipboard>(() => new ClipboardImplementation());
            LoadApplication(new HMIStudio.UI.App());
           
            
            var appleEventManager = NSAppleEventManager.SharedAppleEventManager;

            appleEventManager.SetEventHandler(this, new ObjCRuntime.Selector("handleGetURLEvent:withReplyEvent:"), AEEventClass.Internet, AEEventID.GetUrl);

            base.DidFinishLaunching(notification);
        }

  

        [Export("handleGetURLEvent:withReplyEvent:")]
        private void HandleGetURLEvent(NSAppleEventDescriptor descriptor, NSAppleEventDescriptor replyEvent)
        {
            // Breakpoint here, debug normally and *then* call your URL
            try
            {
                var keyDirectObject = "----";
                var keyword = (((uint)keyDirectObject[0]) << 24 |
                               ((uint)keyDirectObject[1]) << 16 |
                               ((uint)keyDirectObject[2]) << 8 |
                               ((uint)keyDirectObject[3]));

                var openinArgs = descriptor.ParamDescriptorForKeyword(keyword).StringValue;
                ParseOpeningString(openinArgs);
            }
            catch
            {

            }


        }

        void ParseOpeningString(string openinArgs)
        {
            var (start, mins) = Utils.ParseStartupArgs(openinArgs);
            MainPage.OpeningArgs = (start, mins);
            if (start && mins > 0)
                MainPage.DownVM?.Init(mins);
        }

        public override void OpenUrls(NSApplication application, NSUrl[] urls)
        {
            
            if(urls != null && urls.Length > 0)
            {
                var openinArgs = urls[0].AbsoluteString;
                ParseOpeningString(openinArgs);
            }
        }


    }
}

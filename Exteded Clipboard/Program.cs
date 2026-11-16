using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace Exteded_Clipboard;

internal class Program
{
    // Windows API Constants
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private const uint CF_UNICODETEXT = 13;
    private const uint GMEM_MOVEABLE = 0x0002;

    // Keep a static reference to the delegate so the Garbage Collector doesn't clean it up
    private static LowLevelKeyboardProc _proc = HookCallback;
    private static IntPtr _hookID = IntPtr.Zero;

    // Store the parsed config globally so OnKeyPressed can read it
    private static Dictionary<ConsoleKey, string> _keySnippets = new();

    static void Main(string[] args)
    {
        string jsonFilePath = "config.json";
        Console.WriteLine($"Reading config file at: {jsonFilePath}");

        if (File.Exists(jsonFilePath))
        {
            string json = File.ReadAllText(jsonFilePath);
            // System.Text.Json automatically converts string JSON keys ("112") into int keys (112)
            var options = new JsonSerializerOptions
            {
                Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
            };
            _keySnippets = JsonSerializer.Deserialize<Dictionary<ConsoleKey, string>>(json, options) ?? new();

            foreach (var item in _keySnippets)
            {
                Console.WriteLine($"Loaded hotkey: {(ConsoleKey)item.Key} ({item.Key}) -> \"{item.Value}\"");
            }
        }
        else
        {
            Console.WriteLine("Warning: config.json not found.");
        }

        Console.WriteLine("\nListening for keypresses globally... Press Ctrl+C in console to exit.");

        // Start Global Key Listener
        _hookID = SetHook(_proc);

        // Start a Windows Message Loop (Required for the hook to receive events)
        while (GetMessage(out MSG msg, IntPtr.Zero, 0, 0))
        {
            TranslateMessage(ref msg);
            DispatchMessage(ref msg);
        }

        UnhookWindowsHookEx(_hookID);
    }

    // ==========================================
    // 1. FUNCTION THAT RUNS ON EVERY KEYPRESS
    // ==========================================
    private static void OnKeyPressed(int vkCode)
    {
        ConsoleKey key = (ConsoleKey)vkCode;
        Console.WriteLine($"Key Pressed: {key} (Code: {vkCode})");

        // Pass 'key' (ConsoleKey) instead of 'vkCode' (int)
        if (_keySnippets.TryGetValue(key, out string? textToCopy))
        {
            SetClipboardText(textToCopy);
            Console.WriteLine($"-> Clipboard updated with: \"{textToCopy}\"");
        }
    }

    // ==========================================
    // 2. SET CLIPBOARD FUNCTION
    // ==========================================
    public static void SetClipboardText(string text)
    {
        if (!OpenClipboard(IntPtr.Zero)) return;

        try
        {
            EmptyClipboard();

            int bytes = (text.Length + 1) * 2;
            IntPtr hGlobal = GlobalAlloc(GMEM_MOVEABLE, (UIntPtr)bytes);
            if (hGlobal == IntPtr.Zero) return;

            IntPtr target = GlobalLock(hGlobal);
            if (target == IntPtr.Zero) return;

            try
            {
                Marshal.Copy(text.ToCharArray(), 0, target, text.Length);
                Marshal.WriteInt16(target, text.Length * 2, 0);
            }
            finally
            {
                GlobalUnlock(hGlobal);
            }

            SetClipboardData(CF_UNICODETEXT, hGlobal);
        }
        finally
        {
            CloseClipboard();
        }
    }

    // ==========================================
    // 3. PASTE (GET) FROM CLIPBOARD FUNCTION
    // ==========================================
    public static string? GetClipboardText()
    {
        if (!IsClipboardFormatAvailable(CF_UNICODETEXT)) return null;
        if (!OpenClipboard(IntPtr.Zero)) return null;

        try
        {
            IntPtr hGlobal = GetClipboardData(CF_UNICODETEXT);
            if (hGlobal == IntPtr.Zero) return null;

            IntPtr pointer = GlobalLock(hGlobal);
            if (pointer == IntPtr.Zero) return null;

            try
            {
                return Marshal.PtrToStringUni(pointer);
            }
            finally
            {
                GlobalUnlock(hGlobal);
            }
        }
        finally
        {
            CloseClipboard();
        }
    }

    // ==========================================
    // HOOK INFRASTRUCTURE & P/INVOKE DECLARATIONS
    // ==========================================
    private static IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using Process curProcess = Process.GetCurrentProcess();
        using ProcessModule curModule = curProcess.MainModule!;
        return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
    }

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
        {
            int vkCode = Marshal.ReadInt32(lParam);
            OnKeyPressed(vkCode);
        }
        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MSG
    {
        public IntPtr hwnd;
        public uint message;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public System.Drawing.Point pt;
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    private static extern bool GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

    [DllImport("user32.dll")]
    private static extern bool TranslateMessage([In] ref MSG lpMsg);

    [DllImport("user32.dll")]
    private static extern IntPtr DispatchMessage([In] ref MSG lpMsg);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool OpenClipboard(IntPtr hWndNewOwner);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool CloseClipboard();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool EmptyClipboard();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr GetClipboardData(uint uFormat);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool IsClipboardFormatAvailable(uint format);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GlobalAlloc(uint uFlags, UIntPtr dwBytes);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GlobalLock(IntPtr hMem);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GlobalUnlock(IntPtr hMem);
}
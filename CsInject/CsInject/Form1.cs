using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CsInject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        //LPVOID VirtualAllocEx(
        //    HANDLE hProcess,
        //    LPVOID lpAddress,
        //    SIZE_T dwSize,
        //    DWORD flAllocationType,
        //    DWORD flProtect
        //);
        [DllImport("Kernel32.dll")]
        public static extern int VirtualAllocEx(int hProcess, int lpAddress, int dwSize, int flAllocationType, int flProtect);




        //BOOL WriteProcessMemory(
        //    HANDLE hProcess,
        //    LPVOID lpBaseAddress,
        //    LPCVOID lpBuffer,
        //    SIZE_T nSize,
        //    SIZE_T* lpNumberOfBytesWritten
        //);
        [DllImport("Kernel32.dll")]
        public static extern Boolean WriteProcessMemory(int hProcess, int lpBaseAddress, String lpBuffer, int nSize, int lpNumberOfBytesWritten);



        //HMODULE GetModuleHandleA(
        //    LPCSTR lpModuleName
        //);
        [DllImport("Kernel32.dll")]
        public static extern int GetModuleHandleA(String lpModuleName);



        //FARPROC GetProcAddress(
        //    HMODULE hModule,
        //    LPCSTR lpProcName
        //);
        [DllImport("Kernel32.dll")]
        public static extern int GetProcAddress(int hModule, String lpProcName);




        //HANDLE CreateRemoteThread(
        //    HANDLE hProcess,
        //    LPSECURITY_ATTRIBUTES lpThreadAttributes,
        //    SIZE_T dwStackSize,
        //    LPTHREAD_START_ROUTINE lpStartAddress,
        //    LPVOID lpParameter,
        //    DWORD dwCreationFlags,
        //    LPDWORD lpThreadId
        //);
        [DllImport("Kernel32.dll")]
        public static extern int CreateRemoteThread(int hProcess, int lpThreadAttributes, int dwStackSize, int lpStartAddress, int lpParameter, int dwCreationFlags, int lpThreadId
         );



        private void button1_Click(object sender, EventArgs e)
        {
            int WxId = 0;
            int WxHandle = 0;

            Process[] processes = Process.GetProcesses();
            foreach (Process process in processes)
            {
                if (process.ProcessName == "WeChat")
                {
                    WxId = process.Id;
                    WxHandle = (int)(process.Handle);
                    break;
                }
            }

            if (WxId == 0)
            {
                MessageBox.Show("Target process not found.");
                return;

            }

            int MEM_COMMIT = 0x00001000;
            int PAGE_READWRITE = 0x04;


            String DllPath = @"C:\Users\anuwat\source\repos\WeChatHook\Debug\WeChatHook.dll";
            int DllPathSize = DllPath.Length + 1;
            int DllAddress = VirtualAllocEx(WxHandle, 0, DllPathSize, MEM_COMMIT, PAGE_READWRITE);


            if (DllAddress == 0)
            {
                MessageBox.Show("Cannot init dll address.");
                return;
            }


            if (WriteProcessMemory(WxHandle, DllAddress, DllPath, DllPathSize, 0) == false)
            {
                MessageBox.Show("Cannot WriteProcessMemory.");
                return;
            }

            int module = GetModuleHandleA("Kernel32.dll");


            int loadLibraryAddress = GetProcAddress(module, "LoadLibraryA");

            if (loadLibraryAddress == 0)
            {
                MessageBox.Show("Cannot loadLibraryAddress.");
                return;
            }

            if (CreateRemoteThread(WxHandle, 0, 0, loadLibraryAddress, DllAddress, 0, 0) == 0)
            {
                MessageBox.Show("Cannot CreateRemoteThread.");
                return;
            }


            // MessageBox.Show("OKay!");



        }
    }
}
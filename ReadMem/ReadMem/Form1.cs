using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReadMem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }




        //BOOL ReadProcessMemory(
        //    HANDLE hProcess,
        //    LPCVOID lpBaseAddress,
        //    LPVOID lpBuffer,
        //    SIZE_T nSize,
        //    SIZE_T* lpNumberOfBytesRead
        //);


        [DllImport("Kernel32.dll")]
        public static extern int ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, int lpNumberOfBytesRead);


        String GetString(IntPtr hProcess, IntPtr lpBaseAddress, int nSize = 100)
        {
            byte[] data = new byte[nSize];
            if (ReadProcessMemory(hProcess, lpBaseAddress, data, nSize, 0) == 0)
            {
                return "";
            }

            String result = "";
            String TempString = Encoding.ASCII.GetString(data);
            foreach (char item in TempString)
            {
                if (item == '\0')
                {
                    break; ;
                }

                result += item;
            }


            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.textBox1.Clear();
            Process WxProcess = null;
            IntPtr WeChatWinBaseAddress = IntPtr.Zero;
            Process[] processes = Process.GetProcesses();
            String WeChatVersion = "";


            foreach (Process p in processes)
            {
                if (p.ProcessName == "WeChat")
                {
                    WxProcess = p;
                    this.textBox1.AppendText("Details:" + Environment.NewLine);
                    this.textBox1.AppendText("Process Handle:\t" + "0x" + ((int)p.Handle).ToString("X8") + Environment.NewLine);
                    foreach (ProcessModule pm in p.Modules)
                    {
                        if (pm.ModuleName == "WeChatWin.dll")
                        {
                            WeChatWinBaseAddress = pm.BaseAddress;
                            this.textBox1.AppendText("WeChatWinBaseAddress:\t" + "0x" + ((int)pm.BaseAddress).ToString("X8") + Environment.NewLine);
                            WeChatVersion = pm.FileVersionInfo.FileVersion;
                            this.textBox1.AppendText("FileVersion:\t" + pm.FileVersionInfo.FileVersion + Environment.NewLine);

                            break;
                        }
                    }
                }
            }


            if (WxProcess == null)
            {
                this.textBox1.AppendText("Cannot open process.");
                return;
            }

            int WxNameAddress = (int)WeChatWinBaseAddress + 0x16B48AC;
            this.textBox1.AppendText("username:\t" + GetString(WxProcess.Handle, (IntPtr)WxNameAddress) + Environment.NewLine);

            int WxIdAddress = (int)WeChatWinBaseAddress + 0x16B4A10;
            this.textBox1.AppendText("userId:\t" + GetString(WxProcess.Handle, (IntPtr)WxIdAddress) + Environment.NewLine);


            int WxPhoneAddress = (int)WeChatWinBaseAddress + 0x16B4CE8;
            this.textBox1.AppendText("phone:\t" + GetString(WxProcess.Handle, (IntPtr)WxPhoneAddress) + Environment.NewLine);


            int WxTelAddress = (int)WeChatWinBaseAddress + 0x16B48E0;
            this.textBox1.AppendText("tel:\t" + GetString(WxProcess.Handle, (IntPtr)WxTelAddress) + Environment.NewLine);

            int WxCityAddress = (int)WeChatWinBaseAddress + 0x16B4998;
            this.textBox1.AppendText("city:\t" + GetString(WxProcess.Handle, (IntPtr)WxCityAddress) + Environment.NewLine);


            int WxCountryAddress = (int)WeChatWinBaseAddress + 0x16B4A88;
            this.textBox1.AppendText("country:\t" + GetString(WxProcess.Handle, (IntPtr)WxCountryAddress) + Environment.NewLine);




        }



    }
}





//<? xml version="1.0" encoding="utf-8"?>
//<CheatTable>
//  <CheatEntries>
//    <CheatEntry>
//      <ID>0</ID>
//      <Description>"name"</Description>
//      <LastState RealAddress = "10EA48AC" />
//      < VariableType > String </ VariableType >
//      < Length > 11 </ Length >
//      < Unicode > 0 </ Unicode >
//      < CodePage > 0 </ CodePage >
//      < ZeroTerminate > 1 </ ZeroTerminate >
//      < Address > WeChatWin.dll + 16B48AC</Address>
//    </CheatEntry>
//    <CheatEntry>
//      <ID>1</ID>
//      <Description>"id"</Description>
//      <LastState RealAddress = "10EA4A10" />
//      < VariableType > String </ VariableType >
//      < Length > 11 </ Length >
//      < Unicode > 0 </ Unicode >
//      < CodePage > 0 </ CodePage >
//      < ZeroTerminate > 1 </ ZeroTerminate >
//      < Address > WeChatWin.dll + 16B4A10</Address>
//    </CheatEntry>
//    <CheatEntry>
//      <ID>2</ID>
//      <Description>"city"</Description>
//      <LastState RealAddress = "10EA4998" />
//      < VariableType > String </ VariableType >
//      < Length > 10 </ Length >
//      < Unicode > 0 </ Unicode >
//      < CodePage > 0 </ CodePage >
//      < ZeroTerminate > 1 </ ZeroTerminate >
//      < Address > WeChatWin.dll + 16B4998</Address>
//    </CheatEntry>
//    <CheatEntry>
//      <ID>3</ID>
//      <Description>"phone"</Description>
//      <LastState RealAddress = "10EA4CE8" />
//      < VariableType > String </ VariableType >
//      < Length > 10 </ Length >
//      < Unicode > 0 </ Unicode >
//      < CodePage > 0 </ CodePage >
//      < ZeroTerminate > 1 </ ZeroTerminate >
//      < Address > WeChatWin.dll + 16B4CE8</Address>
//    </CheatEntry>
//    <CheatEntry>
//      <ID>4</ID>
//      <Description>"tel"</Description>
//      <LastState RealAddress = "10EA48E0" />
//      < VariableType > String </ VariableType >
//      < Length > 12 </ Length >
//      < Unicode > 0 </ Unicode >
//      < CodePage > 0 </ CodePage >
//      < ZeroTerminate > 1 </ ZeroTerminate >
//      < Address > WeChatWin.dll + 16B48E0</Address>
//    </CheatEntry>
//    <CheatEntry>
//      <ID>5</ID>
//      <Description>"country"</Description>
//      <LastState RealAddress = "10EA4A88" />
//      < VariableType > String </ VariableType >
//      < Length > 2 </ Length >
//      < Unicode > 0 </ Unicode >
//      < CodePage > 0 </ CodePage >
//      < ZeroTerminate > 1 </ ZeroTerminate >
//      < Address > WeChatWin.dll + 16B4A88</Address>
//    </CheatEntry>
//  </CheatEntries>
//</CheatTable>

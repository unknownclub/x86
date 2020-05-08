// C_Inject.cpp : Defines the entry point for the application.
//

#include "stdafx.h"
#include "C_Inject.h"
#include "resource1.h"
#include "string.h"

#include <TlHelp32.h>

#define MAX_LOADSTRING 100

// Global Variables:
HINSTANCE hInst;                                // current instance
WCHAR szTitle[MAX_LOADSTRING];                  // The title bar text
WCHAR szWindowClass[MAX_LOADSTRING];            // the main window class name

// Forward declarations of functions included in this code module:
ATOM                MyRegisterClass(HINSTANCE hInstance);
BOOL                InitInstance(HINSTANCE, int);
LRESULT CALLBACK    WndProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK    About(HWND, UINT, WPARAM, LPARAM);

char dllName[] = "C:\\Users\\anuwat\\source\\repos\\WeChatHook\\Debug\\WeChatHook.dll";
DWORD strSize = strlen(dllName) + 1;

INT_PTR CALLBACK DialogProc(_In_ HWND hwndDlg, _In_ UINT uMsg, _In_ WPARAM wParam, _In_ LPARAM lParam)
{
	switch (uMsg)
	{
	case WM_COMMAND:
		if (wParam == IDB_INJECT)
		{

			wchar_t buff[0x100] = { 0 };
			DWORD weChatProcessID = 0;

			HANDLE handle = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, NULL);
			swprintf_s(buff, L"CreateToolhelp32Snapshot=%p", handle);
			OutputDebugString(buff);
			PROCESSENTRY32 processentry32 = { 0 };
			processentry32.dwSize = sizeof(PROCESSENTRY32);

			BOOL next = Process32Next(handle, &processentry32);
			while (next == TRUE)
			{
				if (wcscmp(processentry32.szExeFile, L"WeChat.exe") == 0)
				{
					weChatProcessID = processentry32.th32ProcessID;
					break;
				}
				next = Process32Next(handle, &processentry32);
			}

			if (weChatProcessID == 0)
			{
				MessageBox(NULL, L"WeChat not found.", L"Error", MB_OK);
				return 0;
			}

			HANDLE hProcess = OpenProcess(PROCESS_ALL_ACCESS, TRUE, weChatProcessID);
			if (hProcess == NULL)
			{
				MessageBox(NULL, L"Cannot open WeChat process.", L"Error", MB_OK);
				return 0;
			}

			LPVOID allocAddress = VirtualAllocEx(hProcess, NULL, strSize, MEM_COMMIT, PAGE_READWRITE);
			if (NULL == allocAddress)
			{
				MessageBox(NULL, L"Can not allocate memory.", L"Error", MB_OK);
				return 0;
			}
			swprintf_s(buff, L"VirtualAllocEx=%p", allocAddress);
			OutputDebugString(buff);

			BOOL result = WriteProcessMemory(hProcess, allocAddress, dllName, strSize, NULL);
			swprintf_s(buff, L"WriteProcessMemory=%p", result);
			OutputDebugString(buff);
			if (result == FALSE)
			{
				MessageBox(NULL, L"Can not allocate memory.", L"Error", MB_OK);
				return 0;
			}

			HMODULE hMODULE = GetModuleHandle(L"Kernel32.dll");
			FARPROC fARPROC = GetProcAddress(hMODULE, "LoadLibraryA");


			if (NULL == fARPROC)
			{
				MessageBox(NULL, L"Can not LoadLibraryA", L"Error", MB_OK);
				return 0;
			}

			HANDLE hANDLE = CreateRemoteThread(hProcess, NULL, 0, (LPTHREAD_START_ROUTINE)fARPROC, allocAddress, 0, NULL);


			if (NULL == hANDLE)
			{
				MessageBox(NULL, L"Can not CreateRemoteThread", L"Error", MB_OK);
				return 0;
			}



		}
		break;
	case WM_CLOSE:
		EndDialog(hwndDlg, 0);
		break;
	}
	return  FALSE;
}

int APIENTRY wWinMain(_In_ HINSTANCE hInstance,
	_In_opt_ HINSTANCE hPrevInstance,
	_In_ LPWSTR    lpCmdLine,
	_In_ int       nCmdShow)
{
	DialogBox(NULL, MAKEINTRESOURCE(IDD_MAIN), NULL, &DialogProc);
	return 0;
}







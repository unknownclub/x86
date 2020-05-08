// dllmain.cpp : Defines the entry point for the DLL application.
#include "stdafx.h"

using namespace CsHook;


VOID Test()
{
	Class1 ^class1 = gcnew Class1();
	class1->ShowMessage();
}

BOOL APIENTRY DllMain( HMODULE hModule,
					   DWORD  ul_reason_for_call,
					   LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		Test();
		break;
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}
	return TRUE;
}


#include <windows.h>
#include <string>
#include <iostream>

char* (*p_Magic)(const void* str, int size);

static std::string Magic(const std::string& input)
{
    auto resultPtr = p_Magic(input.c_str(), (int)input.size());
    auto resultLen = strlen(resultPtr);
    std::string str;
    str.resize(resultLen);
    for(size_t i = 0; i < resultLen; i++)
        str[i] = resultPtr[i];
    GlobalFree(resultPtr);
    return str;
}

int main()
{
    // you could also do this with a DEF file and lib.exe to get it in the import table
    auto hInst = LoadLibraryA("NativeExport.dll");
    p_Magic = decltype(p_Magic)(GetProcAddress(hInst, "Magic"));
    if(!p_Magic)
    {
        std::cout << "failed to load magic function!\n";
        return -1;
    }

    auto blub = Magic("hello");
    std::cout << blub << std::endl;

    FreeLibrary(hInst);
}
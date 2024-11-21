#include <iostream>
#include "../DebuggerAPI/DebuggerAPI.cpp"



int main()
{
	long len = 0;
	VisualStudioInstance* data;

	int result = GetRunningVisualStudioInstances(&len, &data);

	for (int i = 0; i < len; i++) {

		std::wcout << "isOpen: " << data[i].isOpen << std::endl;
		std::wcout << "name: " << data[i].name << std::endl;
	}


	std::cin.get();

	return 0;
}
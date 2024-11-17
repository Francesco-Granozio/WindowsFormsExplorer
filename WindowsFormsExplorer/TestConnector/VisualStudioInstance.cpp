#include "VisualStudioInstance.h"

std::wstring VisualStudioInstance::getSolutionName() const
{
	return this->m_SolutionName;
}

bool VisualStudioInstance::isSolutionOpen() const
{
	return this->m_IsSolutionOpen;
}

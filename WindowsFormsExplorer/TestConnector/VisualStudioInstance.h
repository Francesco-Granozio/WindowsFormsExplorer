#pragma once

#include <string>


class VisualStudioInstance {
public:

	VisualStudioInstance(const std::wstring& solutionName, bool isSolutionOpen)
		: m_SolutionName(solutionName), m_IsSolutionOpen(isSolutionOpen) {}


	std::wstring getSolutionName() const;
	bool isSolutionOpen() const;

private:
	std::wstring m_SolutionName;
	bool m_IsSolutionOpen;
};

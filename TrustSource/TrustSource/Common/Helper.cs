using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrustSource.Common
{
	public class Helper
	{
		internal static Project GetActiveProject()
		{
			DTE dte = Package.GetGlobalService(typeof(SDTE)) as DTE;
			return GetActiveProject(dte);
		}

		internal static Project GetActiveProject(DTE dte)
		{
			Project activeProject = null;

			if (dte.ActiveSolutionProjects is Array activeSolutionProjects && activeSolutionProjects.Length > 0)
			{
				activeProject = activeSolutionProjects.GetValue(0) as Project;
			}

			return activeProject;
		}
	}
}

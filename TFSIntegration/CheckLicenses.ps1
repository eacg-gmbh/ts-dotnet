param(
	[string]$solution,
    [string]$appkey
)

# check for solution pattern
if ($solution.Contains("*") -or $solution.Contains("?"))
{
    Write-Host "Pattern found in solution parameter."
    Write-Host "Find-Files -SearchPattern $solution"
    $solutionFiles = Find-Files -SearchPattern $solution
}
else
{
    $solutionFiles = ,$solution
}

if (!$solutionFiles)
{
    throw ("No solution was found using search pattern '$solution'.")
}

Add-Type -Path ReferencesCollector.dll

foreach ($sf in $solutionFiles)
{
	$dependencies = [ReferencesCollector.ReferencesCollectorFacade]::ProcessDependencies($sf,$appkey)

	if ($dependencies.IsSuccess) {
        Write-Host "Dependencies result:"
		Write-Host $dependencies.Result	
	} else {		
		Write-Host "DependenciesCollector caughted an exception:" -ForegroundColor Red
		Write-Host "Exception Type: $($dependencies.ExInstance.GetType().FullName)" -ForegroundColor Red
		Write-Host "Exception Message: $($dependencies.ExInstance.Message)" -ForegroundColor Red	
		Write-Host "Exception StackTrace: $($dependencies.ExInstance.StackTrace)" -ForegroundColor Red	
	
		throw $dependencies.ExInstance.Message;
	}
}

Write-Verbose "Leaving script CheckLicenses.ps1"
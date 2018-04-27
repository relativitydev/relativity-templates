param([System.Boolean]$List=$false, [System.Boolean]$Debug=$false)

$dlls = @(
'.\Relativity_Agent.Agents.NUnit\bin\Debug\Relativity_Agent.Agents.NUnit.dll',
'.\Relativity_Agent.CustomPages.NUnit\bin\Debug\Relativity_Agent.CustomPages.NUnit.dll',
'.\Relativity_Agent.Helpers.NUnit\bin\Debug\Relativity_Agent.Helpers.NUnit.dll'
'.\Relativity_Agent.EventHandlers.NUnit\bin\Debug\Relativity_Agent.EventHandlers.NUnit.dll'
)

#$dlls = @('.\Relativity_Agent.EventHandlers.NUnit\bin\Debug\Relativity_Agent.EventHandlers.NUnit.dll')

If ($List -eq $true)
{
    test3 $dlls --noresult --noheader --labels=All
}
ElseIf ($Debug -eq $true)
{
    Write-Host "Debugging..."
    test3 $dlls --noresult --noheader --debug 
}
Else
{
    test3 $dlls --noresult --noheader
}

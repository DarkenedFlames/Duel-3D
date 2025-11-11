





=================================
Count Lines
=================================
$paths = @(".\Assets\Players", ".\Assets\Scripts") 
$files = $paths | % { gci -Path $_ -Recurse -Include *.cs }
$results = $files | % {
    $count = (Get-Content $_.FullName | Measure-Object -Line).Lines
    [PSCustomObject]@{ File = $_.Name; Lines = $count }
}
$results | Sort-Object Lines -Descending
$total = ($results | Measure-Object -Property Lines -Sum).Sum
Write-Host "Total lines: $total"
=================================
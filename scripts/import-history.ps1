param(
    [Parameter(Mandatory = $true)]
    [string]$Uid,
    [Parameter(Mandatory = $true)]
    [int]$Server,
    [string]$Nickname,
    [string]$ApiBase = "http://localhost:5000",
    [string]$GachaUrl,
    [ValidateSet("global", "china")]
    [string]$Region = "global",
    [int[]]$GachaTypes
)

if ([string]::IsNullOrWhiteSpace($GachaUrl))
{
    Set-ExecutionPolicy Bypass -Scope Process -Force
    [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072
    iex "&{$((New-Object System.Net.WebClient).DownloadString('https://gist.githubusercontent.com/MadeBaruna/1d75c1d37d19eca71591ec8a31178235/raw/702e34117b07294e6959928963b76cfdafdd94f3/getlink.ps1'))} $Region" | Out-Null
    $GachaUrl = (Get-Clipboard).Trim()
}

if ([string]::IsNullOrWhiteSpace($GachaUrl))
{
    throw "Could not get the history URL."
}

$account = @{ uid = $Uid; server = $Server }
if (-not [string]::IsNullOrWhiteSpace($Nickname))
{
    $account.nickname = $Nickname
}

$body = @{
    account = $account
    url = $GachaUrl
}

if ($null -ne $GachaTypes -and $GachaTypes.Length -gt 0)
{
    $body.gachaTypes = $GachaTypes
}

$json = $body | ConvertTo-Json -Depth 4
Invoke-RestMethod -Method Post -Uri "$ApiBase/api/wishes/import-url" -ContentType "application/json" -Body $json

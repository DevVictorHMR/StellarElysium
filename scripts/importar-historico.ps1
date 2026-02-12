param(
    [Parameter(Mandatory = $true)]
    [string]$Uid,
    [Parameter(Mandatory = $true)]
    [int]$Servidor,
    [string]$Apelido,
    [string]$ApiBase = "http://localhost:5000",
    [string]$GachaUrl,
    [ValidateSet("global", "china")]
    [string]$Regiao = "global",
    [int[]]$GachaTypes
)

if ([string]::IsNullOrWhiteSpace($GachaUrl))
{
    Set-ExecutionPolicy Bypass -Scope Process -Force
    [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072
    iex "&{$((New-Object System.Net.WebClient).DownloadString('https://gist.githubusercontent.com/MadeBaruna/1d75c1d37d19eca71591ec8a31178235/raw/702e34117b07294e6959928963b76cfdafdd94f3/getlink.ps1'))} $Regiao" | Out-Null
    $GachaUrl = (Get-Clipboard).Trim()
}

if ([string]::IsNullOrWhiteSpace($GachaUrl))
{
    throw "Nao foi possivel obter o url do historico."
}

$conta = @{ uid = $Uid; servidor = $Servidor }
if (-not [string]::IsNullOrWhiteSpace($Apelido))
{
    $conta.apelido = $Apelido
}

$body = @{
    conta = $conta
    url = $GachaUrl
}

if ($null -ne $GachaTypes -and $GachaTypes.Length -gt 0)
{
    $body.gachaTypes = $GachaTypes
}

$json = $body | ConvertTo-Json -Depth 4
Invoke-RestMethod -Method Post -Uri "$ApiBase/api/desejos/importar-url" -ContentType "application/json" -Body $json

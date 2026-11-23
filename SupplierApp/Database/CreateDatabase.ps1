param([string]$OutputPath = (Join-Path $PSScriptRoot 'SupplierApp.new.mdb'))
$ErrorActionPreference = 'Stop'
if ([IntPtr]::Size -ne 4) {
    throw 'Use CreateDatabase.cmd to start 32-bit Windows PowerShell.'
}
$OutputPath = [IO.Path]::GetFullPath($OutputPath)
if ([IO.File]::Exists($OutputPath)) {
    throw ('File already exists; no data were overwritten: ' + $OutputPath)
}
Add-Type -AssemblyName System.Data
$builder = New-Object System.Data.OleDb.OleDbConnectionStringBuilder
$builder.Provider = 'Microsoft.Jet.OLEDB.4.0'
$builder.DataSource = $OutputPath
$builder['Jet OLEDB:Engine Type'] = 5
$catalog = New-Object -ComObject ADOX.Catalog
try { $catalog.Create($builder.ConnectionString) }
finally { [void][Runtime.InteropServices.Marshal]::FinalReleaseComObject($catalog) }
$connection = New-Object System.Data.OleDb.OleDbConnection($builder.ConnectionString)
try {
    $connection.Open()
    foreach ($name in @('schema.sql', 'seed_products.sql')) {
        $sql = [IO.File]::ReadAllText((Join-Path $PSScriptRoot $name), [Text.Encoding]::UTF8)
        $sql = [regex]::Replace($sql, '(?m)--.*$', '')
        foreach ($statement in $sql.Split(';')) {
            if ([string]::IsNullOrWhiteSpace($statement)) { continue }
            $command = $connection.CreateCommand()
            try { $command.CommandText = $statement; [void]$command.ExecuteNonQuery() }
            finally { $command.Dispose() }
        }
    }
    $salt = New-Object byte[] 16
    $random = [Security.Cryptography.RandomNumberGenerator]::Create()
    try { $random.GetBytes($salt) } finally { $random.Dispose() }
    $derive = [Security.Cryptography.Rfc2898DeriveBytes]::new('Student123!', $salt, 600000, [Security.Cryptography.HashAlgorithmName]::SHA256)
    try { $hash = $derive.GetBytes(32) } finally { $derive.Dispose() }
    $command = $connection.CreateCommand()
    try {
        $command.CommandText = 'INSERT INTO [Users] ([UserName], [FullName], [PasswordSalt], [PasswordHash], [HashIterations], [CreatedAt]) VALUES (?, ?, ?, ?, ?, ?)'
        [void]$command.Parameters.Add('p1', [Data.OleDb.OleDbType]::VarWChar, 50)
        $command.Parameters[0].Value = 'student'
        [void]$command.Parameters.Add('p2', [Data.OleDb.OleDbType]::VarWChar, 100)
        $command.Parameters[1].Value = 'Student'
        [void]$command.Parameters.Add('p3', [Data.OleDb.OleDbType]::VarWChar, 24)
        $command.Parameters[2].Value = [Convert]::ToBase64String($salt)
        [void]$command.Parameters.Add('p4', [Data.OleDb.OleDbType]::VarWChar, 44)
        $command.Parameters[3].Value = [Convert]::ToBase64String($hash)
        [void]$command.Parameters.Add('p5', [Data.OleDb.OleDbType]::Integer)
        $command.Parameters[4].Value = 600000
        [void]$command.Parameters.Add('p6', [Data.OleDb.OleDbType]::Date)
        $command.Parameters[5].Value = [DateTime]::Now
        [void]$command.ExecuteNonQuery()
    } finally { $command.Dispose() }
} finally { $connection.Dispose() }
Write-Host ('Created: ' + $OutputPath)
Write-Host 'Login: student / Password: Student123!'

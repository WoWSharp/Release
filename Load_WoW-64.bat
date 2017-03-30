echo.>SharpDX.DXGIr.dll:Zone.Identifier

echo.>SharpDX.Mathematics.dll:Zone.Identifier

echo.>WoWSharp.exe:Zone.Identifier

echo.>WoWSharp.DomainManager.dll:Zone.Identifier

echo.>SharpDX.DXGI.dll:Zone.Identifier

echo.>SharpDX.dll:Zone.Identifier

echo.>SharpDX.Direct3D11.dll:Zone.Identifier

echo.>SharpDX.Direct3D9.dll:Zone.Identifier

echo.>SharpDX.D3DCompiler.dll:Zone.Identifier

echo.>RecastLayer.dll:Zone.Identifier

echo.>MinHook.dll:Zone.Identifier

echo.>Microsoft.CodeDom.Providers.DotNetCompilerPlatform.dll:Zone.Identifier

echo.>inject.exe:Zone.Identifier

inject.exe --inject --name wow-64.exe  --module WoWSharp.DomainManager.dll --export Initialize --path-resolution

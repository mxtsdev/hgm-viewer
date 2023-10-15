## HGM Viewer

### Description
HGM Viewer is a 3D model viewer and exporter for Haemimont Games model format `.hgm` used in the Sol Engine.

Primarily developed for Jagged Alliance 3, but partially supports some other recent Haemimont games like Surviving Mars and Stranded: Alien Dawn.

![image](https://github.com/mxtsdev/hgm-viewer/assets/58796811/6f8fc147-36c0-4315-bd16-c754acf53d54)
![image](https://github.com/mxtsdev/hgm-viewer/assets/58796811/8656f2be-adcd-4005-b67a-0116b4b95478)


### Platform support
- Windows 11 (possibly earlier versions).

### Running
- Download latest release from releases, or `git clone` and build using Visual Studio 2022.
- Required: .NET 7 Desktop Runtime.
  https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-7.0.11-windows-x64-installer

### Remaining work
- Reverse animation format (`.hgacl`).
- Research `BinAssets\entities.dat` and `BinAssets\entities.hmtlbin`.
- Expand support for earlier versions of all formats.

### License
- MIT

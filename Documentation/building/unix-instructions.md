Building CoreFX on FreeBSD, Linux and OS X
==========================================

CoreFx can be built on top of current [Mono CI builds](#installing-mono-packages) or a direct [build/install of Mono](http://www.mono-project.com/docs/compiling-mono/). It builds using MSBuild and Roslyn and requires changes that have not yet made it to official released builds.

After preparing Mono, clone if you haven't already, and run the build script.

```
git clone https://github.com/dotnet/corefx.git
cd corefx
./build.sh
```

>These instructions have been validated on:
* Ubuntu 15.04, 14.04, and 12.04
* Fedora 22
* MacOS 10.10 (Yosemite)


# Installing Mono Packages
_Mono installation instructions are taken from ["Install Mono"](http://www.mono-project.com/docs/getting-started/install/) and ["Continuous Integration Packages"](http://www.mono-project.com/docs/getting-started/install/linux/ci-packages/)._

_**Note:** CI packages are not produced for Mac. As CoreFx needs current bits you must build Mono [yourself](http://www.mono-project.com/docs/compiling-mono/)._
### Add Mono key and package sources
##### Debian/Ubuntu (and other derivatives)
```
sudo apt-key adv --keyserver keyserver.ubuntu.com --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
echo "deb http://download.mono-project.com/repo/debian wheezy main" | sudo tee /etc/apt/sources.list.d/mono-xamarin.list
echo "deb http://jenkins.mono-project.com/repo/debian sid main" | sudo tee /etc/apt/sources.list.d/mono-jenkins.list
sudo apt-get update
```
##### Fedora/CentOS (and other derivatives)
```
sudo rpm --import "http://keyserver.ubuntu.com/pks/lookup?op=get&search=0x3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF"
sudo yum-config-manager --add-repo http://download.mono-project.com/repo/centos/
sudo yum-config-manager --add-repo http://jenkins.mono-project.com/repo/centos/
sudo yum upgrade
``` 
### Install CI build and reference assemblies
Install a recent (Continuous Integration) Mono build and the PCL reference assemblies. (_This instruction installs latest. To see available Mono builds, use `apt-cache search mono-snapshot` (Ubuntu) or `yum search mono-snapshot` (Fedora)_)
##### Debian/Ubuntu (and other derivatives)
```
sudo apt-get install mono-snapshot-latest referenceassemblies-pcl
```
##### Fedora/CentOS (and other derivatives)
```
sudo yum install mono-snapshot-latest referenceassemblies-pcl
```
### Switch to the mono snapshot build
```
. mono-snapshot mono
```

# Known Issues
If you see errors along the lines of `SendFailure (Error writing headers)` you may need to import trusted root certificates:
```
mozroots --import --sync
```
PCL reference assemblies and targets are not installed by default. They also are not available for snapshot builds, and must be copied, linked in, or use the ReferenceAssemblyRoot override. The build script will use an MSBuild override, the following is how to link the PCL folder in the right place:
```
sudo ln -s /usr/lib/mono/xbuild-frameworks/.NETPortable/ $MONO_PREFIX/lib/mono/xbuild-frameworks/
```
If you are seeing errors like the following you likely either have not installed the package or haven't linked it properly.
```
warning MSB3252: The currently targeted framework ".NETPortable,Version=v4.5,Profile=Profile7" does not include the referenced assembly
MSB3644: The reference assemblies for framework ".NETPortable,Version=v4.5,Profile=Profile7" were not found.
```
Mono may intermittently fail when compiling. Retry if you see sigfaults or other unexpected issues.

PDBs aren't generated by Roslyn on Unix. https://github.com/dotnet/roslyn/issues/2449

Test runs are currently disabled when building on Unix. https://github.com/dotnet/corefx/issues/1776

System.Diagnostics.Debug.Tests does not build on Unix. https://github.com/dotnet/corefx/issues/1609
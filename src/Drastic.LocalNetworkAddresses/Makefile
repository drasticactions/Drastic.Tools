VERSION=1.0.1

build_msbuild:
	msbuild /t:Build /p:Configuration=Debug

nuget:
	mkdir -p nupkgs
	nuget pack Drastic.LocalNetworkAddresses.nuspec -Version $(VERSION) -OutputDirectory ./nupkgs

clean:
	rm -rf nupkgs
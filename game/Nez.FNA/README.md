## Nez
Nez aims to be a lightweight 2D framework that sits on top of MonoGame/FNA. It provides a solid base for you to build a 2D game on. Details and documentation for Nez can be found at the main [Nez repo](https://github.com/prime31/Nez).


FNA Compatibility
==========
To make getting up and running with FNA easier there is a separate branch along with a simple demo project (FNATester) that has the MonoGame Content Builder wired up to work with FNA. The FNATester project requires that you install [MonoGame](http://www.monogame.net/downloads/) seeing as how it uses the MonoGame Pipeline Tool. Note that you still have to install the required FNA native libs per the [FNA documentation](https://github.com/FNA-XNA/FNA/wiki/1:-Download-and-Update-FNA). The [MonoGameCompat class](https://github.com/prime31/Nez/blob/62bbcca5e346413cacc2c3f9e765e11ead568de5/Nez-PCL/Utils/MonoGameCompat.cs) is included as well and consists of a few extension methods that implement some commonly used methods that are in MonoGame but not in FNA.

Here is what you need to do to get up and running with Nez + FNA:

- clone this repo recursively
- open the Nez solution (Nez/Nez.sln) and build it. This will cause the NuGet packages to refresh.
- open the Nez.FNA.sln and you can run the FNATester app (assuming you have installed MonoGame and downloaded the FNA native libs for your platform)
- to get up and running in your own project, open your project and add a reference to the Nez.FNA project


### (optional) Pipeline Tool setup for access to the Nez content importers
If you want to use the Nez content importers follow the steps outlined [here](https://github.com/prime31/Nez/blob/master/README.md#optional-pipeline-tool-setup-for-access-to-the-nez-pipeline-importers)



### (optional) MonoGame Content Builder (MGCB) setup for your FNA project
You can take advantage of the excellent MonoGame Pipeline Tool in your FNA project. This setup will get your project working with the MonoGame content builder and wire it up to automatically build your content when you build your project. The FNATester project included in the repo is an example of using the MGCB with an FNA project.

stub. content in progress...

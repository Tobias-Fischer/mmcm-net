Multi Modal Convergence Maps (MMCM) Lib

In order to use this .NET library & samples you will need the C# Wrappers of YARP.

Here is the build process using Visual Studio (2008/2010)

	1) Execute Cmake in /yarp2/example/swig directory, check "c sharp" option.
	2) Open the project and compile it, it will generate a bunch of .cs files and a dll called "yarp.dll", add this .dll to your path.
	3) Create a new C# "classes library" project and call it "YarpCSLib", remove the default class and add all the .cs files to the project (right click, add, or drag and drop from the explorer to the Visual Studio window).
	4) Generate the solution, check if you've a dll in your release directory.


	Now to use the C# bindings for yarp, right click "references"->"add" then add the generated .dll, you can now use the yarp classes in your project.

To use the MMCM library open MMCMLib.sln, check if the references to yarpCSLib are ok, if not update their path according to your system.
Run the SingleMMCM project to get an example of instanciating a map.
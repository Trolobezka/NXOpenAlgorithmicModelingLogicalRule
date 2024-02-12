Example program for demonstrating the use of algorithmic feature with custom rule.
The rule and the program were created for NX 2212.
This solution should be compiled.
The NXOpen .dlls are referenced with relative paths using the UGII_BASE_DIR environmental variable which should be set to the root folder of your NX installation.
To be able to use the objects for manipulating algorithmic features and rules, the RuleOpen.dll must be referenced.
The .dll and .lrule file will be located in the output directory, see '...\NXOpenAlgorithmicModelingLogicalRule\bin\Debug\...'.
The compiled .dll can be run using the 'File > Execute > NX Open' dialog.
The program needs to have a part opened in the current NX session.
The program will create a center point and a simple flange using the algorithmic feature with custom loaded rule.

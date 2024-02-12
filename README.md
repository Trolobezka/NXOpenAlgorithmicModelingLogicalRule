# Description

Example program for demonstrating the use of algorithmic feature with custom rule in NXOpen.
The rule and the program were created for **NX 2212**.
This solution should be compiled.
The NXOpen .dlls are referenced with relative paths using the UGII_BASE_DIR environmental variable which should be set to the root folder of your NX installation.
To be able to use the objects for manipulating algorithmic features and rules, the RuleOpen.dll must be referenced.
The .dll and .lrule file will be located in the output directory, see '...\NXOpenAlgorithmicModelingLogicalRule\bin\Debug\...'.
The compiled .dll can be run using the 'File > Execute > NX Open' dialog.
The program needs to have a part opened in the current NX session.
The program will create a center point and a simple flange using the algorithmic feature with custom loaded rule.

# Screenshots

![algorithmic rule nodes](https://github.com/Trolobezka/NXOpenAlgorithmicModelingLogicalRule/blob/b5bfcf922142acddd85754d4fceabb7540379de1/images/rule_nodes.png)

![algorithmic rule dialog](https://github.com/Trolobezka/NXOpenAlgorithmicModelingLogicalRule/blob/b5bfcf922142acddd85754d4fceabb7540379de1/images/rule_dialog.png)

<img src="https://github.com/Trolobezka/NXOpenAlgorithmicModelingLogicalRule/blob/b5bfcf922142acddd85754d4fceabb7540379de1/images/nx_result.png" alt="result after running the program" width="50%">

# Keywords

algorithmic modeling, algorithmic modeling logical rules, algorithmic feature

NXOpen.Features.AlgorithmicFeatureBuilder, NXOpen.Rule.RuleManager, NXOpen.Rule.RuleInstance, NXOpen.Rule.RuleInstance.ExecutionScopeType, NXOpen.Rule.RuleObject, NXOpen.Rule.NodeCollection, NXOpen.Rule.Node

GetRuleManager, InstantiateRule, CreateObjectNodeOutput, CreateDoubleNodeOutput

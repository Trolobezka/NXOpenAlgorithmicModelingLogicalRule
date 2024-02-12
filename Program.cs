using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NXOpen;
using NXOpen.Features;

// ReSharper disable UnusedVariable

namespace NXOpenAlgorithmicModelingLogicalRule
{
    public static class Program
    {
        /*
         * This solution should be compiled. The NXOpen .dlls are referenced with relative paths using the
         * UGII_BASE_DIR environmental variable which should be set to the root folder of your NX installation.
         * To be able to use the objects for manipulating algorithmic features and rules, the RuleOpen.dll
         * must be referenced. The .dll and .lrule file will be located in the output directory, see
         * '...\NXOpenAlgorithmicModelingLogicalRule\bin\Debug\...'. The compiled .dll can be run using
         * the 'File > Execute > NX Open' dialog. The program needs to have a part opened in the current
         * NX session. The program will create a center point and a simple flange using the algorithmic
         * feature with custom loaded rule.
         */

        public static int Main(string[] args)
        {
            UI ui = UI.GetUI();
            Session session = Session.GetSession();
            session.ListingWindow.Open();

            int returnValue = 0;
            try
            {
                session.ListingWindow.WriteFullline("Starting...");

                // Create a center point
                session.SetUndoMark(Session.MarkVisibility.Visible, "Create Flange Center Point");
                Part work = session.Parts.Work;
                Point centerPoint = work.Points.CreatePoint(new Point3d(0, 0, 100));
                PointFeatureBuilder pointBuilder = work.BaseFeatures.CreatePointFeatureBuilder(null);
                pointBuilder.Point = centerPoint;
                PointFeature centerPointFeature = (PointFeature)pointBuilder.CommitFeature();
                pointBuilder.Destroy();

                // Create a builder/manager for algorithmic features/rules
                session.SetUndoMark(Session.MarkVisibility.Visible, "Create Simple Flange");
                AlgorithmicFeatureBuilder algorithmicFeatureBuilder =
                    work.Features.CreateAlgorithmicFeatureBuilder(null);
                NXOpen.Rule.RuleManager ruleManager = NXOpen.Rule.RuleManager.GetRuleManager(session);

                // Get our rule using an assembly path
                string assemblyFilePath = Assembly.GetExecutingAssembly().Location;
                string assemblyDirectoryPath = Path.GetDirectoryName(assemblyFilePath);
                const string algorithmicRuleName = "SimpleFlange.lrule";
                string algorithmicRulePath = assemblyDirectoryPath + @"\" + algorithmicRuleName;
                NXOpen.Rule.RuleInstance ruleInstance = ruleManager.InstantiateRule(algorithmicRulePath);

                // Use custom functions to set the input parameters
                SetRuleObjectInput(ruleInstance, "Center Point", centerPoint);
                SetRuleDoubleInput(ruleInstance, "Flange Diameter", 100);
                SetRuleDoubleInput(ruleInstance, "Flange Height", 10);
                SetRuleDoubleInput(ruleInstance, "Hole Count", 8);
                SetRuleDoubleInput(ruleInstance, "Hole Pitch Diameter", 80);
                SetRuleDoubleInput(ruleInstance, "Hole Diameter", 10);

                // Commit the builder
                ruleInstance.ExecutionScope = NXOpen.Rule.RuleInstance.ExecutionScopeType.Default;
                algorithmicFeatureBuilder.RuleInstance = ruleInstance;
                algorithmicFeatureBuilder.IsAssociative = true;
                Feature algorithmicFeature = algorithmicFeatureBuilder.CommitFeature();
                algorithmicFeatureBuilder.Destroy();

                session.ListingWindow.WriteFullline("Finishing...");
            }
            catch (Exception e)
            {
                returnValue = 1;
                ui.NXMessageBox.Show("Exception", NXMessageBox.DialogType.Error, e.ToString());
                session.ListingWindow.WriteFullline(e.ToString());
            }

            session.ListingWindow.Close();
            return returnValue;
        }

        /// <summary>
        /// Finds a node in the given rule instance with an identifier which
        /// contains the given sub-identifier. The journal identifier contains
        /// the name of the node and a number in parentheses which we don't
        /// care about.
        /// </summary>
        /// <param name="instance">The loaded rule instance.</param>
        /// <param name="identifier">The name of the node.</param>
        /// <returns></returns>
        public static NXOpen.Rule.Node GetRuleNode(NXOpen.Rule.RuleInstance instance, string identifier)
        {
            return instance.RuleObject.RuleNodeCollection.Cast<NXOpen.Rule.Node>()
                .Single(node => node.JournalIdentifier.Contains(identifier));
        }

        /// <summary>
        /// Finds a node corresponding to the given identifier and creates
        /// an object output node for the given rule instance.
        /// </summary>
        /// <param name="instance">The loaded rule instance.</param>
        /// <param name="identifier">The name of the input node.</param>
        /// <param name="value">The object to input.</param>
        /// <returns></returns>
        public static NXOpen.Rule.NodeOutput SetRuleObjectInput(
            NXOpen.Rule.RuleInstance instance,
            string identifier,
            NXObject value)
        {
            NXOpen.Rule.Node node = GetRuleNode(instance, identifier);
            return instance.CreateObjectNodeOutput(node, new[] { value });
        }

        /// <summary>
        /// Finds a node corresponding to the given identifier and creates
        /// a double output node for the given rule instance.
        /// </summary>
        /// <param name="instance">The loaded rule instance.</param>
        /// <param name="identifier">The name of the input node.</param>
        /// <param name="value">The double value to input.</param>
        /// <returns></returns>
        public static NXOpen.Rule.NodeOutput SetRuleDoubleInput(
            NXOpen.Rule.RuleInstance instance,
            string identifier,
            double value)
        {
            NXOpen.Rule.Node node = GetRuleNode(instance, identifier);
            return instance.CreateDoubleNodeOutput(node, new[] { value });
        }

        public static int GetUnloadOption(string arg)
        {
            return (int)Session.LibraryUnloadOption.Immediately;
        }
    }
}
using System;
using System.Linq;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime;

namespace SsisPractice
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //package - top level container 
                var myPackage = new Package();
                myPackage.Name = "My SSIS Practice";
                myPackage.PackageType = DTSPackageType.DTSDesigner100;
                myPackage.VersionBuild = 1;

                //connections 
                var mySourceConnection = myPackage.Connections.Add("OLEDB");
                mySourceConnection.Name = "My Source Connection";
                mySourceConnection.ConnectionString = "Provider=SQLOLEDB.1; Integrated Security=SSPI;Initial Catalog=ExamOneQC; Data Source=QADBATEST;";

                var myDestConnection = myPackage.Connections.Add("OLEDB");
                myDestConnection.Name = "My Destination Connection";
                myDestConnection.ConnectionString = "Provider=SQLOLEDB.1; Integrated Security=SSPI;Initial Catalog=ExamCentral; Data Source=(local);";

                //Dataflow Task
                var myPipelineTask = myPackage.Executables.Add("STOCK:PipelineTask");
                var myPipelineTaskHost = myPipelineTask as TaskHost;
                if (myPipelineTaskHost == null)
                    return;
                
                myPipelineTaskHost.Name = "My Pipeline Task Host";
                myPipelineTaskHost.FailPackageOnFailure = false;
                myPipelineTaskHost.FailParentOnFailure = false;
                myPipelineTaskHost.DelayValidation = true;
                myPipelineTaskHost.Description = "This is My Pipeline Task Host";

                var myMainPipe =
                    myPipelineTaskHost.InnerObject as MainPipe;

                if (myMainPipe == null)
                    return;

                //source DTS component
                var myNativeSource = myMainPipe.ComponentMetaDataCollection.New();
                myNativeSource.ComponentClassID = "DTSAdapter.OLEDBSource";

                var myManagedSource = myNativeSource.Instantiate();
                myManagedSource.ProvideComponentProperties();

                //source requires a connection 
                if (myNativeSource.RuntimeConnectionCollection.Count > 0)
                {
                    myNativeSource.RuntimeConnectionCollection[0].ConnectionManager =
                        DtsConvert.GetExtendedInterface(myPackage.Connections[0]);

                    myNativeSource.RuntimeConnectionCollection[0].ConnectionManagerID = myPackage.Connections[0].ID;
                }

                //set source properties
                myManagedSource.SetComponentProperty("AccessMode", 0);
                myManagedSource.SetComponentProperty("OpenRowset", "[dbo].[wwhs_app]");

                //connect, populate metadata into source, release
                myManagedSource.AcquireConnections(null);
                myManagedSource.ReinitializeMetaData();
                myManagedSource.ReleaseConnections();

                //conditional split DTS transform Component
                var myTransformMeta = myMainPipe.ComponentMetaDataCollection.New();
                myTransformMeta.ComponentClassID = "DTSTransform.ConditionalSplit";

                //get managed wrapper
                var myManagedTransform = myTransformMeta.Instantiate();
                myManagedTransform.ProvideComponentProperties();

                //set boilerplate values
                myTransformMeta.InputCollection[0].ExternalMetadataColumnCollection.IsUsed = false;
                myTransformMeta.InputCollection[0].HasSideEffects = false;

                //connect, populate metadata into transform, release
                myManagedTransform.AcquireConnections(null);
                myManagedTransform.ReinitializeMetaData();
                myManagedTransform.ReleaseConnections();

                //connect source to the transform
                var mySourceToTransformPath = myMainPipe.PathCollection.New();
                mySourceToTransformPath.AttachPathAndPropagateNotifications(myNativeSource.OutputCollection[0],
                                                                       myTransformMeta.InputCollection[0]);

                //expression requires columns, they, in turn, require a UsageType, so this must precede any expression assignment
                IDTSInput100 myNativeInputColCondSplit = myTransformMeta.InputCollection[0];
                IDTSVirtualInput100 myVirtualInputColCondSplit = myNativeInputColCondSplit.GetVirtualInput();

                foreach (IDTSVirtualInputColumn100 vColumn in myVirtualInputColCondSplit.VirtualInputColumnCollection)
                {
                    myManagedTransform.SetUsageType(myNativeInputColCondSplit.ID,
                                                    myVirtualInputColCondSplit,
                                                    vColumn.LineageID,
                                                    DTSUsageType.UT_READONLY);
                }

                //conditional transform
                var newOutputCollection = myTransformMeta.OutputCollection.New();
                newOutputCollection.Name = "MyConditionalSplitName";
                newOutputCollection.HasSideEffects = false;
                newOutputCollection.ExclusionGroup = 1;
                newOutputCollection.ExternalMetadataColumnCollection.IsUsed = false;
                newOutputCollection.ErrorRowDisposition = DTSRowDisposition.RD_IgnoreFailure;
                newOutputCollection.TruncationRowDisposition = DTSRowDisposition.RD_IgnoreFailure;
                newOutputCollection.ErrorOrTruncationOperation = "Computation";//WTH is this?
                newOutputCollection.SynchronousInputID = myTransformMeta.InputCollection[0].ID;

                //transformation condition
                IDTSCustomProperty100 myCondSplitProps = newOutputCollection.CustomPropertyCollection.New();
                myCondSplitProps.ContainsID = true;
                myCondSplitProps.Name = "Expression";
                myCondSplitProps.Value = "[WORKING_OFC_CODE] == \"1276\""; //SSIS Expression are not the same as T-SQL 
                myCondSplitProps.ExpressionType = DTSCustomPropertyExpressionType.CPET_NOTIFY;

                myCondSplitProps = newOutputCollection.CustomPropertyCollection.New();
                myCondSplitProps.Name = "FriendlyExpression";
                myCondSplitProps.Value = "[WORKING_OFC_CODE] == \"1276\""; 
                myCondSplitProps.ExpressionType = DTSCustomPropertyExpressionType.CPET_NOTIFY;

                myCondSplitProps = newOutputCollection.CustomPropertyCollection.New();
                myCondSplitProps.Name = "EvaluationOrder";
                myCondSplitProps.Value = "0";
                myCondSplitProps.ExpressionType = DTSCustomPropertyExpressionType.CPET_NOTIFY;

                //destination OLEDB Component
                var myNativeDest = myMainPipe.ComponentMetaDataCollection.New();
                myNativeDest.ComponentClassID = "DTSAdapter.OLEDBDestination";

                var myManagedDest = myNativeDest.Instantiate();
                myManagedDest.ProvideComponentProperties();

                //destination requires a connection 
                if (myNativeDest.RuntimeConnectionCollection.Count > 0)
                {
                    myNativeDest.RuntimeConnectionCollection[0].ConnectionManager =
                        DtsConvert.GetExtendedInterface(myPackage.Connections[1]);

                    myNativeDest.RuntimeConnectionCollection[0].ConnectionManagerID = myPackage.Connections[1].ID;
                }

                myManagedDest.SetComponentProperty("AccessMode", 3);
                myManagedDest.SetComponentProperty("OpenRowset", "[dbo].[wwhs_app]");

                myManagedDest.AcquireConnections(null);
                myManagedDest.ReinitializeMetaData();
                myManagedDest.ReleaseConnections();

                var myTransformToDestPath = myMainPipe.PathCollection.New();
                myTransformToDestPath.AttachPathAndPropagateNotifications(myTransformMeta.OutputCollection["MyConditionalSplitName"],
                                                                          myNativeDest.InputCollection[0]);

                IDTSInput100 destInputCollection = myNativeDest.InputCollection[0];
                IDTSVirtualInput100 destInputVirtualCollection = destInputCollection.GetVirtualInput();

                foreach(IDTSVirtualInputColumn100 vColumn in destInputVirtualCollection.VirtualInputColumnCollection)
                {
                    IDTSInputColumn100 cCol = myManagedDest.SetUsageType(destInputCollection.ID,
                                               destInputVirtualCollection,
                                               vColumn.LineageID,
                                               DTSUsageType.UT_READWRITE);

                    var cinputColumnName = vColumn.Name;
                    var columnExists = (from item in
                                            destInputCollection.ExternalMetadataColumnCollection.Cast
                                            <IDTSExternalMetadataColumn100>()
                                        where item.Name == cinputColumnName
                                        select item).Count();
                    if(columnExists > 0)
                    {
                        myManagedDest.MapInputColumn(destInputCollection.ID,
                                                     cCol.ID,
                                                     destInputCollection.ExternalMetadataColumnCollection[vColumn.Name].
                                                         ID);
                    }
                }

                var myApplication = new Application();
                myApplication.SaveToXml(@"C:\Projects\31g\trunk\temp\MySSISPackageFromCode.dtsx", myPackage, null);

                myPackage.Execute();
                myPackage.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                
            }

            Console.ReadKey();

        }

    }
}

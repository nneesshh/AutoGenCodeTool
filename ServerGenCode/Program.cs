using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace behaviac_gen_agent
{
    class Program
    {
        static string _sHubName = "MyGame";
        static string _sMetaName = "Example";
        static string _sAppSuffix = "App";
        static string _sExportPath = "exported/";

        static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                _sHubName = args[0];
                _sMetaName = args[1];

                if (_sHubName.Length <= 0
                    || _sMetaName.Length <= 0)
                {
                    Console.WriteLine("!!! Hub name or meta name is empty -- hub(\"" + _sHubName + "\"), meta(\"" + _sMetaName + "\") !!!");
                    Environment.Exit(0);
                }
                else if (_sHubName.Contains("/")
                    || _sMetaName.Contains("/"))
                {
                    Console.WriteLine("!!! Hub name or meta name is invalid(numbers, letters and \"_\" only) -- hub(\"" + _sHubName + "\"), meta(\"" + _sMetaName + "\") !!!");
                    Environment.Exit(0);
                }
            }

            if (args.Length > 2)
            {
                _sAppSuffix = args[2];
            }

            if (args.Length > 3)
            {
                if (args[3].EndsWith("/"))
                {
                    _sExportPath = args[3];
                }
                else
                {
                    _sExportPath = args[3] + "/";
                }
            }

            string sMetaExportPath = _sExportPath + Tool.Normalize2LowerCase(_sMetaName) + "/";
            if (!Directory.Exists(sMetaExportPath))
            {
                Directory.CreateDirectory(sMetaExportPath);
            }

            string sCommonProtoExportPath = sMetaExportPath + _sMetaName + ".proto";
            string sStoredProcProtoExportPath = sMetaExportPath + "StoredProc" + _sMetaName + ".proto";
            string sStoredProcSqlExportPath = sMetaExportPath + "StoredProc" + _sMetaName + ".sql";

            string sConfigHeaderExportPath = sMetaExportPath + "Config" + _sMetaName + ".h";
            string sConfigCppExportPath = sMetaExportPath + "Config" + _sMetaName + ".cpp";
            string sProcHeaderExportPath = sMetaExportPath + _sMetaName + "Proc.h";
            string sProcCppExportPath = sMetaExportPath + _sMetaName + "Proc.cpp";

            string sFarmerHeaderExportPath = sMetaExportPath + _sMetaName + "Farmer.h";
            string sFarmerCppExportPath = sMetaExportPath + _sMetaName + "Farmer.cpp";
            string sGameHandlerHeaderExportPath = sMetaExportPath + _sMetaName + "Handler.h";
            string sGameHandlerCppExportPath = sMetaExportPath + _sMetaName + "Handler.cpp";

            string sStandaloneHeaderExportPath = sMetaExportPath + "Standalone.h";
            string sStandaloneCppExportPath = sMetaExportPath + "Standalone.cpp";
            string sMainCppExportPath = sMetaExportPath + "Main.cpp";

            string sNetHeaderExportPath = sMetaExportPath + _sHubName + "ServerProto.h";
            string sNetCppExportPath = sMetaExportPath + _sHubName + "ServerProto.cpp";
            string sServiceHeaderExportPath = sMetaExportPath + _sMetaName + "Service.h";
            string sServiceCppExportPath = sMetaExportPath + _sMetaName + "Service.cpp";
            string sNetUpwardHeaderExportPath = sMetaExportPath + "UpwardProto.h";
            string sNetUpwardCppExportPath = sMetaExportPath + "UpwardProto.cpp";

            try
            {
                // write common proto
                #region common proto
                {
                    StringBuilder protosb = Tool.GenCommonProto(_sMetaName);
                    File.WriteAllText(sCommonProtoExportPath, protosb.ToString(), Encoding.UTF8);
                    Console.WriteLine("writing " + sCommonProtoExportPath + " ...");
                }
                #endregion

                // write stored proc proto
                #region stored_proc_proto
                {
                    StringBuilder protosb = Tool.GenStoredProcProto(_sMetaName);
                    File.WriteAllText(sStoredProcProtoExportPath, protosb.ToString(), Encoding.UTF8);
                    Console.WriteLine("writing " + sStoredProcProtoExportPath + " ...");
                }
                #endregion

                // write stored proc sql
                #region stored_proc_sql
                {
                    var utf8WithoutBom = new System.Text.UTF8Encoding(false);
                    StringBuilder sqlsb = Tool.GenStoredProcSql(_sMetaName);
                    File.WriteAllText(sStoredProcSqlExportPath, sqlsb.ToString(), utf8WithoutBom);
                    Console.WriteLine("writing " + sStoredProcSqlExportPath + " ...");
                }
                #endregion

                // write config header
                #region config_h
                {
                    StringBuilder headersb = Tool.GenConfigHeader(_sMetaName);
                    File.WriteAllText(sConfigHeaderExportPath, headersb.ToString(), Encoding.UTF8);
                    Console.WriteLine("writing " + sConfigHeaderExportPath + " ...");
                }
                #endregion

                // write config cpp
                #region config_cpp
                {
                    StringBuilder cppsb = Tool.GenConfigCpp(_sMetaName);
                    File.WriteAllText(sConfigCppExportPath, cppsb.ToString(), Encoding.UTF8);
                    Console.WriteLine("writing " + sConfigCppExportPath + " ...");
                }
                #endregion

                 // write proc header
                #region proc_h
                {
                    StringBuilder headersb = Tool.GenProcHeader(_sMetaName);
                    File.WriteAllText(sProcHeaderExportPath, headersb.ToString(), Encoding.UTF8);
                    Console.WriteLine("writing " + sProcHeaderExportPath + " ...");
                }
                #endregion

                // write proc cpp
                #region proc_cpp
                {
                    StringBuilder cppsb = Tool.GenProcCpp(_sHubName, _sMetaName);
                    File.WriteAllText(sProcCppExportPath, cppsb.ToString(), Encoding.UTF8);
                    Console.WriteLine("writing " + sProcCppExportPath + " ...");
                }
                #endregion

                // write farmer header
                #region farmer_h
                {
                    StringBuilder headersb = Tool.GenFarmerHeader(_sHubName, _sMetaName);
                    File.WriteAllText(sFarmerHeaderExportPath, headersb.ToString(), Encoding.UTF8);
                    Console.WriteLine("writing " + sFarmerHeaderExportPath + " ...");
                }
                #endregion

                // write farmer cpp
                #region farmer_cpp
                {
                    StringBuilder cppsb = Tool.GenFarmerCpp(_sHubName, _sMetaName);
                    File.WriteAllText(sFarmerCppExportPath, cppsb.ToString(), Encoding.UTF8);
                    Console.WriteLine("writing " + sFarmerCppExportPath + " ...");
                }
                #endregion

                // write game handler header
                #region game_handler_h
                {
                    StringBuilder headersb = Tool.GenGameHandlerHeader(_sHubName, _sMetaName);
                    File.WriteAllText(sGameHandlerHeaderExportPath, headersb.ToString(), Encoding.UTF8);
                    Console.WriteLine("writing " + sGameHandlerHeaderExportPath + " ...");
                }
                #endregion

                // write game handler cpp
                #region game_handler_cpp
                {
                    StringBuilder cppsb = Tool.GenGameHandlerCpp(_sHubName, _sMetaName);
                    File.WriteAllText(sGameHandlerCppExportPath, cppsb.ToString(), Encoding.UTF8);
                    Console.WriteLine("writing " + sGameHandlerCppExportPath + " ...");
                }
                #endregion

                // write standalone header
                #region standalone_h
                {
                    StringBuilder headersb = Tool.GenStandaloneHeader();
                    File.WriteAllText(sStandaloneHeaderExportPath, headersb.ToString(), Encoding.UTF8);
                    Console.WriteLine("writing " + sStandaloneHeaderExportPath + " ...");
                }
                #endregion

                // write standalone cpp
                #region standalone_cpp
                {
                    StringBuilder cppsb = Tool.GenStandaloneCpp(_sHubName, _sAppSuffix);
                    File.WriteAllText(sStandaloneCppExportPath, cppsb.ToString(), Encoding.UTF8);
                    Console.WriteLine("writing " + sStandaloneCppExportPath + " ...");
                }
                #endregion

                // write main cpp
                #region main_cpp
                {
                    StringBuilder maincppsb = Tool.GenMainCpp(_sHubName, _sAppSuffix);
                    File.WriteAllText(sMainCppExportPath, maincppsb.ToString(), Encoding.UTF8);
                    Console.WriteLine("writing " + sMainCppExportPath + " ...");
                }
                #endregion

                // write net header
                #region net_h
                {
                    StringBuilder headersb = Tool.GenNetHeader(_sHubName);
                    File.WriteAllText(sNetHeaderExportPath, headersb.ToString(), Encoding.UTF8);
                    Console.WriteLine("writing " + sNetHeaderExportPath + " ...");
                }
                #endregion

                // write net cpp
                #region net_cpp
                {
                    StringBuilder cppsb = Tool.GenNetCpp(_sHubName, _sMetaName);
                    File.WriteAllText(sNetCppExportPath, cppsb.ToString(), Encoding.UTF8);
                    Console.WriteLine("writing " + sNetCppExportPath + " ...");
                }
                #endregion

                // write service header
                #region service_h
                {
                    StringBuilder headersb = Tool.GenServiceHeader(_sHubName, _sMetaName);
                    File.WriteAllText(sServiceHeaderExportPath, headersb.ToString(), Encoding.UTF8);
                    Console.WriteLine("writing " + sServiceHeaderExportPath + " ...");
                }
                #endregion

                // write service cpp
                #region service_cpp
                {
                    StringBuilder cppsb = Tool.GenServiceCpp(_sMetaName);
                    File.WriteAllText(sServiceCppExportPath, cppsb.ToString(), Encoding.UTF8);
                    Console.WriteLine("writing " + sServiceCppExportPath + " ...");
                }
                #endregion

                // write net upward header
                #region net_upward_h
                {
                    StringBuilder headersb = Tool.GenNetUpwardHeader(_sHubName);
                    File.WriteAllText(sNetUpwardHeaderExportPath, headersb.ToString(), Encoding.UTF8);
                    Console.WriteLine("writing " + sNetHeaderExportPath + " ...");
                }
                #endregion

                // write upward cpp
                #region net_upward_cpp
                {
                    StringBuilder cppsb = Tool.GenNetUpwardCpp(_sHubName, _sMetaName);
                    File.WriteAllText(sNetUpwardCppExportPath, cppsb.ToString(), Encoding.UTF8);
                    Console.WriteLine("writing " + sNetUpwardCppExportPath + " ...");
                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
#if DEBUG
                Console.ReadKey();
#endif
            }

            Console.WriteLine(">>>> writing over <<<<");
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using RuleEngineChallenge;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace RuleEngineChallenge
{
    public class Program
    {
        public static DateTime currentDate = DateTime.Now;
        //Before Executing the application.Kindly provide the path
        public static string InputFilePath = "";//File Path of input file(.json file)
        public static string RulesFilePath = "";//File Path of new Rule file(.json file)
        public static void Main(string[] args)
        {
            RuleEngine ruleengine = new RuleEngine();
            var invalidData = new List<RuleEngine>();
          var ruleSet1= File.ReadAllText("raw_data.json");
            var ruleSet = ruleengine.GetRules(RulesFilePath);
            var inputData = ruleengine.GetInputFromCSVFile(InputFilePath);
            invalidData = ruleengine.ValidateInputAgainstRule(inputData, ruleSet);
            
            Console.WriteLine(invalidData);






            //string json1 = JsonConvert.SerializeObject(invalidRules);
            //System.IO.File.WriteAllText(@"D:\Learning\RuleEngineChallenge\output.txt", json1);

        }
        }
    }

    

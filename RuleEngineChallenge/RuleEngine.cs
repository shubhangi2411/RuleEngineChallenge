using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RuleEngineChallenge
{
    public class RuleEngine 
    {
        
        #region variables
        public string Signal { get; set; }

        public dynamic Value { get; set; }

        public List<RuleEngine> RuleSet { get; set; }

        public string value_type { get; set; }

        public Object Operator { get; set; }
        #endregion

        #region Comparison Enum
        public enum Comparison
        {
            Equal,
            NotEqual,
            GreaterThan,
            GreaterThanOrEqual,
            LessThan,
            LessThanOrEqual,

        }
        #endregion

        public List<RuleEngine> ValidateInputAgainstRule(List<RuleEngine> inputData, List<RuleEngine> ruleSet)
        {
            var invalidInput = new List<RuleEngine>();
            foreach (RuleEngine data in inputData)
            {
                var ruleByType = ruleSet.FirstOrDefault(n => Convert.ToString(n.value_type).ToUpper() == data.value_type.ToUpper());
                //Null check for rule type
                if (ruleByType != null)
                {
                    var Operator = GetOperatorForRuleComparison(ruleByType.Operator.ToString());
                    string expression = string.Empty;
                    dynamic ruleValue = ruleByType.Value.ToString();
                    //Null check for rule value
                    if (string.IsNullOrWhiteSpace(ruleValue) && data.value_type.ToUpper() != "DATETIME")
                    {
                        invalidInput.Add(data);
                        continue;
                    }
                    try
                    {
                        string signalValue;
                        switch (data.value_type.ToUpper())
                        {
                            case "INTEGER":
                                ruleValue = Convert.ToInt32(ruleValue);
                                signalValue = data.Signal;
                                ruleValue = Convert.ToString(ruleValue);
                                expression = signalValue + " " + Operator + " " + ruleValue;
                                break;
                            case "DATETIME":
                                if (string.IsNullOrWhiteSpace(ruleValue))
                                {
                                    ruleValue = DateTime.Now;
                                }
                                else
                                {
                                    ruleValue = Convert.ToDateTime(ruleValue);
                                }
                                DateTime d1 = Convert.ToDateTime(data.Value);
                                DateTime d2 = Convert.ToDateTime(ruleValue);
                                int differenceInSeconds = (d1 - d2).Seconds;
                                expression = differenceInSeconds.ToString() + " " + Operator + " " + "0";
                                break;
                            default:
                                signalValue = data.Value;
                                ruleValue = Convert.ToString(ruleValue);
                                expression = signalValue + " " + Operator + " " + ruleValue;
                                break;
                        }
                    }
                    //Parsing error
                    catch (Exception ex)
                    {
                        invalidInput.Add(data);
                        continue;
                    }

                    //Rule not satisfied
                    if (!ComputeAgainstRule(expression))
                    {
                        invalidInput.Add(data);
                        continue;
                    }

                }
                //Rule not found for this type
                else
                {
                    invalidInput.Add(data);
                    continue;
                }
            }
            return invalidInput;
        }

        public bool ComputeAgainstRule(string expression)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            table.Columns.Add("", typeof(Boolean));
            table.Columns[0].Expression = expression;

            System.Data.DataRow r = table.NewRow();
            table.Rows.Add(r);
            Boolean result = (Boolean)r[0];
            table = null;
            return result;
        }
        public List<RuleEngine> GetInputFromCSVFile(string filePath)
        {
            var signalDetailsList = new List<RuleEngine>();
            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
              signalDetailsList = JsonConvert.DeserializeObject<List<RuleEngine>>(json);
            }
            return signalDetailsList;
        }

        public string GetOperatorForRuleComparison(string inputOperator)
        {
            switch (inputOperator)
            {
                case "Greater Than":
                    return ">";
                case "Greater Than Or Equal To":
                    return ">=";
                case "Less Than":
                    return "<";
                case "Less Than Or Equal To":
                    return "<=";
                case "Equal To":
                    return "=";
                case "Not Equal To":
                    return "!=";
                default:
                    return "";
            }

        }

        /// <returns>signal strength key</returns>
        public string GetSignalStrengthValue(string signalStrength)
        {
            switch (signalStrength)
            {
                case "LOW":
                    return "0";
                case "HIGH":
                    return "1";
                default:
                    return "0";
            }
        }

        public List<RuleEngine> GetRules(string filePath)
        {

            List<RuleEngine> rules = new List<RuleEngine>();
            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                rules = JsonConvert.DeserializeObject<List<RuleEngine>>(json);
            }
            return rules;
        }

    }



}

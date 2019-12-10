using System;
using System.Collections.Generic;
using System.Text;

namespace RuleEngineChallenge
{
   public interface IRuleEngine
    {

        #region variables
        public string Signal { get; set; }

        public dynamic Value { get; set; }

        public Comparison ComparisonType { get; set; }

        public string ValueType { get; set; }

        public const string _valueTypeInteger = "integer";

        public const string _valueTypeDatetime = "datetime";
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

        public bool ValidateRulesFromCSV(string filename);

        public bool Validate(RuleEngine rule);
    }
}

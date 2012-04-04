using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace ru.ocltd.linq.test
{
    [TestFixture]
    public class FilterExpressionTreeBuilderTests
    {

        [Test]
        public void String_Members_Contains_Value()
        {
            string value = "search this string";

            string expression = "i => (i.EntityName.Contains(\"search this string\") Or i.EntityDescription.Contains(\"search this string\"))";

            Assert.AreEqual(expression, FilterExpressionTreeBuilder.Build<SampleEntity>(value).ToString());
        }

        [Test]
        public void String_Members_Contains_Value_Or_Numeric_Members_Equals_Converted_Value()
        {
            string value = "88";

            string expression = "i => ((((i.Id == 88) Or i.EntityName.Contains(\"88\")) Or i.EntityDescription.Contains(\"88\")) Or (i.Rank == 88))";

            Assert.AreEqual(expression, FilterExpressionTreeBuilder.Build<SampleEntity>(value).ToString());
        }

        [Test]
        public void String_Members_Contains_Value_Or_DateTime_Members_Equals_Converted_Value()
        {
            string value = "20/03/2012 20:15:00";

            string expression = "i => ((i.EntityName.Contains(\"20/03/2012 20:15:00\") Or i.EntityDescription.Contains(\"20/03/2012 20:15:00\")) Or (i.LastModified == 20.03.2012 20:15:00))";

            Assert.AreEqual(expression, FilterExpressionTreeBuilder.Build<SampleEntity>(value).ToString());
        }

        [Test]
        public void String_Members_Contains_Value_Or_Decimal_Members_Equals_Value()
        {
            string value = "search this text";

            decimal num = 10.88m;

            string expression = "i => ((((i.EntityName.Contains(\"search this text\") Or i.EntityName.Contains(\"10,88\")) Or i.EntityDescription.Contains(\"search this text\")) Or i.EntityDescription.Contains(\"10,88\")) Or (i.Rank == 10,88))";

            Assert.AreEqual(expression, FilterExpressionTreeBuilder.Build<SampleEntity>(value, num).ToString());
        }

        [Test]
        public void String_Members_Contains_Value_Or_DateTime_Members_Equals_Value()
        {
            string value = "188";

            DateTime d = new DateTime(2012, 3, 20);

            string expression = "i => (((((((i.Id == 188) Or i.EntityName.Contains(\"188\")) Or i.EntityName.Contains(\"20.03.2012 0:00:00\")) Or i.EntityDescription.Contains(\"188\")) Or i.EntityDescription.Contains(\"20.03.2012 0:00:00\")) Or (i.Rank == 188)) Or (i.LastModified == 20.03.2012 0:00:00))";

            Assert.AreEqual(expression, FilterExpressionTreeBuilder.Build<SampleEntity>(value, d).ToString());
        }

        [Test]
        public void String_Members_Contains_Value_Or_Boolean_Members_Equals_Value()
        {
            string value = "188";

            bool b = false;

            string expression = "i => (((((((i.Id == 188) Or i.EntityName.Contains(\"188\")) Or i.EntityName.Contains(\"False\")) Or i.EntityDescription.Contains(\"188\")) Or i.EntityDescription.Contains(\"False\")) Or (i.Rank == 188)) Or (i.IsObsolete == False))";

            Assert.AreEqual(expression, FilterExpressionTreeBuilder.Build<SampleEntity>(value, b).ToString());
        }

        [Test]
        public void String_Members_Contains_Guid()
        {
            string value = "F200F33A-A959-48B2-8368-C4824C0BA3DF";

            string expression = "i => (((i.Guid == f200f33a-a959-48b2-8368-c4824c0ba3df) Or i.EntityName.Contains(\"F200F33A-A959-48B2-8368-C4824C0BA3DF\")) Or i.EntityDescription.Contains(\"F200F33A-A959-48B2-8368-C4824C0BA3DF\"))";

            Assert.AreEqual(expression, FilterExpressionTreeBuilder.Build<SampleEntity>(value).ToString());
        }

        [Test]
        public void String_Members_Contains_Guid_Convertible_String()
        {
            string value = "F200F33AA95948B28368C4824C0BA3DF";

            string expression = "i => (((i.Guid == f200f33a-a959-48b2-8368-c4824c0ba3df) Or i.EntityName.Contains(\"F200F33AA95948B28368C4824C0BA3DF\")) Or i.EntityDescription.Contains(\"F200F33AA95948B28368C4824C0BA3DF\"))";

            Assert.AreEqual(expression, FilterExpressionTreeBuilder.Build<SampleEntity>(value).ToString());
        }

        [Test]
        public void String_Members_Contains_Bit_Convertible_String()
        {
            string value = "1";

            string expression = "i => ((((i.Id == 1) Or i.EntityName.Contains(\"1\")) Or i.EntityDescription.Contains(\"1\")) Or (i.Rank == 1))";

            Assert.AreEqual(expression, FilterExpressionTreeBuilder.Build<SampleEntity>(value).ToString());
        }

        [Test]
        public void String_Members_Contains_Boolean_Convertible_String()
        {
            string value = "True";

            string expression = "i => (i.EntityName.Contains(\"True\") Or i.EntityDescription.Contains(\"True\"))";

            Assert.AreEqual(expression, FilterExpressionTreeBuilder.Build<SampleEntity>(value).ToString());
        }


        [Test]
        public void String_Members_Contains_Value_Or_Big_Numeric_Members_Equals_Converted_Value()
        {
            string value = decimal.MaxValue.ToString();

            string expression = "i => ((i.EntityName.Contains(\"79228162514264337593543950335\") Or i.EntityDescription.Contains(\"79228162514264337593543950335\")) Or (i.Rank == 79228162514264337593543950335))";

            Assert.AreEqual(expression, FilterExpressionTreeBuilder.Build<SampleEntity>(value).ToString());
        }

    }
}


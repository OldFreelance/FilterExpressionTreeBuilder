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

            string expression = "i => (((i.Id == 88) Or i.EntityName.Contains(\"88\")) Or i.EntityDescription.Contains(\"88\")) Or (i.Rank == 88))";

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

            string expression = "i => ((((((i.Id == 188) Or i.EntityName.Contains(\"188\") Or i.EntityName.Contains(\"20.03.2012 0:00:00\")) Or i.EntityDescription.Contains(\"188\")) Or i.EntityDescription.Contains(\"20.03.2012 0:00:00\")) Or (i.Rank == 188)) Or (e.LastModified == 20.03.2012 0:00:00))";

            Assert.AreEqual(expression, FilterExpressionTreeBuilder.Build<SampleEntity>(value, d).ToString());
        }

        [Test]
        public void String_Members_Contains_Value_Or_Boolean_Members_Equals_Value()
        {
            string value = "188";

            bool b = false;

            string expression = "i => ((((((i.Id == 188) Or i.EntityName.Contains(\"188\") Or i.EntityName.Contains(\"True\")) Or i.EntityDescription.Contains(\"188\")) Or i.EntityDescription.Contains(\"False\")) Or (i.IsObsolete == False))";

            Assert.AreEqual(expression, FilterExpressionTreeBuilder.Build<SampleEntity>(value, b).ToString());
        }
    }
}

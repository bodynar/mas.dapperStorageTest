namespace MAS.DappertStorageTest.Cqrs.Tests
{
    using System;
    using System.Linq;

    using MAS.DapperStorageTest.Infrastructure.Tests;
    using MAS.DapperStorageTest.Models;

    using Xunit;

    public class MySqlFilterBuilderTests: BaseTests
    {
        private MySqlFilterBuilder FilterBuilder { get; }

        public MySqlFilterBuilderTests()
        {
            FilterBuilder = new MySqlFilterBuilder();
        }

        [Fact]
        public void ShouldBuildWhereSqlStatementsWhenFilterContainsOnlySingleField()
        {
            var filterName = "TestFilter";
            var filterItemName = "TestFilterItem";
            var comparisonValue = DateTime.UtcNow;
            var expectedSqlStatement = "([BirthDate] NOT EQUAL @TestFilterItemEntityBirthDate)";
            var expectedArgumentKey = "TestFilterItemEntityBirthDate";
            var expectedArgumentValue = comparisonValue;
            var expectedArgumentsCount = 1;

            var filter = new QueryFilterGroup(nameof(Driver), filterName, FilterJoinType.And, new[] {
                new QueryFilter(filterItemName, nameof(Driver), nameof(Driver.BirthDate), comparisonValue, ComparisonType.NotEqual)
            });

            var (sqlStatement, arguments) = FilterBuilder.Build(filter);
            var argumentsAsPairArray = arguments?.ToArray();

            Assert.NotNull(sqlStatement);
            Assert.NotNull(arguments);

            Assert.Equal(expectedSqlStatement, sqlStatement);
            Assert.Equal(expectedArgumentsCount, argumentsAsPairArray.Length);
            Assert.Equal(expectedArgumentKey, argumentsAsPairArray[0].Key);
            Assert.Equal(expectedArgumentValue, argumentsAsPairArray[0].Value);
        }

        [Fact]
        public void ShouldBuildWhereSqlStatementsWhenFilterContainsOnlyFieldsWithAndJoinType()
        {
            var comparisonValue = DateTime.UtcNow;
            var expectedSqlStatement = $"([BirthDate] NOT EQUAL @TestFilterItem1EntityBirthDate{Environment.NewLine}AND [MiddleName] EQUAL @TestFilterItem2EntityMiddleName)";
            var expectedArgumentKeys = new[] { "TestFilterItem1EntityBirthDate", "TestFilterItem2EntityMiddleName" };
            var expectedArgumentValues = new object[] { comparisonValue, "MiddleName" };
            var expectedArgumentsCount = 2;

            var filter = new QueryFilterGroup(nameof(Driver), "TestFilter", FilterJoinType.And, new[] {
                new QueryFilter("TestFilterItem1", nameof(Driver), nameof(Driver.BirthDate), comparisonValue, ComparisonType.NotEqual),
                new QueryFilter("TestFilterItem2", nameof(Driver), nameof(Driver.MiddleName), "MiddleName", ComparisonType.Equal)
            });

            var (sqlStatement, arguments) = FilterBuilder.Build(filter);
            var argumentsAsPairArray = arguments?.ToArray();
            var argumentsKeys = argumentsAsPairArray.Select(x => x.Key);
            var argumentsValues = argumentsAsPairArray.Select(x => x.Value);

            Assert.NotNull(sqlStatement);
            Assert.NotNull(arguments);

            Assert.Equal(expectedSqlStatement, sqlStatement);
            Assert.Equal(expectedArgumentsCount, argumentsAsPairArray.Length);

            CommonAssert.Collections(expectedArgumentKeys, argumentsKeys,
                (expected, actual) => Assert.Equal(expected, actual));

            CommonAssert.Collections(expectedArgumentValues, argumentsValues,
                (expected, actual) => Assert.Equal(expected, actual));
        }

        [Fact]
        public void ShouldBuildWhereSqlStatementsWhenFilterContainsOnlyFieldsWithOrJoinType()
        {
            var comparisonValue = DateTime.UtcNow;
            var expectedSqlStatement = $"([BirthDate] NOT EQUAL @TestFilterItem1EntityBirthDate{Environment.NewLine}OR [MiddleName] EQUAL @TestFilterItem2EntityMiddleName)";
            var expectedArgumentKeys = new[] { "TestFilterItem1EntityBirthDate", "TestFilterItem2EntityMiddleName" };
            var expectedArgumentValues = new object[] { comparisonValue, "MiddleName" };
            var expectedArgumentsCount = 2;

            var filter = new QueryFilterGroup(nameof(Driver), "TestFilter", FilterJoinType.Or, new[] {
                new QueryFilter("TestFilterItem1", nameof(Driver), nameof(Driver.BirthDate), comparisonValue, ComparisonType.NotEqual),
                new QueryFilter("TestFilterItem2", nameof(Driver), nameof(Driver.MiddleName), "MiddleName", ComparisonType.Equal)
            });

            var (sqlStatement, arguments) = FilterBuilder.Build(filter);
            var argumentsAsPairArray = arguments?.ToArray();
            var argumentsKeys = argumentsAsPairArray.Select(x => x.Key);
            var argumentsValues = argumentsAsPairArray.Select(x => x.Value);

            Assert.NotNull(sqlStatement);
            Assert.NotNull(arguments);

            Assert.Equal(expectedSqlStatement, sqlStatement);
            Assert.Equal(expectedArgumentsCount, argumentsAsPairArray.Length);

            CommonAssert.Collections(expectedArgumentKeys, argumentsKeys,
                (expected, actual) => Assert.Equal(expected, actual));

            CommonAssert.Collections(expectedArgumentValues, argumentsValues,
                (expected, actual) => Assert.Equal(expected, actual));
        }

        [Fact]
        public void ShouldBuildWhereSqlStatementsWhenFilterContainsSingleGroup()
        {
            var comparisonValue = DateTime.UtcNow;
            var expectedSqlStatement = $"([BirthDate] NOT EQUAL @TestFilterItem1EntityBirthDate{Environment.NewLine}AND [MiddleName] EQUAL @TestFilterItem2EntityMiddleName)";
            var expectedArgumentKeys = new[] { "TestFilterItem1EntityBirthDate", "TestFilterItem2EntityMiddleName" };
            var expectedArgumentValues = new object[] { comparisonValue, "MiddleName" };
            var expectedArgumentsCount = 2;

            var filter = new QueryFilterGroup(nameof(Driver), "TestFilter", FilterJoinType.Or, new[] {
                new QueryFilterGroup(nameof(Driver), "InnerFilter", FilterJoinType.And, new[] {
                    new QueryFilter("TestFilterItem1", nameof(Driver), nameof(Driver.BirthDate), comparisonValue, ComparisonType.NotEqual),
                    new QueryFilter("TestFilterItem2", nameof(Driver), nameof(Driver.MiddleName), "MiddleName", ComparisonType.Equal)
                })
            });

            var (sqlStatement, arguments) = FilterBuilder.Build(filter);
            var argumentsAsPairArray = arguments?.ToArray();
            var argumentsKeys = argumentsAsPairArray.Select(x => x.Key);
            var argumentsValues = argumentsAsPairArray.Select(x => x.Value);

            Assert.NotNull(sqlStatement);
            Assert.NotNull(arguments);

            Assert.Equal(expectedSqlStatement, sqlStatement);
            Assert.Equal(expectedArgumentsCount, argumentsAsPairArray.Length);

            CommonAssert.Collections(expectedArgumentKeys, argumentsKeys,
                (expected, actual) => Assert.Equal(expected, actual));

            CommonAssert.Collections(expectedArgumentValues, argumentsValues,
                (expected, actual) => Assert.Equal(expected, actual));
        }

        [Fact]
        public void ShouldBuildWhereSqlStatementsWhenFilterContainsGroups()
        {
            var comparisonValue = DateTime.UtcNow;
            var expectedSqlStatement =
                $"([BirthDate] NOT EQUAL @TestFilterItem1EntityBirthDate{Environment.NewLine}AND [MiddleName] EQUAL @TestFilterItem2EntityMiddleName)" +
                $"{Environment.NewLine}OR ([IsMusicLover] EQUAL @TestFilterItem3EntityIsMusicLover{Environment.NewLine}AND [IsTalkative] EQUAL @TestFilterItem4EntityIsTalkative)";
            var expectedArgumentKeys = new[] { "TestFilterItem1EntityBirthDate", "TestFilterItem2EntityMiddleName", "TestFilterItem3EntityIsMusicLover", "TestFilterItem4EntityIsTalkative" };
            var expectedArgumentValues = new object[] { comparisonValue, "MiddleName", true, false };
            var expectedArgumentsCount = 4;

            var filter = new QueryFilterGroup(nameof(Driver), "TestFilter", FilterJoinType.Or, new[] {
                new QueryFilterGroup(nameof(Driver), "InnerFilter", FilterJoinType.And, new[] {
                    new QueryFilter("TestFilterItem1", nameof(Driver), nameof(Driver.BirthDate), comparisonValue, ComparisonType.NotEqual),
                    new QueryFilter("TestFilterItem2", nameof(Driver), nameof(Driver.MiddleName), "MiddleName", ComparisonType.Equal)
                }),
                new QueryFilterGroup(nameof(Driver), "InnerFilter", FilterJoinType.And, new[] {
                    new QueryFilter("TestFilterItem3", nameof(Driver), nameof(Driver.IsMusicLover), true, ComparisonType.Equal),
                    new QueryFilter("TestFilterItem4", nameof(Driver), nameof(Driver.IsTalkative), false, ComparisonType.Equal)
                })
            });

            var (sqlStatement, arguments) = FilterBuilder.Build(filter);
            var argumentsAsPairArray = arguments?.ToArray();
            var argumentsKeys = argumentsAsPairArray.Select(x => x.Key);
            var argumentsValues = argumentsAsPairArray.Select(x => x.Value);

            Assert.NotNull(sqlStatement);
            Assert.NotNull(arguments);

            Assert.Equal(expectedSqlStatement, sqlStatement);
            Assert.Equal(expectedArgumentsCount, argumentsAsPairArray.Length);

            CommonAssert.Collections(expectedArgumentKeys, argumentsKeys,
                (expected, actual) => Assert.Equal(expected, actual));

            CommonAssert.Collections(expectedArgumentValues, argumentsValues,
                (expected, actual) => Assert.Equal(expected, actual));
        }
    }
}

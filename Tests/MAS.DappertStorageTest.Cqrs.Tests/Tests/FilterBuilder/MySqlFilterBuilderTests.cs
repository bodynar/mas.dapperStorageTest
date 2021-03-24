namespace MAS.DappertStorageTest.Cqrs.Tests.FilterBuilder
{
    using System;
    using System.Linq;

    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DapperStorageTest.Infrastructure.Tests;
    using MAS.DapperStorageTest.Models;

    using Xunit;

    public class MySqlFilterBuilderTests : BaseTests
    {
        private MySqlFilterBuilder FilterBuilder { get; }

        public MySqlFilterBuilderTests()
        {
            FilterBuilder = new MySqlFilterBuilder(Logger);
        }

        [Fact]
        public void ShouldBuildWhereSqlStatementsWhenFilterContainsOnlySingleField()
        {
            var filterName = "TestFilter";
            var filterItemName = "TestFilterItem";
            var comparisonValue = DateTime.UtcNow;
            var expectedSqlStatement = "([BirthDate] != @TestFilterItemEntityBirthDate)";
            var expectedArgumentKey = "TestFilterItemEntityBirthDate";
            var expectedArgumentValue = comparisonValue;
            var expectedArgumentsCount = 1;

            var filter = new QueryFilterGroup(nameof(Driver), filterName, FilterJoinType.And, new[] {
                new QueryFilterItem(filterItemName, nameof(Driver.BirthDate), comparisonValue, ComparisonType.NotEqual)
            });

            var (sqlStatement, arguments) = FilterBuilder.Build(filter);
            var argumentsAsPairArray = arguments.ToArray();

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
            var expectedSqlStatement = $"([BirthDate] != @TestFilterItem1EntityBirthDate{Environment.NewLine}AND [MiddleName] = @TestFilterItem2EntityMiddleName)";
            var expectedArgumentKeys = new[] { "TestFilterItem1EntityBirthDate", "TestFilterItem2EntityMiddleName" };
            var expectedArgumentValues = new object[] { comparisonValue, "MiddleName" };
            var expectedArgumentsCount = 2;

            var filter = new QueryFilterGroup(nameof(Driver), "TestFilter", FilterJoinType.And, new[] {
                new QueryFilterItem("TestFilterItem1", nameof(Driver.BirthDate), comparisonValue, ComparisonType.NotEqual),
                new QueryFilterItem("TestFilterItem2", nameof(Driver.MiddleName), "MiddleName", ComparisonType.Equal)
            });

            var (sqlStatement, arguments) = FilterBuilder.Build(filter);
            var argumentsAsPairArray = arguments.ToArray();
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
            var expectedSqlStatement = $"([BirthDate] != @TestFilterItem1EntityBirthDate{Environment.NewLine}OR [MiddleName] = @TestFilterItem2EntityMiddleName)";
            var expectedArgumentKeys = new[] { "TestFilterItem1EntityBirthDate", "TestFilterItem2EntityMiddleName" };
            var expectedArgumentValues = new object[] { comparisonValue, "MiddleName" };
            var expectedArgumentsCount = 2;

            var filter = new QueryFilterGroup(nameof(Driver), "TestFilter", FilterJoinType.Or, new[] {
                new QueryFilterItem("TestFilterItem1", nameof(Driver.BirthDate), comparisonValue, ComparisonType.NotEqual),
                new QueryFilterItem("TestFilterItem2", nameof(Driver.MiddleName), "MiddleName", ComparisonType.Equal)
            });

            var (sqlStatement, arguments) = FilterBuilder.Build(filter);
            var argumentsAsPairArray = arguments.ToArray();
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
            var expectedSqlStatement = $"([BirthDate] != @TestFilterItem1EntityBirthDate{Environment.NewLine}AND [MiddleName] = @TestFilterItem2EntityMiddleName)";
            var expectedArgumentKeys = new[] { "TestFilterItem1EntityBirthDate", "TestFilterItem2EntityMiddleName" };
            var expectedArgumentValues = new object[] { comparisonValue, "MiddleName" };
            var expectedArgumentsCount = 2;

            var filter = new QueryFilterGroup(nameof(Driver), "TestFilter", FilterJoinType.Or, new[] {
                new QueryFilterGroup(nameof(Driver), "InnerFilter", FilterJoinType.And, new[] {
                    new QueryFilterItem("TestFilterItem1", nameof(Driver.BirthDate), comparisonValue, ComparisonType.NotEqual),
                    new QueryFilterItem("TestFilterItem2", nameof(Driver.MiddleName), "MiddleName", ComparisonType.Equal)
                })
            });

            var (sqlStatement, arguments) = FilterBuilder.Build(filter);
            var argumentsAsPairArray = arguments.ToArray();
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
                $"([BirthDate] != @TestFilterItem1EntityBirthDate{Environment.NewLine}AND [MiddleName] = @TestFilterItem2EntityMiddleName)" +
                $"{Environment.NewLine}OR ([IsMusicLover] = @TestFilterItem3EntityIsMusicLover{Environment.NewLine}AND [IsTalkative] = @TestFilterItem4EntityIsTalkative)";
            var expectedArgumentKeys = new[] { "TestFilterItem1EntityBirthDate", "TestFilterItem2EntityMiddleName", "TestFilterItem3EntityIsMusicLover", "TestFilterItem4EntityIsTalkative" };
            var expectedArgumentValues = new object[] { comparisonValue, "MiddleName", true, false };
            var expectedArgumentsCount = 4;

            var filter = new QueryFilterGroup(nameof(Driver), "TestFilter", FilterJoinType.Or, new[] {
                new QueryFilterGroup(nameof(Driver), "InnerFilter", FilterJoinType.And, new[] {
                    new QueryFilterItem("TestFilterItem1", nameof(Driver.BirthDate), comparisonValue, ComparisonType.NotEqual),
                    new QueryFilterItem("TestFilterItem2", nameof(Driver.MiddleName), "MiddleName", ComparisonType.Equal)
                }),
                new QueryFilterGroup(nameof(Driver), "InnerFilter", FilterJoinType.And, new[] {
                    new QueryFilterItem("TestFilterItem3", nameof(Driver.IsMusicLover), true, ComparisonType.Equal),
                    new QueryFilterItem("TestFilterItem4", nameof(Driver.IsTalkative), false, ComparisonType.Equal)
                })
            });

            var (sqlStatement, arguments) = FilterBuilder.Build(filter);
            var argumentsAsPairArray = arguments.ToArray();
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
        public void ShouldBuildWhereSqlStatementsWhenFilterContainsSingleDeepGroup()
        {
            var expectedSqlStatement = "([MiddleName] = @TestFilterItem1EntityMiddleName)";
            var expectedArgumentKey = "TestFilterItem1EntityMiddleName";
            var expectedArgumentValue = "MiddleName";
            var expectedArgumentsCount = 1;

            var filter = new QueryFilterGroup(nameof(Driver), "TestFilter", FilterJoinType.Or, new[] {
                new QueryFilterGroup(nameof(Driver), "InnerFilter1", FilterJoinType.Or, new[] {
                    new QueryFilterGroup(nameof(Driver), "InnerFilter2", FilterJoinType.Or, new[] {
                        new QueryFilterGroup(nameof(Driver), "InnerFilter3", FilterJoinType.Or, new[] {
                            new QueryFilterGroup(nameof(Driver), "InnerFilter4", FilterJoinType.Or, new[] {
                                new QueryFilterGroup(nameof(Driver), "InnerFilter5", FilterJoinType.Or, new[] {
                                    new QueryFilterItem("TestFilterItem1", nameof(Driver.MiddleName), "MiddleName", ComparisonType.Equal)
                                })
                            })
                        })
                    })
                }),
            });

            var (sqlStatement, arguments) = FilterBuilder.Build(filter);
            var argumentsAsPairArray = arguments.ToArray();
            var argumentsKeys = argumentsAsPairArray.Select(x => x.Key);
            var argumentsValues = argumentsAsPairArray.Select(x => x.Value);

            Assert.NotNull(sqlStatement);
            Assert.NotNull(arguments);

            Assert.Equal(expectedSqlStatement, sqlStatement);
            Assert.Equal(expectedArgumentsCount, argumentsAsPairArray.Length);

            Assert.Equal(expectedArgumentKey, argumentsAsPairArray[0].Key);
            Assert.Equal(expectedArgumentValue, argumentsAsPairArray[0].Value);
        }

        [Fact]
        public void ShouldLogEmptyFieldFilterStatements()
        {
            var filterName = "TestFilter";
            var filterItemNames = new[] { "TestFilterItem1", "TestFilterItem2", "TestFilterItem3" };
            var expectedWarnMessage = "[MySqlFilterBuilder] Empty filter items: [TestFilterItem1, TestFilterItem2, TestFilterItem3]";
            var expectedWarningsCount = 1;

            var filter = new QueryFilterGroup(nameof(Driver), filterName, FilterJoinType.And, new[] {
                new QueryFilterItem(filterItemNames[0], nameof(Driver.BirthDate), DateTime.UtcNow, ComparisonType.None),
                new QueryFilterItem(filterItemNames[1], nameof(Driver.CreatedOn), DateTime.UtcNow, ComparisonType.None),
                new QueryFilterItem(filterItemNames[2], nameof(Driver.ModifiedOn), DateTime.UtcNow, ComparisonType.None),
            });

            FilterBuilder.Build(filter);
            var warnings = GetLogs(LogLevel.Warn);

            Assert.NotEmpty(warnings);
            Assert.Equal(expectedWarningsCount, warnings.Count());

            var firstWarning = warnings.First();
            Assert.Equal(expectedWarnMessage, firstWarning);
        }

        [Fact]
        public void ShouldLogEmptyGroupFilterStatements()
        {
            var filterName = "TestFilter";
            var filterItemNames = new[] { "TestFilterGroup1", "TestFilterGroup2", "TestFilterGroup3" };
            var expectedWarnMessage = "[MySqlFilterBuilder] Empty filter groups: [TestFilterGroup1, TestFilterGroup2, TestFilterGroup3]";
            var expectedWarningsCount = 1;

            var filter = new QueryFilterGroup(nameof(Driver), filterName, FilterJoinType.And, new[] {
                new QueryFilterGroup(nameof(Driver), filterItemNames[0], FilterJoinType.None, new QueryFilterItem[] { }),
                new QueryFilterGroup(nameof(Driver), filterItemNames[1], FilterJoinType.None, new QueryFilterItem[] { }),
                new QueryFilterGroup(nameof(Driver), filterItemNames[2], FilterJoinType.None, new QueryFilterItem[] { })
            });

            FilterBuilder.Build(filter);
            var warnings = GetLogs(LogLevel.Warn);

            Assert.NotEmpty(warnings);
            Assert.Equal(expectedWarningsCount, warnings.Count());

            var firstWarning = warnings.First();
            Assert.Equal(expectedWarnMessage, firstWarning);
        }
    }
}

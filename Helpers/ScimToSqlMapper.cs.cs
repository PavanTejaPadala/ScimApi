using System.Text;
using System.Text.RegularExpressions;

namespace ScimApi.Helpers
{
    public static class ScimToSqlMapper
    {
        public static string MapScimFilterToSql(string scimFilter)
        {
            var sqlBuilder = new StringBuilder("SELECT * FROM [dbo].[tbUser] WHERE 1=1"); 

            if (!string.IsNullOrEmpty(scimFilter))
            {
                // Handle 'and' and 'or' logical operators
                var conditions = scimFilter.Split(new[] { " and ", " or " }, StringSplitOptions.None);
                foreach (var condition in conditions)
                {
                    var trimmedCondition = condition.Trim();
                    if (trimmedCondition.Contains("eq"))
                    {
                        ProcessEqualityCondition(trimmedCondition, sqlBuilder);
                    }
                    else if (trimmedCondition.Contains("co"))
                    {
                        ProcessContainsCondition(trimmedCondition, sqlBuilder);
                    }
                    else if (trimmedCondition.Contains("sw"))
                    {
                        ProcessStartsWithCondition(trimmedCondition, sqlBuilder);
                    }
                    else if (trimmedCondition.Contains("gt"))
                    {
                        ProcessGreaterThanCondition(trimmedCondition, sqlBuilder);
                    }
                    // Add more conditions as needed
                }
            }

            return sqlBuilder.ToString();
        }

        private static void ProcessEqualityCondition(string condition, StringBuilder sqlBuilder)
        {
            var parts = condition.Split(new[] { " eq " }, StringSplitOptions.None);
            if (parts.Length == 2)
            {
                var attribute = parts[0].Trim();
                var value = ExtractValue(parts[1].Trim(), '"'); // Extract value enclosed in quotes

                switch (attribute)
                {
                    case "userName":
                        sqlBuilder.Append($" AND [UserName] = '{value}'");
                        break;
                    case "emails.value":
                        sqlBuilder.Append($" AND [EmailAddress] = '{value}'");
                        break;
                    case "displayName":
                        sqlBuilder.Append($" AND [DisplayName] = '{value}'");
                        break;
                    case "active":
                        var isActive = value.ToLower() == "true";
                        sqlBuilder.Append($" AND [Enabled] = {(isActive ? 1 : 0)}");

                        break;
                        
                }
            }
        }

        private static void ProcessContainsCondition(string condition, StringBuilder sqlBuilder)
        {
            var parts = condition.Split(new[] { " co " }, StringSplitOptions.None);
            if (parts.Length == 2)
            {
                var attribute = parts[0].Trim();
                var value = ExtractValue(parts[1].Trim(), '"');

                switch (attribute)
                {
                    case "name.givenName":
                        sqlBuilder.Append($" AND [FirstName] LIKE '%{value}%'");
                        break;
                    case "userName":
                        sqlBuilder.Append($" AND [UserName] LIKE '%{value}%'");
                        break;
                    case "displayName":
                        sqlBuilder.Append($" AND [DisplayName] LIKE '%{value}%'");
                        break;
                    case "emails.value":
                        sqlBuilder.Append($" AND [EmailAddress] LIKE '%{value}%'");
                        break;
                    // Add more cases for other attributes as needed
                    case "enabled":
                        var isEnabled = value.ToLower() == "true";
                        sqlBuilder.Append($" AND [Enabled] = {(isEnabled ? 1 : 0)}");
                        break;
                    case "created":
                        sqlBuilder.Append($" AND [Created] LIKE '%{value}%'"); // Assuming 'Created' is a field in the database
                        break;
                    case "lastModifiedDate":
                        sqlBuilder.Append($" AND [LastModified] LIKE '%{value}%'"); // Assuming 'LastModified' is a field in the database
                        break;
                    default:
                        throw new NotSupportedException($"Filtering by '{attribute}' is not supported.");
                }
            }
        }


        private static void ProcessStartsWithCondition(string condition, StringBuilder sqlBuilder)
        {
            var parts = condition.Split(new[] { " sw " }, StringSplitOptions.None);
            if (parts.Length == 2)
            {
                var attribute = parts[0].Trim();
                var value = ExtractValue(parts[1].Trim(), '"');

                switch (attribute)
                {
                    case "userName":
                        sqlBuilder.Append($" AND [UserName] LIKE '{value}%'");
                        break;

                    case "displayName":
                        sqlBuilder.Append($" AND [DisplayName] LIKE '{value}%'");
                        break;

                    case "emails.value":
                        sqlBuilder.Append($" AND [EmailAddress] LIKE '{value}%'");
                        break;

                    // Add more cases for additional attributes if necessary
                    default:
                        throw new NotSupportedException($"Filtering by '{attribute}' is not supported.");
                }
            }
        }


        private static void ProcessGreaterThanCondition(string condition, StringBuilder sqlBuilder)
        {
            var parts = condition.Split(new[] { " gt " }, StringSplitOptions.None);
            if (parts.Length == 2)
            {
                var attribute = parts[0].Trim();
                var value = ExtractValue(parts[1].Trim(), '"');

                if (attribute == "meta.lastModified")
                {
                    sqlBuilder.Append($" AND [LastModifiedDate] > '{value}'");
                }
            }
        }

        private static string ExtractValue(string filter, char quoteChar)
        {
            var startIndex = filter.IndexOf(quoteChar) + 1;
            var endIndex = filter.IndexOf(quoteChar, startIndex);
            return filter.Substring(startIndex, endIndex - startIndex);
        }
    }
}

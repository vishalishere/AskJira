﻿using System;
using Xunit;

namespace Domain.Tests
{
    public class Given_search_criteria
    {
        [Fact]
        public void Default_search_values_search_for_just_the_date()
        {
            var date = DateTime.Parse("4/4/2015 00:00");

            var dateFrom = date.AddDays(-2).Date;
            var dateTo = date.Date;
            var expected = string.Format("https://jira.advancedcsg.com/rest/api/2/search?jql=project=LCSMLCAND (created >= '2015-04-02 12:00' AND created <= '2015-04-04 12:00')");
            var actual = new QueryBuilder().Build(dateFrom, dateTo);
            
            Assert.Equal(expected,actual);
        }
    }

    public class QueryBuilder
    {
        public string Build(DateTime dateFrom, DateTime dateTo, string searchText = "", string issueType = "Any", string client = "", string component = "0", string version = "0")
        {
            var v = "";
            if (!string.IsNullOrEmpty(version) && version != "0")
                v = string.Format(" AND FixVersion = '{0}'", version);

            var c = "";
            if (!string.IsNullOrEmpty(component) && component != "0")
                c = string.Format(" AND Component = '{0}'", component);

            var s = "";
            if (!string.IsNullOrEmpty(searchText))
                s = string.Format(" AND (summary ~ '{0}' OR description ~ '{0}' OR comment ~ '{0}')", searchText);

            var cl = "";
            if (!string.IsNullOrEmpty(client))
                client = string.Format(" AND cf[10200]~'{0}'", client);

            var i = "";
            if (issueType != "Any")
                i = string.Format(" AND issuetype = '{0}'", issueType);

            var dateRange = string.Format("AND (created >= '{0}' AND created <= '{1}')", dateFrom.Date.ToString("yyyy-MM-dd h:mm").Replace('/', '-'), dateTo.Date.ToString("yyyy-MM-dd h:mm").Replace('/', '-'));
            
            return string.Format("https://jira.advancedcsg.com/rest/api/2/search?jql=project=LCSMLC{0}{1}{2}{3}{4}{5}", s, dateRange, v, c, client, i);
        }
    }
}
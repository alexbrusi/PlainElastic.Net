using System;
using PlainElastic.Net.Builders;


namespace PlainElastic.Net.Queries
{
    /// <summary>
    /// Provides sophisticated interface to construct ElasicSearch queries.
    /// For details about ES querying see: http://www.elasticsearch.org/guide/reference/query-dsl/
    /// and http://www.elasticsearch.org/guide/reference/api/search/
    /// </summary>
    public class QueryBuilder<T> : CompositeQueryBase
    {

        protected override string QueryTemplate
        {
            get { return "{{ {0} }}"; }
        }


        /// <summary>
        /// The query element within the search request body allows to define a query using the Query DSL.
        /// see http://www.elasticsearch.org/guide/reference/api/search/query.html
        /// </summary>
        public QueryBuilder<T> Query(Func<Query<T>, Query<T>> query)
        {
            RegisterQueryExpression(query);
            return this;
        }

        /// <summary>
        /// The filter element within the search request can be used to accomplish it.
        /// see http://www.elasticsearch.org/guide/reference/api/search/filter.html
        /// </summary>
        public QueryBuilder<T> Filter(Func<Filter<T>, Filter<T>> filter)
        {
            RegisterQueryExpression(filter);
            return this;
        }

        /// <summary>
        /// The starting from index of the hits to return. Defaults to 0.
        /// </summary>
        public QueryBuilder<T> From (int from = 0 )
        {
            var fromParam = " 'from': {0}".SmartQuoteF(from);
            RegisterJsonQuery(fromParam);

            return this;
        }

        /// <summary>
        /// The number of hits to return. Defaults to 10.
        /// </summary>
        public QueryBuilder<T> Size(int size = 10)
        {
            var sizeParam = " 'size': {0}".SmartQuoteF(size);
            RegisterJsonQuery(sizeParam);

            return this;
        }

        /// <summary>
        /// When sorting on a field, scores are not computed. 
        /// By setting track_scores to true, scores will still be computed and tracked.
        /// see http://www.elasticsearch.org/guide/reference/api/search/sort.html
        /// </summary>
        public QueryBuilder<T> TrackScores(bool trackScores = false)
        {
            var param = " 'track_scores': {0}".SmartQuoteF(trackScores.AsString());
            RegisterJsonParam(param);

            return this;
        }

        /// <summary>
        /// Allows to add one or more sort on specific fields. Each sort can be reversed as well. 
        /// The sort is defined on a per field level, with special field name for _score to sort by score.
        /// see http://www.elasticsearch.org/guide/reference/api/search/sort.html
        /// </summary>
        public QueryBuilder<T> Sort(Func<Sort<T>, Sort<T>> sort)
        {
            RegisterQueryExpression(sort);
            return this;
        }


        /// <summary>
        /// Adds a custom section.
        /// You can use ' instead of " to simplify queryFormat creation.
        /// </summary>
        public QueryBuilder<T> Custom(string customFormat, params string[] args)
        {
            var query = customFormat.SmartQuoteF(args);
            RegisterJsonQuery(query);
            return this;
        }


        /// <summary>
        /// Builds JSON query.
        /// </summary>
        public string Build()
        {
            return (this as IJsonConvertible).ToJson();
        }


        /// <summary>
        /// Builds beatified JSON query.
        /// </summary>
        public string BuildBeautified()
        {
            return Build().ButifyJson();
        }


        public override string ToString()
        {
            return BuildBeautified();
        }
    }
}
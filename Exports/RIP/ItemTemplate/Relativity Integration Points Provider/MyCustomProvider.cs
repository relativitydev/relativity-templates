using System;
using System.Collections.Generic;
using System.Data;
using kCura.IntegrationPoints.Contracts.Models;
using kCura.IntegrationPoints.Contracts.Provider;
using Newtonsoft.Json;
using Relativity.API;

namespace $rootnamespace$
{
    [kCura.IntegrationPoints.Contracts.DataSourceProvider(Constants.Guids.Provider.MY_CUSTOM_PROVIDER)]
    public class $safeitemname$ : IDataSourceProvider
    {
        public IHelper Helper;

        public $safeitemname$(IHelper helper)
        {
            Helper = helper;
        }

        /// <summary>
        /// This method runs on the web server while users are configuring their integration point and returns fields that correspond to your data so users can map them to their selected import object.
        /// </summary>
        /// <param name="options">User settings saved on the custom page, it passed as a JSON string</param>
        /// <returns>This method should return a list of fields for users to map to Relativity objects</returns>
        public IEnumerable<FieldEntry> GetFields(string options)
        {
            var fieldEntries = new List<FieldEntry>();
            
            fieldEntries.Add(new FieldEntry { DisplayName = "ID", FieldIdentifier = "ID", IsIdentifier = true });
            fieldEntries.Add(new FieldEntry { DisplayName = "Description", FieldIdentifier = "Description", IsIdentifier = false });

            return fieldEntries;
        }

        /// <summary>
        /// This method is executed on integration points agents to import the data into Relativity. It is executed continuously to import the data in small batches until the full import is complete. Use it to access your data, find the records that correspond to the IDs passed as a parameter and return it as a data reader.
        /// </summary>
        /// <param name="fields">Fields returned from the GetFields() Method</param>
        /// <param name="entryIds">A batch of IDs returned from the GetBatchableIds() Method</param>
        /// <param name="options">User settings saved on the custom page, it passed as a JSON string</param>
        /// <returns>A data reader populated with the data corresponding to the batch of IDs</returns>
        public IDataReader GetData(IEnumerable<FieldEntry> fields, IEnumerable<string> entryIds, string options)
        {
            var configuration = JsonConvert.DeserializeObject<ExampleConfigurationModel>(options);
            var dataSource = new DataTable();
            return dataSource.CreateDataReader();
        }

        /// <summary>
        /// Use this method to return all the IDs associated with the data you want to import
        /// </summary>
        /// <param name="identifier">The field marked as the identifier in the list of fields returned from GetFields()</param>
        /// <param name="options">User settings saved on the custom page, it passed as a JSON string</param>
        /// <returns>A data reader populated with a list of IDs corresponding to the data you want to import</returns>
        public IDataReader GetBatchableIds(FieldEntry identifier, string options)
        {
            var configuration = JsonConvert.DeserializeObject<ExampleConfigurationModel>(options);
            var dataSource = new DataTable();
            dataSource.Columns.Add(new DataColumn(identifier.FieldIdentifier, typeof(String)));
            dataSource.Rows.Add("10000");
            dataSource.Rows.Add("10001");
            dataSource.Rows.Add("10002");
            return dataSource.CreateDataReader();
        }
    }
}

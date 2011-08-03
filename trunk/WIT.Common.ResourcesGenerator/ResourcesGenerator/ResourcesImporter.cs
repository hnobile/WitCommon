using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Data.OleDb;

namespace ResourcesGenerator
{
    public class ResourcesImporter
    {
        private string connectionString;

        public ResourcesImporter(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private int GetResourcesInitRowIndex()
        {
            return 1;
        }

        public Resource Import()
        {
            DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.OleDb");
            DbDataAdapter adapter = factory.CreateDataAdapter();

            DbCommand selectCommand = factory.CreateCommand();
            selectCommand.CommandText = "SELECT * FROM [Resources$]";

            DbConnection connection = factory.CreateConnection();
            connection.ConnectionString = connectionString;

            selectCommand.Connection = connection;

            adapter.SelectCommand = selectCommand;

            DataSet dsTestInfo = new DataSet();
            try
            {
                adapter.Fill(dsTestInfo);
            }
            catch (System.Data.OleDb.OleDbException ex)
            {
                throw new ResourcesImporterException(ex.Message, ex);
            }
            Resource resource = null;
            if (dsTestInfo.Tables.Count == 1)
            {
                resource = this.BuildResource(dsTestInfo.Tables[0]);
            }
            connection.Close();
            return resource;
        }

        private Resource BuildResource(DataTable table)
        {
            ResourcesImporterException resourceImporterException = new ResourcesImporterException();

            Resource resource = new Resource();
            //TIPO
            if (!string.IsNullOrEmpty(GetResourceType(table)))
            {
                resource.Type = GetResourceType(table);
            }
            else
            {
                resourceImporterException.ErrorTypes.Add(ResourcesImporterException.ResourceImporterExceptionType.InvalidType);
            }

            resource.Resources = BuildResources(table, resourceImporterException);


            if (resourceImporterException.ErrorTypes.Count <= 0)
            {
                return resource;
            }
            else
            {
                throw resourceImporterException;
            }  
        }

        private Dictionary<string,string> BuildResources(DataTable table, ResourcesImporterException resourceImporterException)
        {
            Dictionary<string,string> resources = null;
            for (int rowNavigationIndex = GetResourcesInitRowIndex(); rowNavigationIndex < table.Rows.Count; rowNavigationIndex++)
            {
                if (resources == null)
                {
                    resources = new Dictionary<string, string>();
                }
                DataRow row = table.Rows[rowNavigationIndex];
                if (string.IsNullOrEmpty(GetKeyText(row)) && //KET TEXT
                    string.IsNullOrEmpty(GetResourceText(row))) //RESOURCE TEXT
                {
                    resourceImporterException.ErrorTypes.Add(ResourcesImporterException.ResourceImporterExceptionType.InvalidResources);
                    return null;
                }

                if (!string.IsNullOrEmpty(GetKeyText(row)) && !string.IsNullOrEmpty(GetResourceText(row)))
                {
                    resources.Add(GetKeyText(row), GetResourceText(row));
                }
                
            }
            return resources;
        }

        private string GetResourceType(DataTable table)
        {
            return table.Rows[0].ItemArray[1].ToString();
        }

        private string GetKeyText(DataRow row)
        {
            return row.ItemArray[0].ToString();
        }
        private string GetResourceText(DataRow row)
        {
            return row.ItemArray[1].ToString();
        }
    }
}

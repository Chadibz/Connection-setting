using System;
using System.Runtime.Remoting.Contexts;
using System.Web.Services.Description;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;

namespace CreateAndDisplayEntityAttributes
{
    class Program
    {
        public static CrmServiceClient _service;

        //cette fonction permet de recherecher un enregistrement en utilisant le cle alertnative 

        public static Entity RetrieveRecordByKey(string nomEntite, string nomCle, string cle, IOrganizationService service)
        {
            var keys = new KeyAttributeCollection
            { {nomCle,cle}
            };
            var request = new RetrieveRequest
            {
                ColumnSet = new ColumnSet(nomEntite + "id"),
                Target = new EntityReference(nomEntite, keys)
            };
            try
            {
                var response = (RetrieveResponse)service.Execute(request);
                return response.Entity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        static void Main(string[] args)
        {
            try
            {
                var connectionString = @"AuthType=Office365;Url=https://pfe.crm12.dynamics.com/;
                                      Username=chedi.bouzayene@inetum.com;Password=Aa22121999b@@";

                _service = new CrmServiceClient(connectionString);
                if (!_service.IsReady)
                {
                    Console.WriteLine("Failed to connect to Dynamics 365.");
                    return;
                }
                // Entity target = (Entity)context.InputParameters["Target"];

                RetrieveEntityRequest retrieveEntityRequest = new RetrieveEntityRequest
                {
                    EntityFilters = EntityFilters.All,
                    LogicalName = "account"
                };

                RetrieveEntityResponse retrieveEntityResponse = (RetrieveEntityResponse)_service.Execute(retrieveEntityRequest);
                EntityMetadata entityMetadata = retrieveEntityResponse.EntityMetadata;

                Console.WriteLine("Entity attributes:");
                foreach (object attribute in entityMetadata.Attributes)
                {
                    AttributeMetadata a = (AttributeMetadata)attribute;
                    Console.WriteLine("Attribute Logical Name: " + a.LogicalName);

                    var attributeRecord = new Entity("ext_fields");
                    attributeRecord["ext_schema_name"] = a.SchemaName;
                    attributeRecord["ext_name"] = a.LogicalName;
                    attributeRecord["ext_entity"] = new EntityReference("account", new Guid("94470938-d519-ef11-840a-002248d9c803"));

                    _service.Create(attributeRecord);
                }
            }
            catch { throw new Exception(); }


        }
    }
}

        
            



        
      





using System;
using System.Linq;
using Elasticsearch.Net;
using Nest;
using Xunit;

namespace ElasticSpike.Spec
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var uris = new[]
            {
                new Uri("http://localhost:9200")
            };

            var connectionPool = new SniffingConnectionPool(uris);
            var settings = new ConnectionSettings(connectionPool)
                .DefaultIndex("products");
            
            var client = new ElasticClient(settings);

            var product = new Product(1, 3);

            var response = client.IndexDocument(product);
            
            var searchResponse = client.Search<Product>(s => s
                .From(0)
                .Size(10)
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Id)
                        .Query("1")
                    )
                )
            );

            var people = searchResponse.Documents;
            
            Assert.True(people.Any());


        }
    }

    public class Product
    {
        public int Id { get; set; }
        public double Price { get; set; }

        public Product()
        {
        }

        public Product(int id, double price)
        {
            Id = id;
            Price = price;
        }
    }
}
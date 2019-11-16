using System;

namespace notifier.dal.attributes
{
    /// <summary>
    /// properties of mongoDbCollection.
    /// used on each entity in order to specify collection name.
    /// </summary>
    public class MongoCollectionAttribute : Attribute
    {
        public string Name { get; set; }
    }
}

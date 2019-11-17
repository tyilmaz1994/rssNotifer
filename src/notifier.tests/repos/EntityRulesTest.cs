using notifier.dal.attributes;
using notifier.dal.entities;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace notifier.tests.repos
{
    public class EntityRulesTest
    {
        [Fact]
        public void Every_entity_must_have_MongoCollectionAttribute_Test()
        {
            Assembly assembly = Assembly.Load("notifier.dal");
            var types = assembly.GetTypes().Where(t => string.Equals(t.Namespace, "notifier.dal.entities", StringComparison.Ordinal)).ToArray();

            foreach (var item in types)
            {
                if (item == typeof(BaseEntity))
                    continue;

                var attr = item.GetCustomAttribute<MongoCollectionAttribute>();

                Assert.True(attr != null, item.FullName + " must have MongoCollectionAttribute");
                Assert.False(string.IsNullOrEmpty(attr.Name), item.FullName + ".Name must not null or empty");
            }
        }

        [Fact]
        public void Every_entity_must_inherit_BaseEntity_Test()
        {
            Assembly assembly = Assembly.Load("notifier.dal");
            var types = assembly.GetTypes().Where(t => string.Equals(t.Namespace, "notifier.dal.entities", StringComparison.Ordinal)).ToArray();

            foreach (var item in types)
            {
                if (item == typeof(BaseEntity))
                    continue;

                Assert.True(item.BaseType == typeof(BaseEntity), item.FullName + " must inherit BaseEntity");
            }
        }
    }
}
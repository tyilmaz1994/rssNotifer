using notifier.dal.entities;
using System;
using System.Reflection;

namespace notifier.tests.helpers
{
    public static class GenerateHelper
    {
        public static T GenerateEntity<T>() where T: BaseEntity
        {
            T entity = Activator.CreateInstance<T>();

            foreach (var item in typeof(T).GetProperties())
            {
                if (item.PropertyType == typeof(string))
                {
                    item.SetRandomValueString(entity);
                }
                else if (item.PropertyType == typeof(int))
                {
                    item.SetRandomValueInt(entity);
                }
                else if (item.PropertyType == typeof(short))
                {
                    item.SetRandomValueShort(entity);
                }
            }

            entity.Id = null;
            entity.Active = 1;
            return entity;
        }

        public static T ChangeEntityValues<T>(T entity) where T : BaseEntity
        {
            foreach (var item in typeof(T).GetProperties())
            {
                if(item.Name == "Id")
                {
                    continue;
                }
                else if (item.PropertyType == typeof(string))
                {
                    item.SetRandomValueString(entity);
                }
                else if (item.PropertyType == typeof(int))
                {
                    item.SetRandomValueInt(entity);
                }
                else if (item.PropertyType == typeof(short))
                {
                    item.SetRandomValueShort(entity);
                }
            }

            entity.Active = 1;
            return entity;
        }

        public static void SetRandomValueString(this PropertyInfo property, object? item)
        {
            property.SetValue(item, GenerateString());
        }
        public static void SetRandomValueInt(this PropertyInfo property, object? item)
        {
            property.SetValue(item, RandomInt(int.MaxValue));
        }

        public static void SetRandomValueShort(this PropertyInfo property, object? item)
        {
            property.SetValue(item, (short)RandomInt(short.MaxValue));
        }


        private static string GenerateString()
        {
            var guild = Guid.NewGuid().ToString();
            int rnd = RandomInt(guild.Length);

            return guild.Substring(0, rnd);
        }

        private static int RandomInt(int max)
        {
            return new Random().Next(1, max);
        }
    }
}

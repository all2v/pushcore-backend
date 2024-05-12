using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Astrum.Infrastructure.Integrations.YouTrack.JsonConverter
{
    public class CustomFieldConverter : Newtonsoft.Json.JsonConverter
    {
        //objectType is the type as specified for List<myModel> (i.e. myModel)
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader); //json from myModelList > model
            var list = Activator.CreateInstance(objectType) as System.Collections.IList; // new list to return
            var itemType = objectType.GenericTypeArguments[0]; // type of the list (myModel)
            if (token.Type.ToString() == "Object") //Object
            {
                var child = token.Children();
                var newObject = Activator.CreateInstance(itemType);
                serializer.Populate(token.CreateReader(), newObject);
                list.Add(newObject);
            }
            else //Array
            {
                foreach (var child in token.Children())
                {
                    var newObject = Activator.CreateInstance(itemType);
                    serializer.Populate(child.CreateReader(), newObject);
                    list.Add(newObject);
                }
            }
            return list;

        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsGenericType && (objectType.GetGenericTypeDefinition() == typeof(List<>));
        }
        public override bool CanWrite => false;
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}

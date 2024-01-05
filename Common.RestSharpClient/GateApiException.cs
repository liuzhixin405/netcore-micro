using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace RestSharpComponent
{
    [DataContract]
    public class GateApiException : ApiException
    {
        [DataMember(Name ="label",EmitDefaultValue =false)]
        public string ErrorLabel { get; set; }

        public string ErrorMessage
        {
            get { return string.IsNullOrWhiteSpace(this._errorDetail) ? this._errorMessage : this._errorDetail; }
        }
        [DataMember(Name = "message")]
        private string _errorMessage;

        [DataMember(Name = "detail")]
        private string _errorDetail;


        [JsonConstructor]
        public GateApiException(string label,string message=default(string),string detail=default(string))
        {
            this.ErrorLabel = label;
            this._errorMessage = message;
            this._errorDetail = detail;
        }

        public override string Message => JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings { NullValueHandling= NullValueHandling.Ignore});
    }
}

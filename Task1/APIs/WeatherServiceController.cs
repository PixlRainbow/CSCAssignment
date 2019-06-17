using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace CSCAssignment.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherServiceController : ControllerBase
    {
        [HttpGet]
        [Produces("application/xml")]
        public IActionResult Get()
        {
            //simulate delay
            Task.Delay(2500);
            //"https://api.worldweatheronline.com/premium/v1/weather.ashx?key=4bc466f03e764a03827150230192204&q=China&format=xml&num_of_days=5";
            UriBuilder url = new UriBuilder(){
                Scheme = "https",
                Host="api.worldweatheronline.com",
                Path="premium/v1/weather.ashx",
                Query=QueryHelpers.AddQueryString("", new Dictionary<string,string>(){
                    {"key", "4bc466f03e764a03827150230192204"},
                    {"q", "China"},
                    {"format", "xml"},
                    {"num_of_days", "5"}
                }).Substring(1) //remove leading question mark, UriBuilder will add it
            };

            XmlDocument wsResponseXmlDoc = MakeRequest(url.ToString());
            if(wsResponseXmlDoc != null){
                return Ok(wsResponseXmlDoc);
            } else
            {
                wsResponseXmlDoc = CreateErrorDocument("Could not connect to WorldWeatherOnline");
                return new ObjectResult(wsResponseXmlDoc){
                    StatusCode = 502
                };
            }
        }
        public static XmlDocument MakeRequest(string requestUrl){
            try {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                //set timeout to 15 seconds
                request.Timeout = 15 * 1000;
                request.KeepAlive = false;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());
                return xmlDoc;
            } catch (Exception e) {
                return null;
            }
        }
        public static XmlDocument CreateErrorDocument(string message){
            XmlDocument doc = new XmlDocument();
            
            //follows error document schema set by worldweatheronline
            XmlElement data = doc.CreateElement("data");
            doc.AppendChild(data);
                XmlElement error = doc.CreateElement("error");
                data.AppendChild(error);
                    XmlElement msg = doc.CreateElement("msg");
                    msg.InnerText = message;
                    error.AppendChild(msg);

            return doc;
        }
    }
}
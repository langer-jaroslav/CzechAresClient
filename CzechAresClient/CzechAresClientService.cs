using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using CzechAresClient.Models;

namespace CzechAresClient;

public interface ICzechAresClientService
{
    Task<Record?> SearchByCompanyIdAsync(string companyId);
}
public class CzechAresClientService : ICzechAresClientService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly string _apiUrl;

    public CzechAresClientService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _apiUrl = "https://www.info.mfcr.cz/cgi-bin/ares/";
    }

    public async Task<Record?> SearchByCompanyIdAsync(string companyId)
    {
        var url = _apiUrl + $"darv_bas.cgi?ico={companyId.Trim()}";
        url = TrimUrl(url);

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var client = GetHttpClient();

        var response = await client.SendAsync(request);

        if (!response.IsSuccessStatusCode) return null;

        var data = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(data))
            return null;
        var result = await DeserializeXmlAsync(data);
        return result;
    }

    private async Task<Record?> DeserializeXmlAsync(string xml)
    {
        var result = new Record();
        using var memoryStream = new MemoryStream();
        await using var writer = new StreamWriter(memoryStream);
        await writer.WriteAsync(xml);
        await writer.FlushAsync();
        memoryStream.Position = 0;

        var xmlReader = new XmlTextReader(memoryStream);

        while (xmlReader.Read())
        {
            if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name.Equals("D:ICO"))
            {
                xmlReader.MoveToElement();
                xmlReader.Read();
                result.CompanyId = xmlReader.Value;
            }
            if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name.Equals("D:DIC"))
            {
                xmlReader.MoveToElement();
                xmlReader.Read();
                result.CompanyVatId = xmlReader.Value;
            }
            else if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name.Equals("D:OF"))
            {
                xmlReader.MoveToElement();
                xmlReader.Read();
                result.CompanyName = xmlReader.Value;
            }
            else if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name.Equals("D:NS"))
            {
                xmlReader.MoveToElement();
                xmlReader.Read();
                result.Country = xmlReader.Value;
            }
            else if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name.Equals("D:N"))
            {
                xmlReader.MoveToElement();
                xmlReader.Read();
                result.City = xmlReader.Value;
            }
            else if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name.Equals("D:PSC"))
            {
                xmlReader.MoveToElement();
                xmlReader.Read();
                result.PostCode = xmlReader.Value;
            }
            else if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name.Equals("D:NU"))
            {
                xmlReader.MoveToElement();
                xmlReader.Read();
                result.Street = xmlReader.Value;
            }
            else if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name.Equals("D:CD"))
            {
                xmlReader.MoveToElement();
                xmlReader.Read();
                result.BuildingNumber = xmlReader.Value;
            }
            else if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name.Equals("D:CO"))
            {
                xmlReader.MoveToElement();
                xmlReader.Read();
                result.OrientationNumber = xmlReader.Value;
            }
            xmlReader.MoveToElement();

        }
        await memoryStream.DisposeAsync();

        if (string.IsNullOrWhiteSpace(result.CompanyName) && string.IsNullOrWhiteSpace(result.CompanyId))
            return null;
        return result;
    }

    private HttpClient GetHttpClient()
    {
        var client = _clientFactory.CreateClient();
        return client;
    }
    private static string TrimUrl(string url)
    {
        if (url.EndsWith("?"))
            url = url[..^1];
        if (url.EndsWith("&"))
            url = url[..^1];
        return url;
    }
}

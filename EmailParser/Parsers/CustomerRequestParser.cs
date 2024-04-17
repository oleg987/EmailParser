using EmailParser.Entities;

namespace EmailParser.Parsers;

public static class CustomerRequestParser
{
    public static CustomerRequest? Parse(string sender, string body)
    {
        return sender switch
        {
            "info@it-school.com.ua" => ParseItSchoolRequest(body),
            "admin@abiturients.info" => ParseAbiturientsInfoRequest(body),
            _ => null
        };
    }
    
    private static CustomerRequest ParseItSchoolRequest(string inputText)
    {
        CustomerRequest customerRequest = new CustomerRequest();

        customerRequest.Source = "info@it-school.com.ua";
        
        string[] lines = inputText.Split('\n');

        foreach (string line in lines)
        {
            string[] parts = line.Split(':');
            if (parts.Length == 2)
            {
                string key = parts[0].Trim();
                string value = parts[1].Trim();
                
                switch (key)
                {
                    case "Имя":
                        customerRequest.Name = value;
                        break;
                    case "Телефон":
                        customerRequest.Phone = value;
                        break;
                    case "Viber":
                        customerRequest.Viber = value == "1";
                        break;
                    case "Telegram":
                        customerRequest.Telegram = value == "1";
                        break;
                    case "E-mail":
                        customerRequest.Email = value;
                        break;
                    case "Дополнительная информация / сообщение":
                        customerRequest.AdditionalInfo = value;
                        break;
                    case "Имя ребенка (для детей)":
                        customerRequest.ChildName = value;
                        break;
                    case "Возраст (для детей)":
                        if (int.TryParse(value, out var age))
                        {
                            customerRequest.Age = age;
                        }
                        break;
                    case "Откуда вы узнали о нас":
                        customerRequest.WhereFindUs = value;
                        break;
                    case "Выбранный курс":
                        customerRequest.Course = value;
                        break;
                }
            }
        }

        return customerRequest;
    }
    
    private static CustomerRequest ParseAbiturientsInfoRequest(string inputText)
    {
        CustomerRequest customerRequest = new CustomerRequest();
        
        string[] lines = inputText.Split('\n');

        return customerRequest;
    }
}

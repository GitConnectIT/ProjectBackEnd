using Shared.ResponseFeatures;
using System.Xml.Linq;

namespace Shared.DTC;

public class ClientColumn
{
    public static string id = "Id";
    public static string firstName = "Emri";
    public static string lastName = "Mbiemri";
    public static string fullName = "Emri";
    public static string email = "Email";
    public static string state = "Shteti";
    public static string actions = null;

    public static string GetPropertyDescription(string propertyName)
    {
        switch (propertyName)
        {
            case nameof(id):
                return id;
            case nameof(firstName):
                return firstName;
            case nameof(lastName):
                return lastName;
            case nameof(email):
                return email;
            case nameof(state):
                return state;
            case nameof(fullName): 
                return fullName;
            case nameof(actions):
                return null;
            default:
                return "";
        }
    }

    public static bool GetPropertyIsHidden(string propertyName)
    {
        switch (propertyName)
        {
            case nameof(id):
                return true;
            case nameof(firstName):
            case nameof(lastName):
            case nameof(fullName):
            case nameof(email):
            case nameof(state):
            case nameof(actions):
                return false;
            default:
                return true;
        }
    }

    public static bool GetPropertyFilterable(string propertyName)
    {
        switch (propertyName)
        {
            case nameof(id):
                return false;
            case nameof(firstName):
            case nameof(lastName):
            case nameof(fullName):
            case nameof(email):
            case nameof(state):
            case nameof(actions):
                return true;
            default:
                return true;
        }
    }

    public static bool GetPropertyHideable(string propertyName)
    {
        switch (propertyName)
        {
            case nameof(id):
                return true;
            case nameof(firstName):
            case nameof(lastName):
            case nameof(fullName):
            case nameof(email):
            case nameof(state):
            case nameof(actions):
                return false;
            default:
                return true;
        }
    }

    public static int GetPropertyMinWidth(string propertyName)
    {
        switch (propertyName)
        {
            case nameof(id):
                return 50;
            case nameof(firstName):
            case nameof(lastName):
            case nameof(fullName):
            case nameof(email):
            case nameof(state):
            case nameof(actions):
                return 80;
            default:
                return 50;
        }
    }

    public static DataType GetPropertyDataType(string propertyName)
    {
        switch (propertyName)
        {
            case nameof(id):
                return DataType.Number;
            case nameof(firstName):
            case nameof(lastName):
            case nameof(email):
            case nameof(state):
            case nameof(fullName):
                return DataType.String;
            case nameof(actions):
                return DataType.Actions;
            default:
                return DataType.String;
        }
    }

    public static object[] GetPropertyActions(string propertyName)
    {

        object[] actionsData =
        {
                new { name = "edit", icon= "fa-regular fa-pen-to-square", color = "blue"},
                new { name = "delete", icon= "fa-regular fa-trash-can", color = "red" },
            };

        switch (propertyName)
        {
            case nameof(id):
            case nameof(firstName):
            case nameof(lastName):
            case nameof(email):
            case nameof(state):
            case nameof(fullName):
                return null;
            case nameof(actions):
                return actionsData;
            default:
                return actionsData;
        }
    }
}
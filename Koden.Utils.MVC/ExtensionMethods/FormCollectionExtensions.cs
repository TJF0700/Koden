using System;
using System.Web.Mvc;

/// <summary>
/// Extensions to parse the FormCollection returned
/// </summary>
public static class FormCollectionExtensions
{
    /// <summary>
    /// Gets the checkbox value from a FormCollection.
    /// </summary>
    /// <param name="frmCollection">The Form collection.</param>
    /// <param name="key">The key for the checkbox.</param>
    /// <returns></returns>
    public static Boolean kGetCheckboxValue(this FormCollection frmCollection, string key)
    {
        if (frmCollection == null)
            throw new ArgumentException("Form Collection is Empty");
        if (String.IsNullOrEmpty(key))
            throw new ArgumentException(String.Format("Invalid/Empty Key ({0})", key));
        if (frmCollection[key] == null)
            throw new ArgumentException(String.Format("Key ({0}) not found in Form Collection", key));

        var u = frmCollection[key].Split(',');
        if (u.Length > 0) return Convert.ToBoolean(u[0]);
        throw new Exception("Unable to get Checkbox Value from Form Collection: " + key);
    }

    /// <summary>
    /// Gets the Nullable Date value from a FormCollection.
    /// </summary>
    /// <param name="frmCollection">The Form collection.</param>
    /// <param name="key">The key for the nullable date.</param>
    /// <returns></returns>
    public static DateTime? kGetDateNullable(this FormCollection frmCollection, string key)
    {
        if (frmCollection == null)
            throw new ArgumentException("Form Collection is Empty");
        if (String.IsNullOrEmpty(key))
            throw new ArgumentException(String.Format("Invalid/Empty Key ({0})", key));
        if (frmCollection[key] == null)
            throw new ArgumentException(String.Format("Key ({0}) not found in Form Collection", key));
        if (frmCollection[key].Trim().Length == 10) return Convert.ToDateTime(frmCollection[key]);

        return null;
    }
    /// <summary>
    /// Gets the Date value from a FormCollection.
    /// </summary>
    /// <param name="frmCollection">The Form collection.</param>
    /// <param name="key">The key for the date.</param>
    /// <returns></returns>
    public static DateTime kGetDate(this FormCollection frmCollection, string key)
    {
        if (frmCollection == null)
            throw new ArgumentException("Form Collection is Empty");
        if (String.IsNullOrEmpty(key))
            throw new ArgumentException(String.Format("Invalid/Empty Key ({0})", key));
        if (frmCollection[key] == null)
            throw new ArgumentException(String.Format("Key ({0}) not found in Form Collection", key));

        if (frmCollection[key].Trim().Length == 10) return Convert.ToDateTime(frmCollection[key]);

        throw new Exception("Unable to get DateTime Value from Form Collection: " + key);
    }

    /// <summary>
    /// Gets the string value from a FormCollection.
    /// </summary>
    /// <param name="frmCollection">The Form collection.</param>
    /// <param name="key">The key for the string.</param>
    /// <returns></returns>
    public static string kGetString(this FormCollection frmCollection, string key)
    {
        if (frmCollection == null)
            throw new ArgumentException("Form Collection is Empty");
        if (String.IsNullOrEmpty(key))
            throw new ArgumentException(String.Format("Invalid/Empty Key ({0})", key));
        if (frmCollection[key] == null)
            throw new ArgumentException(String.Format("Key ({0}) not found in Form Collection", key));


        var data = frmCollection[key];
        return !String.IsNullOrEmpty(data) ? data : null;
    }

    /// <summary>
    /// Gets the integer value from a FormCollection.
    /// </summary>
    /// <param name="frmCollection">The Form collection.</param>
    /// <param name="key">The key for the integer.</param>
    /// <returns></returns>
    public static Int32? kGetIntNullable(this FormCollection frmCollection, string key)
    {
        if (frmCollection == null)
            throw new ArgumentException("Form Collection is Empty");
        if (String.IsNullOrEmpty(key))
            throw new ArgumentException(String.Format("Invalid/Empty Key ({0})", key));
        if (frmCollection[key] == null)
            throw new ArgumentException(String.Format("Key ({0}) not found in Form Collection", key));

        var data = frmCollection[key];
        if (!String.IsNullOrEmpty(data)) return Convert.ToInt32(data.Replace("$", "").Replace(",", ""));
        return null;
    }

    /// <summary>
    /// Gets the nullable integer value from a FormCollection.
    /// </summary>
    /// <param name="frmCollection">The Form collection.</param>
    /// <param name="key">The key for the nullable integer.</param>
    /// <returns></returns>
    public static Int32 kGetInt(this FormCollection frmCollection, string key)
    {
        if (frmCollection == null)
            throw new ArgumentException("Form Collection is Empty");
        if (String.IsNullOrEmpty(key))
            throw new ArgumentException(String.Format("Invalid/Empty Key ({0})", key));
        if (frmCollection[key] == null)
            throw new ArgumentException(String.Format("Key ({0}) not found in Form Collection", key));


        var data = frmCollection[key];
        if (!String.IsNullOrEmpty(data)) return Convert.ToInt32(data.Replace("$", "").Replace(",", ""));
        throw new Exception("Non-Nullable Integer is invalid: " + key);
    }

    /// <summary>
    /// Gets the nullable decimal value from a FormCollection.
    /// </summary>
    /// <param name="frmCollection">The Form collection.</param>
    /// <param name="key">The key for the nullable decimal.</param>
    /// <returns></returns>
    public static Decimal? kGetDecimalNullable(this FormCollection frmCollection, string key)
    {
        if (frmCollection == null)
            throw new ArgumentException("Form Collection is Empty");
        if (String.IsNullOrEmpty(key))
            throw new ArgumentException(String.Format("Invalid/Empty Key ({0})", key));
        if (frmCollection[key] == null)
            throw new ArgumentException(String.Format("Key ({0}) not found in Form Collection", key));


        var data = frmCollection[key];
        if (!String.IsNullOrEmpty(data)) return Convert.ToDecimal(data.Replace("$", "").Replace(",", ""));
        return null;
    }

    /// <summary>
    /// Gets the decimal value from a FormCollection.
    /// </summary>
    /// <param name="frmCollection">The Form collection.</param>
    /// <param name="key">The key for the decimal.</param>
    /// <returns></returns>
    public static Decimal kGetDecimal(this FormCollection frmCollection, string key)
    {
        if (frmCollection == null)
            throw new ArgumentException("Form Collection is Empty");
        if (String.IsNullOrEmpty(key))
            throw new ArgumentException(String.Format("Invalid/Empty Key ({0})", key));
        if (frmCollection[key] == null)
            throw new ArgumentException(String.Format("Key ({0}) not found in Form Collection", key));


        var data = frmCollection[key];
        if (!String.IsNullOrEmpty(data)) return Convert.ToDecimal(data.Replace("$", "").Replace(",", ""));
        throw new Exception("Non-Nullable Integer is invalid: " + key);
    }

}


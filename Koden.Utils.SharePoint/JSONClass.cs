#region License
// Copyright (c) 2014 Tim Fischer
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the 'Software'), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion
using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Koden.Utils.SharePoint
{
    #region GetContextWebInformation
    public class RootobjectContextWebInfo
    {
        public ContextWebInfoD d { get; set; }
    }

    public class ContextWebInfoD
    {
        public Getcontextwebinformation GetContextWebInformation { get; set; }
    }

    public class Getcontextwebinformation
    {
        public __MetadataContextWebInfo __metadata { get; set; }
        public int FormDigestTimeoutSeconds { get; set; }
        public string FormDigestValue { get; set; }
        public string LibraryVersion { get; set; }
        public string SiteFullUrl { get; set; }
        public Supportedschemaversions SupportedSchemaVersions { get; set; }
        public string WebFullUrl { get; set; }
    }

    public class __MetadataContextWebInfo
    {
        public string type { get; set; }
    }

    public class Supportedschemaversions
    {
        public __MetadataSchemaVersions __metadata { get; set; }
        public string[] results { get; set; }
    }

    public class __MetadataSchemaVersions
    {
        public string type { get; set; }
    }

    #endregion

    #region "SharePoint FolderInfo"
  
    public class DefaultSPFolderInfo
    {
        public dSPFolderInfo d { get; set; }
    }
    public class dSPFolderInfo
    {
        public __Metadata __metadata { get; set; }
        public jsonFiles Files { get; set; }
       //[JsonProperty("ListItemAllFields")]
        public fldListItemAllfields ListItemAllFields { get; set; }
        public ParentFolder ParentFolder { get; set; }
        public Properties Properties { get; set; }
        public jsonFolders Folders { get; set; }
        public int ItemCount { get; set; }
        public string Name { get; set; }
        public string ServerRelativeUrl { get; set; }
        public string WelcomePage { get; set; }
    }
    public class __Metadata
    {
        public string id { get; set; }
        public string uri { get; set; }
        public string type { get; set; }
    }

    public class jsonFiles
    {
        public __Deferred __deferred { get; set; }
        public SPItemInfo[] results {get;set;}
    }


    public class Deferred
    {
        public __Deferred __deferred { get; set; }
    }

    
    public class fldListItemAllfields
    {
        public __Metadata __metadata { get; set; }
        public __Deferred __deferred { get; set; }
        public Deferred FirstUniqueAncestorSecurableObject { get; set; }
        public Deferred RoleAssignments { get; set; }
        public Deferred AttachmentFiles { get; set; }
        public Deferred ContentType { get; set; }
        public Deferred FieldValuesAsHtml { get; set; }
        public Deferred FieldValuesAsText { get; set; }
        public Deferred FieldValuesForEdit { get; set; }
        public Deferred File { get; set; }
        public Deferred Folder { get; set; }
        public Deferred ParentList { get; set; }
        public int FileSystemObjectType { get; set; }
        public int Id { get; set; }
        public string ContentTypeId { get; set; }
        public object Title { get; set; }
        public object MCF_x0020_Control_x0020_IDId { get; set; }
        public object MCF_x0020_Perms_x0020_Are_x0020_Set { get; set; }
        public object MCF_x0020_Path { get; set; }
        public object MCFFolderPath { get; set; }
        public object MCFPermIDId { get; set; }
        public object LocNameId { get; set; }
        public object CIDCalc { get; set; }
        public object TEMP { get; set; }
        public object MCFCtrlLink { get; set; }
        public int ID { get; set; }
        public DateTime Created { get; set; }
        public int AuthorId { get; set; }
        public DateTime Modified { get; set; }
        public int EditorId { get; set; }
        public object OData__CopySource { get; set; }
        public object CheckoutUserId { get; set; }
        public string OData__UIVersionString { get; set; }
        public string GUID { get; set; }
    }

    public class ListItemAllFields
    {
        public __Deferred __deferred { get; set; }
    }
    public class Properties
    {
        public __Deferred __deferred { get; set; }
    }
    public class jsonFolders
    {
        public __Deferred __deferred { get; set; }
        public jsonFolder[] results { get; set; }
    }
    public class ParentFolder
    {
        public __Deferred __deferred { get; set; }
    }


// Folder Results
    public class DefaultSPFolderList
    {
        public jsonFolders d { get; set; }
    }



    public class jsonFolder
    {
        public __Metadata __metadata { get; set; }
        public __Deferred __deferred { get; set; }
        public jsonFiles Files { get; set; }
        public ListItemAllFields ListItemAllFields { get; set; }
        public ParentFolder ParentFolder { get; set; }
        public Properties Properties { get; set; }
        public jsonFolders Folders { get; set; }
        public int ItemCount { get; set; }
        public string Name { get; set; }
        public string ServerRelativeUrl { get; set; }
        public string WelcomePage { get; set; }
    }

 
#endregion

    #region SharePointFileInfo

    public class DefaultSPItemInfo
    {
        public SPItemInfo d { get; set; }
    }

    public class SPItemInfo
    {
        public __Metadata __metadata { get; set; }
        public __Deferred __deferred { get; set; }
        public Author Author { get; set; }
        public Checkedoutbyuser CheckedOutByUser { get; set; }
        public ListItemAllFields ListItemAllFields { get; set; }
        public Lockedbyuser LockedByUser { get; set; }
        public Modifiedby ModifiedBy { get; set; }
        public Versions Versions { get; set; }
        public string CheckInComment { get; set; }
        public int CheckOutType { get; set; }
        public string ContentTag { get; set; }
        public int CustomizedPageStatus { get; set; }
        public string ETag { get; set; }
        public bool Exists { get; set; }
        public string Length { get; set; }
        public int Level { get; set; }
        public int MajorVersion { get; set; }
        public int MinorVersion { get; set; }
        public string Name { get; set; }
        public string ServerRelativeUrl { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeLastModified { get; set; }
        public object Title { get; set; }
        public int UIVersion { get; set; }
        public string UIVersionLabel { get; set; }
    }

  
    public class __Deferred
    {
        public string uri { get; set; }
    }

    public class Author
    {
        public __Deferred __deferred { get; set; }
    }


    public class Checkedoutbyuser
    {
        public __Deferred __deferred { get; set; }
    }


    public class Lockedbyuser
    {
        public __Deferred __deferred { get; set; }
    }


    public class Modifiedby
    {
        public __Deferred __deferred { get; set; }
    }


    public class Versions
    {
        public __Deferred __deferred { get; set; }
    }


    #endregion

}

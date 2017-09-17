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

namespace Koden.Utils.SharePoint
{

    public enum AppType
    {
        Doclib,
        List,
        Tenant,
        Instance,
        Feature,
        CommonList
    }
    public enum BaseType
    {
        None = -1,
        GenericList = 0,
        DocumentLibrary = 1,
        Unused = 2,
        DiscussionBoard = 3,
        Survey = 4,
        Issue = 5
    }

    public enum BrowserFileHandling
    {
        Permissive,
        Strict
    }
    public enum CalendarType
    {
        None = 0,
        Gregorian = 1,
        Japan = 3,
        Taiwan = 4,
        Korea = 5,
        Hijri = 6,
        Thai = 7,
        Hebrew = 8,
        GregorianMEFrench = 9,
        GregorianArabic = 10,
        GregorianXLITEnglish = 11,
        GregorianXLITFrench = 12,
        KoreaJapanLunar = 14,
        ChineseLunar = 15,
        SakaEra = 16,
        UmAlQura = 23
    }

    [Flags]
    public enum AddFieldOptions
    {
        DefaultValue = 0,
        AddToDefaultContentType = 1,
        AddToNoContentType = 2,
        AddToAllContentTypes = 4,
        AddFieldInternalNameHint = 8,
        AddFieldToDefaultView = 16,
        AddFieldCheckDisplayName = 32
    }

    public enum CheckinType
    {
        MinorCheckIn,
        MajorCheckIn,
        OverwriteCheckIn
    }

    public enum CheckOutType
    {
        Online,
        Offline,
        None
    }

    public enum DocumentTemplateType
    {
        Invalid,
        Word,
        Excel,
        PowerPoint,
        OneNote,
        ExcelForm,
        Max
    }

    public enum FieldType
    {
        Invalid,
        Integer,
        Text,
        Note,
        DateTime,
        Counter,
        Choice,
        Lookup,
        Boolean,
        Number,
        Currency,
        URL,
        Computed,
        Threading,
        Guid,
        MultiChoice,
        GridChoice,
        Calculated,
        File,
        Attachments,
        User,
        Recurrence,
        CrossProjectLink,
        ModStat,
        Error,
        ContentTypeId,
        PageSeparator,
        ThreadIndex,
        WorkflowStatus,
        AllDayEvent,
        WorkflowEventType,
        Geolocation,
        OutcomeChoice,
        MaxItems
    }

    public enum FileLevel : byte
    {
        Published = 1,
        Draft = 2,
        Checkout = 255
    }
    public enum FileSystemObjectType
    {
        Invalid = -1,
        File = 0,
        Folder = 1,
        Web = 2
    }

    public enum ListTemplateType
    {
        InvalidType = -1,
        NoListTemplate = 0,
        GenericList = 100,
        DocumentLibrary = 101,
        Survey = 102,
        Links = 103,
        Announcements = 104,
        Contacts = 105,
        Events = 106,
        Tasks = 107,
        DiscussionBoard = 108,
        PictureLibrary = 109,
        DataSources = 110,
        WebTemplateCatalog = 111,
        UserInformation = 112,
        WebPartCatalog = 113,
        ListTemplateCatalog = 114,
        XMLForm = 115,
        MasterPageCatalog = 116,
        NoCodeWorkflows = 117,
        WorkflowProcess = 118,
        WebPageLibrary = 119,
        CustomGrid = 120,
        SolutionCatalog = 121,
        NoCodePublic = 122,
        ThemeCatalog = 123,
        DesignCatalog = 124,
        AppDataCatalog = 125,
        DataConnectionLibrary = 130,
        WorkflowHistory = 140,
        GanttTasks = 150,
        HelpLibrary = 151,
        AccessRequest = 160,
        TasksWithTimelineAndHierarchy = 171,
        MaintenanceLogs = 175,
        Meetings = 200,
        Agenda = 201,
        MeetingUser = 202,
        Decision = 204,
        MeetingObjective = 207,
        TextBox = 210,
        ThingsToBring = 211,
        HomePageLibrary = 212,
        Posts = 301,
        Comments = 302,
        Categories = 303,
        Facility = 402,
        Whereabouts = 403,
        CallTrack = 404,
        Circulation = 405,
        Timecard = 420,
        Holidays = 421,
        IMEDic = 499,
        ExternalList = 600,
        MySiteDocumentLibrary = 700,
        IssueTracking = 1100,
        AdminTasks = 1200,
        HealthRules = 1220,
        HealthReports = 1221,
        DeveloperSiteDraftApps = 1230,
        AlchemyApp = 3100
    }

    public enum PageType
    {
        Invalid = -1,
        DefaultView = 0,
        NormalView = 1,
        DialogView = 2,
        View = 3,
        DisplayForm = 4,
        DisplayFormDialog = 5,
        EditForm = 6,
        EditFormDialog = 7,
        NewForm = 8,
        NewFormDialog = 9,
        SolutionForm = 10,
        PAGE_MAXITEMS = 11
    }


    public enum PayloadType
    {
        Content,
        File
    }



    public enum ObjectType
    {
        List,
        Folder,
        Item,
        File,
        Web
    }

    public enum Scope
    {
        Web,
        Site,
        Lists,
        Folders
    }

    public enum Operation
    {
        CREATE,
        READ,
        UPDATE,
        DELETE,
        UPLOAD
    }
}

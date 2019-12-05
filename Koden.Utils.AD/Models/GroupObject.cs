namespace Koden.Utils.AD
{
    public class GroupObject
    {
        public GroupObject(string name, string groupType, string eMail, string description)
        {
            Name = name;
            GroupType = groupType;
            EMail = eMail;
            Description = description;
        }

        public string Name { get; set; }
        public string GroupType { get; set; }
        public string EMail { get; set; }
        public string Description { get; set; }

    }
}

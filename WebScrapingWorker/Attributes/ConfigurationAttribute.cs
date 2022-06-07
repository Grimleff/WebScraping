using System;

namespace WebScrapingWorker.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConfigurationAttribute : Attribute
    {
        public ConfigurationAttribute(string sectionName)
        {
            SectionName = sectionName;
        }

        public string SectionName { get; }
    }
}
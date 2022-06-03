using System;

namespace WebScrapingWorker.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConfigurationAttribute : Attribute
    {
        public string SectionName { get; }

        public ConfigurationAttribute(string sectionName)
        {
            SectionName = sectionName;
        }
    }
}
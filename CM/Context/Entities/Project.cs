using CM.Context.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CM.Context.Entities
{
    class Project : BaseEntity
    {
        public int ProjectID { get; set; }

        string name;
        public string Name
        {
            get { return name; }
            set { SetValue(ref name, value); }
        }

        string developmentUrl;
        public string DevelopmentUrl
        {
            get { return developmentUrl; }
            set { SetValue(ref developmentUrl, value); }
        }

        string dbConnectionString;
        public string DBConnectionString
        {
            get { return dbConnectionString; }
            set { SetValue(ref dbConnectionString, value); }
        }

        string localAppPublishPath;
        public string LocalAppPublishPath
        {
            get { return localAppPublishPath; }
            set { SetValue(ref localAppPublishPath, value); }
        }

        string localFrontPublishPath;
        public string LocalFrontPublishPath
        {
            get { return localFrontPublishPath; }
            set { SetValue(ref localFrontPublishPath, value); }
        }

        string developmentAppPublishPath;
        public string DevelopmentAppPublishPath
        {
            get { return developmentAppPublishPath; }
            set { SetValue(ref developmentAppPublishPath, value); }
        }

        string developmentFrontPublishPath;
        public string DevelopmentFrontPublishPath
        {
            get { return developmentFrontPublishPath; }
            set { SetValue(ref developmentFrontPublishPath, value); }
        }

        string devPublishPathUser;
        public string DevPublishPathUser
        {
            get { return devPublishPathUser; }
            set { SetValue(ref devPublishPathUser, value); }
        }

        string devPublishPathPsw;
        public string DevPublishPathPassword
        {
            get { return devPublishPathPsw; }
            set { SetValue(ref devPublishPathPsw, value); }
        }

        public virtual ICollection<Cas> Cases { get; set; }
    }
}

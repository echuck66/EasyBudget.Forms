﻿//
//  Copyright 2018  CrawfordNET Solutions, LLC
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using System;
using System.Text;

namespace EasyBudget.Business.ViewModels
{

    public abstract class BaseViewModel
    {
        internal string dbFilePath;

        StringBuilder sbErrorBuilder { get; set; }

        public string ErrorCondition 
        {
            get
            {
                return sbErrorBuilder.ToString();
            }
        }

        // Transitional Properties ****************

        public bool IsDirty { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }

        public bool IsNew { get; set; }

        //*****************************************

        internal BaseViewModel(string dbFilePath)
        {
            this.dbFilePath = dbFilePath;
            sbErrorBuilder = new StringBuilder();
        }

        internal void WriteErrorCondition(string error)
        {
            sbErrorBuilder.AppendLine(error);
        }

        internal void WriteErrorCondition(Exception ex)
        {
            sbErrorBuilder.AppendLine(ex.Message);

            if (ex.InnerException != null)
            {
                WriteErrorCondition(ex.InnerException);
            }
        }
    }

}

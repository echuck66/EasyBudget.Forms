//
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
using System.ComponentModel;
using System.Text;
using EasyBudget.Business.ChartModels;
using Microsoft.AppCenter.Crashes;

namespace EasyBudget.Business.ViewModels
{

    public abstract class BaseViewModel: INotifyPropertyChanged
    {
        internal string dbFilePath;


        public virtual event PropertyChangedEventHandler PropertyChanged;

        // Error tracking
        StringBuilder sbErrorBuilder { get; set; }
        public string ErrorCondition 
        {
            get
            {
                return sbErrorBuilder.ToString();
            }
        }

        // Transitional Properties ****************
        public IChartDataPack ChartDataPack 
        {
            get
            {
                return GetChartData();
            }
        }


        bool _IsDirty;
        public bool IsDirty 
        {
            get
            {
                return _IsDirty;
            }
            set
            {
                if (_IsDirty != value)
                {
                    _IsDirty = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsDirty)));
                }
            }
        }

        bool _CanEdit;
        public bool CanEdit 
        {
            get
            {
                return _CanEdit;
            }
            set
            {
                if (_CanEdit != value)
                {
                    _CanEdit = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanEdit)));
                }
            }
        }

        bool _CanDelete;
        public bool CanDelete 
        {
            get
            {
                return _CanDelete;
            }
            set
            {
                if (_CanDelete != value)
                {
                    _CanDelete = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanDelete)));
                }
            }
        }

        bool _IsNew;
        public bool IsNew 
        {
            get
            {
                return _IsNew;
            }
            set
            {
                if (_IsNew != value)
                {
                    _IsNew = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsNew)));
                }
            }
        }

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
            Crashes.TrackError(ex);

            sbErrorBuilder.AppendLine(ex.Message);

            if (ex.InnerException != null)
            {
                WriteErrorCondition(ex.InnerException);
            }
        }

        public abstract IChartDataPack GetChartData();

    }

}
